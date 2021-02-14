using System;
using System.Collections;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class PlayerDuelGump : Gump
	{
		private Duel m_Duel;

		public PlayerDuelGump(Duel duel)
			: base( 0, 0 )
		{
			m_Duel = duel;

			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddImageTiled(112, 109, 328, 289, 9304);
			this.AddAlphaRegion(114, 111, 323, 47);
			this.AddAlphaRegion(114, 161, 160, 234);
			this.AddLabel(235, 126, 100, @"Setup A Duel");
			this.AddCheck(135, 209, 2360, 2361, m_Duel.MagicWeapons, (int)Buttons.btnWeapons);
			this.AddLabel(154, 204, 1158, @"Magic Weapons");
			this.AddCheck(135, 234, 2360, 2361, m_Duel.MagicArmor, (int)Buttons.btnArmor);
			this.AddLabel(154, 229, 1158, @"Magic Armor");
			this.AddCheck(135, 259, 2360, 2361, m_Duel.Potions, (int)Buttons.btnPotions);
			this.AddLabel(154, 254, 1158, @"Potions");
			this.AddCheck(135, 284, 2360, 2361, m_Duel.Bandages, (int)Buttons.btnBandages);
			this.AddLabel(154, 279, 1158, @"Bandages");
			this.AddCheck(135, 309, 2360, 2361, m_Duel.Mounts, (int)Buttons.btnMounts);
			this.AddLabel(154, 304, 1158, @"Mounts");
			this.AddCheck(135, 334, 2360, 2361, m_Duel.Ranked, (int)Buttons.btnRanked);
			this.AddLabel(154, 329, 1158, @"Ranked");
			this.AddCheck(135, 359, 2360, 2361, m_Duel.FiveX, (int)Buttons.btn5x);
			this.AddLabel(154, 354, 1158, @"5x");
			this.AddLabel(135, 174, 100, @"Duel Options");
			this.AddAlphaRegion(277, 161, 160, 234);
			this.AddLabel(317, 174, 100, @"Choose Your");
			this.AddLabel(325, 195, 100, @"Opponent");
			this.AddButton(284, 259, 5402, 5401, (int)Buttons.btnAdd, GumpButtonType.Reply, 0);
			this.AddButton(321, 356, 242, 241, (int)Buttons.btnCancel, GumpButtonType.Reply, 0);
		}
		
		public enum Buttons
		{
			btnWeapons = 1,
			btnArmor,
			btnPotions,
			btnBandages,
			btnMounts,
			btnRanked,
			btn5x,
			btnAdd,
			btnCancel
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 8: //Add
					{
						from.CloseGump(typeof(PlayerDuelGump));

						int[] switches = info.Switches;

						for (int i = 0; i < m_Duel.Rules.Length; ++i)
							m_Duel.Rules[i] = false;

						for (int i = 0; i < switches.Length; ++i)
							m_Duel.Rules[switches[i] - 1] = true;

						from.Target = new DuelTarget(m_Duel);

						break;
					}
				case 9: //Cancel
					{
						from.CloseGump(typeof(PlayerDuelGump));
						DuelCore.CancelDuel(m_Duel);

						break;
					}
			}
		}
	}
}