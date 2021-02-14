// Premium Spawner

using System;
using System.Collections;
using System.IO;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Network;

namespace Server
{
	public class SpawnGenerator
	{
		private static int m_Count;
		private static int m_MapOverride = -1;
		private static int m_IDOverride = -1;
		private static double m_MinTimeOverride = -1;
		private static double m_MaxTimeOverride = -1;
		private const bool TotalRespawn = true;
		private const int Team = 0;

		public static void Initialize()
		{
			CommandSystem.Register( "SpawnGen", AccessLevel.Administrator, new CommandEventHandler( SpawnGen_OnCommand ) );
		}

		[Usage( "SpawnGen [<filename>]|[unload <id>]|[remove <region>|<rect>]|[save <region>|<rect>]" )]
		[Description( "Generates spawners from Data/Monsters/*.map" )]
		private static void SpawnGen_OnCommand( CommandEventArgs e )
		{
			if ( e.ArgString == null || e.ArgString == "" )
			{
				e.Mobile.SendMessage( "Usage: SpawnGen [<filename>]|[remove <region>|<rect>|<ID>]|[save <region>|<rect>|<ID>]" );
			}
			else if ( e.Arguments[0].ToLower() == "remove" && e.Arguments.Length == 2 )
			{
				Remove( e.Mobile, e.Arguments[1].ToLower() );
			}
			else if ( e.Arguments[0].ToLower() == "remove" && e.Arguments.Length == 5 )
			{
				int x1 = Utility.ToInt32( e.Arguments[1] );
				int y1 = Utility.ToInt32( e.Arguments[2] );
				int x2 = Utility.ToInt32( e.Arguments[3] );
				int y2 = Utility.ToInt32( e.Arguments[4] );
				Remove( e.Mobile, x1, y1, x2, y2 );
			}
			else if ( e.ArgString.ToLower() == "remove" )
			{
				Remove( e.Mobile, ""  );
			}
			else if ( e.Arguments[0].ToLower() == "save" && e.Arguments.Length == 2 )
			{
				Save( e.Mobile, e.Arguments[1].ToLower() );
			}
			else if ( e.Arguments[0].ToLower() == "unload" && e.Arguments.Length == 2 )
			{
				int ID = Utility.ToInt32( e.Arguments[1] );
				Remove( ID );
			}
			else if ( e.Arguments[0].ToLower() == "savebyhand" )
			{
				SaveByHand();
			}
			else if ( e.Arguments[0].ToLower() == "save" && e.Arguments.Length == 5 )
			{
				int x1 = Utility.ToInt32( e.Arguments[1] );
				int y1 = Utility.ToInt32( e.Arguments[2] );
				int x2 = Utility.ToInt32( e.Arguments[3] );
				int y2 = Utility.ToInt32( e.Arguments[4] );
				Save( e.Mobile, x1, y1, x2, y2 );
			}
			else if ( e.ArgString.ToLower() == "save" )
			{
				Save( e.Mobile, "" );
			}
			else
			{
				Parse( e.Mobile, e.ArgString );
			}
		}

		private static void Remove( Mobile from, string region )
		{
			World.Broadcast( 0x35, true, "Spawns are being removed, please wait." );
			DateTime startTime = DateTime.Now;
			int count = 0;
			ArrayList itemsremove = new ArrayList();

			foreach ( Item itemremove in World.Items.Values )
			{ 
				if ( itemremove is PremiumSpawner && ( region == null || region == "" ) )
				{
					itemsremove.Add( itemremove );
					count +=1;
				}
				else if ( itemremove is PremiumSpawner && ( ( (Region)Region.Find( itemremove.Location, itemremove.Map ) ).Name.ToLower() == region && itemremove.Map == from.Map ) )
				{
					itemsremove.Add( itemremove );
					count +=1;
				}
			}

			foreach ( Item itemremove2 in itemsremove )
			{
				itemremove2.Delete();
			}

			DateTime endTime = DateTime.Now;
			World.Broadcast( 0x35, true, "{0} spawns have been removed. The entire process took {1:F1} seconds.", count, (endTime - startTime).TotalSeconds );
		}

