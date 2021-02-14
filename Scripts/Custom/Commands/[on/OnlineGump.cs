using System;
using System.Collections;
using System.Collections.Generic;

using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
	public class OnlineGump : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("on", AccessLevel.Player, new CommandEventHandler(Msg_OnCommand));
		}

		[Usage("on")]
		[Description("List of client connected, able to send messages.")]
		private static void Msg_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendGump(new OnlineGump(e.Mobile));
		}

		public static bool OldStyle = PropsConfig.OldStyle;

		public static int GumpOffsetX = PropsConfig.GumpOffsetX;
		public static int GumpOffsetY = PropsConfig.GumpOffsetY;

		public static int TextHue = PropsConfig.TextHue;
		public static int TextOffsetX = PropsConfig.TextOffsetX;

		public static int OffsetGumpID = PropsConfig.OffsetGumpID;
		public static int HeaderGumpID = PropsConfig.HeaderGumpID;
		public static int EntryGumpID = PropsConfig.EntryGumpID;
		public static int BackGumpID = PropsConfig.BackGumpID;
		public static int SetGumpID = PropsConfig.SetGumpID;

		public static int SetWidth = PropsConfig.SetWidth;
		public static int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
		public static int SetButtonID1 = PropsConfig.SetButtonID1;
		public static int SetButtonID2 = PropsConfig.SetButtonID2;

		public static int PrevWidth = PropsConfig.PrevWidth;
		public static int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
		public static int PrevButtonID1 = PropsConfig.PrevButtonID1;
		public static int PrevButtonID2 = PropsConfig.PrevButtonID2;

		public static int NextWidth = PropsConfig.NextWidth;
		public static int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
		public static int NextButtonID1 = PropsConfig.NextButtonID1;
		public static int NextButtonID2 = PropsConfig.NextButtonID2;

		public static int OffsetSize = PropsConfig.OffsetSize;

		public static int EntryHeight = PropsConfig.EntryHeight;
		public static int BorderSize = PropsConfig.BorderSize;

		private static bool PrevLabel = false, NextLabel = false;

		private static int PrevLabelOffsetX = PrevWidth + 1;
		private static int PrevLabelOffsetY = 0;

		private static int NextLabelOffsetX = -29;
		private static int NextLabelOffsetY = 0;

		private static int EntryWidth = 180;
		private static int EntryCount = 15;

		private static int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth + OffsetSize;
		private static int TotalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (EntryCount + 1));

		private static int BackWidth = BorderSize + TotalWidth + BorderSize;
		private static int BackHeight = BorderSize + TotalHeight + BorderSize;

		private Mobile m_Owner;
		private ArrayList m_Mobiles;
		private int m_Page;

		private class InternalComparer : IComparer
		{
			public static readonly IComparer Instance = new InternalComparer();

			public InternalComparer()
			{
			}

			public int Compare( object x, object y )
			{
				if ( x == null && y == null )
					return 0;
				else if ( x == null )
					return -1;
				else if ( y == null )
					return 1;

				Mobile a = x as Mobile;
				Mobile b = y as Mobile;

				if ( a == null || b == null )
					throw new ArgumentException();

				else if ( a.AccessLevel < b.AccessLevel )
					return 1;
				else
					return CaseInsensitiveComparer.Default.Compare( a.Name, b.Name );
			}
		}

		public OnlineGump(Mobile owner)
			: this(owner, BuildList(owner), 0)
		{
		}

		public OnlineGump(Mobile owner, ArrayList list, int page)
			: base(GumpOffsetX, GumpOffsetY)
		{
			owner.CloseGump(typeof(WhoGump));

			m_Owner = owner;
			m_Mobiles = list;

			Initialize(page);
		}

		public static ArrayList BuildList(Mobile owner)
		{
			ArrayList list = new ArrayList();
			List<NetState> states = NetState.Instances;

			for (int i = 0; i < states.Count; ++i)
			{
				Mobile m = states[i].Mobile;

				if (m != null && (m == owner || m.AccessLevel == AccessLevel.Player))
					list.Add(m);
			}

			list.Sort(InternalComparer.Instance);

			return list;
		}

		public void Initialize(int page)
		{
			m_Page = page;

			int count = m_Mobiles.Count - (page * EntryCount);

			if (count < 0)
				count = 0;
			else if (count > EntryCount)
				count = EntryCount;

			int totalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (count + 1));

			AddPage(0);

			AddBackground(0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID);
			AddImageTiled(BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), totalHeight, OffsetGumpID);

			int x = BorderSize + OffsetSize;
			int y = BorderSize + OffsetSize;

			int emptyWidth = TotalWidth - PrevWidth - NextWidth - (OffsetSize * 4) - (OldStyle ? SetWidth + OffsetSize : 0);

			if (!OldStyle)
				AddImageTiled(x - (OldStyle ? OffsetSize : 0), y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0), EntryHeight, EntryGumpID);

			AddLabel(x + TextOffsetX, y, TextHue, String.Format("Page {0} of {1} ({2})", page + 1, (m_Mobiles.Count + EntryCount - 1) / EntryCount, m_Mobiles.Count));

			x += emptyWidth + OffsetSize;

			if (OldStyle)
				AddImageTiled(x, y, TotalWidth - (OffsetSize * 3) - SetWidth, EntryHeight, HeaderGumpID);
			else
				AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

			if (page > 0)
			{
				AddButton(x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0);

				if (PrevLabel)
					AddLabel(x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous");
			}

			x += PrevWidth + OffsetSize;

			if (!OldStyle)
				AddImageTiled(x, y, NextWidth, EntryHeight, HeaderGumpID);

			if ((page + 1) * EntryCount < m_Mobiles.Count)
			{
				AddButton(x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1);

				if (NextLabel)
					AddLabel(x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next");
			}

			for (int i = 0, index = page * EntryCount; i < EntryCount && index < m_Mobiles.Count; ++i, ++index)
			{
				x = BorderSize + OffsetSize;
				y += EntryHeight + OffsetSize;

				Mobile m = (Mobile)m_Mobiles[index];

				AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
				AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, GetHueFor(m), m.Deleted ? "(deleted)" : m.Name);

				x += EntryWidth + OffsetSize;

				if (SetGumpID != 0)
					AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

				if (m.NetState != null && !m.Deleted)
					AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, i + 3, GumpButtonType.Reply, 0);

			}
		}

		private static int GetHueFor(Mobile m) 
      { 
         switch ( m.AccessLevel ) 
         {
			 case AccessLevel.Owner: return 1160;
	    case AccessLevel.Developer: return 1301;
            case AccessLevel.Administrator: return 0x516; 
            case AccessLevel.Seer: return 0x144; 
            case AccessLevel.GameMaster: return 0x21; 
            case AccessLevel.Counselor: return 0x2; 
            case AccessLevel.Player: default: 
            { 
               if ( m.Kills >= 5 ) 
                  return 0x21; 
               else if ( m.Criminal ) 
                  return 0x3B1; 

               return 0x58; 
            } 
         } 
      }

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 0: // Closed 
					{
						return;
					}
				case 1: // Previous 
					{
						if (m_Page > 0)
							from.SendGump(new OnlineGump(from, m_Mobiles, m_Page - 1));

						break;
					}
				case 2: // Next 
					{
						if ((m_Page + 1) * EntryCount < m_Mobiles.Count)
							from.SendGump(new OnlineGump(from, m_Mobiles, m_Page + 1));

						break;
					}
				default:
					{
						int index = (m_Page * EntryCount) + (info.ButtonID - 3);

						if (index >= 0 && index < m_Mobiles.Count)
						{
							Mobile m = (Mobile)m_Mobiles[index];

							if (m.Deleted)
							{
								from.SendMessage("That player has deleted their character.");
								from.SendGump(new OnlineGump(from, m_Mobiles, m_Page));
							}
							else if (m.NetState == null)
							{
								from.SendMessage("That player is no longer online.");
								from.SendGump(new OnlineGump(from, m_Mobiles, m_Page));
							}
							else if (m == m_Owner || !m.Hidden || m_Owner.AccessLevel > m.AccessLevel)
							{
								from.SendGump(new OnlineClientGump(from, m.NetState));
							}
							else
							{
								from.SendMessage("You cannot see them.");
								from.SendGump(new OnlineGump(from, m_Mobiles, m_Page));
							}
						}

						break;
					}
			}
		}
	}
}
