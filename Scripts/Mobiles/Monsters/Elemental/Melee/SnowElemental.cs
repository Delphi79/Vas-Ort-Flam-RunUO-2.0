using System;
using Server.Items;
using Server.Network;
using System.Collections;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a snow elemental corpse" )]
	public class SnowElemental : BaseCreature
	{
		[Constructable]
		public SnowElemental()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a snow elemental";
			Body = 163;
			BaseSoundID = 263;

			SetStr( 326, 355 );
			SetDex( 166, 185 );
			SetInt( 71, 95 );

			SetHits( 196, 213 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.MagicResist, 50.1, 65.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.Wrestling, 80.1, 100.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 50;

			PackItem( new IronOre( 3 ) );
			PackItem( new BlackPearl( 3 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override bool BleedImmune { get { return true; } }

		public override int TreasureMapLevel { get { return Utility.RandomList( 2, 3 ); } }

		public SnowElemental( Serial serial )
			: base( serial )
		{
		}

		public override void OnDeath( Container c )
		{
			if ( c == null )
				return;

			for ( int i = 0; i < c.Items.Count; ++i )
			{
				c.Items[i].MoveToWorld( Location, Map );
			}

			c.Delete();
			Delete();
		}

		#region Cold Radiation
		private DateTime m_LastRadiated;
		private Hashtable m_Mobiles = new Hashtable();

		protected override bool OnMove( Direction d )
		{
			if ( !IsDeadBondedPet )
			{
				if ( m_LastRadiated <= DateTime.Now )
					m_LastRadiated = DateTime.Now.AddSeconds( Utility.Random( 10 ) );
				IPooledEnumerable eable = GetMobilesInRange( 2 );
				foreach ( Mobile m in eable )
					if ( m_Mobiles[m] == null )
						m_Mobiles[m] = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( RadiationCallBack ), m );
			}

			return base.OnMove( d );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m_LastRadiated <= DateTime.Now )
				m_LastRadiated = DateTime.Now.AddSeconds( Utility.Random( 10 ) );
			if ( !IsDeadBondedPet && m_Mobiles[m] == null && Utility.InRange( Location, m.Location, 2 ) && !Utility.InRange( Location, oldLocation, 2 ) )
				m_Mobiles[m] = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( RadiationCallBack ), m );

			base.OnMovement( m, oldLocation );
		}

		public void RadiationCallBack( object state )
		{
			Mobile m = (Mobile)state;

			if ( Deleted || !Alive || !Utility.InRange( Location, m.Location, 2 ) )
			{
				( (Timer)m_Mobiles[m] ).Stop();
				m_Mobiles[m] = null;
				return;
			}

			if ( this != m && m.AccessLevel == AccessLevel.Player && m_LastRadiated <= DateTime.Now && Server.Spells.SpellHelper.ValidIndirectTarget( m, this ) && CanBeHarmful( m, false, false ) )
			{
				if ( m.NetState != null )
				{
					AOS.Damage( m, this, Utility.Random( 10, 10 ), 0, 100, 0, 0, 0, true );
					m.RevealingAction();
					DoHarmful( m );
					m.NetState.Send( new MessageLocalizedAffix( Serial.MinusOne, -1, MessageType.Label, 0x3C3, 3, 1008111, "", AffixType.Prepend | AffixType.System, m.Name, "" ) );
					m_LastRadiated = DateTime.Now.AddSeconds( Utility.Random( 5, 5 ) );
				}
				else
				{
					return;
				}
			}
		}
		#endregion

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