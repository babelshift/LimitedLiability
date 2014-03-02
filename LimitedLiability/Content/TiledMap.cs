using LimitedLiability.Agents;
using LimitedLiability.Simulation;
using SharpDL;
using SharpDL.Graphics;
using SharpTiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LimitedLiability.Content
{
	/// <summary>A TiledMap is a representation of a .tmx map created with the Tiled Map Editor. You can access layers and tiles within
	/// layers by accessing the properties of this class after instantiation.
	/// </summary>
	public class TiledMap : IDisposable
	{
		#region Members

		private List<TileLayer> tileLayers = new List<TileLayer>();
		private List<MapObjectLayer> mapObjectLayers = new List<MapObjectLayer>();
		private List<MapCell> mapCells = new List<MapCell>();

		private AgentFactory agentFactory;

		#endregion Members

		#region Properties

		/// <summary>
		/// The number of tiles that make up this map in the horizontal direction (defined in TMX file)
		/// </summary>
		public int HorizontalTileCount { get; private set; }

		/// <summary>
		/// The number of tiles that make up this map in the vertical direction (defined in TMX file)
		/// </summary>
		public int VerticalTileCount { get; private set; }

		/// <summary>
		/// The number of tiles across (left to right) that make up this map
		/// </summary>
		public int PixelWidth { get; private set; }

		/// <summary>
		/// The number of tiles down (top to bottom) that make up this map
		/// </summary>
		public int PixelHeight { get; private set; }

		/// <summary>
		/// The width of each tile in the map (all tiles are the same width)
		/// </summary>
		public int TileWidth { get; private set; }

		/// <summary>
		/// The height of each tile in the map (all tiles are the same height)
		/// </summary>
		public int TileHeight { get; private set; }

		public MapCell OriginMapCell { get { return mapCells[0]; } }

		/// <summary>
		/// Map cells contain references to tiles, map objects, and an optional equipment occupant. These map cells are used to draw things in the correct order,
		/// path agents to the correct location, stop agents from pathing to certain locations, and more.
		/// </summary>
		public IEnumerable<MapCell> MapCells { get { return mapCells; } }

		/// <summary>
		/// Tile layers contain Tiles read from the Tiled map editor.
		/// </summary>
		public IEnumerable<TileLayer> TileLayers { get { return tileLayers; } }

		/// <summary>
		/// Map object layers contain map objects read from the Tiled map editor.
		/// </summary>
		public IEnumerable<MapObjectLayer> MapObjectLayers { get { return mapObjectLayers; } }

		#endregion Properties

		#region Constructors

		/// <summary
		/// >Default constructor creates a map from a .tmx file and creates any associated tileset textures by using the passed renderer.
		/// </summary>
		/// <param name="filePath">Path to the .tmx file to load</param>
		/// <param name="renderer">Renderer object used to load tileset textures</param>
		public TiledMap(string filePath, Renderer renderer, AgentFactory agentFactory, string contentRoot = "")
		{
			this.agentFactory = agentFactory;

			MapContent mapContent = new MapContent(filePath, renderer, contentRoot);

			TileWidth = mapContent.TileWidth;
			TileHeight = mapContent.TileHeight;

			PixelWidth = mapContent.Width * TileWidth;
			PixelHeight = mapContent.Height * TileHeight;

			HorizontalTileCount = mapContent.Width;
			VerticalTileCount = mapContent.Height;

			CreateLayers(mapContent);
			CalculateTilePositions(mapContent.Orientation);
			CalculatePathNodeNeighbors();
			CreateMapCells(mapContent);
			InitializeMapCells();
		}

		#endregion Constructors

		#region Map Population Methods

		/// <summary>
		/// Create tile layers and object layers based on what we find in the Tiled Map TMX file.
		/// </summary>
		/// <param name="mapContent">Map contentManager.</param>
		private void CreateLayers(MapContent mapContent)
		{
			if (mapContent == null) throw new ArgumentNullException("mapContent");

			foreach (LayerContent layerContent in mapContent.Layers)
			{
				if (layerContent is TileLayerContent)
				{
					TileLayer tileLayer = CreateTileLayer(layerContent, mapContent.TileSets);
					tileLayers.Add(tileLayer);
				}
				else if (layerContent is ObjectLayerContent)
				{
					MapObjectLayer mapObjectLayer = CreateObjectLayer(layerContent, mapContent.Orientation);
					mapObjectLayers.Add(mapObjectLayer);
				}
			}
		}

		/// <summary>
		/// Creates a tile layer by reading the contentManager we got from the SharpTiles library. This will create our tile layers of type "Floor" and "Objects".
		/// </summary>
		/// <param name="layerContent"></param>
		/// <param name="tileSets"></param>
		/// <returns></returns>
		private TileLayer CreateTileLayer(LayerContent layerContent, IEnumerable<TileSetContent> tileSets)
		{
			if (layerContent == null) throw new ArgumentNullException("layerContent");
			if (tileSets == null) throw new ArgumentNullException("tileSets");

			TileLayerContent tileLayerContent = layerContent as TileLayerContent;

			TileLayerType tileLayerType = GetTileLayerType(layerContent);

			TileLayer tileLayer = new TileLayer(layerContent.Name, tileLayerContent.Width, tileLayerContent.Height, tileLayerType);
			foreach (uint tileID in tileLayerContent.Data)
			{
				uint flippedHorizontallyFlag = 0x80000000;
				uint flippedVerticallyFlag = 0x40000000;
				int tileIndex = (int)(tileID & ~(flippedVerticallyFlag | flippedHorizontallyFlag));
				Tile tile = CreateTile(tileIndex, tileSets, tileLayerType);
				tileLayer.AddTile(tile);
			}

			return tileLayer;
		}

		/// <summary>
		/// Returns the type of the tile layer according to the layer's name as read from the Tiled map editor.
		/// </summary>
		/// <param name="layerContent"></param>
		/// <returns></returns>
		private static TileLayerType GetTileLayerType(LayerContent layerContent)
		{
			if (layerContent == null) throw new ArgumentNullException("layerContent");

			TileLayerType tileLayerType = TileLayerType.None;
			if (layerContent.Name.Contains("Floor"))
				tileLayerType = TileLayerType.Floor;
			else if (layerContent.Name.Contains("Objects"))
				tileLayerType = TileLayerType.Objects;
			return tileLayerType;
		}

		/// <summary>
		/// Based on a passed tile index, create a Tile by looking up which TileSet it belongs to, assign the proper TilSet texture,
		/// and find the bounds of the rectangle that encompasses the correct tile texture within the total tileset texture.
		/// </summary>
		/// <param name="tileIndex">Index of the tile (GID) within the map file</param>
		/// <param name="tileSets">Enumerable list of tilesets used to find out which tileset a tile belongs to</param>
		/// <param name="tileLayerType"></param>
		/// <returns></returns>
		private Tile CreateTile(int tileIndex, IEnumerable<TileSetContent> tileSets, TileLayerType tileLayerType)
		{
			if (tileSets == null) throw new ArgumentNullException("tileSets");

			Tile tile = new Tile();

			// we don't want to look up tiles with ID 0 in tile sets because Tiled Map Editor treats ID 0 as an empty tile
			if (tileIndex > Tile.EmptyTileID)
			{
				Texture tileSetTexture = null;
				Rectangle source = new Rectangle();

				foreach (TileSetContent tileSet in tileSets)
				{
					if (tileIndex - tileSet.FirstGID < tileSet.Tiles.Count)
					{
						tileSetTexture = tileSet.Texture;
						source = tileSet.Tiles[tileIndex - tileSet.FirstGID].SourceTextureBounds;
						break;
					}
				}

				TileType tileType = GetTileType(tileLayerType);

				tile = new Tile(tileSetTexture, source, TileWidth, TileHeight, tileType);
			}

			return tile;
		}

		/// <summary>
		/// Based on the tile layer type passed, this method returns a tile type for all tiles that would be contained in a layer of that type.
		/// </summary>
		/// <param name="tileLayerType"></param>
		/// <returns></returns>
		private static TileType GetTileType(TileLayerType tileLayerType)
		{
			TileType tileType = TileType.None;
			if (tileLayerType == TileLayerType.Floor)
				tileType = TileType.Floor;
			if (tileLayerType == TileLayerType.Objects)
				tileType = TileType.Object;
			return tileType;
		}

		/// <summary>
		/// Creates the proper map object layer based on the layer name such as collidables and path nodes.
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="orientation"></param>
		/// <returns></returns>
		private MapObjectLayer CreateObjectLayer(LayerContent layer, Orientation orientation)
		{
			if (layer == null) throw new ArgumentNullException("layer");

			ObjectLayerContent objectLayerContent = layer as ObjectLayerContent;

			MapObjectLayerType mapObjectLayerType;
			MapObjectType mapObjectType;

			if (objectLayerContent.Name == "DeadZones")
			{
				mapObjectLayerType = MapObjectLayerType.DeadZone;
				mapObjectType = MapObjectType.DeadZone;
			}
			else if (objectLayerContent.Name == "PathNodes")
			{
				mapObjectLayerType = MapObjectLayerType.PathNode;
				mapObjectType = MapObjectType.PathNode;
			}
			else if (objectLayerContent.Name == "Equipment")
			{
				mapObjectLayerType = MapObjectLayerType.Equipment;
				mapObjectType = MapObjectType.Equipment;
			}
			else
				throw new Exception("Unknown map object layer type. Did you name your layer correctly? \"DeadZones\", \"PathNodes\", and \"Equipment\" are valid layers.");

			MapObjectLayer mapObjectLayer = new MapObjectLayer(objectLayerContent.Name, mapObjectLayerType);

			foreach (ObjectContent objectContent in objectLayerContent.MapObjects)
			{
				if (mapObjectLayer.Type == MapObjectLayerType.PathNode)
				{
					PathNode pathNode = new PathNode(objectContent.Name, objectContent.Bounds, orientation, objectContent.Properties);
					mapObjectLayer.AddMapObject(pathNode);
				}
				else if (mapObjectLayer.Type == MapObjectLayerType.Equipment)
				{
					EquipmentObjectType equipmentObjectType = GetEquipmentObjectType(objectContent);
					EquipmentObject equipmentObject = new EquipmentObject(objectContent.Name, objectContent.Bounds, orientation, objectContent.Properties, equipmentObjectType);
					mapObjectLayer.AddMapObject(equipmentObject);
				}
				else
				{
					MapObject mapObject = new MapObject(objectContent.Name, objectContent.Bounds, orientation, mapObjectType, objectContent.Properties);
					mapObjectLayer.AddMapObject(mapObject);
				}
			}

			return mapObjectLayer;
		}

		/// <summary>
		/// Returns the type of the equipment object contained within the passed object contentManager.
		/// </summary>
		/// <param name="objectContent"></param>
		/// <returns></returns>
		private static EquipmentObjectType GetEquipmentObjectType(ObjectContent objectContent)
		{
			if (objectContent == null) throw new ArgumentNullException("objectContent");

			EquipmentObjectType equipmentObjectType = EquipmentObjectType.Unknown;

			if (objectContent.Type == "SnackMachine")
				equipmentObjectType = EquipmentObjectType.SnackMachine;
			else if (objectContent.Type == "SodaMachine")
				equipmentObjectType = EquipmentObjectType.SodaMachine;
			else if (objectContent.Type == "OfficeDesk")
				equipmentObjectType = EquipmentObjectType.OfficeDesk;
			else if (objectContent.Type == "WaterFountain")
				equipmentObjectType = EquipmentObjectType.WaterFountain;

			if (equipmentObjectType == EquipmentObjectType.Unknown)
				throw new Exception("A map object of type \"Equipment\" was found but the type is unknown.");

			return equipmentObjectType;
		}

		/// <summary>
		/// Create empty map cells based on tile counts in the Tiled Map TMX. For example, a tile map of 15x15 tiles will be translated into
		/// 15x15 map cells. These map cells are then later populated with 0-N tiles, 0-4 dead zones, and 0-4 path nodes.
		/// </summary>
		/// <param name="mapContent">Map contentManager.</param>
		private void CreateMapCells(MapContent mapContent)
		{
			if (mapContent == null) throw new ArgumentNullException("mapContent");

			for (int y = 0; y < mapContent.Height; y++)
			{
				for (int x = 0; x < mapContent.Width; x++)
				{
					MapCell mapCell = new MapCell(CoordinateHelper.WorldGridCellWidth, CoordinateHelper.WorldGridCellHeight);
					mapCell.WorldGridIndex = new Point(x, y);
					mapCells.Add(mapCell);
				}
			}
		}

		/// <summary>
		/// Build map cells based on the layers contained within. Tile layers will be used to determine z-index tiles on each map cell. Object layers
		/// will be used to determine dead zones and path nodes in the map cells.
		/// </summary>
		private void InitializeMapCells()
		{
			foreach (var tileLayer in tileLayers)
			{
				// loop through all tiles on this tile layer
				foreach (var tile in tileLayer.Tiles)
				{
					// get the map cell that is associated with the world grid index that this tile is on
					MapCell mapCell = mapCells.FirstOrDefault(mc => mc.WorldGridIndex == tile.WorldGridIndex);

					// we have a map cell at this index, so copy our data to it and add the tile with the z-index
					if (mapCell != null)
					{
						if (tile.IsEmpty) continue;

						mapCell.WorldPosition = tile.WorldPosition;

						if (tile.Type == TileType.Floor)
							mapCell.FloorTile = tile;
						else if (tile.Type == TileType.Object)
							mapCell.AddTileObject(tile);
					}
					else
						throw new Exception(String.Format("No map cell found at [{0},{1}].", tile.WorldGridIndex.X, tile.WorldGridIndex.Y));
				}
			}

			AddMapObjectsToMapCells();
		}

		/// <summary>
		/// Adds the dead zones and path nodes to map cells. Each map cell can contains 0-4 dead zones and 0-4 path nodes based on the map's object layers.
		/// This method will loop through object layers and add the dead zone and path node objects to the map cell's collections if the map cell contains
		/// that object. Objects must be cell/axis aligned or an exception is thrown.
		/// </summary>
		private void AddMapObjectsToMapCells()
		{
			foreach (var mapObjectLayer in mapObjectLayers)
			{
				foreach (MapObject mapObject in mapObjectLayer.MapObjects)
				{
					// get the map cell that contains us (where two of our edges line up with two of the map cell edges)
					MapCell mapCell = GetMapCellAlignedWithMapObject(mapObject);

					// if we are in a valid map cell, add the map object appropriately based on type
					if (mapCell != null)
					{
						if (mapObject.Type == MapObjectType.DeadZone)
							mapCell.Type = MapCellType.DeadZone;
						else if (mapObject.Type == MapObjectType.PathNode)
						{
							mapCell.Type = MapCellType.PathNode;
							mapCell.ContainedPathNode = (PathNode)mapObject;
						}
						else if (mapObject.Type == MapObjectType.Equipment)
						{
							Equipment equipment = CreateEquipmentFromEquipmentObjectSubtype(mapObject, mapCell);
							mapCell.OccupantEquipment = equipment;
						}
					}
					else
						throw new Exception("MapObjects must be map axis aligned.");
				}
			}
		}

		/// <summary>
		/// Returns a reference to an equipment agent based on the map object passed. Will assign the map cells position to the created agent.
		/// </summary>
		/// <param name="mapObject"></param>
		/// <param name="mapCell"></param>
		/// <returns></returns>
		private Equipment CreateEquipmentFromEquipmentObjectSubtype(MapObject mapObject, MapCell mapCell)
		{
			if (mapObject == null) throw new ArgumentNullException("mapObject");
			if (mapCell == null) throw new ArgumentNullException("mapCell");

			Equipment equipment = null;
			EquipmentObject equipmentObject = (EquipmentObject)mapObject;

			switch (equipmentObject.Subtype)
			{
				case EquipmentObjectType.SnackMachine:
					equipment = agentFactory.CreateSnackMachine(TimeSpan.Zero, mapCell.WorldPosition);
					break;

				case EquipmentObjectType.SodaMachine:
					equipment = agentFactory.CreateSodaMachine(TimeSpan.Zero, mapCell.WorldPosition);
					break;

				case EquipmentObjectType.OfficeDesk:
					equipment = agentFactory.CreateOfficeDesk(TimeSpan.Zero, mapCell.WorldPosition);
					break;

				case EquipmentObjectType.WaterFountain:
					equipment = agentFactory.CreateWaterFountain(TimeSpan.Zero, mapCell.WorldPosition);
					break;
			}

			return equipment;
		}

		/// <summary>
		/// Returns the map cell that is axis aligned with the passed map object (shared 4 borders with).
		/// </summary>
		/// <param name="mapObject"></param>
		/// <returns></returns>
		private MapCell GetMapCellAlignedWithMapObject(MapObject mapObject)
		{
			if (mapObject == null) throw new ArgumentNullException("mapObject");

			MapCell mapCell = mapCells.FirstOrDefault(
				mc => (mc.Bounds.Left == mapObject.Bounds.Left && mc.Bounds.Top == mapObject.Bounds.Top)
					  || (mc.Bounds.Left == mapObject.Bounds.Left && mc.Bounds.Bottom == mapObject.Bounds.Bottom)
					  || (mc.Bounds.Right == mapObject.Bounds.Right && mc.Bounds.Top == mapObject.Bounds.Top)
					  || (mc.Bounds.Right == mapObject.Bounds.Right && mc.Bounds.Bottom == mapObject.Bounds.Bottom));
			return mapCell;
		}

		/// <summary>
		/// Loops through all path node layers, calculates the position (world grid index) of each immediate neighbor to the node,
		/// and adds the neighbors to a collection. These neighbors are used in path finding algorithms.
		/// </summary>
		private void CalculatePathNodeNeighbors()
		{
			IEnumerable<MapObjectLayer> pathNodeLayers = mapObjectLayers.Where(mol => mol.Type == MapObjectLayerType.PathNode);
			foreach (var pathNodeLayer in pathNodeLayers)
			{
				foreach (var pathNode in pathNodeLayer.MapObjects)
				{
					var realPathNode = (PathNode)pathNode;
					IEnumerable<PathNode> neighboringPathNodes = pathNodeLayer.GetNeighboringPathNodes(realPathNode);
					foreach (var neighboringPathNode in neighboringPathNodes)
						realPathNode.AddNeighbor(neighboringPathNode);
				}
			}
		}

		/// <summary>
		/// Loop through all tiles in all tile layers and calculate their X,Y coordinates. This will be used
		/// by renderers to paint the textures in the correct position of the rendering target.
		/// </summary>
		private void CalculateTilePositions(Orientation mapOrientation)
		{
			foreach (TileLayer tileLayer in tileLayers)
			{
				for (int y = 0; y < tileLayer.Height; y++)
				{
					for (int x = 0; x < tileLayer.Width; x++)
					{
						Tile tile = tileLayer.Tiles[y * tileLayer.Width + x];

						TilePosition tilePosition = GetTilePositionBasedOnOrientation(mapOrientation, x, y);

						tile.WorldPosition = tilePosition.WorldPosition;
						tile.WorldGridIndex = new Point(x, y);
					}
				}
			}
		}

		/// <summary>
		/// Returns the tile position (world and project) of the passed coordinates based on the project orientation (isometric, orthogonal).
		/// </summary>
		/// <param name="mapOrientation"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private TilePosition GetTilePositionBasedOnOrientation(Orientation mapOrientation, int x, int y)
		{
			Vector worldPosition = Vector.Zero;
			Vector projectedPosition = Vector.Zero;

			if (mapOrientation == Orientation.Isometric)
			{
				// we divide tile width by 2 here because our world space has half the width as the screen space tiles
				// for example, a tile map with 80 wide by 40 high tiles in projected screen space will be represented
				// in world space in a 40 wide by 40 high grid
				worldPosition = new Vector(x * TileWidth / 2, y * TileHeight);
				projectedPosition = CoordinateHelper.WorldSpaceToScreenSpace(worldPosition.X, worldPosition.Y,
					CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);
			}
			else if (mapOrientation == Orientation.Orthogonal)
			{
				projectedPosition = new Vector(x * TileWidth, y * TileHeight);
				worldPosition = new Vector(x * TileWidth, y * TileHeight);
			}

			return new TilePosition(worldPosition, projectedPosition);
		}

		#endregion Map Population Methods

		#region Finder Methods

		public void ReplaceMapCellAtPosition(MapCell mapCellToAdd, Vector position)
		{
			if (mapCellToAdd == null) throw new ArgumentNullException("mapCellToAdd");

			MapCell mapCellToReplace = mapCells.FirstOrDefault(mc => mc.Bounds.Contains(position));

			if(mapCellToReplace == null) 
				throw new InvalidOperationException(String.Format("Cannot replace a cell at location ({0},{1}) because there is no cell at that position.", position.X, position.Y));

			mapCellToAdd.WorldGridIndex = mapCellToReplace.WorldGridIndex;
			mapCellToAdd.WorldPosition = mapCellToReplace.WorldPosition;

			int index = mapCells.IndexOf(mapCellToReplace);

			mapCells.Remove(mapCellToReplace);
			
			mapCells.Insert(index, mapCellToAdd);
		}

		/// <summary>
		/// Returns the path node that contains the passed vector position. The path node must be enabled in order to be returned. Path nodes are enabled unless
		/// otherwise disabled by the placement of an obstacle or object on the node.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public PathNode GetPathNodeAtWorldPosition(Vector worldPosition)
		{
			IEnumerable<PathNode> pathNodes = GetActivePathNodes();

			// the rounding of the position coordinates here is causing problems finding the path nodes i think
			PathNode pathNode = pathNodes.FirstOrDefault(pn => pn.Bounds.Contains(worldPosition));

			if (pathNode != null)
				return pathNode;

			throw new Exception(String.Format("No path node found at [{0},{1}]", worldPosition.X, worldPosition.Y));
		}

		/// <summary>
		/// Returns all path nodes in all map cells.
		/// </summary>
		/// <returns></returns>
		public IList<PathNode> GetPathNodes()
		{
			return mapCells
				.Where(mc => mc.Type == MapCellType.PathNode)
				.Select(mc => mc.ContainedPathNode).ToList();
		}

		/// <summary>
		/// Returns all enabled path nodes in all map cells.
		/// </summary>
		/// <returns></returns>
		public IList<PathNode> GetActivePathNodes()
		{
			return GetPathNodes().Where(pn => pn.IsEnabled).ToList();
		}

		/// <summary>
		/// Returns the map cell located at the passed world position. Coordinates are rounded prior
		/// to check because map cells exist in whole number coordinates.
		/// </summary>
		/// <param name="worldPosition"></param>
		/// <returns></returns>
		public MapCell GetMapCellAtWorldPosition(Vector worldPosition)
		{
			int worldX = (int)(Math.Floor(worldPosition.X / CoordinateHelper.WorldGridCellWidth));
			int worldY = (int)(Math.Floor(worldPosition.Y / CoordinateHelper.WorldGridCellHeight));

			// if we are within the bounds of the world
			if (worldX >= 0 && worldY >= 0 && worldX < HorizontalTileCount && worldY < VerticalTileCount)
			{
				// if we are not exceeding the count of our collection
				if ((worldY * HorizontalTileCount + worldX) < mapCells.Count)
				{
					// O(1) lookup
					MapCell mapCell = mapCells[worldY * HorizontalTileCount + worldX];
					return mapCell;
				}
			}

			// there was no cell found at the coordinates provided or it was outside the valid range
			return null;
		}

		public IReadOnlyList<MapCell> GetMapCellAtWorldPositionAndNeighbors(Vector worldPosition, int tileCountRight,
			int tileCountDown)
		{
			List<MapCell> mapCells = new List<MapCell>();

			for (int i = 0; i < tileCountRight; i++)
			{
				for (int j = 0; j < tileCountDown; j++)
				{
					MapCell mapCellNeighbor = GetMapCellAtWorldPosition(
						new Vector(
							worldPosition.X + (CoordinateHelper.WorldGridCellWidth * i),
							worldPosition.Y + (CoordinateHelper.WorldGridCellHeight * j)
						)
					);
					mapCells.Add(mapCellNeighbor);
				}
			}

			return mapCells;
		}

		public IReadOnlyCollection<Equipment> GetEquipmentOccupants()
		{
			return (from mapCell
					in mapCells
					where mapCell.OccupantEquipment != null
					select mapCell.OccupantEquipment)
					.ToList();
		}

		public void RemoveEquipment(Guid equipmentId)
		{
			MapCell mapCell = mapCells.FirstOrDefault(mc => mc.OccupantEquipment != null && mc.OccupantEquipment.ID == equipmentId);
			if (mapCell != null)
				mapCell.OccupantEquipment = null;
		}

		#endregion Finder Methods

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			List<MapCell> mapCells = new List<MapCell>();

			mapCells.AddRange(MapCells);
			mapCells.Sort((d1, d2) => d1.Depth.CompareTo(d2.Depth));

			foreach (var mapCell in mapCells)
				mapCell.Draw(gameTime, renderer);
		}

		public void Draw(GameTime gameTime, Renderer renderer, int x, int y, bool isOverlappingDeadZone)
		{
			List<MapCell> mapCells = new List<MapCell>();

			mapCells.AddRange(MapCells);
			mapCells.Sort((d1, d2) => d1.Depth.CompareTo(d2.Depth));

			foreach (var mapCell in mapCells)
				mapCell.Draw(gameTime, renderer, x, y, isOverlappingDeadZone);
		}

		#region Dispose

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			foreach (TileLayer tileLayer in tileLayers)
				tileLayer.Dispose();
		}

		#endregion Dispose
	}
}