		private static void Save( Mobile from, string region )
		{
			World.Broadcast( 0x35, true, "Spawns are being saved, please wait." );
			DateTime startTime = DateTime.Now;
			int count = 0;
			ArrayList itemssave = new ArrayList();

			foreach ( Item itemsave in World.Items.Values )
			{ 
				if ( itemsave is PremiumSpawner && ( region == null || region == "" ) )
				{
					itemssave.Add( itemsave );
					count +=1;
				}
				else if ( itemsave is PremiumSpawner && ( ( (Region)Region.Find( itemsave.Location, itemsave.Map ) ).Name.ToLower() == region && itemsave.Map == from.Map ) )
				{
					itemssave.Add( itemsave );
					count +=1;
				}
			}

			if ( !Directory.Exists( "Data/Monsters" ) )
				Directory.CreateDirectory( "Data/Monsters" );


			using ( StreamWriter op = new StreamWriter( "Data/Monsters/spawns.map" ) )
			{
				foreach ( PremiumSpawner itemsave2 in itemssave )
				{
					int mapnumber = 0;
					switch ( itemsave2.Map.ToString() )
					{
						case "Felucca":
							mapnumber = 1;
							break;
						case "Trammel":
							mapnumber = 2;
							break;
						case "Ilshenar":
							mapnumber = 3;
							break;
						case "Malas":
							mapnumber = 4;
							break;
						case "Tokuno":
							mapnumber = 5;
							break;
						default:
							mapnumber = 6;
							Console.WriteLine( "Monster Parser: Warning, unknown map {0}", itemsave2.Map );
							break;
					}

					string timer1a = itemsave2.MinDelay.ToString();
					string[] timer1b = timer1a.Split( ':' );
					int timer1c = ( Utility.ToInt32( timer1b[0] ) * 60 ) + Utility.ToInt32( timer1b[1] );

					string timer2a = itemsave2.MaxDelay.ToString();
					string[] timer2b = timer2a.Split( ':' );
					int timer2c = ( Utility.ToInt32( timer2b[0] ) * 60 ) + Utility.ToInt32( timer2b[1] );

					string towrite = "* " + itemsave2.CreaturesName[0];

					for ( int i = 1; i < itemsave2.CreaturesName.Count; ++i )
					{
						towrite = towrite + ":" + itemsave2.CreaturesName[i].ToString();
					}

					op.WriteLine( towrite + " {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", itemsave2.X, itemsave2.Y, itemsave2.Z, mapnumber, timer1c, timer2c, itemsave2.HomeRange, itemsave2.SpawnRange, itemsave2.SpawnID, itemsave2.Count );
				}
			}

			DateTime endTime = DateTime.Now;
			World.Broadcast( 0x35, true, "{0} spawns have been saved. The entire process took {1:F1} seconds.", count, (endTime - startTime).TotalSeconds );
		}

		private static void Remove( int ID )
		{
			World.Broadcast( 0x35, true, "Spawns are being removed, please wait." );
			DateTime startTime = DateTime.Now;
			int count = 0;
			ArrayList itemsremove = new ArrayList();

			foreach ( Item itemremove in World.Items.Values )
			{ 
				if ( itemremove is PremiumSpawner && ((PremiumSpawner)itemremove).SpawnID == ID )
				{
					itemsremove.Add( itemremove );
					count +=1;
				}
			}

			foreach ( Item itemremove2 in itemsremove )
			{
				itemremove2.Delete();
			}

			DateTime endTime = DateTime.Now;
			World.Broadcast( 0x35, true, "{0} spawns have been removed in {1:F1} seconds.", count, (endTime - startTime).TotalSeconds );
		}

