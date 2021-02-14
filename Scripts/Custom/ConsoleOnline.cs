using System;

using Server.Network;

namespace Server
{
    public class ConsoleOnline
    {
        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(EventSink_Login);
            EventSink.Logout += new LogoutEventHandler(EventSink_Logout);
        }

        static void EventSink_Logout(LogoutEventArgs e)
        {
            Console.Title = String.Format("Clients Online: {0}", NetState.Instances.Count);
        }

        static void EventSink_Login(LoginEventArgs e)
        {
            Console.Title = String.Format( "Clients Online: {0}", NetState.Instances.Count );
        }
    }
}
