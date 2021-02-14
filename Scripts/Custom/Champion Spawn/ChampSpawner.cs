using System;
using System.Collections.Generic;
using System.Collections;

using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class MonsterSpawnEntry
	{
		private Type m_Monster;
		private int m_Amount;

		public Type Monster { get { return m_Monster; } set { m_Monster = value; } }
		public int Amount { get { return m_Amount; } set { m_Amount = value; } }

		public MonsterSpawnEntry( Type monster, int amount )
		{
			m_Monster = monster;
			m_Amount = amount;
		}
	}

	public class ChampSpawner : Item
	{
		private bool m_Enabled;
		private bool m_Broadcast;

		private Point3D m_Top;
		private Point3D m_Bottom;

		private List<Mobile> m_Spawned;

		private Timer m_Timer;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Enabled
		{
			get { return m_Enabled; }
			set
			{
				m_Enabled = value;

				if ( m_Enabled )
					Spawn();
				else
					Despawn();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Broadcast { get { return m_Broadcast; } set { m_Broadcast = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Top { get { return m_Top; } set { m_Top = value; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Bottom { get { return m_Bottom; } set { m_Bottom = value; } }

		public List<Mobile> Spawned { get { return m_Spawned; } set { m_Spawned = value; } }

		private MonsterSpawnEntry[] m_Entries = new MonsterSpawnEntry[]
			{
									//Monster							//Amount
				new MonsterSpawnEntry(typeof(BoneKnight),					50),
				new MonsterSpawnEntry(typeof(RatmanArcher),					30),
				new MonsterSpawnEntry(typeof(StoneGargoyle),				20),
				new MonsterSpawnEntry(typeof(StoneHarpy),					30),
				new MonsterSpawnEntry(typeof(Zombie),						30),
				new MonsterSpawnEntry(typeof(OgreLord),						30),
				new MonsterSpawnEntry(typeof(GiantSpider),					50),
				new MonsterSpawnEntry(typeof(LichLord),			            20),
				new MonsterSpawnEntry(typeof(Mummy),						20),
				new MonsterSpawnEntry(typeof(DreadSpider),	                20)
			};

		[Constructable]
		public ChampSpawner() : base( 0xFA6 )
		{
			m_Spawned = new List<Mobile>();

			Visible = false;
			Movable = false;

			Name = "a champion spawner";
		}

		public void Spawn()
		{
			Despawn();

			for ( int i = 0; i < m_Entries.Length; ++i )
				for ( int count = 0; count < m_Entries[i].Amount; ++count )
					AddMonster( m_Entries[i].Monster, false );

			m_Timer = new InternalTimer( this, false );
			m_Timer.Start();
		}

		public void Despawn()
		{
			if ( m_Timer != null )
				m_Timer.Stop();

			for ( int i = 0; i < m_Spawned.Count; ++i )
				if ( m_Spawned[i] != null && !m_Spawned[i].Deleted )
					m_Spawned[i].Delete();

			m_Spawned.Clear();
		}

		public void Restart()
		{
			Despawn();

			//Wait 10 hours before next respawn
			Timer.DelayCall(TimeSpan.FromHours(10.0), new TimerStateCallback(delegate(object state)
			{
				if ( m_Broadcast )
					World.Broadcast( 1161, false, "[System Message]: Attention! The champion spawn has now respawned." );

				Spawn();

			}), null);
		}

		public void AddMonster( Type type, bool champ )
		{
			object monster = Activator.CreateInstance( type );

			if ( monster != null && monster is Mobile )
			{
				int x = Utility.Random( m_Top.X, ( m_Bottom.X - m_Top.X ) );
				int y = Utility.Random( m_Top.Y, ( m_Bottom.Y - m_Top.Y ) );

				Point3D location = new Point3D( x, y, m_Top.Z );

				Mobile from = (Mobile)monster;

				if ( champ && Utility.Random(100) == 50 )
					from.AddItem( new ClothingBlessDeed() );

				from.MoveToWorld( location, this.Map );

				m_Spawned.Add( from );
			}
		}

		public void SpawnChamp()
		{
			Despawn();

			AddMonster( typeof( Mephitis ), true ); //Champion that is spawned

			m_Timer = new InternalTimer( this, true );
			m_Timer.Start();
		}

		public ChampSpawner( Serial serial )
			: base( serial )
		{
		}

		public override void OnDelete()
		{
			Despawn();

			base.OnDelete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); //version

			writer.Write( m_Enabled );
			writer.Write( m_Broadcast );

			writer.Write( m_Top );
			writer.Write( m_Bottom );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Enabled = reader.ReadBool();
						m_Broadcast = reader.ReadBool();

						m_Top = reader.ReadPoint3D();
						m_Bottom = reader.ReadPoint3D();

						break;
					}
			}

			m_Spawned = new List<Mobile>();
		}

		public class InternalTimer : Timer
		{
			ChampSpawner m_Spawner;

			bool m_Champ;
			int lastCount; //Prevent spamming the shard

			public InternalTimer(ChampSpawner spawner, bool champ)
				: base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Spawner = spawner;
				m_Champ = champ;
			}

			protected override void OnTick()
			{
				int count = 0;

				for ( int i = 0; i < m_Spawner.Spawned.Count; ++i )
					if ( m_Spawner.Spawned[i] != null && !m_Spawner.Spawned[i].Deleted && m_Spawner.Spawned[i].Alive )
						++count;

				if ( !m_Champ ) //Monsters
				{
					if ( m_Spawner.Broadcast && lastCount != count && ( count % 50 ) == 0 )
					{
						World.Broadcast( 1161, false, String.Format( "[System Message]: Attention! There are {0} monsters left to kill at the champion spawn.", count ) );
						lastCount = count;
					}

					if ( count == 0 ) //All monsters have been slayed
						m_Spawner.SpawnChamp();
				}
				else //Champion
				{
					if ( count == 0 ) //Champion is dead
					{
						if ( m_Spawner.Broadcast )
							World.Broadcast( 1161, false, "[System Message]: Attention! The champion has been slayed. The monsters will respawn in ten hours." );

						m_Spawner.Restart();
					}
				}
			}
		}
	}
}