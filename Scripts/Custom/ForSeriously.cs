using System;
using System.Collections.Generic;
using System.Text;

using Server;

namespace Server.Commands
{
    public class ForSeriously
    {
        public static void Initialize()
        {
            CommandSystem.Register( "ForSeriouslyLOL", AccessLevel.Administrator, new CommandEventHandler( OnCommand_ForSeriouslyLOL ) );
		}

        public static void OnCommand_ForSeriouslyLOL(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            foreach (Mobile mob in World.Mobiles.Values)
                if (mob != null)
                    mob.SendMessage(51, "The server has crashed!");
        }
    }
}
