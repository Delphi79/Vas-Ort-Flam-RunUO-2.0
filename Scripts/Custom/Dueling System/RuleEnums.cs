using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public enum GenericRules
    {
        AllowWands = 0x0001,
        AllowMounts = 0x0002,
        AllowBetting = 0x0004,
        KeepScore = 0x0008,
    }

    public enum WeaponDamageRules
    {
        Regular = 0x01,
        Ruin = 0x02,
        Might = 0x04,
        Force = 0x08,
        Power = 0x10,
        Vanq = 0x20
    }

    public enum WeaponAccuracyRules
    {
        Regular = 0x01,
        Accurate = 0x02,
        Surpassingly = 0x04,
        Eminently = 0x08,
        Exceedingly = 0x10,
        Supremely = 0x20
    }

    public enum WeaponDurabilityRules
    {
        Regular = 0x01,
        Durable = 0x02,
        Substantial = 0x04,
        Massive = 0x08,
        Fortified = 0x10,
        Indestructable = 0x20
    }

    public enum ArmorDurabilityRules
    {
        Regular = 0x01,
        Durable = 0x01,
        Substantial = 0x01,
        Massive = 0x01,
        Fortified = 0x01,
        Indestructable = 0x01
    }

    public enum ArmorProtectionRules
    {
        Regular = 0x01,
        Defense = 0x02,
        Guarding = 0x04,
        Hardening = 0x08,
        Fortification = 0x10,
        Indestructable = 0x20
    }

    public enum PotionRules
    {
        All = 0x01,
        Heal = 0x02,
        Cure = 0x04,
        Refresh = 0x08,
        Agility = 0x10,
        Strength = 0x20,
        Explosion = 0x40,
        Poison = 0x80
    }
}
