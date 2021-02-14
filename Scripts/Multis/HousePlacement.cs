using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Regions;

namespace Server.Multis
{
	public enum HousePlacementResult
	{
		Valid,
		BadRegion,
		BadLand,
		BadStatic,
		BadItem,
		NoSurface,
		BadRegionHidden
	}

	public class HousePlacement
	{
		private const int YardSize = 5;

		// Any land tile which matches one of these ID numbers is considered a road and cannot be placed over.
		private static int[] m_RoadIDs = new int[]
			{
				0x071, 0x08C,
				0x0E8, 0x0EB,
				0x14C, 0x14F,
				0x161, 0x174,
				0x1F0, 0x1F3,
				0x26E, 0x279,
				0x27E, 0x281,
				0x324, 0x3AC,
				0x547, 0x556,
				0x597, 0x5A6,
				0x637, 0x63A,
				0x7AE, 0x7B1,
				0x442, 0x479, // Sand stones
				0x501, 0x510, // Sand stones
				0x009, 0x015, // Furrows
				0x150, 0x15C  // Furrows 
			};

		public static HousePlacementResult Check( Mobile from, int multiID, Point3D center, out ArrayList toMove )
		{
			// If this spot is considered valid, every item and mobile in this list will be moved under the house sign
			toMove = new ArrayList();

			Map map = from.Map;

			if ( map == null || map == Map.Internal )
				return HousePlacementResult.BadLand; // A house cannot go here

			if ( from.AccessLevel >= AccessLevel.GameMaster )
				return HousePlacementResult.Valid; // Staff can place anywhere

			if ( map == Map.Ilshenar )
				return HousePlacementResult.BadRegion; // No houses in Ilshenar

			NoHousingRegion noHousingRegion = (NoHousingRegion) Region.Find( center, map ).GetRegion( typeof( NoHousingRegion ) );

			if ( noHousingRegion != null )
				return HousePlacementResult.BadRegion;

			// This holds data describing the internal structure of the house
			MultiComponentList mcl = MultiData.GetComponents( multiID );

			if ( multiID >= 0x13EC && multiID < 0x1D00 )
				HouseFoundation.AddStairsTo( ref mcl ); // this is a AOS house, add the stairs

			// Location of the nortwest-most corner of the house
			Point3D start = new Point3D( center.X + mcl.Min.X, center.Y + mcl.Min.Y, center.Z );

			// These are storage lists. They hold items and mobiles found in the map for further processing
			ArrayList items = new ArrayList(), mobiles = new ArrayList();

			// These are also storage lists. They hold location values indicating the yard and border locations.
			ArrayList yard = new ArrayList(), borders = new ArrayList();

			/* RULES:
			 * 
			 * 1) All tiles which are around the -outside- of the foundation must not have anything impassable.
			 * 2) No impassable object or land tile may come in direct contact with any part of the house.
			 * 3) Five tiles from the front and back of the house must be completely clear of all house tiles.
			 * 4) The foundation must rest flatly on a surface. Any bumps around the foundation are not allowed.
			 * 5) No foundation tile may reside over terrain which is viewed as a road.
			 */

			for ( int x = 0; x < mcl.Width; ++x )
			{
				for ( int y = 0; y < mcl.Height; ++y )
				{
					int tileX = start.X + x;
					int tileY = start.Y + y;

					Tile[] addTiles = mcl.Tiles[x][y];

					if ( addTiles.Length == 0 )
						continue; // There are no tiles here, continue checking somewhere else

					Point3D testPoint = new Point3D( tileX, tileY, center.Z );

					Region reg = Region.Find( testPoint, map );

					if ( !reg.AllowHousing( from, testPoint ) ) // Cannot place houses in dungeons, towns, treasure map areas etc
					{
						if ( reg.IsPartOf( typeof( TreasureRegion ) ) )
							return HousePlacementResult.BadRegionHidden;

						return HousePlacementResult.BadRegion;
					}

					Tile landTile = map.Tiles.GetLandTile( tileX, tileY );
					int landID = landTile.ID & 0x3FFF;

					Tile[] oldTiles = map.Tiles.GetStaticTiles( tileX, tileY, true );

					Sector sector = map.GetSector( tileX, tileY );

					items.Clear();

					for ( int i = 0; i < sector.Items.Count; ++i )
					{
						Item item = sector.Items[i];

						if ( item.Visible && item.X == tileX && item.Y == tileY )
							items.Add( item );
					}

					mobiles.Clear();

					for ( int i = 0; i < sector.Mobiles.Count; ++i )
					{
						Mobile m = (Mobile)sector.Mobiles[i];

						if ( m.X == tileX && m.Y == tileY )
							mobiles.Add( m );
					}

					int landStartZ = 0, landAvgZ = 0, landTopZ = 0;

					map.GetAverageZ( tileX, tileY, ref landStartZ, ref landAvgZ, ref landTopZ );

					bool hasFoundation = false;

					for ( int i = 0; i < addTiles.Length; ++i )
					{
						Tile addTile = addTiles[i];

						if ( addTile.ID == 0x4001 ) // Nodraw
							continue;

						TileFlag addTileFlags = TileData.ItemTable[addTile.ID & 0x3FFF].Flags;

						bool isFoundation = ( addTile.Z == 0 && (addTileFlags & TileFlag.Wall) != 0 );
						bool hasSurface = false;

						if ( isFoundation )
							hasFoundation = true;

						int addTileZ = center.Z + addTile.Z;
						int addTileTop = addTileZ + addTile.Height;

						if ( (addTileFlags & TileFlag.Surface) != 0 )
							addTileTop += 16;

						if ( addTileTop > landStartZ && landAvgZ > addTileZ )
							return HousePlacementResult.BadLand; // Broke rule #2

						if ( isFoundation && ((TileData.LandTable[landTile.ID & 0x3FFF].Flags & TileFlag.Impassable) == 0) && landAvgZ == center.Z )
							hasSurface = true;

						for ( int j = 0; j < oldTiles.Length; ++j )
						{
							Tile oldTile = oldTiles[j];
							ItemData id = TileData.ItemTable[oldTile.ID & 0x3FFF];

							if ( (id.Impassable || (id.Surface && (id.Flags & TileFlag.Background) == 0)) && addTileTop > oldTile.Z && (oldTile.Z + id.CalcHeight) > addTileZ )
								return HousePlacementResult.BadStatic; // Broke rule #2
							/*else if ( isFoundation && !hasSurface && (id.Flags & TileFlag.Surface) != 0 && (oldTile.Z + id.CalcHeight) == center.Z )
								hasSurface = true;*/
						}

						for ( int j = 0; j < items.Count; ++j )
						{
							Item item = (Item)items[j];
							ItemData id = item.ItemData;

							if ( addTileTop > item.Z && (item.Z + id.CalcHeight) > addTileZ )
							{
								if ( item.Movable )
									toMove.Add( item );
								else if ( (id.Impassable || (id.Surface && (id.Flags & TileFlag.Background) == 0)) )
									return HousePlacementResult.BadItem; // Broke rule #2
							}
							/*else if ( isFoundation && !hasSurface && (id.Flags & TileFlag.Surface) != 0 && (item.Z + id.CalcHeight) == center.Z )
							{
								hasSurface = true;
							}*/
						}

						if ( isFoundation && !hasSurface )
							return HousePlacementResult.NoSurface; // Broke rule #4

						for ( int j = 0; j < mobiles.Count; ++j )
						{
							Mobile m = (Mobile)mobiles[j];

							if ( addTileTop > m.Z && (m.Z + 16) > addTileZ )
								toMove.Add( m );
						}
					}

					for ( int i = 0; i < m_RoadIDs.Length; i += 2 )
					{
						if ( landID >= m_RoadIDs[i] && landID <= m_RoadIDs[i + 1] )
							return HousePlacementResult.BadLand; // Broke rule #5
					}

					if ( hasFoundation )
					{
						for ( int xOffset = -1; xOffset <= 1; ++xOffset )
						{
							for ( int yOffset = -YardSize; yOffset <= YardSize; ++yOffset )
							{
								Point2D yardPoint = new Point2D( tileX + xOffset, tileY + yOffset );

								if ( !yard.Contains( yardPoint ) )
									yard.Add( yardPoint );
							}
						}

						for ( int xOffset = -1; xOffset <= 1; ++xOffset )
						{
							for ( int yOffset = -1; yOffset <= 1; ++yOffset )
							{
								if ( xOffset == 0 && yOffset == 0 )
									continue;

								// To ease this rule, we will not add to the border list if the tile here is under a base floor (z<=8)

								int vx = x + xOffset;
								int vy = y + yOffset;

								if ( vx >= 0 && vx < mcl.Width && vy >= 0 && vy < mcl.Height )
								{
									Tile[] breakTiles = mcl.Tiles[vx][vy];
									bool shouldBreak = false;

									for ( int i = 0; !shouldBreak && i < breakTiles.Length; ++i )
									{
										Tile breakTile = breakTiles[i];

										if ( breakTile.Height == 0 && breakTile.Z <= 8 && TileData.ItemTable[breakTile.ID & 0x3FFF].Surface )
											shouldBreak = true;
									}

									if ( shouldBreak )
										continue;
								}

								Point2D borderPoint = new Point2D( tileX + xOffset, tileY + yOffset );

								if ( !borders.Contains( borderPoint ) )
									borders.Add( borderPoint );
							}
						}
					}
				}
			}

			for ( int i = 0; i < borders.Count; ++i )
			{
				Point2D borderPoint = (Point2D)borders[i];

				Tile landTile = map.Tiles.GetLandTile( borderPoint.X, borderPoint.Y );
				int landID = landTile.ID & 0x3FFF;

				if ( (TileData.LandTable[landID].Flags & TileFlag.Impassable) != 0 )
					return HousePlacementResult.BadLand;

				for ( int j = 0; j < m_RoadIDs.Length; j += 2 )
				{
					if ( landID >= m_RoadIDs[j] && landID <= m_RoadIDs[j + 1] )
						return HousePlacementResult.BadLand; // Broke rule #5
				}

				Tile[] tiles = map.Tiles.GetStaticTiles( borderPoint.X, borderPoint.Y, true );

				for ( int j = 0; j < tiles.Length; ++j )
				{
					Tile tile = tiles[j];
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

					if ( id.Impassable || (id.Surface && (id.Flags & TileFlag.Background) == 0 && (tile.Z + id.CalcHeight) > (center.Z + 2)) )
						return HousePlacementResult.BadStatic; // Broke rule #1
				}

				Sector sector = map.GetSector( borderPoint.X, borderPoint.Y );
				List<Item> sectorItems = sector.Items;

				for ( int j = 0; j < sectorItems.Count; ++j )
				{
					Item item = sectorItems[j];

					if ( item.X != borderPoint.X || item.Y != borderPoint.Y || item.Movable )
						continue;

					ItemData id = item.ItemData;

					if ( id.Impassable || (id.Surface && (id.Flags & TileFlag.Background) == 0 && (item.Z + id.CalcHeight) > (center.Z + 2)) )
						return HousePlacementResult.BadItem; // Broke rule #1
				}
			}

			for ( int i = 0; i < yard.Count; ++i )
			{
				Point2D yardPoint = (Point2D)yard[i];

				IPooledEnumerable eable = map.GetMultiTilesAt( yardPoint.X, yardPoint.Y );

				foreach ( Tile[] tile in eable )
				{
					for ( int j = 0; j < tile.Length; ++j )
					{
						if ( (TileData.ItemTable[tile[j].ID & 0x3FFF].Flags & (TileFlag.Impassable | TileFlag.Surface)) != 0 )
						{
							eable.Free();
							return HousePlacementResult.BadStatic; // Broke rule #3
						}
					}
				}

				eable.Free();
			}

			return HousePlacementResult.Valid;

		}
	}
}