		private static void SaveByHand()
		{
			World.Broadcast( 0x35, true, "Spawns are being saved, please wait." );
			DateTime startTime = DateTime.Now;
			int count = 0;
			ArrayList itemssave = new ArrayList();

			foreach ( Item itemsave in World.Items.Values )
			{ 
				if ( itemsave is PremiumSpawner && ((PremiumSpawner)itemsave).SpawnID == 1 )
				{
					itemssave.Add( itemsave );
					count +=1;
				}
			}

			if ( !Directory.Exists( "Data/Monsters" ) )
				Directory.CreateDirectory( "Data/Monsters" );

			using ( StreamWriter op = new StreamWriter( "Data/Monsters/byhand.map" ) )
			{
				foreach ( PremiumSpawner itemsave2 in itemssave )
				{
					int mapnumber = 0;
					switch ( itemsave2.Map.ToString() )
					{
						case "Felucca":
							mapnumber = 1;
							break;
						case "Trammel":
							mapnumber = 2;
							break;
						case "Ilshenar":
							mapnumber = 3;
							break;
						case "Malas":
							mapnumber = 4;
							break;
						case "Tokuno":
							mapnumber = 5;
							break;
						default:
							mapnumber = 6;
							Console.WriteLine( "Monster Parser: Warning, unknown map {0}", itemsave2.Map );
							break;
					}

					string timer1a = itemsave2.MinDelay.ToString();
					string[] timer1b = timer1a.Split( ':' );
					int timer1c = ( Utility.ToInt32( timer1b[0] ) * 60 ) + Utility.ToInt32( timer1b[1] );

					string timer2a = itemsave2.MaxDelay.ToString();
					string[] timer2b = timer2a.Split( ':' );
					int timer2c = ( Utility.ToInt32( timer2b[0] ) * 60 ) + Utility.ToInt32( timer2b[1] );

					string towrite = "* " + itemsave2.CreaturesName[0];

					for ( int i = 1; i < itemsave2.CreaturesName.Count; ++i )
					{
						towrite = towrite + ":" + itemsave2.CreaturesName[i].ToString();
					}

					op.WriteLine( towrite + " {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", itemsave2.X, itemsave2.Y, itemsave2.Z, mapnumber, timer1c, timer2c, itemsave2.HomeRange, itemsave2.SpawnRange, itemsave2.SpawnID, itemsave2.Count );
				}
			}

			DateTime endTime = DateTime.Now;
			World.Broadcast( 0x35, true, "{0} spawns have been saved. The entire process took {1:F1} seconds.", count, (endTime - startTime).TotalSeconds );
		}

		private static void Remove( Mobile from, int x1, int y1, int x2, int y2 )
		{
			World.Broadcast( 0x35, true, "Spawns are being removed, please wait." );
			DateTime startTime = DateTime.Now;
			int count = 0;
			ArrayList itemsremove = new ArrayList();

			foreach ( Item itemremove in World.Items.Values )
			{ 
				if ( itemremove is PremiumSpawner && ( ( itemremove.X >= x1 && itemremove.X <= x2 ) && ( itemremove.Y >= y1 && itemremove.Y <= y2 ) && itemremove.Map == from.Map ) )
				{
					itemsremove.Add( itemremove );
					count +=1;
				}
			}

			foreach ( Item itemremove2 in itemsremove )
			{
				itemremove2.Delete();
			}

			DateTime endTime = DateTime.Now;
			World.Broadcast( 0x35, true, "{0} spawns have been removed. The entire process took {1:F1} seconds.", count, (endTime - startTime).TotalSeconds );
		}

