using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Dueling
{
    public class DuelRegion : Region
    {
        public DuelRegion(Rectangle2D[] rects)
            : base("Duel Pit", Map.Felucca, 40, rects)
        {

        }

        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            return base.AllowBeneficial(from, target);
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return base.AllowHousing(from, p);
        }

        public override bool CanUseStuckMenu(Mobile m)
        {
            return base.CanUseStuckMenu(m);
        }

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            Duel duel;

            if (DuelCore.CheckDuel(m, out duel))
                return duel.SpellAllowed(s);

            return true;
        }

        public override bool OnDamage(Mobile m, ref int Damage)
        {
            return base.OnDamage(m, ref Damage);
        }

        public override bool OnDeath(Mobile m)
        {
            return base.OnDeath(m);
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
        }

        public override bool OnResurrect(Mobile m)
        {
            return base.OnResurrect(m);
        }

        public override bool OnSkillUse(Mobile m, int Skill)
        {
            Duel duel;

            if (DuelCore.CheckDuel(m, out duel))
                return duel.SkillAllowed(Skill);            

            return true;
        }

        public virtual bool HasCurrentDuel()
        {
            List<Mobile> mobiles = GetMobiles();

            for (int i = 0; i < mobiles.Count; i++)
                if (DuelCore.CheckDuel(mobiles[i]))
                    return true;

            return false;
        }
    }
}
