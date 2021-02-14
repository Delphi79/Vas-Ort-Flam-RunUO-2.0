using System;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Items;
using Server.Spells;

namespace Server
{   
    public class EventController
    {
        private GenericRules _GenericRules;
        private WeaponAccuracyRules _WeaponAccRules;
        private WeaponDamageRules _WeaponDamRules;
        private WeaponDurabilityRules _WeaponDurRules;
        private ArmorDurabilityRules _ArmorDurRules;
        private ArmorProtectionRules _ArmorProtRules;
        private PotionRules _PotionRules;

        private BitArray _SkillsAllowed;
        private BitArray _SpellsAllowed;
        
        public bool KeepScore
        {
            get
            {
                return ((_GenericRules & GenericRules.KeepScore) != 0);
            }
            set
            {
                if (value)
                    _GenericRules |= GenericRules.KeepScore;
                else
                    _GenericRules &= ~GenericRules.KeepScore;
            }
        }
        public bool AllowMounts
        {
            get
            {
                return ((_GenericRules & GenericRules.AllowMounts) != 0);
            }
            set
            {
                if (value)
                    _GenericRules |= GenericRules.AllowMounts;
                else
                    _GenericRules &= ~GenericRules.AllowMounts;
            }
        }
        public bool AllowBetting
        {
            get
            {
                return ((_GenericRules & GenericRules.AllowBetting) != 0);
            }
            set
            {
                if (value)
                    _GenericRules |= GenericRules.AllowBetting;
                else
                    _GenericRules &= ~GenericRules.AllowBetting;
            }
        }
        public bool AllowWands
        {
            get
            {
                return ((_GenericRules & GenericRules.AllowWands) != 0);
            }
            set
            {
                if (value)
                    _GenericRules |= GenericRules.AllowWands;
                else
                    _GenericRules &= ~GenericRules.AllowWands;
            }
        }

        public EventController()
        {
            _SkillsAllowed = new BitArray(SkillInfo.Table.Length, true);
            _SpellsAllowed = new BitArray(SpellRegistry.Types.Length, true);
        }
                
        public bool WeaponDurabilityAllowed(WeaponDurabilityLevel level)
        {
            switch (level)
            {
                case WeaponDurabilityLevel.Durable:
                    return ((_WeaponDurRules & WeaponDurabilityRules.Durable) != 0);
                case WeaponDurabilityLevel.Fortified:
                    return ((_WeaponDurRules & WeaponDurabilityRules.Fortified) != 0);
                case WeaponDurabilityLevel.Indestructible:
                    return ((_WeaponDurRules & WeaponDurabilityRules.Indestructable) != 0);
                case WeaponDurabilityLevel.Massive:
                    return ((_WeaponDurRules & WeaponDurabilityRules.Massive) != 0);
                case WeaponDurabilityLevel.Substantial:
                    return ((_WeaponDurRules & WeaponDurabilityRules.Substantial) != 0);
                default: return true;
            }
        }

        public bool WeaponDamageAllowed(WeaponDamageLevel level)
        {
            switch (level)
            {
                case WeaponDamageLevel.Force:
                    return ((_WeaponDamRules & WeaponDamageRules.Force) != 0);
                case WeaponDamageLevel.Might:
                    return ((_WeaponDamRules & WeaponDamageRules.Might) != 0);
                case WeaponDamageLevel.Power:
                    return ((_WeaponDamRules & WeaponDamageRules.Power) != 0);
                case WeaponDamageLevel.Ruin:
                    return ((_WeaponDamRules & WeaponDamageRules.Ruin) != 0);
                case WeaponDamageLevel.Vanq:
                    return ((_WeaponDamRules & WeaponDamageRules.Vanq) != 0);
                default: return true;
            }
        }

        public bool WeaponAccuracyAllowed(WeaponAccuracyLevel level)
        {
            switch (level)
            {
                case WeaponAccuracyLevel.Accurate:
                    return ((_WeaponAccRules & WeaponAccuracyRules.Accurate) != 0);
                case WeaponAccuracyLevel.Eminently:
                    return ((_WeaponAccRules & WeaponAccuracyRules.Eminently) != 0);
                case WeaponAccuracyLevel.Exceedingly:
                    return ((_WeaponAccRules & WeaponAccuracyRules.Exceedingly) != 0);
                case WeaponAccuracyLevel.Supremely:
                    return ((_WeaponAccRules & WeaponAccuracyRules.Supremely) != 0);
                case WeaponAccuracyLevel.Surpassingly:
                    return ((_WeaponAccRules & WeaponAccuracyRules.Surpassingly) != 0);
                default: return true;
            }
        }

