using System;

using Server;
using Server.Commands;
using Server.Gumps;

namespace Server.Spells
{
	public class SpellController
	{
		private static SpellController _Instance = new SpellController();

		public static void Initialize()
		{
			CommandSystem.Register("SpellControl", AccessLevel.Administrator, new CommandEventHandler(SpellControl_OnCommand));
		}

		[Usage("SpellControl")]
		[Description("Displays a gump to dynamically change spell options, timings, and damages.")]
		public static void SpellControl_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;

			if (m == null)
				return;

			if (e.ArgString.IndexOf("restore") != -1)
			{
				RestoreDefaults();
				return;
			}

			m.SendGump(new PropertiesGump(m, SpellController.Instance));
		}

		public static SpellController Instance { get { return _Instance; } }

		#region Spellbook - Private

		//First Circle
		private static string _HealDamage = "1d5+0";
		private static string _MagicArrowDamage = "1d4+0";

		//Second Circle
		private static string _HarmDamage = "1d10+0";

		//Third Circle
		private static string _FireballDamage = "5d2+5";

		//Fourth Circle
		private static string _GreaterHealDamage = "2d5+0";
		private static string _LightningDamage = "6d3+2";

		//Sixth Circle
		private static string _EnergyBoltDamage = "10d3+6";
		private static string _ExplosionDamage = "12d3+3";

		//Seventh Circle
		private static string _ChainLightningDamage = "10d4+10";
		private static string _FlameStrikeDamage = "20d2+10";
		private static string _MeteorSwarmDamage = "15d2+8";

		#endregion

		#region Spell.cs Stuff - Private

		//Spell.cs Stuff
        private static double _MeditationModifier = .40;
        private static double _SpellResistModifier = 3.0;

		private static TimeSpan _NextSpellDelay = TimeSpan.FromSeconds(0.75);
		private static TimeSpan _AnimateDelay = TimeSpan.FromSeconds(1.5);

		private static SkillName _CastSkill = SkillName.Magery;
		private static SkillName _DamageSkill = SkillName.EvalInt;

		private static bool _RevealOnCast = true;
		private static bool _ClearHandsOnCast = true;
		private static bool _ShowHandMovement = true;

		private static bool _DelayedDamage = false;

		private static bool _DelayedDamageStacking = true;

		private static int _CastDelayBase = 3;
		private static int _CastDelayCircleScalar = 1;
		private static int _CastDelayFastScalar = 1;
		private static int _CastDelayPerSecond = 4;
		private static int _CastDelayMinimum = 1;

		private static int _CastRecoveryBase = 1;
		private static int _CastRecoveryCircleScalar = 0;
		private static int _CastRecoveryFastScalar = 1;
		private static int _CastRecoveryPerSecond = 3;
		private static int _CastRecoveryMinimum = 0;

		#endregion

		#region Spellbook - Public

