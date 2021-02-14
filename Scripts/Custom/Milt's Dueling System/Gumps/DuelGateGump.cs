using System;
using System.Collections.Generic;

using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Server.Duels
{
	public class DuelGateGump : Gump
	{
		List<int> m_ButtonIDs;

		public DuelGateGump(Mobile from)
			: base( 0, 0 )
		{
			m_ButtonIDs = new List<int>();

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 400, (DuelCore.Arenas.Count * 39 + 25) + 74, 3500);
			this.AddLabel(22, 19, 100, @"Project X Dueling System - Travel Gate");

			for (int i = 0; i < DuelCore.Arenas.Count; ++i)
			{
				int x = 17;
				int y = (i + 1) * 39 + 25;

				this.AddAlphaRegion(x, y, 366, 35);

				this.AddRadio(x + 2, y + 4, 9721, 9724, false, i + 3);
				this.AddLabel(x + 39, y + 9, 100, DuelCore.Arenas[i].Name);

				if (i == DuelCore.Arenas.Count - 1) //Last iteration
				{
					this.AddButton(x + 286, y + 39, 247, 248, (int)Buttons.btnOK, GumpButtonType.Reply, 0);
				}

				m_ButtonIDs.Add(i + 3);
			}

			this.AddImage(295, 22, 9000);
		}
		
		public enum Buttons
		{
			btnOK = 1
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			Mobile from = sender.Mobile;

			switch (info.ButtonID)
			{
				case 1: //OK
					{
						for (int i = 0; i < m_ButtonIDs.Count; ++i)
						{
							if(info.IsSwitched(m_ButtonIDs[i]))
							{
								try
								{
									DuelArena arena = DuelCore.Arenas[m_ButtonIDs[i] - 3];

									BaseCreature.TeleportPets(from, arena.Home, arena.Map);

									from.MoveToWorld(arena.Home, arena.Map);

									from.PlaySound(0x1FE);
								}
								catch
								{
									from.SendMessage("There was a problem in using the gate. Please try again.");
								}

								break;
							}
						}

						break;
					}
			}
		}
	}
}