using System;

using Server;

namespace Server.TournamentSystem
{
	public class TCore
	{
		public static readonly bool Enabled = false; //Is the system enabled?

		public static void Initialize()
		{
			if (Enabled)
			{
				EventSink.WorldLoad += new WorldLoadEventHandler(EventSink_WorldLoad);
				EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);
			}
		}

		public static void EventSink_WorldLoad()
		{
			//Load tournament data
		}

		public static void EventSink_WorldSave(WorldSaveEventArgs e)
		{
			//Save tournament data
		}
	}
}