		//First Circle
		[CommandProperty(AccessLevel.Administrator)]
		public static string HealDamage { get { return _HealDamage; } set { _HealDamage = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string MagicArrowDamage { get { return _MagicArrowDamage; } set { _MagicArrowDamage = value; } }

		//Second Circle
		[CommandProperty(AccessLevel.Administrator)]
		public static string HarmDamage { get { return _HarmDamage; } set { _HarmDamage = value; } }

		//Third Circle
		[CommandProperty(AccessLevel.Administrator)]
		public static string FireballDamage { get { return _FireballDamage; } set { _FireballDamage = value; } }

		//Fourth Circle
		[CommandProperty(AccessLevel.Administrator)]
		public static string GreaterHealDamage { get { return _GreaterHealDamage; } set { _GreaterHealDamage = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string LightningDamage { get { return _LightningDamage; } set { _LightningDamage = value; } }

		//Sixth Circle
		[CommandProperty(AccessLevel.Administrator)]
		public static string EnergyBoltDamage { get { return _EnergyBoltDamage; } set { _EnergyBoltDamage = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string ExplosionDamage { get { return _ExplosionDamage; } set { _ExplosionDamage = value; } }
		
		//Seventh Circle
		[CommandProperty(AccessLevel.Administrator)]
		public static string ChainLightningDamage { get { return _ChainLightningDamage; } set { _ChainLightningDamage = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string FlameStrikeDamage { get { return _FlameStrikeDamage; } set { _FlameStrikeDamage = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string MeteorSwarmDamage { get { return _MeteorSwarmDamage; } set { _MeteorSwarmDamage = value; } }

		#endregion

		#region Spell.cs Stuff - Public
        
        //Spell.cs Stuff
        [CommandProperty(AccessLevel.Administrator)]
        public static double MeditationModifier { get { return _MeditationModifier; } set { _MeditationModifier = value; } }
        [CommandProperty(AccessLevel.Administrator)]
        public static double SpellResistModifier { get { return _SpellResistModifier; } set { _SpellResistModifier = value; } }

		[CommandProperty(AccessLevel.Administrator)]
		public static TimeSpan NextSpellDelay{ get{ return _NextSpellDelay; } set{ _NextSpellDelay = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static TimeSpan AnimateDelay{ get{ return _AnimateDelay; } set{ _AnimateDelay = value; } }

		[CommandProperty(AccessLevel.Administrator)]
		public static SkillName CastSkill{ get{ return _CastSkill; } set{ _CastSkill = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static SkillName DamageSkill{ get{ return _DamageSkill; } set{ _DamageSkill = value; } }

		[CommandProperty(AccessLevel.Administrator)]
		public static bool RevealOnCast{ get{ return _RevealOnCast; } set{ _RevealOnCast = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static bool ClearHandsOnCast{ get{ return _ClearHandsOnCast; } set{ _ClearHandsOnCast = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static bool ShowHandMovement{ get{ return _ShowHandMovement; } set{ _ShowHandMovement = value; } }

		[CommandProperty(AccessLevel.Administrator)]
		public static bool DelayedDamage{ get{ return _DelayedDamage; } set{ _DelayedDamage = value; } }

		[CommandProperty(AccessLevel.Administrator)]
		public static bool DelayedDamageStacking{ get{ return _DelayedDamageStacking; } set{ _DelayedDamageStacking = value; } }

		[CommandProperty(AccessLevel.Administrator)]
		public static int CastDelayBase{ get{ return _CastDelayBase; } set{ _CastDelayBase = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static int CastDelayCircleScalar{ get{ return _CastDelayCircleScalar; } set{ _CastDelayCircleScalar = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static int CastDelayFastScalar{ get{ return _CastDelayFastScalar; } set{ _CastDelayFastScalar = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static int CastDelayPerSecond{ get{ return _CastDelayPerSecond; } set{ _CastDelayPerSecond = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static int CastDelayMinimum{ get{ return _CastDelayMinimum; } set{ _CastDelayMinimum = value; } }

		[CommandProperty(AccessLevel.Administrator)]
		public static int CastRecoveryBase{ get{ return _CastRecoveryBase; } set{ _CastRecoveryBase = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static int CastRecoveryCircleScalar{ get{ return _CastRecoveryCircleScalar; } set{ _CastRecoveryCircleScalar = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static int CastRecoveryFastScalar{ get{ return _CastRecoveryFastScalar; } set{ _CastRecoveryFastScalar = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static int CastRecoveryPerSecond{ get{ return _CastRecoveryPerSecond; } set{ _CastRecoveryPerSecond = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static int CastRecoveryMinimum{ get{ return _CastRecoveryMinimum; } set{ _CastRecoveryMinimum = value; } }

		#endregion

		public static int GetDamage(string dice)
		{
			string[] d = dice.Split(new char[] { 'd', '+' });

			return Utility.Dice(Int32.Parse(d[0]), Int32.Parse(d[1]), Int32.Parse(d[2]));
		}

		public static void RestoreDefaults()
		{
			//First Circle
			_HealDamage = "1d5+0";
			_MagicArrowDamage = "1d4+2";

			//Second Circle
			_HarmDamage = "1d10+0";

			//Third Circle
			_FireballDamage = "5d2+5";

			//Fourth Circle
			_GreaterHealDamage = "2d5+0";
			_LightningDamage = "6d3+2";

			//Sixth Circle
			_EnergyBoltDamage = "10d3+6";
			_ExplosionDamage = "12d3+3";

			//Seventh Circle
			_ChainLightningDamage = "10d4+10";
			_FlameStrikeDamage = "20d2+10";
			_MeteorSwarmDamage = "15d2+8";

			//Spell.cs Stuff
			_NextSpellDelay = TimeSpan.FromSeconds(0.75);
			_AnimateDelay = TimeSpan.FromSeconds(1.5);

			_CastSkill = SkillName.Magery;
			_DamageSkill = SkillName.EvalInt;

			_RevealOnCast = true;
			_ClearHandsOnCast = true;
			_ShowHandMovement = true;

			_DelayedDamage = false;

			_DelayedDamageStacking = true;

			_CastDelayBase = 3;
			_CastDelayCircleScalar = 1;
			_CastDelayFastScalar = 1;
			_CastDelayPerSecond = 4;
			_CastDelayMinimum = 1;

			_CastRecoveryBase = 1;
			_CastRecoveryCircleScalar = 0;
			_CastRecoveryFastScalar = 1;
			_CastRecoveryPerSecond = 3;
			_CastRecoveryMinimum = 0;

            _SpellResistModifier = 3.0;
            _MeditationModifier = 0.40;
		}
	}
}