		private static void Save( Mobile from, int x1, int y1, int x2, int y2 )
		{
			World.Broadcast( 0x35, true, "Spawns are being saved, please wait." );
			DateTime startTime = DateTime.Now;
			int count = 0;
			ArrayList itemssave = new ArrayList();

			foreach ( Item itemsave in World.Items.Values )
			{ 
				if ( itemsave is PremiumSpawner && ( ( itemsave.X >= x1 && itemsave.X <= x2 ) && ( itemsave.Y >= y1 && itemsave.Y <= y2 ) && itemsave.Map == from.Map ) )
				{
					itemssave.Add( itemsave );
					count +=1;
				}
			}

			if ( !Directory.Exists( "Data/Monsters" ) )
				Directory.CreateDirectory( "Data/Monsters" );

			using ( StreamWriter op = new StreamWriter( "Data/Monsters/spawns.map" ) )
			{
				foreach ( PremiumSpawner itemsave2 in itemssave )
				{
					int mapnumber = 0;
					switch ( itemsave2.Map.ToString() )
					{
						case "Felucca":
							mapnumber = 1;
							break;
						case "Trammel":
							mapnumber = 2;
							break;
						case "Ilshenar":
							mapnumber = 3;
							break;
						case "Malas":
							mapnumber = 4;
							break;
						case "Tokuno":
							mapnumber = 5;
							break;
						default:
							mapnumber = 6;
							Console.WriteLine( "Monster Parser: Warning, unknown map {0}", itemsave2.Map );
							break;
					}

					string timer1a = itemsave2.MinDelay.ToString();
					string[] timer1b = timer1a.Split( ':' );
					int timer1c = ( Utility.ToInt32( timer1b[0] ) * 60 ) + Utility.ToInt32( timer1b[1] );

					string timer2a = itemsave2.MaxDelay.ToString();
					string[] timer2b = timer2a.Split( ':' );
					int timer2c = ( Utility.ToInt32( timer2b[0] ) * 60 ) + Utility.ToInt32( timer2b[1] );

					string towrite = "* " + itemsave2.CreaturesName[0];

					for ( int i = 1; i < itemsave2.CreaturesName.Count; ++i )
					{
						towrite = towrite + ":" + itemsave2.CreaturesName[i].ToString();
					}

					op.WriteLine( towrite + " {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", itemsave2.X, itemsave2.Y, itemsave2.Z, mapnumber, timer1c, timer2c, itemsave2.HomeRange, itemsave2.SpawnRange, itemsave2.SpawnID, itemsave2.Count );
				}
			}

			DateTime endTime = DateTime.Now;
			World.Broadcast( 0x35, true, "{0} spawns have been saved. The entire process took {1:F1} seconds.", count, (endTime - startTime).TotalSeconds );
		}

