using System;
using Server.Network;

namespace Server
{
	public class StaffNotifications
	{
		public static void BroadcastMessage(AccessLevel ac, int hue, string message)
		{
			foreach (NetState state in NetState.Instances)
			{
				Mobile m = state.Mobile;

				if (m != null && m.AccessLevel >= ac)
					m.SendMessage(hue, message);
			}
		}
		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler(EventSink_Login);
			EventSink.Logout += new LogoutEventHandler(EventSink_Logout);
		}
		private static void EventSink_Login(LoginEventArgs args)
		{
			Mobile m = args.Mobile;
			BroadcastMessage(AccessLevel.Administrator, 0x851, String.Format("[Staff Message]: {0} has joined the server.", args.Mobile.Name));
		}
		private static void EventSink_Logout(LogoutEventArgs args)
		{
			Mobile m = args.Mobile;
			BroadcastMessage(AccessLevel.Administrator, 0x851, String.Format("[Staff Message]: {0} has left the server.", args.Mobile.Name));
		}
	}
}