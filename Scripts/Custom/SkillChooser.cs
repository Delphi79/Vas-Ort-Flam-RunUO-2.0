using System;
using System.Collections.Generic;

using Server;
using Server.Gumps;
using Server.Accounting;
using Server.Network;

namespace Server.Misc
{
	public class SkillChooser
	{
		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler(EventSink_Login);
		}

		static void EventSink_Login(LoginEventArgs e)
		{
			Mobile from = e.Mobile;
			Account acc = (Account)from.Account;

			if (acc.GetTag("skillPick") == null ||
				acc.GetTag("skillPick") == "1" ||
				acc.GetTag("skillPick") == "2")
			{
				from.SendGump(new SkillChooserGump());
				from.SendMessage("You have available skill points to spend, but they are for your entire account, not just this character. Please remember to spend them wisely!");
			}
		}

		public class SkillChooserGump : Gump
		{
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

			private List<string> m_Skills;
			private int m_Page;

			public SkillChooserGump()
				: this(BuildList(), 0)
			{
			}

			public SkillChooserGump(List<string> list, int page)
				: base(GumpOffsetX, GumpOffsetY)
			{
				m_Skills = list;

				Initialize(page);
			}

			public static List<string> BuildList()
			{
				List<string> skills = new List<string>();

				for (int i = 0; i < SkillInfo.Table.Length; ++i)
				{
					if (i > 48)
						break;

					skills.Add(SkillInfo.Table[i].Name);
				}

				return skills;
			}

			public void Initialize(int page)
			{
				m_Page = page;

				int count = m_Skills.Count - (page * EntryCount);

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

				AddLabel(x + TextOffsetX, y, TextHue, String.Format("Page {0} of {1} ({2})", page + 1, (m_Skills.Count + EntryCount - 1) / EntryCount, m_Skills.Count));

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

				if ((page + 1) * EntryCount < m_Skills.Count)
				{
					AddButton(x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1);

					if (NextLabel)
						AddLabel(x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next");
				}

				for (int i = 0, index = page * EntryCount; i < EntryCount && index < m_Skills.Count; ++i, ++index)
				{
					x = BorderSize + OffsetSize;
					y += EntryHeight + OffsetSize;

					string m = m_Skills[index];

					AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
					AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, 0x59, m);

					x += EntryWidth + OffsetSize;

					if (SetGumpID != 0)
						AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

					AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, i + 3, GumpButtonType.Reply, 0);
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
								from.SendGump(new SkillChooserGump(m_Skills, m_Page - 1));

							break;
						}
					case 2: // Next 
						{
							if ((m_Page + 1) * EntryCount < m_Skills.Count)
								from.SendGump(new SkillChooserGump(m_Skills, m_Page + 1));

							break;
						}
					default:
						{
							int index = (m_Page * EntryCount) + (info.ButtonID - 3);

							if (index >= 0 && index < m_Skills.Count)
							{
								Account acc = (Account)from.Account;

								if (acc.GetTag("skillPick") == null ||
									acc.GetTag("skillPick") == "1" ||
									acc.GetTag("skillPick") == "2")
								{
									string m = m_Skills[index];

									if (from.Skills[index].Base != 100.0)
									{
										double toRaise = ((double)(100.0 - from.Skills[index].Base));

										double skillsTotal = ((double)from.SkillsTotal / (double)10.0);

										double skillsCap = ((double)from.SkillsCap / (double)10.0);

										if (skillsTotal + toRaise <= skillsCap)
										{
											from.Skills[index].Base = 100.0;
											from.SendMessage(String.Format("You have raised your skill in {0} to 100.", m));

											if (acc.GetTag("skillPick") == null)
											{
												acc.SetTag("skillPick", "1");
												from.SendMessage("You have 2 skill points left on your account.");
												from.SendGump(new SkillChooserGump(m_Skills, m_Page));
											}
											else if (acc.GetTag("skillPick") == "1")
											{
												acc.SetTag("skillPick", "2");
												from.SendMessage("You have 1 skill point remaining on your account.");
												from.SendGump(new SkillChooserGump(m_Skills, m_Page));
											}
											else if (acc.GetTag("skillPick") == "2")
											{
												acc.SetTag("skillPick", "3");
												from.SendMessage("You have no more skill points remaining on this account.");
											}
										}
										else
										{
											from.SendMessage("Raising that skill to 100 would exceed the skillcap.");
											from.SendGump(new SkillChooserGump(m_Skills, m_Page));
										}
									}
									else
									{
										from.SendMessage("That skill is already at 100.");
										from.SendGump(new SkillChooserGump(m_Skills, m_Page));
									}
								}
								else
									from.SendMessage("You have already raised 3 skills.");
							}

							break;
						}
				}
			}
		}
	}
}