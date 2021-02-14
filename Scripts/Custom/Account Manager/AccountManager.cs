using System;
using System.Text;
using System.Threading;
using System.Net.Mail;
using System.Collections.Generic;

using Server;
using Server.Mobiles;
using Server.Commands;

namespace Server.Accounting
{
    public class AccountManager
    {
        private static Dictionary<Serial, ChangeRequest> _AuthList;
        private static Dictionary<Serial, ChangeRequestTimer> _Timers;
        private static MailAddress _FromAddress;
        private static readonly string _EmailServer = "mail.px.gamersground.net";

        public static Dictionary<Serial, ChangeRequest> AuthList { get { return _AuthList; } set { _AuthList = value; } }
        public static Dictionary<Serial, ChangeRequestTimer> Timers { get { return _Timers; } set { _Timers = value; } }

        public static void Initialize()
        {           
            _FromAddress = new MailAddress("noreply@projectxuo.net", "Project X Account Manager");
            _AuthList = new Dictionary<Serial, ChangeRequest>();
            _Timers = new Dictionary<Serial, ChangeRequestTimer>();

            EventSink.Login += new LoginEventHandler(EventSink_Login);

            Commands.CommandSystem.Register("account", AccessLevel.Player, new CommandEventHandler(OnCommand_Account));
            Commands.CommandSystem.Register("auth", AccessLevel.Player, new CommandEventHandler(OnCommand_Auth));
        }

        private static void SendMailMessage(MailMessage message, Mobile m)
        {
            SmtpClient client = new SmtpClient(_EmailServer, 25);
            client.Credentials = new System.Net.NetworkCredential("noreply@projectxuo.net", "jeff");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.SendCompleted += new SendCompletedEventHandler(Client_SendCompleted);
            Console.WriteLine("Account Manager: {0}'s email being sent to {1}", m.Name, message.To.ToString());
            client.SendAsync(message, m.Serial);
        }

        private static void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Serial serial = (Serial)e.UserState;

            if (e.Error == null)
            {
                Mobile m = World.FindMobile(serial);
                Console.WriteLine("Account Manager: {0}'s email was sucessfully sent.", m.Name);

                if (m != null)
                    m.SendMessage("Your email was sent successfully.");
            }
            else
            {
                Mobile m = World.FindMobile(serial);

                if (m != null)
                {
                    m.SendMessage("The authentication message was unsuccessfully sent to the supplied email address.");
                    if (_AuthList.ContainsKey(serial))
                    {
                        ChangeRequest req = _AuthList[serial];
                        Console.WriteLine("Account Manager: {0}'s email was invalid.", m.Name);
                        if (req.RequestType == RequestType.Email)
                            m.SendMessage("Your email has been reset and will need to be re-entered when you login.");
                        else if (req.RequestType == RequestType.ChangeEmail)
                            m.SendMessage("A email was unable to be sent to the registered address. Please contact a GM reguarding this issue.");
                        else
                            m.SendMessage("A email was unable to be sent to the registered address. Please contact a GM reguarding this issue.");

                        _AuthList.Remove(serial);

                        if (_Timers.ContainsKey(serial))
                        {
                            _Timers[serial].Stop();
                            _Timers.Remove(serial);
                        }
                    }
                }
            }
        }

        static void EventSink_Login(LoginEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            if (_AuthList.ContainsKey(m.Serial))
                return;            

            Account acc = (Account)m.Account;

            foreach (Mobile mob in World.Mobiles.Values)
            {
                if (mob != null && _AuthList.ContainsKey(mob.Serial) && (Account)mob.Account != null)
                    if (acc == (Account)mob.Account)
                        return;
            }

            if (acc == null)
                return;

            if (acc.GetTag("EMAIL") == "" || acc.GetTag("EMAIL") == null)
                m.SendGump(new EmailGump());
            else
                m.SendMessage("This account is registered to " + acc.GetTag("EMAIL"));
        }

        public static void OnCommand_Account(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            Account acc = (Account)m.Account;

            if (acc.GetTag("EMAIL") == null)
            {
                m.SendMessage("You have to register a email address before you can use this command.");
                m.CloseGump(typeof(EmailGump));
                m.SendGump(new EmailGump());
                return;
            }

            m.CloseGump(typeof(AccountConfigGump));
            m.SendGump(new AccountConfigGump());
        }

        public static void OnCommand_Auth(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            if (e.ArgString == null || e.ArgString == "")
            {
                m.SendMessage("Usage: [auth <key>");
                return;
            }

            if (!_AuthList.ContainsKey(m.Serial))
            {
                m.SendMessage("You do not have any account requests that need authorization");
                return;
            }

            ChangeRequest req = _AuthList[m.Serial];

            bool Auth = req.Authenticate(e.ArgString);

            if (Auth)
            {
                switch (req.RequestType)
                {
                    case RequestType.ChangeEmail:
                        {
                            Account acc = (Account)m.Account;

                            if (acc != null)
                            {
                                acc.SetTag("EMAIL", req.RequestString);
                                m.SendMessage("Your registered email address has been changed.");
                            }

                            break;
                        }
                    case RequestType.Email:
                        {
                            Account acc = (Account)m.Account;

                            if (acc != null)
                            {
                                acc.AddTag("EMAIL", req.RequestString);
                                m.SendMessage("Your email address has been registered with this account.");
                            }

                            break;
                        }
                    case RequestType.Password:
                        {
                            Account acc = (Account)m.Account;

                            if (acc != null)
                            {
                                acc.SetPassword(req.RequestString);
                                m.SendMessage("Your account password has been changed.");
                            }

                            break;
                        }
                }

                if (_Timers.ContainsKey(m.Serial))
                {
                    ChangeRequestTimer timer = _Timers[m.Serial];
                    timer.Stop();

                    _Timers.Remove(m.Serial);
                    _AuthList.Remove(m.Serial);
                }
            }
            else
                m.SendMessage("That key was invalid");
        }

