using System;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class DuelArenaEditorGump : Gump
	{
		private DuelArena m_Arena;

		public DuelArenaEditorGump(DuelArena arena)
			: base( 0, 0 )
		{
			m_Arena = arena;

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
			this.AddLabel(43, 95, 792, @"-Arena Editor");
			this.AddBackground(358, 295, 101, 48, 5120);
			this.AddButton(376, 306, 4014, 4015, (int)Buttons.btnHome, GumpButtonType.Reply, 0);
			this.AddLabel(412, 308, 0, @"Home");
			this.AddButton(32, 166, 4011, 4012, (int)Buttons.btnSetArena, GumpButtonType.Reply, 0);
			this.AddLabel(76, 166, 792, @"Set arena bounds");
			this.AddButton(33, 196, 4011, 4012, (int)Buttons.btnSetSpec, GumpButtonType.Reply, 0);
			this.AddLabel(76, 196, 792, @"Set spectator bounds");
			this.AddButton(33, 226, 4011, 4012, (int)Buttons.btnSetHome, GumpButtonType.Reply, 0);
			this.AddButton(33, 256, 4011, 4012, (int)Buttons.btnShowArena, GumpButtonType.Reply, 0);
			this.AddLabel(76, 226, 792, @"Set arena home");
			this.AddLabel(76, 256, 792, @"Show arena bounds");
			this.AddButton(76, 287, 4014, 4015, (int)Buttons.btnPageDown, GumpButtonType.Reply, 0);
			this.AddButton(111, 287, 4005, 4006, (int)Buttons.btnPageUp, GumpButtonType.Reply, 1);
			this.AddLabel(149, 287, 862, @"Page 1/2");
			this.AddBackground(43, 117, 145, 24, 9350);
			this.AddTextEntry(48, 119, 137, 21, 862, 100, m_Arena.Name);
			this.AddButton(194, 119, 4011, 4012, (int)Buttons.btnSubmitName, GumpButtonType.Reply, 0);
		}
		
		public enum Buttons
		{
			btnHome = 1,
			btnSetArena,
			btnSetSpec,
			btnSetHome,
			btnShowArena,
			btnPageDown,
			btnPageUp,
			btnSubmitName
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 0: //Right click to close
				case 1: //Home
					{
						from.CloseGump(typeof(DuelArenaEditorGump));

						from.SendGump(new DuelConfigGump());
						break;
					}
				case 2: //Set Arena
					{
						from.CloseGump(typeof(DuelArenaEditorGump));

						m_Arena.ChooseArea(from, true);
						break;
					}
				case 3: //Set Spectator
					{
						from.CloseGump(typeof(DuelArenaEditorGump));

						m_Arena.ChooseArea(from, false);
						break;
					}
				case 4: //Set Home
					{
						from.CloseGump(typeof(DuelArenaEditorGump));

						from.SendGump(new PropertiesGump(from, m_Arena));
						break;
					}
				case 5: //Show Arena
					{
						from.CloseGump(typeof(DuelArenaEditorGump));

						m_Arena.ShowBounds(m_Arena.DuelCoords);

						from.SendGump(new DuelArenaEditorGump(m_Arena));
						break;
					}
				case 6: //Page Down
					{
						from.CloseGump(typeof(DuelArenaEditorGump));

						from.SendGump(new DuelArenaEditorGump(m_Arena));
						break;
					}
				case 7: //Page Up
					{
						from.CloseGump(typeof(DuelArenaEditorGump));

						from.SendGump(new DuelArenaEditorGump2(m_Arena));
						break;
					}
				case 8: //Submit Name
					{
						foreach (TextRelay relay in info.TextEntries)
						{
							if(relay.EntryID == 100)
							{
								from.CloseGump(typeof(DuelArenaEditorGump));

								m_Arena.Name = relay.Text;

								from.SendGump(new DuelArenaEditorGump(m_Arena));
								break;
							}
						}

						break;
					}
			}
		}
	}
}