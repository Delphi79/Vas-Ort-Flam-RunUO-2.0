using System;
using Server;
using Server.Gumps;

namespace Server.Accounting
{
    public class ChangeRequestGump : Gump
    {
        private RequestType _Type;

        public ChangeRequestGump(RequestType type)
            : base(0, 0)
        {
            _Type = type;
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(8, 18, 363, 88, 9200);
            this.AddLabel(15, 24, 54, String.Format("Please enter the new {0}", type == RequestType.Password ? "Password" : "email address" ) );
            this.AddBackground(14, 44, 349, 25, 9350);
            this.AddLabel(282, 76, 54, @"Submit");
            this.AddTextEntry(17, 46, 340, 20, 0, (int)Buttons.TextEntry1, @"");
            this.AddButton(331, 76, 4014, 4015, (int)Buttons.sbmtBtn, GumpButtonType.Reply, 0);
        }

        public enum Buttons
        {
            TextEntry1 = 1,
            sbmtBtn,
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null)
                return;

            if (info.ButtonID == 0)
                return;

            if (_Type == RequestType.ChangeEmail)
            {
                string email = info.TextEntries[0].Text;

                if ((email == null || email == "") || email.IndexOf('@') == -1)
                {
                    m.SendMessage("That email address was not valid.");
                    return;
                }

                m.SendMessage("A email was sent to the email address registered to this account.");
                AccountManager.HandleEmailChangeRequest(m, email);
            }
            else
            {
                string password = info.TextEntries[0].Text;

                if (password.Length > 16)
                {
                    m.SendMessage("Your password can contain no more then 16 characters.");
                    return;
                }

                m.SendMessage("A email was sent to the email address registered to this account.");
                AccountManager.HandlePasswordChangeRequest(m, password);
            }
        }
    }
}