        public ChangeRequest CreateRequest(Mobile m, RequestType type, string requestString, string authKey)
        {
            if (m == null)
                return null;

            ChangeRequest request = new ChangeRequest(m, type, authKey, requestString);
            return request;
        }

        public static MailMessage CreateMailMessage(Mobile m, RequestType type, string authKey, string emailAddress)
        {
            //Console.WriteLine("EMAIL: {0}", emailAddress);
            MailMessage mail = new MailMessage(_FromAddress, new MailAddress(emailAddress));
            mail.Subject = "Accounting Authentication Code";
            mail.Body = CreateMessage(m, type, authKey);
            return mail;
        }

        public static string CreateMessage(Mobile m, RequestType type, string authKey)
        {
            DateTime expire = DateTime.Now + TimeSpan.FromHours(2.0);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0},\n\n", m.Name);
            sb.AppendFormat("You have requested to {0}.\n\n",
                type == RequestType.ChangeEmail ? "release this e-mail address from your account" :
                type == RequestType.Email ? "register this email address to your account." :
                "change the password registered to this account.");
            sb.Append("To finalize this request, you must enter the following string (while in game) exactly as it appears.\n\n");
            sb.AppendFormat("[auth {0}\n\n", authKey);
            sb.AppendFormat("This key will expire at {0} {1}, If you have any questions, comments, suggestions, or require assistance, please do not hesitate to page or visit our forums at http://pokey.f13nd.net/forum/ .\n\n",
                expire.ToShortDateString(), expire.ToShortTimeString());
            sb.Append("Thank you,\nThe Project X Administration Team.\nhttp://www.projectxuo.net");

            return sb.ToString();
        }

        public static string CreateKey()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 16; i++)
                sb.Append(_KeyChars[Utility.Random(_KeyChars.Length)]);

            return sb.ToString();
        }

        private static char[] _KeyChars = new char[]
            {
                'a','b','c','d','e','f','g','h','i',
                'j','k','l','m','n','o','p','q','r',
                's','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I',
                'J','K','L','M','N','O','P','Q','R',
                'S','T','U','V','W','X','Y','Z',
                '1','2','3','4','5','6','7','8','9',
                '0'
            };

        internal static void HandleEmailEntry(Mobile m, string email)
        {
            if (_AuthList.ContainsKey(m.Serial))
            {
                m.SendMessage("You already have a transaction awaiting authentication, please check your email.");
                return;
            }

            string key = CreateKey();

            ChangeRequest request = new ChangeRequest(m, RequestType.Email, key, email);
            ChangeRequestTimer timer = new ChangeRequestTimer(m);

            _AuthList.Add(m.Serial, request);
            _Timers.Add(m.Serial, timer);

            timer.Start();

            MailMessage mail = CreateMailMessage(m, request.RequestType, key, email);

            SendMailMessage(mail, m);
            m.SendMessage("A email has been sent to the supplied address. Please read the email for further instructions.");
        }

        internal static void HandleEmailChangeRequest(Mobile m, string newEmail)
        {
            if (_AuthList.ContainsKey(m.Serial))
            {
                m.SendMessage("You already have a transaction awaiting authentication, please check your email.");
                return;
            }

            string key = CreateKey();

            ChangeRequest request = new ChangeRequest(m, RequestType.ChangeEmail, key, newEmail);
            ChangeRequestTimer timer = new ChangeRequestTimer(m);

            _AuthList.Add(m.Serial, request);
            _Timers.Add(m.Serial, timer);

            timer.Start();

            SendMailMessage(CreateMailMessage(m, RequestType.ChangeEmail, key, ((Account)m.Account).GetTag("EMAIL")), m);
            //m.SendMessage("A email has been sent to the supplied address.  Please read the email for further instructions.");
        }

        internal static void HandlePasswordChangeRequest(Mobile m, string newPassword)
        {
            if (_AuthList.ContainsKey(m.Serial))
            {
                m.SendMessage("You already have a transaction awaiting authentication, please check your email.");
                return;
            }

            string key = CreateKey();

            ChangeRequest request = new ChangeRequest(m, RequestType.Password, key, newPassword);
            ChangeRequestTimer timer = new ChangeRequestTimer(m);

            _AuthList.Add(m.Serial, request);
            _Timers.Add(m.Serial, timer);

            timer.Start();

            SendMailMessage(CreateMailMessage(m, RequestType.Password, key, ((Account)m.Account).GetTag("EMAIL")), m);
            //m.SendMessage("A email has been sent to the supplied address.  Please read the email for further instructions.");
        }
    }

    public class ChangeRequestTimer : Timer
    {
        private Mobile _Mobile;

        public ChangeRequestTimer(Mobile m)
            : base(TimeSpan.FromHours(2.0))
        {
            _Mobile = m;
        }

        protected override void OnTick()
        {
            if (_Mobile == null)
                return;

            if (AccountManager.AuthList.ContainsKey(_Mobile.Serial))
                AccountManager.AuthList.Remove(_Mobile.Serial);

            if (AccountManager.Timers.ContainsKey(_Mobile.Serial))
                AccountManager.Timers.Remove(_Mobile.Serial);

            Stop();
        }
    }
}
