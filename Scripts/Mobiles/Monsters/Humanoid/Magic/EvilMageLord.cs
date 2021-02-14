using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an evil mage lord corpse" )]
	public class EvilMageLord : BaseCreature
	{
		[Constructable]
		public EvilMageLord()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 0x190;
			Name = NameList.RandomName( "evil mage lord" );
			SpeechHue = 0x3C3;//Changed from random hue for old school feel...
			HairItemID = Utility.RandomList( 0x2044, 0x2045, 0x203C, 0x203B, 0x203D, 0x2047, 0x2048, 0x2049 );//Male Styles...
			HairHue = Utility.RandomBlueHue();
			Hue = Utility.RandomSkinHue();

			SetStr( 81, 105 );
			SetDex( 191, 215 );
			SetInt( 126, 150 );
			SetHits( 49, 63 );
			SetDamage( 5, 10 );

			SetSkill( SkillName.EvalInt, 80.2, 100.0 );
			SetSkill( SkillName.Magery, 95.1, 100.0 );
			SetSkill( SkillName.Meditation, 27.5, 50.0 );
			SetSkill( SkillName.MagicResist, 77.5, 100.0 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.Wrestling, 20.3, 80.0 );

			Fame = 10500;
			Karma = -10500;

			VirtualArmor = 16;
			PackReg( 23 );

			AddItem( new Robe( 1308 ) );

			if ( Utility.RandomBool() )
				AddItem( new Shoes( Utility.RandomBlueHue() ) );
			else
				AddItem( new Sandals( Utility.RandomBlueHue() ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override bool CanRummageCorpses { get { return true; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override int Meat { get { return 1; } }
		public override int TreasureMapLevel { get { return 3; } }

		public EvilMageLord( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}