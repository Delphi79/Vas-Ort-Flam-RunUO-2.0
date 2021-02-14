using System;

using Server;
using Server.Commands;
using Server.Targeting;
using Server.Gumps;

namespace Server.Duels
{
	public class GetDuelInfo
	{
		public static void Initialize()
		{
			CommandSystem.Register("DuelInfo", AccessLevel.Player, new CommandEventHandler(GetDuelInfo_OnCommand));
		}

		public static void GetDuelInfo_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new DuelInfoTarget();
			e.Mobile.SendMessage("Select the person whose duel info you would like to see.");
		}

		public class DuelInfoTarget : Target
		{
			public DuelInfoTarget() : base(100, false, TargetFlags.None) { }

			protected override void OnTarget(Mobile from, object target)
			{
				if (target is Mobile)
				{
					Mobile targ = (Mobile)target;

					if (DuelCore.Infos.ContainsKey(targ.Serial))
						from.SendGump(new DuelInfoGump(DuelCore.GetInfo(targ)));
					else
						from.SendMessage("That person has not dueled anyone.");
				}
			}
		}

		public class DuelInfoGump : Gump
		{
			DuelInfo m_Info;

			public DuelInfoGump(DuelInfo info)
			: base( 0, 0 )
			{
				m_Info = info;
				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;
				this.AddPage(0);
				this.AddBackground(100, 115, 278, 313, 9270);
				this.AddLabel(124, 139, 100, info.Mobile.Name);
				this.AddLabel(124, 169, 1149, String.Format("Wins: {0}", info.Wins));
				this.AddLabel(124, 199, 1149, String.Format("Losses: {0}", info.Losses));
				this.AddLabel(183, 243, 100, @"History (last 50)");
				this.AddHtml( 116, 265, 245, 144, GetLog(), (bool)true, (bool)true);
			}

			string GetLog()
			{
				string log = "";

				for (int i = 0; i < m_Info.Log.Count; ++i)
				{
					log += m_Info.Log[i];
				}

				return log;
			}
		}
	}
}