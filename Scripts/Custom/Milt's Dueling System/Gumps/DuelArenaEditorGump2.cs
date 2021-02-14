using System;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class DuelArenaEditorGump2 : Gump
	{
		private DuelArena m_Arena;

		public DuelArenaEditorGump2(DuelArena arena)
			: base(0, 0)
		{
			m_Arena = arena;

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
			this.AddLabel(43, 95, 792, @"-Arena Editor");
			this.AddBackground(358, 295, 101, 48, 5120);
			this.AddButton(376, 306, 4014, 4015, (int)Buttons.btnHome, GumpButtonType.Page, 0);
			this.AddLabel(412, 308, 0, @"Home");
			this.AddButton(32, 166, 4011, 4012, (int)Buttons.btnShowSpec, GumpButtonType.Reply, 0);
			this.AddLabel(76, 166, 792, @"Show spectator bounds");
			this.AddButton(33, 196, 4011, 4012, (int)Buttons.btnStop, GumpButtonType.Reply, 0);
			this.AddLabel(76, 196, 792, @"Stop current duel");
			this.AddButton(33, 226, 4011, 4012, (int)Buttons.btnEnable, GumpButtonType.Reply, 0);
			this.AddButton(33, 256, 4011, 4012, (int)Buttons.btnDelete, GumpButtonType.Reply, 0);
			this.AddLabel(50, 120, 862, m_Arena.Name);
			this.AddLabel(76, 226, 792, String.Format("Enable/Disable arena ({0})", m_Arena.Enabled ? "Enabled" : "Disabled"));
			this.AddLabel(76, 256, 792, @"Delete arena");
			this.AddButton(76, 287, 4014, 4015, (int)Buttons.btnPageDown, GumpButtonType.Reply, 0);
			this.AddButton(111, 287, 4005, 4006, (int)Buttons.btnPageUp, GumpButtonType.Reply, 0);
			this.AddLabel(149, 287, 862, @"Page 2/2");
		}

		public enum Buttons
		{
			btnHome = 1,
			btnShowSpec,
			btnStop,
			btnEnable,
			btnDelete,
			btnPageUp,
			btnPageDown
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 0: //Right click to close
				case 1: //Home
					{
						from.CloseGump(typeof(DuelArenaEditorGump2));

						from.SendGump(new DuelConfigGump());
						break;
					}
				case 2: //Show Spec
					{
						from.CloseGump(typeof(DuelArenaEditorGump2));

						m_Arena.ShowBounds(m_Arena.SpectatorCoords);

						from.SendGump(new DuelArenaEditorGump2(m_Arena));
						break;
					}
				case 3: //Stop
					{
						from.CloseGump(typeof(DuelArenaEditorGump2));

						from.SendMessage("This feature has not yet been implimented.");

						from.SendGump(new DuelArenaEditorGump2(m_Arena));
						break;
					}
				case 4: //Enable / Disable
					{
						from.CloseGump(typeof(DuelArenaEditorGump2));

						m_Arena.Enabled = !m_Arena.Enabled;

						from.SendGump(new DuelArenaEditorGump2(m_Arena));
						break;
					}
				case 5: //Delete arena
					{
						from.CloseGump(typeof(DuelArenaEditorGump2));

						m_Arena.Unregister();

						if(DuelCore.Arenas.Contains(m_Arena))
							DuelCore.Arenas.Remove(m_Arena);

						from.SendGump(new DuelArenaEditGump(1));
						break;
					}
				case 6: //Page Up
					{
						from.CloseGump(typeof(DuelArenaEditorGump2));

						from.SendGump(new DuelArenaEditorGump2(m_Arena));
						break;
					}
				case 7: //Page Down
					{
						from.CloseGump(typeof(DuelArenaEditorGump2));

						from.SendGump(new DuelArenaEditorGump(m_Arena));
						break;
					}
			}
		}
	}
}