		public static void Parse( Mobile from, string filename )
		{
			string monster_path1 = Path.Combine( Core.BaseDirectory, "Data/Monsters" );
			string monster_path = Path.Combine( monster_path1, filename );
			m_Count = 0;

			if ( File.Exists( monster_path ) )
			{
				ArrayList list = new ArrayList();

				from.SendMessage( "Generating Spawners..." );
				m_MapOverride = -1;
				m_IDOverride = -1;
				m_MinTimeOverride = -1;
				m_MaxTimeOverride = -1;

				using ( StreamReader ip = new StreamReader( monster_path ) )
				{
					string line;

					while ( (line = ip.ReadLine()) != null )
					{
						string[] split = line.Split( ' ' );

						if ( split.Length == 2  )
						{
							if ( split[0].ToLower() == "overridemap" )
								m_MapOverride = Utility.ToInt32( split[1] );
							if ( split[0].ToLower() == "overrideid" )
								m_IDOverride = Utility.ToInt32( split[1] );
							if ( split[0].ToLower() == "overridemintime" )
								m_MinTimeOverride = Utility.ToDouble( split[1] );
							if ( split[0].ToLower() == "overridemaxtime" )
								m_MaxTimeOverride = Utility.ToDouble( split[1] );
						}

						if ( split.Length < 9 )
							continue;

						switch(split[0].ToLower()) 
						{
							case "##"://Comment Line
								break;
							case "*"://Place By class
								PlaceNPC( split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], split[11], split[1].Split(':') );
								break;

							//Place By Type

// ----------------------------------------------------> ANIMALS
							case "+bears:": 
								PlaceNPC( split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], "Brownbear", "Grizzlybear", "Blackbear" ); 
								break; 
							case "+wolves:": 
								PlaceNPC( split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], "Direwolf", "Timberwolf", "Greywolf" ); 
								break; 
							case "+cows:": 
								PlaceNPC( split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], "Bull", "Cow" ); 
								break; 
							case "+roden:": 
								PlaceNPC( split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], "Rabbit", "JackRabbit" ); 
								break; 
							case "+felin:": 
								PlaceNPC( split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], "Cougar", "Panther" ); 
								break; 
							case "+misc:": 
								PlaceNPC( split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], "Goat", "Mountaingoat", "Greathart", "Hind", "Sheep", "Boar", "Llama", "snake", "pig" ); 
								break; 
							case "+bird:": 
								PlaceNPC( split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], "Bird", "Eagle" ); 
								break; 
							case "+mount:": 
								PlaceNPC( split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8], split[9], split[10], "Horse", "Ridablellama" ); 
								break;
						}
					}
				}

				m_MapOverride = -1;
				m_IDOverride = -1;
				m_MinTimeOverride = -1;
				m_MaxTimeOverride = -1;

				from.SendMessage( "Done, added {0} spawners",m_Count );
			}
			else
			{
				from.SendMessage( "{0} not found!", monster_path );
			}
		}

		public static void PlaceNPC( string sx, string sy, string sz, string sm, string smintime, string smaxtime, string shomerange, string sspawnrange, string sspawnid, string snpccount, params string[] types )
		{
			if ( types.Length == 0 )
				return;

			int x = Utility.ToInt32( sx );
			int y = Utility.ToInt32( sy );
			int z = Utility.ToInt32( sz );
			int map = Utility.ToInt32( sm );
			double dmintime = Utility.ToDouble( smintime );
			if ( m_MinTimeOverride != -1 )
				dmintime = m_MinTimeOverride;
			TimeSpan mintime = TimeSpan.FromMinutes( dmintime );
			double dmaxtime = Utility.ToDouble( smaxtime );
			if ( m_MaxTimeOverride != -1 )
			{
				if ( m_MaxTimeOverride < dmintime )
				{
					dmaxtime = dmintime;
				}
				else
				{
					dmaxtime = m_MaxTimeOverride;
				}
			}

			TimeSpan maxtime = TimeSpan.FromMinutes( dmaxtime );
			int homerange = Utility.ToInt32( shomerange );
		        int spawnrange = Utility.ToInt32( sspawnrange );
			int spawnid = Utility.ToInt32( sspawnid );
			int npccount = Utility.ToInt32( snpccount );

			if ( m_MapOverride != -1 )
				map = m_MapOverride;

			if ( m_IDOverride != -1 )
				spawnid = m_IDOverride;

			switch ( map )
			{
				case 0://Trammel and Felucca
					MakeSpawner( types, x, y, z, Map.Felucca, mintime, maxtime, homerange, spawnrange, spawnid, npccount );
					MakeSpawner( types, x, y, z, Map.Trammel, mintime, maxtime, homerange, spawnrange, spawnid, npccount );
					break;
				case 1://Felucca
					MakeSpawner( types, x, y, z, Map.Felucca, mintime, maxtime, homerange, spawnrange, spawnid, npccount );
					break;
				case 2://Trammel
					MakeSpawner( types, x, y, z, Map.Trammel, mintime, maxtime, homerange, spawnrange, spawnid, npccount );
					break;
				case 3://Ilshenar
					MakeSpawner( types, x, y, z, Map.Ilshenar, mintime, maxtime, homerange, spawnrange, spawnid, npccount );
					break;
				case 4://Malas
					MakeSpawner( types, x, y, z, Map.Malas, mintime, maxtime, homerange, spawnrange, spawnid, npccount );
					break;
				case 5://Tokuno
					MakeSpawner( types, x, y, z, Map.Maps[4], mintime, maxtime, homerange, spawnrange, spawnid, npccount );
					break;
				default:
					Console.WriteLine( "Spawn Parser: Warning, unknown map {0}", map );
					break;
			}
		}

		private static void MakeSpawner( string[] types, int x, int y, int z, Map map, TimeSpan mintime, TimeSpan maxtime, int homerange, int spawnrange, int spawnid, int npccount )
		{
			if ( types.Length == 0 )
				return;

			PremiumSpawner spawner = new PremiumSpawner( npccount, mintime, maxtime, Team, homerange, spawnrange, spawnid, new ArrayList( types ) );

			spawner.MoveToWorld( new Point3D( x, y, z ), map );

			if ( TotalRespawn )
			{
				spawner.Respawn();

				if ( ((PremiumSpawner)spawner).SpawnID == 132 ) // if is ChampionSpawn
				{
					spawner.BringToHome();
				}
			}
			
			m_Count++;
		}
	}
}