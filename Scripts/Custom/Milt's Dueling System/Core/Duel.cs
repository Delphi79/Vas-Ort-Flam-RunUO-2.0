using System;
using System.Collections.Generic;
using System.Collections;

using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;

namespace Server.Duels
{
	public enum DuelState
	{
		Setup,
		Duel,
		End,
		Cancel
	}

	public class Duel
	{
		private Mobile m_Attacker;
		private Mobile m_Defender;

		private DuelArena m_Arena;

		private DuelState m_State;

		public Mobile Attacker { get { return m_Attacker; } set { m_Attacker = value; } }
		public Mobile Defender { get { return m_Defender; } set { m_Defender = value; } }

		public DuelArena Arena { get { return m_Arena; } set { m_Arena = value; } }

		public DuelState State { get { return m_State; } set { m_State = value; } }

		#region Duel Options

		private bool[] m_Rules;

		public bool[] Rules { get { return m_Rules; } set { m_Rules = value; } }

		public bool MagicWeapons { get { return m_Rules[0]; } set { m_Rules[0] = value; } }
		public bool MagicArmor { get { return m_Rules[1]; } set { m_Rules[1] = value; } }
		public bool Potions { get { return m_Rules[2]; } set { m_Rules[2] = value; } }
		public bool Bandages { get { return m_Rules[3]; } set { m_Rules[3] = value; } }
		public bool Mounts { get { return m_Rules[4]; } set { m_Rules[4] = value; } }
		public bool Ranked { get { return m_Rules[5]; } set { m_Rules[5] = value; } }
		public bool FiveX { get { return m_Rules[6]; } set { m_Rules[6] = value; } }

		#endregion

		public Duel(Mobile attacker, Mobile defender)
		{
			m_Attacker = attacker;
			m_Defender = defender;

			m_Rules = new bool[]
			{
				true,  //Magic Weapons
				true,  //Magic Armor
				false, //Potions
				true,  //Bandages
				false, //Mounts
				true,  //Ranked
				false, //5x
			};
		}

		public bool IsRestrictedSpell(ISpell s)
		{
			Spell spell = (Spell)s;

			if (spell is Spells.First.ReactiveArmorSpell)
				return true;
			if (spell is Spells.Third.TeleportSpell)
				return true;
			if (spell is Spells.Third.WallOfStoneSpell)
				return true;
			if (spell is Spells.Fourth.FireFieldSpell)
				return true;
			if (spell is Spells.Fourth.RecallSpell)
				return true;
			if (spell is Spells.Fifth.BladeSpiritsSpell)
				return true;
			if (spell is Spells.Fifth.MagicReflectSpell)
				return true;
			if (spell is Spells.Fifth.ParalyzeSpell)
				return true;
			if (spell is Spells.Fifth.PoisonFieldSpell)
				return true;
			if (spell is Spells.Fifth.SummonCreatureSpell)
				return true;
			if (spell is Spells.Sixth.InvisibilitySpell)
				return true;
			if (spell is Spells.Sixth.MarkSpell)
				return true;
			if (spell is Spells.Sixth.ParalyzeFieldSpell)
				return true;
			if (spell is Spells.Seventh.GateTravelSpell)
				return true;
			if (spell is Spells.Eighth.AirElementalSpell)
				return true;
			if (spell is Spells.Eighth.EarthElementalSpell)
				return true;
			if (spell is Spells.Eighth.EnergyVortexSpell)
				return true;
			if (spell is Spells.Eighth.FireElementalSpell)
				return true;
			if (spell is Spells.Eighth.SummonDaemonSpell)
				return true;
			if (spell is Spells.Eighth.WaterElementalSpell)
				return true;

			return false;
		}

		public bool IsRestrictedSkill(int skill)
		{
			if (skill == 9)					//Peacemaking
				return true;
			if (skill == 17 && !Bandages)   //Healing
				return true;
			if (skill == 21)				//Hiding
				return true;
			if (skill == 33)				//Stealing
				return true;

			return false;
		}
	}
}