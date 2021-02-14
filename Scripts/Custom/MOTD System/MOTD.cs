using System;
using System.IO;
using System.Collections.Generic;

using Server;
using Server.Gumps;
using Server.Commands;
using Server.Mobiles;
using Server.Accounting;

namespace Server.MOTD
{
	public class MOTD 
	{
        private static List<Publish> _Publishes;

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler(EventSink_Login);
			EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
            CommandSystem.Register("UpdateMOTD", AccessLevel.Administrator, new CommandEventHandler(OnCommand_UpdateMOTD));

            LoadPublishes();
		}

        private static void LoadPublishes()
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(Core.BaseDirectory, "Data\\Motd"));
            FileInfo[] files = dir.GetFiles("*.txt");

            if( _Publishes == null )
                _Publishes = new List<Publish>();

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];
                string name = Path.GetFileNameWithoutExtension(file.Name);
                string info = "";

                using (StreamReader reader = new StreamReader(file.FullName))
                {
                    info = reader.ReadToEnd();
                    reader.Close();
                }

                _Publishes.Add( new Publish(name, info) );
            }

            _Publishes.Sort(new Comparison<Publish>(Compare));
        }

        private static void OnCommand_UpdateMOTD(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;


        }

        static int Compare(Publish one, Publish two)
        {
            char[] oneChars = one.Name.ToCharArray();
            char[] twoChars = two.Name.ToCharArray();

            int length = Math.Min(oneChars.Length, twoChars.Length);

            for (int i = 0; i < length; i++)
            {
                if ((int)oneChars[i] < (int)twoChars[i])
                    return 1;
                else if ((int)oneChars[i] > (int)twoChars[i])
                    return -1;
            }

            if (oneChars.Length > twoChars.Length)
                return -1;
            else if (oneChars.Length < twoChars.Length)
                return 1;
            else
                return 0;               
        }

        static void EventSink_Speech(SpeechEventArgs e)
		{
			if (e.Speech.ToLower().IndexOf("motd") != -1 && _Publishes.Count > 0)
			{
                    e.Mobile.CloseGump(typeof(MOTDGump));
                    e.Mobile.SendGump(new MOTDGump(_Publishes));             
			}
		}

		static void EventSink_Login(LoginEventArgs e)
		{
            Mobile m = e.Mobile;

            if (m == null)
                return;

            Account acc = (Account)m.Account;

            if (_Publishes.Count > 0 && acc.GetTag(_Publishes[0].Name) == null)
                e.Mobile.SendGump(new MOTDGump(_Publishes));
		}  
	}
}