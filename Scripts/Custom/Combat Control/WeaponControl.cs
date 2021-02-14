using System;
using System.Text;
using System.Collections.Generic;

using Server;
using Server.Items;
using Server.Gumps;
using Server.Commands;

namespace Server.Items
{
	public class WeaponControl
	{
		private static string _AxeDamage = "3d10+3";
		private static string _BattleAxe = "2d17+4";
		private static string _DoubleAxe = "1d31+4";
		private static string _ExecutionersAxe = "3d10+3";
		private static string _Hatchet = "1d16+1";
		private static string _LargeBattleAxe = "2d17+4";
		private static string _TwoHandedAxe = "2d18+3";
		private static string _WarAxe = "6d4+3";

		[CommandProperty(AccessLevel.Administrator)]
		public static string AxeDamage { get { return _AxeDamage; } set { _AxeDamage = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string BattleAxeDamage { get { return _BattleAxe; } set { _BattleAxe = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string DoubleAxeDamage { get { return _DoubleAxe; } set { _DoubleAxe = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string ExecutionersAxeDamage { get { return _ExecutionersAxe; } set { _ExecutionersAxe = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string HatchetDamage { get { return _Hatchet; } set { _Hatchet = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string LargeBattleAxeDamage { get { return _LargeBattleAxe; } set { _LargeBattleAxe = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string TwoHandedAxeDamage { get { return _TwoHandedAxe; } set { _TwoHandedAxe = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string WarAxeDamage { get { return _WarAxe; } set { _WarAxe = value; } }

		private static string _ButcherKnife = "2d7+0";
		private static string _Cleaver = "1d12+1";
		private static string _Dagger = "3d5+0";
		private static string _SkinningKnife = "1d10+0";

		[CommandProperty(AccessLevel.Administrator)]
		public static string ButcherKnifeDamage { get { return _ButcherKnife; } set { _ButcherKnife = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string CleaverDamage { get { return _Cleaver; } set { _Cleaver = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string DaggerDamage { get { return _Dagger; } set { _Dagger = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string SkinningKnifeDamage { get { return _SkinningKnife; } set { _SkinningKnife = value; } }

		private static string _Club = "4d5+4";
		private static string _FireworksWand = "2d3+0";
		private static string _HammerPick = "3d10+3";
		private static string _Mace = "6d5+2";
        private static string _MagicWand = "2d3+0";
		private static string _Maul = "5d5+5";
		private static string _Scepter = "3d6+2";
		private static string _WarHammer = "7d5+1";
		private static string _WarMace = "5d5+5";

		[CommandProperty(AccessLevel.Administrator)]
		public static string ClubDamage { get { return _Club; } set { _Club = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string FireworksWandDamage { get { return _FireworksWand; } set { _FireworksWand = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string HammerPick { get { return _HammerPick; } set { _HammerPick = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string MaceDamage { get { return _Mace; } set { _Mace = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string MagicWandDamage { get { return _MagicWand; } set { _MagicWand = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string MaulDamage { get { return _Maul; } set { _Maul = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string ScepterDamage { get { return _Scepter; } set { _Scepter = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string WarHammerDamage { get { return _WarHammer; } set { _WarHammer = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string WarMaceDamage { get { return _WarMace; } set { _WarMace = value; } }

		private static string _Bardiche = "2d20+3";
		private static string _Halberd = "2d23+3";
		private static string _Scythe = "2d9+3";

		[CommandProperty(AccessLevel.Administrator)]
		public static string BardicheDamage { get { return _Bardiche; } set { _Bardiche = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string HalberdDamage { get { return _Halberd; } set { _Halberd = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string ScytheDamage { get { return _Scythe; } set { _Scythe = value; } }

		private static string _Bow = "4d9+5";
		private static string _JukaBow = "2d12+5";
		private static string _CompositeBow = "2d9+4";
		private static string _Crossbow = "5d8+3";
		private static string _HeavyCrossbow = "5d10+6";
		private static string _RepeatingCrossbow = "1d12+4";

		[CommandProperty(AccessLevel.Administrator)]
		public static string BowDamage { get { return _Bow; } set { _Bow = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string JukaBowDamage { get { return _JukaBow; } set { _JukaBow = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string CompositeBowDamage { get { return _CompositeBow; } set { _CompositeBow = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string CrossbowDamage { get { return _Crossbow; } set { _Crossbow = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string HeavyCrossbowDamage { get { return _HeavyCrossbow; } set { _HeavyCrossbow = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string RepeatingCrossbowDamage { get { return _RepeatingCrossbow; } set { _RepeatingCrossbow = value; } }

		private static string _BladedStaff = "1d17+8";
		private static string _DoubleBladedStaff = "2d15+7";
		private static string _Pike = "1d15+5";
		private static string _Pitchfork = "4d4+0";
		private static string _ShortSpear = "2d15+2";
		private static string _Spear = "2d18+0";
		private static string _TribalSpear = "4d6+2";
		private static string _WarFork = "1d29+3";

		[CommandProperty(AccessLevel.Administrator)]
		public static string BladedStaffDamage { get { return _BladedStaff; } set { _BladedStaff = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string DoubleBladedStaffDamage { get { return _DoubleBladedStaff; } set { _DoubleBladedStaff = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string PikeDamage { get { return _Pike; } set { _Pike = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string PitchforkDamage { get { return _Pitchfork; } set { _Pitchfork = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string ShortSpearDamage { get { return _ShortSpear; } set { _ShortSpear = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string SpearDamage { get { return _Spear; } set { _Spear = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string TribalSpearDamage { get { return _TribalSpear; } set { _TribalSpear = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string WarForkDamage { get { return _WarFork; } set { _WarFork = value; } }

		private static string _BlackStaff = "5d6+3";
		private static string _GlacierStaff = "4d4+5";
		private static string _GnarledStaff = "5d5+5";
		private static string _QuarterStaff = "5d5+3";
		private static string _ShepherdsCrook = "3d4+0";

		[CommandProperty(AccessLevel.Administrator)]
		public static string BlackStaffDamage { get { return _BlackStaff; } set { _BlackStaff = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string GlacierStaffDamage { get { return _GlacierStaff; } set { _GlacierStaff = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string GnarledStaffDamage { get { return _GnarledStaff; } set { _GnarledStaff = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string QuarterStaffDamage { get { return _QuarterStaff; } set { _QuarterStaff = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string ShepherdsCrookDamage { get { return _ShepherdsCrook; } set { _ShepherdsCrook = value; } }

		private static string _BoneHarvester = "4d5+3";
		private static string _Broadsword = "2d13+3";
		private static string _CrescentBlade = "4d5+3";
		private static string _Cutlass = "2d12+4";
		private static string _Katana = "3d8+2";
		private static string _Kryss = "1d26+2";
		private static string _Lance = "4d5+3";
		private static string _Longsword = "2d15+3";
		private static string _Scimitar = "2d14+2";
		private static string _ThinLongsword = "2d15+3";
		private static string _VikingSword = "4d8+2";

		[CommandProperty(AccessLevel.Administrator)]
		public static string BoneHarvesterDamage { get { return _BoneHarvester; } set { _BoneHarvester = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string BroadswordDamage { get { return _Broadsword; } set { _Broadsword = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string CrescentBladeDamage { get { return _CrescentBlade; } set { _CrescentBlade = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string CutlassDamage { get { return _Cutlass; } set { _Cutlass = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string KatanaDamage { get { return _Katana; } set { _Katana = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string KryssDamage { get { return _Kryss; } set { _Kryss = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string LanceDamage { get { return _Lance; } set { _Lance = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string LongswordDamage { get { return _Longsword; } set { _Longsword = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string ScimitarDamage { get { return _Scimitar; } set { _Scimitar = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string ThinLongswordDamage { get { return _ThinLongsword; } set { _ThinLongsword = value; } }
		[CommandProperty(AccessLevel.Administrator)]
		public static string VikingSwordDamage { get { return _VikingSword; } set { _VikingSword = value; } }

		private static WeaponControl _Instance = new WeaponControl();
		public static WeaponControl Instance { get { return _Instance; } }

		public static int GetDamage(string dice)
		{
			string[] d = dice.Split(new char[] { 'd', '+' });

            if (d.Length < 3)
            {
                RestoreDefaults();
                return 1;
            }

			return Utility.Dice(Int32.Parse(d[0]), Int32.Parse(d[1]), Int32.Parse(d[2]));
		}



		public static void Initialize()
		{
			CommandSystem.Register("WeaponControl", AccessLevel.Administrator, new CommandEventHandler(WeaponControl_OnCommand));
			EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);

			Load();
		}

		private static void Load()
		{

		}

		private static void EventSink_WorldSave(WorldSaveEventArgs e)
		{

		}

		[Usage("WeaponControl")]
		[Description("Displays a gump to change weapon damages on a global scale.")]
		public static void WeaponControl_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;

			if (m == null)
				return;

			if (e.ArgString.IndexOf("restore") != -1)
			{
				RestoreDefaults();
				return;
			}

			m.SendGump(new PropertiesGump(m, WeaponControl.Instance));
		}

		private static void Serialize(GenericWriter writer)
		{

		}

		public static void RestoreDefaults()
		{
			_AxeDamage = "3d8+3";
			_BattleAxe = "2d16+4";
			_DoubleAxe = "1d28+3";
			_ExecutionersAxe = "1d16+1";
			_Hatchet = "2d16+2";
			_LargeBattleAxe = "1d15+0";
			_TwoHandedAxe = "2d15+3";
			_WarAxe = "6d4+3";

			_ButcherKnife = "1d7+0";
			_Cleaver = "1d8+1";
			_Dagger = "2d5+0";
			_SkinningKnife = "1d5+0";

			_Club = "3d6+2";
			_FireworksWand = "1d4+0";
			_HammerPick = "3d10+3";
			_Mace = "5d5+2";
			_MagicWand = "1d4+0";
			_Maul = "4d5+5";
			_Scepter = "3d6+2";
			_WarHammer = "5d8+2";
			_WarMace = "5d5+5";

			_Bardiche = "2d20+3";
			_Halberd = "2d23+3";
			_Scythe = "2d9+3";

			_Bow = "2d12+3";
			_JukaBow = "2d12+5";
			_CompositeBow = "2d9+4";
			_Crossbow = "3d10+3";
			_HeavyCrossbow = "3d12+10";
			_RepeatingCrossbow = "1d12+4";

			_BladedStaff = "1d17+8";
			_DoubleBladedStaff = "2d15+7";
			_Pike = "1d15+5";
			_Pitchfork = "4d4+0";
			_ShortSpear = "2d13+2";
			_Spear = "4d5+5";
			_TribalSpear = "4d6+2";
			_WarFork = "2d10+3";

			_BlackStaff = "3d3+3";
			_GlacierStaff = "4d4+5";
			_GnarledStaff = "4d4+5";
			_QuarterStaff = "5d4+3";
			_ShepherdsCrook = "3d4+0";

			_BoneHarvester = "4d5+3";
			_Broadsword = "2d11+3";
			_CrescentBlade = "4d5+3";
			_Cutlass = "2d12+2";
			_Katana = "2d10+2";
			_Kryss = "2d10+2";
			_Lance = "4d5+3";
			_Longsword = "2d13+3";
			_Scimitar = "2d12+2";
			_ThinLongsword = "2d13+3";
			_VikingSword = "3d8+2";
		}
	}
}