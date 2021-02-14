using System;
using System.Collections.Generic;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class DuelArenaEditGump : Gump
	{
		private List<int> m_ButtonIDs;

		private static readonly int entriesPerPage = 4;
		private int m_Page;

		private List<DuelArena> m_Arenas;

		public DuelArenaEditGump(int page)
			: base( 0, 0 )
		{
			m_Page = page;

			m_ButtonIDs = new List<int>();

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
			this.AddButton(275, 271, 4011, 4012, (int)Buttons.btnAdd, GumpButtonType.Reply, 0);
			this.AddLabel(43, 95, 792, @"-Edit Arenas");
			this.AddBackground(358, 295, 101, 48, 5120);
			this.AddButton(376, 306, 4014, 4015, (int)Buttons.btnHome, GumpButtonType.Reply, 1);
			this.AddLabel(412, 308, 0, @"Home");
			this.AddLabel(36, 136, 862, @"Edit");
			this.AddLabel(315, 271, 862, @"Add new arena");
			this.AddLabel(76, 136, 862, @"Arena Name");
			this.AddLabel(211, 136, 862, @"ID #");
			this.AddButton(76, 287, 4014, 4015, (int)Buttons.btnPageDown, GumpButtonType.Reply, 0);
			this.AddButton(111, 287, 4005, 4006, (int)Buttons.btnPageUp, GumpButtonType.Reply, 0);

			InitializeComponent();
		}
		
		public enum Buttons
		{
			btnHome = 1,
			btnAdd,
			btnPageDown,
			btnPageUp,
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			if (m_ButtonIDs.Contains(info.ButtonID))
			{
				from.CloseGump(typeof(DuelArenaEditGump));

				from.SendGump( new DuelArenaEditorGump(m_Arenas[GetIndex(info.ButtonID)]));
				return;
			}

			switch (info.ButtonID)
			{
				case 0: //Right click to close
				case 1: //Home
					{
						from.CloseGump(typeof(DuelArenaEditGump));

						from.SendGump(new DuelConfigGump());
						break;
					}
				case 2: //Add
					{
						from.CloseGump(typeof(DuelArenaEditGump));

						DuelCore.Arenas.Add(new DuelArena("New Arena"));

						from.SendGump(new DuelArenaEditGump(1));
						break;
					}
				case 3: //Page Down
					{
						from.CloseGump(typeof(DuelArenaEditGump));

						from.SendGump(new DuelArenaEditGump(m_Page - 1));
						break;
					}
				case 4: //Page Up
					{
						from.CloseGump(typeof(DuelArenaEditGump));

						from.SendGump(new DuelArenaEditGump(m_Page + 1));
						break;
					}
			}
		}

		public void InitializeComponent()
		{
			List<DuelArena> master = DuelCore.Arenas;

			int TotalEntries = master.Count;

			if (TotalEntries == 0)
				return;

			int pagesTotal = (int)Math.Ceiling(((double)TotalEntries / entriesPerPage));

			if (m_Page > pagesTotal || m_Page < 1)
				m_Page = 1;

			int start = (m_Page - 1) * entriesPerPage;

			m_Arenas = new List<DuelArena>();

			for (int i = start; i < master.Count; ++i)
			{
				if (m_Arenas.Count < entriesPerPage)
					m_Arenas.Add(master[i]);
				else
					break;
			}

			for (int i = 0; i < m_Arenas.Count; ++i)
			{
				int location = 136 + ((i + 1) * 30);
				int id = (i + 5) * 2;

				this.AddButton(32, location, 4011, 4012, id, GumpButtonType.Reply, 0);
				this.AddLabel(76, location, 792, m_Arenas[i].Name);
				this.AddLabel(211, location, 792, GetIndexFor(m_Arenas[i]));

				m_ButtonIDs.Add(id);
			}

			this.AddLabel(149, 287, 862, String.Format("Page {0}/{1}", m_Page, pagesTotal));
		}

		private string GetIndexFor(DuelArena from)
		{
			for (int i = 0; i < DuelCore.Arenas.Count; ++i)
				if (DuelCore.Arenas[i] == from)
					return i.ToString();

			return "0";
		}

		private int GetIndex(int id) { return (id / 2) - 5; }
	}
}