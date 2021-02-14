using System;
using System.Text;
using System.Collections.Generic;

using Server;
using Server.Mobiles;
using Server.Commands;
using Server.Targeting;

namespace Server.Commands
{
    public class FixStatMod
    {
        public static void Initialize()
        {
            CommandSystem.Register("FixStatMods", AccessLevel.Administrator, new CommandEventHandler(OnCommand_FixStatMods));
        }

        public static void OnCommand_FixStatMods(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            m.SendMessage("Please target the player you wish to clear.");
            m.Target = new InternalTarget(m);
        }

        public class InternalTarget : Target
        {
            private Mobile _Mobile;

            public InternalTarget(Mobile m)
                : base(15, false, TargetFlags.None)
            {
                _Mobile = m;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if( targeted == null )
                    return;

                if (targeted is PlayerMobile)
                {
                    PlayerMobile m = (PlayerMobile)targeted;

                    if (m != null)
                    {
                        for (int i = 0; i < m.StatMods.Count; i++)
                            if (m.StatMods[i] != null)
                            {
                                _Mobile.SendMessage("Removing StatMod Named: {0}", m.StatMods[i].Name);
                                m.RemoveStatMod(m.StatMods[i].Name);
                            }
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target. Please try again");
                    from.Target = new InternalTarget(_Mobile);
                }
            }
        }
    } 
}
