using System;

using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Guilds;
using Server.Network;

namespace Server.Misc
{
	public class MiltGuildChat
	{
		public static void Initialize()
		{
			CommandSystem.Register("g", AccessLevel.Player, new CommandEventHandler(GuildChat_OnCommand));
		}

		public static void GuildChat_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;

			if (e.ArgString.Length > 0)
			{
				if (from.Guild == null)
				{
					from.SendMessage("You must be in a guild to use guild chat!");
					return;
				}


				foreach (NetState state in NetState.Instances)
				{
					Mobile m = state.Mobile;

					if (m != null && m.Guild != null && m.Guild == from.Guild)
					{
						if (e.ArgString != "")
						{
							m.SendMessage(0x42, "{0} (Guild Chat): {1}", from.Name, e.ArgString);
						}
					}
				}
			}
		}
	}
}