        public bool ArmorProtectionAllowed(ArmorProtectionLevel level)
        {
            switch (level)
            {
                case ArmorProtectionLevel.Defense:
                    return ((_ArmorProtRules & ArmorProtectionRules.Defense) != 0);
                case ArmorProtectionLevel.Fortification:
                    return ((_ArmorProtRules & ArmorProtectionRules.Fortification) != 0);
                case ArmorProtectionLevel.Guarding:
                    return ((_ArmorProtRules & ArmorProtectionRules.Guarding) != 0);
                case ArmorProtectionLevel.Hardening:
                    return ((_ArmorProtRules & ArmorProtectionRules.Hardening) != 0);
                case ArmorProtectionLevel.Invulnerability:
                    return ((_ArmorProtRules & ArmorProtectionRules.Indestructable) != 0);                
                default: return true;
            }
        }

        public bool ArmorDurabilityAllowed(ArmorDurabilityLevel level)
        {
            switch (level)
            {
                case ArmorDurabilityLevel.Durable:
                    return ((_ArmorDurRules & ArmorDurabilityRules.Durable) != 0);
                case ArmorDurabilityLevel.Fortified:
                    return ((_ArmorDurRules & ArmorDurabilityRules.Fortified) != 0);
                case ArmorDurabilityLevel.Indestructible:
                    return ((_ArmorDurRules & ArmorDurabilityRules.Indestructable) != 0);
                case ArmorDurabilityLevel.Massive:
                    return ((_ArmorDurRules & ArmorDurabilityRules.Massive) != 0);
                case ArmorDurabilityLevel.Substantial:
                    return ((_ArmorDurRules & ArmorDurabilityRules.Substantial) != 0);
                default: return true;
            }
        }

        public bool PotionAllowed(BasePotion potion)
        {
            if (potion is BaseAgilityPotion)
                return ((_PotionRules & PotionRules.Agility) != 0);
            else if (potion is BaseAgilityPotion)
                return ((_PotionRules & PotionRules.Cure) != 0);
            else if (potion is BaseAgilityPotion)
                return ((_PotionRules & PotionRules.Explosion) != 0);
            else if (potion is BaseAgilityPotion)
                return ((_PotionRules & PotionRules.Heal) != 0);
            else if (potion is BaseAgilityPotion)
                return ((_PotionRules & PotionRules.Poison) != 0);
            else if (potion is BaseAgilityPotion)
                return ((_PotionRules & PotionRules.Refresh) != 0);
            else if (potion is BaseAgilityPotion)
                return ((_PotionRules & PotionRules.Strength) != 0);

            return true;
        }

        public int GetRegistryNumber(ISpell s)
        {
            Type[] t = SpellRegistry.Types;

            for (int i = 0; i < t.Length; i++)
            {
                if (s.GetType() == t[i])
                    return i;
            }

            return -1;
        }

        public bool SpellAllowed(ISpell s)
        {
            if (_SpellsAllowed.Length != SpellRegistry.Types.Length)
            {
                _SpellsAllowed = new BitArray(SpellRegistry.Types.Length);

                for (int i = 0; i < _SpellsAllowed.Length; i++)
                    _SpellsAllowed[i] = false;
            }

            int regNum = GetRegistryNumber(s);

            if (regNum < 0)	//Happens with unregistered Spells
                return false;

            return _SpellsAllowed[regNum];
        }

        public bool SkillAllowed(int skill)
        {
            if (_SkillsAllowed.Length != SkillInfo.Table.Length)
            {
                _SkillsAllowed = new BitArray(SkillInfo.Table.Length);

                for (int i = 0; i < _SkillsAllowed.Length; i++)
                    _SkillsAllowed[i] = false;
            }

            if (skill < 0)
                return false;

            return _SkillsAllowed[skill];
        }
    }
}
