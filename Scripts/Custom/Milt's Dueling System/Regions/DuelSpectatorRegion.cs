using System;

using Server;
using Server.Regions;

namespace Server.Duels
{
	public class DuelSpectatorRegion : GuardedRegion
	{
		private DuelArena m_Arena;

		public DuelSpectatorRegion( DuelArena arena, Map map, params Rectangle3D[] area ) : base( arena.Name + " - Spectator", map, 51, area )
		{
			m_Arena = arena;
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override bool OnBeginSpellCast( Mobile from, ISpell s )
		{
			from.SendMessage("You may not cast spells here.");
			return false;
		}

		public override bool OnSkillUse( Mobile from, int skill )
		{
			from.SendMessage("You may not use skills here.");
			return false;
		}
	}
}
