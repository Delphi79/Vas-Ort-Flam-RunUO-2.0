using System;

using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Duels
{
	public class DuelTarget : Target
	{
		private Duel m_Duel;

		public DuelTarget(Duel duel)
			: base(100, false, TargetFlags.None)
		{
			m_Duel = duel;
		}

		protected override void OnTargetOutOfLOS(Mobile from, object targeted)
		{
			from.SendMessage("That is not in your line of site, please select a new target.");
			from.Target = new DuelTarget(m_Duel);
		}

		protected override void OnTargetNotAccessible(Mobile from, object targeted)
		{
			from.SendMessage("That is not accessible, please select a new target.");
			from.Target = new DuelTarget(m_Duel);
		}

		protected override void OnTargetUntargetable(Mobile from, object targeted)
		{
			from.SendMessage("You cannot target that, please select a new target.");
			from.Target = new DuelTarget(m_Duel);
		}

		protected override void OnTargetDeleted(Mobile from, object targeted)
		{
			from.SendMessage("You cannot see that, please select a new target.");
			from.Target = new DuelTarget(m_Duel);
		}

		protected override void OnCantSeeTarget(Mobile from, object targeted)
		{
			from.SendMessage("You cannot see that, please select a new target.");
			from.Target = new DuelTarget(m_Duel);
		}

		protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
		{
			base.OnTargetCancel(from, cancelType);

			from.SendMessage("You have cancelled the duel.");
			m_Duel.State = DuelState.Cancel;
			DuelCore.CancelDuel(m_Duel);
		}

		protected override void OnTarget(Mobile from, object target)
		{
			if (target is PlayerMobile)
			{
				Mobile m = (Mobile)target;

				if (m.NetState == null)
				{
					from.SendMessage("That player is not online, please select a new target.");
					from.Target = new DuelTarget(m_Duel);
					return;
				}

				if (m == from)
				{
					from.SendMessage("You cannot duel yourself, please select a new target.");
					from.Target = new DuelTarget(m_Duel);
					return;
				}

				if (m.Criminal)
				{
					from.SendMessage("You may not start a duel with someone who is flagged criminal, please select a new target.");
					from.Target = new DuelTarget(m_Duel);
					return;
				}

				if (Spells.SpellHelper.CheckCombat(m))
				{
					from.SendMessage("That person is currently in combat, please select a new target.");
					from.Target = new DuelTarget(m_Duel);
					return;
				}

				if (m_Duel.FiveX && m.SkillsTotal > 5000)
				{
					from.SendMessage("That player has over 500 skillpoints and is not a valid candidate for a 5x duel. Please select a new target.");
					from.Target = new DuelTarget(m_Duel);
					return;
				}

				if (DuelCore.FindDuel(m) == null)
				{
					if (!m.HasGump(typeof(PlayerDuelAcceptGump)))
					{
						if (!m.HasGump(typeof(PlayerDuelGump)))
						{
							if (m.Target == null)
							{
								m_Duel.Defender = m;
								m.SendGump(new PlayerDuelAcceptGump(m_Duel));
								from.SendMessage("You have sent them an invitation. Waiting for them to accept...");

								Timer.DelayCall(TimeSpan.FromSeconds(20.0), new TimerStateCallback(delegate(object state)
								{
									if (m_Duel.State == DuelState.Setup)
									{
										m.CloseGump(typeof(PlayerDuelAcceptGump));
										m.SendMessage("Your duel invitation has timed out.");
										from.SendMessage(String.Format("Your duel invitation with {0} has timed out.", m.Name));
										DuelCore.CancelDuel(m_Duel);
									}
								}), null);
							}
							else
							{
								from.SendMessage("You may not start a duel with a person that is targeting something. Please select a new target.");
								from.Target = new DuelTarget(m_Duel);
							}
						}
						else
						{
							from.SendMessage("That person is already setting up a duel. Please select a new target.");
							from.Target = new DuelTarget(m_Duel);
						}
					}
					else
					{
						from.SendMessage("That player has already been offered a duel. Please select a new target.");
						from.Target = new DuelTarget(m_Duel);
					}
				}
				else
				{
					from.SendMessage("That player is already in a duel. Please select a new target.");
					from.Target = new DuelTarget(m_Duel);
				}
			}
			else
			{
				from.SendMessage("You may only duel players, please select a new target.");
				from.Target = new DuelTarget(m_Duel);
			}
		}
	}
}
