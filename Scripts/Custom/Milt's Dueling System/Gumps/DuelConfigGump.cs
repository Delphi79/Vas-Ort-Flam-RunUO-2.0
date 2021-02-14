using System;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class DuelConfigGump : Gump
	{
		public DuelConfigGump()
			: base( 0, 0 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(22, 59, 411, 259, 5120);
			this.AddLabel(43, 72, 1153, @"Milt's Dueling System");
			this.AddImage(242, 62, 105);
			this.AddImage(355, 62, 106);
			this.AddImage(301, 129, 107);
			this.AddImage(273, 202, 108);
			this.AddImage(245, 129, 109);
			this.AddImage(298, 62, 110);
			this.AddImage(353, 129, 111);
			this.AddImage(321, 202, 112);
			this.AddLabel(84, 163, 792, @"Arena Management");
			this.AddButton(45, 163, 4011, 4012, (int)Buttons.btnArena, GumpButtonType.Reply, 0);
			this.AddLabel(84, 199, 862, String.Format("Enable / Disable System ({0})", (DuelCore.Enabled ? "Enabled" : "Disabled")));
			this.AddButton(45, 199, 4011, 4012, (int)Buttons.btnEnable, GumpButtonType.Reply, 0);
			this.AddLabel(84, 235, 542, @"Global Commands/Settings");
			this.AddButton(45, 235, 4011, 4012, (int)Buttons.btnSettings, GumpButtonType.Reply, 0);
			this.AddLabel(43, 95, 1153, @"-Home");
		}
		
		public enum Buttons
		{
			btnArena = 1,
			btnEnable,
			btnSettings
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 1: //Arena Management
					{
						from.CloseGump(typeof(DuelConfigGump));
						from.SendGump(new DuelArenaManageGump());
						break;
					}
				case 2: //Toggle Enable
					{
						from.CloseGump(typeof(DuelConfigGump));
						DuelCore.Enabled = !DuelCore.Enabled;
						from.SendGump(new DuelConfigGump());
						break;
					}
				case 3: //Global Commands/Settings
					{
						from.CloseGump(typeof(DuelConfigGump));
						from.SendGump(new DuelCommandGump());
						break;
					}
			}
		}
	}
}