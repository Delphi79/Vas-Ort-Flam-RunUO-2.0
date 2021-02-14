using System;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class DuelArenaManageGump : Gump
	{
		public DuelArenaManageGump()
			: base(0, 0)
		{
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
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
			this.AddButton(48, 160, 4011, 4012, (int)Buttons.btnEdit, GumpButtonType.Reply, 0);
			this.AddLabel(43, 95, 792, @"-Arena Management");
			this.AddBackground(358, 295, 101, 48, 5120);
			this.AddButton(376, 306, 4014, 4015, (int)Buttons.btnHome, GumpButtonType.Reply, 0);
			this.AddLabel(412, 308, 0, @"Home");
			this.AddLabel(87, 160, 792, @"Edit arenas");
			this.AddButton(48, 202, 4011, 4012, (int)Buttons.btnShow, GumpButtonType.Reply, 0);
			this.AddLabel(87, 202, 792, @"Show all arenas");
			this.AddButton(48, 244, 4011, 4012, (int)Buttons.btnDelete, GumpButtonType.Reply, 0);
			this.AddLabel(87, 244, 792, @"Delete all arenas");
		}

		public enum Buttons
		{
			btnEdit = 1,
			btnHome,
			btnShow,
			btnDelete
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 1: //Edit arenas
					{
						from.CloseGump(typeof(DuelArenaManageGump));

						from.SendGump(new DuelArenaEditGump(1));
						break;
					}
				case 0: //Right click to close
				case 2: //Home
					{
						from.CloseGump(typeof(DuelArenaManageGump));

						from.SendGump(new DuelConfigGump());
						break;
					}
				case 3: //Show all
					{
						from.CloseGump(typeof(DuelArenaManageGump));

						for (int i = 0; i < DuelCore.Arenas.Count; ++i)
						{
							DuelCore.Arenas[i].ShowBounds(DuelCore.Arenas[i].DuelCoords);
							DuelCore.Arenas[i].ShowBounds(DuelCore.Arenas[i].SpectatorCoords);
						}

						from.SendGump(new DuelArenaManageGump());
						break;
					}
				case 4: //Delete all
					{
						from.CloseGump(typeof(DuelArenaManageGump));

						DuelCore.Arenas.Clear();

						from.SendGump(new DuelArenaManageGump());
						break;
					}
			}
		}
	}
}