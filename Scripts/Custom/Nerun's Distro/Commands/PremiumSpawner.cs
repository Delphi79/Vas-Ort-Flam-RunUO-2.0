// By David
// SpawnID added by Nerun
using System;
using System.IO;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class PremiumSpawner : Item
	{
		private int m_Team;
		private int m_HomeRange;
		private int m_SpawnRange;
// ---------------------
		private int m_SpawnID;
// ---------------------
		private int m_Count;
		private TimeSpan m_MinDelay;
		private TimeSpan m_MaxDelay;
		private ArrayList m_CreaturesName;
		private ArrayList m_Creatures;
		private DateTime m_End;
		private InternalTimer m_Timer;
		private bool m_Running;
		private bool m_Group;
		private WayPoint m_WayPoint;

		public bool IsFull{ get{ return ( m_Creatures != null && m_Creatures.Count >= m_Count ); } }
		
		public ArrayList CreaturesName
		{
			get { return m_CreaturesName; }
			set
			{
				m_CreaturesName = value;
				if ( m_CreaturesName.Count < 1 )
					Stop();

				InvalidateProperties();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Count
		{
			get { return m_Count; }
			set { m_Count = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public WayPoint WayPoint
		{
			get
			{
				return m_WayPoint;
			}
			set
			{
				m_WayPoint = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Running
		{
			get { return m_Running; }
			set
			{
				if ( value )
					Start();
				else
					Stop();

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int HomeRange
		{
			get { return m_HomeRange; }
			set { m_HomeRange = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int SpawnRange
		{
			get { return m_SpawnRange; }
			set { m_SpawnRange = ((value > m_HomeRange)? m_HomeRange : value); }
		}

// ---------------------
		[CommandProperty( AccessLevel.GameMaster )]
		public int SpawnID
		{
			get { return m_SpawnID; }
			set { m_SpawnID = value; InvalidateProperties(); }
		}
// ---------------------

		[CommandProperty( AccessLevel.GameMaster )]
		public int Team
		{
			get { return m_Team; }
			set { m_Team = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan MinDelay
		{
			get { return m_MinDelay; }
			set { m_MinDelay = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan MaxDelay
		{
			get { return m_MaxDelay; }
			set { m_MaxDelay = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public TimeSpan NextSpawn
		{
			get
			{
				if ( m_Running )
					return m_End - DateTime.Now;
				else
					return TimeSpan.FromSeconds( 0 );
			}
			set
			{
				Start();
				DoTimer( value );
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Group
		{
			get { return m_Group; }
			set { m_Group = value; InvalidateProperties(); }
		}

		[Constructable]
		public PremiumSpawner( int amount, int minDelay, int maxDelay, int team, int homeRange, string creatureName ) : base( 0x1f13 )
		{
			ArrayList creaturesName = new ArrayList();
			creaturesName.Add( creatureName.ToLower() );
// ---------------------
// homeRange
			InitSpawn( amount, TimeSpan.FromMinutes( minDelay ), TimeSpan.FromMinutes( maxDelay ), team, homeRange, homeRange, homeRange, creaturesName );
		}

		[Constructable]
// int spawnID
		public PremiumSpawner( int amount, int minDelay, int maxDelay, int team, int homeRange, int spawnRange, int spawnID, string creatureName ) : base( 0x1f13 )
		{
			ArrayList creaturesName = new ArrayList();
			creaturesName.Add( creatureName.ToLower() );
// spawnID
			InitSpawn( amount, TimeSpan.FromMinutes( minDelay ), TimeSpan.FromMinutes( maxDelay ), team, homeRange, spawnRange, spawnID, creaturesName );
		}

		[Constructable]
		public PremiumSpawner( string creatureName ) : base( 0x1f13 )
		{
			ArrayList creaturesName = new ArrayList();
			creaturesName.Add( creatureName.ToLower() );
// 5-10 min, 1
			InitSpawn( 1, TimeSpan.FromMinutes( 5 ), TimeSpan.FromMinutes( 10 ), 0, 4, 4, 1, creaturesName );
		}

		[Constructable]
		public PremiumSpawner() : base( 0x1f13 )
		{
			ArrayList creaturesName = new ArrayList();
// 5-10 min, 1
			InitSpawn( 1, TimeSpan.FromMinutes( 5 ), TimeSpan.FromMinutes( 10 ), 0, 4, 4, 1, creaturesName );
		}

		public PremiumSpawner( int amount, TimeSpan minDelay, TimeSpan maxDelay, int team, int homeRange, ArrayList creaturesName ) : base( 0x1f13 )
		{
// homeRange
			InitSpawn( amount, minDelay, maxDelay, team, homeRange, homeRange, homeRange, creaturesName );
		}

// int spawnID
		public PremiumSpawner( int amount, TimeSpan minDelay, TimeSpan maxDelay, int team, int homeRange, int spawnRange, int spawnID, ArrayList creaturesName ) : base( 0x1f13 )
		{
// spawnID
			InitSpawn( amount, minDelay, maxDelay, team, homeRange, spawnRange, spawnID, creaturesName );
		}
//int spawnID
		public void InitSpawn( int amount, TimeSpan minDelay, TimeSpan maxDelay, int team, int homeRange, int spawnRange, int spawnID, ArrayList creaturesName )
		{
			Visible = false;
			Movable = false;
			m_Running = true;
			m_Group = false;
			Name = "Premium Spawner";
			m_MinDelay = minDelay;
			m_MaxDelay = maxDelay;
			m_Count = amount;
			m_Team = team;
			m_HomeRange = homeRange;
			m_SpawnRange = ((spawnRange > homeRange)? homeRange : spawnRange);
// added
			m_SpawnID = spawnID;
// --------------------
			m_CreaturesName = creaturesName;
			m_Creatures = new ArrayList();
			DoTimer( TimeSpan.FromSeconds( 1 ) );
		}
			
		public PremiumSpawner( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel < AccessLevel.GameMaster )
				return;

			PremiumSpawnerGump g = new PremiumSpawnerGump( this );
			from.SendGump( g );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Running )
			{
				list.Add( 1060742 );

				list.Add( 1060656, m_Count.ToString() );
				list.Add( 1061169, m_HomeRange.ToString() );
				list.Add( 1060658, "group\t{0}", m_Group );
				list.Add( 1060659, "team\t{0}", m_Team );
				list.Add( 1060660, "speed\t{0} to {1}", m_MinDelay, m_MaxDelay );

				for ( int i = 0; i < 3 && i < m_CreaturesName.Count; ++i )
					list.Add( 1060661 + i, "{0}\t{1}", m_CreaturesName[i], CountCreatures( (string)m_CreaturesName[i] ) );
			}
			else
			{
				list.Add( 1060743 ); // inactive
			}
		}

		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );

			if ( m_Running )
				LabelTo( from, "[Running]" );
			else
				LabelTo( from, "[Off]" );
		}

		public void Start()
		{
			if ( !m_Running )
			{
				if ( m_CreaturesName.Count > 0 )
				{
					m_Running = true;
					DoTimer();
				}
			}
		}

		public void Stop()
		{
			if ( m_Running )
			{
				m_Timer.Stop();
				m_Running = false;
			}
		}

		public void Defrag()
		{
			bool removed = false;

			for ( int i = 0; i < m_Creatures.Count; ++i )
			{
				object o = m_Creatures[i];

				if ( o is Item )
				{
					Item item = (Item)o;

					if ( item.Deleted || item.Parent != null )
					{
						m_Creatures.RemoveAt( i );
						--i;
						removed = true;
					}
				}
				else if ( o is Mobile )
				{
					Mobile m = (Mobile)o;

					if ( m.Deleted )
					{
						m_Creatures.RemoveAt( i );
						--i;
						removed = true;
					}
					else if ( m is BaseCreature )
					{
						if ( ((BaseCreature)m).Controlled || ((BaseCreature)m).IsStabled )
						{
							m_Creatures.RemoveAt( i );
							--i;
							removed = true;
						}
					}
				}
				else
				{
					m_Creatures.RemoveAt( i );
					--i;
					removed = true;
				}
			}

			if ( removed )
				InvalidateProperties();
		}

		public void OnTick()
		{
			DoTimer();

			if ( m_Group )
			{
				Defrag();

				if  ( m_Creatures.Count == 0 )
				{
					Respawn();
				}
				else
				{
					return;
				}
			}
			else
			{
				Spawn();
			}
		}
		
		public void Respawn()
		{
			RemoveCreatures();

			for ( int i = 0; i < m_Count; i++ )
				Spawn();
		}
		
		public void Spawn()
		{
			if ( m_CreaturesName.Count > 0 )
				Spawn( Utility.Random( m_CreaturesName.Count ) );
		}
		
		public void Spawn( string creatureName )
		{
			for ( int i = 0; i < m_CreaturesName.Count; i++ )
			{
				if ( (string)m_CreaturesName[i] == creatureName )
				{
					Spawn( i );
					break;
				}
			}
		}

		public void Spawn( int index )
		{
			Map map = Map;

			if ( map == null || map == Map.Internal || m_CreaturesName.Count == 0 || index >= m_CreaturesName.Count )
				return;

			Defrag();

			if ( m_Creatures.Count >= m_Count )
				return;

			Type type = SpawnerType.GetType( (string)m_CreaturesName[index] );

			if ( type != null )
			{
				try
				{
					object o = Activator.CreateInstance( type );

					if ( o is Mobile )
					{
						Mobile m = (Mobile)o;

						m_Creatures.Add( m );
						InvalidateProperties();
// Base Vendors now spawn at any position (maps set SpawnRange to 0)
// this will spawn WanderingHealer at any point too (spawnrange = homerange)
						Point3D loc = ( /*m is BaseVendor ? this.Location :*/ GetSpawnPosition() );

						m.MoveToWorld( loc, map );

						if ( m is BaseCreature )
						{
							BaseCreature c = (BaseCreature)m;

							c.RangeHome = m_HomeRange;

							c.CurrentWayPoint = m_WayPoint;

							if ( m_Team > 0 )
								c.Team = m_Team;

							c.Home = this.Location;
						}
					}
					else if ( o is Item )
					{
						Item item = (Item)o;

						m_Creatures.Add( item );
						InvalidateProperties();

						item.MoveToWorld( GetSpawnPosition(), map );
					}
				}
				catch
				{
				}
			}
		}

		public Point3D GetSpawnPosition()
		{
			Map map = Map;

			if ( map == null )
				return Location;

			// Try 10 times to find a Spawnable location.
			for ( int i = 0; i < 10; i++ )
			{
				int x = Location.X + (Utility.Random( (m_SpawnRange * 2) + 1 ) - m_SpawnRange);
				int y = Location.Y + (Utility.Random( (m_SpawnRange * 2) + 1 ) - m_SpawnRange);
				int z = Map.GetAverageZ( x, y );

				if ( Map.CanSpawnMobile( new Point2D( x, y ), this.Z ) )
					return new Point3D( x, y, this.Z );
				else if ( Map.CanSpawnMobile( new Point2D( x, y ), z ) )
					return new Point3D( x, y, z );
			}

			return this.Location;
		}

		public void DoTimer()
		{
			if ( !m_Running )
				return;

			int minSeconds = (int)m_MinDelay.TotalSeconds;
			int maxSeconds = (int)m_MaxDelay.TotalSeconds;

			TimeSpan delay = TimeSpan.FromSeconds( Utility.RandomMinMax( minSeconds, maxSeconds ) );
			DoTimer( delay );
		}

		public void DoTimer( TimeSpan delay )
		{
			if ( !m_Running )
				return;

			m_End = DateTime.Now + delay;

			if ( m_Timer != null )
				m_Timer.Stop();

			m_Timer = new InternalTimer( this, delay );
			m_Timer.Start();
		}

		private class InternalTimer : Timer
		{
			private PremiumSpawner m_Spawner;

			public InternalTimer( PremiumSpawner spawner, TimeSpan delay ) : base( delay )
			{
				if ( spawner.IsFull )
					Priority = TimerPriority.FiveSeconds;
				else
					Priority = TimerPriority.OneSecond;

				m_Spawner = spawner;
			}

			protected override void OnTick()
			{
				if ( m_Spawner != null )
					if ( !m_Spawner.Deleted )
						m_Spawner.OnTick();
			}
		}

		public int CountCreatures( string creatureName )
		{
			Defrag();

			int count = 0;

			for ( int i = 0; i < m_Creatures.Count; ++i )
				if ( Insensitive.Equals( creatureName, m_Creatures[i].GetType().Name ) )
					++count;

			return count;
		}

		public void RemoveCreatures( string creatureName )
		{
			Defrag();

			creatureName = creatureName.ToLower();

			for ( int i = 0; i < m_Creatures.Count; ++i )
			{
				object o = m_Creatures[i];

				if ( Insensitive.Equals( creatureName, o.GetType().Name ) )
				{
					if ( o is Item )
						((Item)o).Delete();
					else if ( o is Mobile )
						((Mobile)o).Delete();
				}
			}

			InvalidateProperties();
		}
		
		public void RemoveCreatures()
		{
			Defrag();

			for ( int i = 0; i < m_Creatures.Count; ++i )
			{
				object o = m_Creatures[i];

				if ( o is Item )
					((Item)o).Delete();
				else if ( o is Mobile )
					((Mobile)o).Delete();
			}

			InvalidateProperties();
		}
		
		public void BringToHome()
		{
			Defrag();

			for ( int i = 0; i < m_Creatures.Count; ++i )
			{
				object o = m_Creatures[i];

				if ( o is Mobile )
				{
					Mobile m = (Mobile)o;

					m.MoveToWorld( Location, Map );

				}
				else if ( o is Item )
				{
					Item item = (Item)o;

					item.MoveToWorld( Location, Map );
				}
			}
		}

		public override void OnDelete()
		{
			base.OnDelete();

			RemoveCreatures();
			if ( m_Timer != null )
				m_Timer.Stop();
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			writer.Write( m_SpawnRange );

// -------------------------
			writer.Write( m_SpawnID );
// -------------------------

			writer.Write( m_WayPoint );

			writer.Write( m_Group );

			writer.Write( m_MinDelay );
			writer.Write( m_MaxDelay );
			writer.Write( m_Count );
			writer.Write( m_Team );
			writer.Write( m_HomeRange );
			writer.Write( m_Running );
			
			if ( m_Running )
				writer.WriteDeltaTime( m_End );

			writer.Write( m_CreaturesName.Count );

			for ( int i = 0; i < m_CreaturesName.Count; ++i )
				writer.Write( (string)m_CreaturesName[i] );

			writer.Write( m_Creatures.Count );

			for ( int i = 0; i < m_Creatures.Count; ++i )
			{
				object o = m_Creatures[i];

				if ( o is Item )
					writer.Write( (Item)o );
				else if ( o is Mobile )
					writer.Write( (Mobile)o );
				else
					writer.Write( Serial.MinusOne );
			}
		}

		private static WarnTimer m_WarnTimer;

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				{
					m_SpawnRange = reader.ReadInt();
// ----------------------------------------
					m_SpawnID = reader.ReadInt();
// ----------------------------------------

					goto case 2;
				}

				case 2:
				{
					m_WayPoint = reader.ReadItem() as WayPoint;

					goto case 1;
				}

				case 1:
				{
					m_Group = reader.ReadBool();
					
					goto case 0;
				}

				case 0:
				{
					m_MinDelay = reader.ReadTimeSpan();
					m_MaxDelay = reader.ReadTimeSpan();
					m_Count = reader.ReadInt();
					m_Team = reader.ReadInt();
					m_HomeRange = reader.ReadInt();
					m_Running = reader.ReadBool();

					TimeSpan ts = TimeSpan.Zero;

					if ( m_Running )
						ts = reader.ReadDeltaTime() - DateTime.Now;
					
					int size = reader.ReadInt();

					m_CreaturesName = new ArrayList( size );

					for ( int i = 0; i < size; ++i )
					{
						string typeName = reader.ReadString();

						m_CreaturesName.Add( typeName );

						if ( SpawnerType.GetType( typeName ) == null )
						{
							if ( m_WarnTimer == null )
								m_WarnTimer = new WarnTimer();

							m_WarnTimer.Add( Location, Map, typeName );
						}
					}

					int count = reader.ReadInt();

					m_Creatures = new ArrayList( count );

					for ( int i = 0; i < count; ++i )
					{
						IEntity e = World.FindEntity( reader.ReadInt() );

						if ( e != null )
							m_Creatures.Add( e );
					}

					if ( m_Running )
						DoTimer( ts );

					break;
				}
			}

			m_SpawnRange = ( version <= 2 ? m_HomeRange : m_SpawnRange ); //fix SpawnRange until first Deserialize of ver 3
		}

		private class WarnTimer : Timer
		{
			private ArrayList m_List;

			private class WarnEntry
			{
				public Point3D m_Point;
				public Map m_Map;
				public string m_Name;

				public WarnEntry( Point3D p, Map map, string name )
				{
					m_Point = p;
					m_Map = map;
					m_Name = name;
				}
			}

			public WarnTimer() : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_List = new ArrayList();
				Start();
			}

			public void Add( Point3D p, Map map, string name )
			{
				m_List.Add( new WarnEntry( p, map, name ) );
			}

			protected override void OnTick()
			{
				try
				{
					Console.WriteLine( "Warning: {0} bad spawns detected, logged: 'badspawn.log'", m_List.Count );

					using ( StreamWriter op = new StreamWriter( "badspawn.log", true ) )
					{
						op.WriteLine( "# Bad spawns : {0}", DateTime.Now );
						op.WriteLine( "# Format: X Y Z F Name" );
						op.WriteLine();

						foreach ( WarnEntry e in m_List )
							op.WriteLine( "{0}\t{1}\t{2}\t{3}\t{4}", e.m_Point.X, e.m_Point.Y, e.m_Point.Z, e.m_Map, e.m_Name );

						op.WriteLine();
						op.WriteLine();
					}
				}
				catch
				{
				}
			}
		}
	}
}