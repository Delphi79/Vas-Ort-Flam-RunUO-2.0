using System;

using Server;
using Server.Spells;
using Server.Mobiles;
using Server.Regions;
using Server.Items;

namespace Server.Duels
{
	public class DuelRegion : GuardedRegion
	{
		private DuelArena m_Arena;

		public DuelRegion( DuelArena arena, Map map, params Rectangle3D[] area ) : base( arena.Name, map, 52, area )
		{
			m_Arena = arena;
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override bool CanUseStuckMenu( Mobile from )
		{
			from.SendMessage( "You may not use the stuck menu here." );

			return false;
		}

		public override bool OnBeginSpellCast( Mobile from, ISpell s )
		{
			if (m_Arena.Duel != null && m_Arena.Duel.IsRestrictedSpell(s))
			{
				from.SendMessage("You may not cast that spell here.");
				return false;
			}

			if (((Spell)s).Info.Name == "Ethereal Mount" && m_Arena.Duel != null && !m_Arena.Duel.Mounts)
			{
				from.SendMessage("You may not mount your ethereal here.");
				return false;
			}

			return true;
		}

		public override bool OnSkillUse( Mobile from, int skill )
		{
			if(m_Arena.Duel != null && m_Arena.Duel.IsRestrictedSkill(skill))
			{
				from.SendMessage( "You may not use that skill here." ); 
				return false;
			}

			return base.OnSkillUse( from, skill );
		}
		
		public override bool OnMoveInto( Mobile from, Direction d, Point3D newLocation, Point3D oldLocation )
		{
			if( !this.Contains( oldLocation ) )
			{
				from.SendMessage( "You may not enter this area." );
				return false; 
			}

			return true;
		}

		public override bool OnDoubleClick(Mobile from, object o)
		{
			if(m_Arena.Duel != null && !m_Arena.Duel.Potions && o is BasePotion)
			{
				from.SendMessage("Potions have been restricted for this duel.");
				return false;
			}

			if (m_Arena.Duel != null && !m_Arena.Duel.Bandages && o is Bandage)
			{
				from.SendMessage("Bandages have been restricted for this duel.");
				return false;
			}

			return base.OnDoubleClick(from, o);
		}
	}
}
