using System;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class PlayerDuelAcceptGump : Gump
	{
		private Duel m_Duel;

		public PlayerDuelAcceptGump(Duel duel)
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
			this.AddLabel(235, 126, 100, @"Duel Invitation");
			this.AddLabel(154, 204, 1158, @"Magic Weapons");
			this.AddLabel(154, 229, 1158, @"Magic Armor");
			this.AddLabel(154, 254, 1158, @"Potions");
			this.AddLabel(154, 279, 1158, @"Bandages");
			this.AddLabel(154, 304, 1158, @"Mounts");
			this.AddLabel(154, 329, 1158, @"Ranked");
			this.AddLabel(154, 354, 1158, @"5x");
			this.AddLabel(135, 174, 100, @"Duel Options");
			this.AddAlphaRegion(277, 161, 160, 234);
			this.AddLabel(297, 170, 100, @"You have recieved a");
			this.AddLabel(302, 194, 100, @"challenge to a duel");
			this.AddLabel(304, 253, 1158, (m_Duel.Attacker.Name));
			this.AddButton(323, 325, 247, 248, (int)Buttons.btnOK, GumpButtonType.Reply, 0);
			this.AddButton(323, 356, 242, 241, (int)Buttons.btnCancel, GumpButtonType.Reply, 0);

			for (int i = 0; i < m_Duel.Rules.Length; ++i)
			{
				int y = 184 + (i + 1) * 25;
				this.AddImage(135, y, m_Duel.Rules[i] ? 2361 : 2360);
			}

			this.AddLabel(336, 218, 100, @"from:");
			this.AddLabel(309, 294, 100, @"Do you accept?");
		}
		
		public enum Buttons
		{
			btnOK = 1,
			btnCancel,
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 1: //OK
					{
						from.CloseGump(typeof(PlayerDuelAcceptGump));

						DuelCore.InitializeDuel(m_Duel);

						break;
					}
				case 2: //Cancel
					{
						from.CloseGump(typeof(PlayerDuelAcceptGump));

						if(m_Duel.Attacker != null)
							m_Duel.Attacker.SendMessage(String.Format("{0} has declined your duel request.", from.Name));

						from.SendMessage("You decide not to duel them.");

						m_Duel.State = DuelState.Cancel;

						DuelCore.CancelDuel(m_Duel);

						break;
					}
			}
		}
	}
}