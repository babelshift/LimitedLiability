using SharpDL;
using SharpDL.Graphics;
using SharpTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Descriptors;
using MyThirdSDL.Simulation;

namespace MyThirdSDL.Content
{
	/// <summary>A TiledMap is a representation of a .tmx map created with the Tiled Map Editor. You can access layers and tiles within
	/// layers by accessing the properties of this class after instantiation.
	/// </summary>
	public class TiledMap : IDisposable
	{
		private List<TileLayer> tileLayers = new List<TileLayer>();
		private List<MapObjectLayer> mapObjectLayers = new List<MapObjectLayer>();
		private List<MapCell> mapCells = new List<MapCell>();

		/// <summary>
		/// The number of tiles across (left to right) that make up this map
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// The number of tiles down (top to bottom) that make up this map
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// The width of each tile in the map (all tiles are the same width)
		/// </summary>
		public int TileWidth { get; private set; }

		/// <summary>
		/// The height of each tile in the map (all tiles are the same height)
		/// </summary>
		public int TileHeight { get; private set; }

		public IEnumerable<MapCell> MapCells { get { return mapCells; } }

		public IEnumerable<TileLayer> TileLayers { get { return tileLayers; } }

		public IEnumerable<MapObjectLayer> MapObjectLayers { get { return mapObjectLayers; } }

		/// <summary>Default constructor creates a map from a .tmx file and creates any associated tileset textures by using the passed renderer.
		/// </summary>
		/// <param name="filePath">Path to the .tmx file to load</param>
		/// <param name="renderer">Renderer object used to load tileset textures</param>
		public TiledMap(string filePath, Renderer renderer, string contentRoot = "")
		{
			MapContent mapContent = new MapContent(filePath, renderer, contentRoot);

			TileWidth = mapContent.TileWidth;
			TileHeight = mapContent.TileHeight;

			Width = mapContent.Width * TileWidth;
			Height = mapContent.Height * TileHeight;

			CreateLayers(mapContent);
			CalculateTilePositions(mapContent.Orientation);
			CalculatePathNodeNeighbors();
			CreateMapCells(mapContent);
			BuildMapCells();
		}

		/// <summary>
		/// Create tile layers and object layers based on what we find in the Tiled Map TMX file.
		/// </summary>
		/// <param name="mapContent">Map content.</param>
		private void CreateLayers(MapContent mapContent)
		{
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

		private TileLayer CreateTileLayer(LayerContent layerContent, IEnumerable<TileSetContent> tileSets)
		{
			TileLayerContent tileLayerContent = layerContent as TileLayerContent;

			TileLayerType tileLayerType = TileLayerType.None;
			if (layerContent.Name.Contains("Ground"))
				tileLayerType = TileLayerType.Ground;
			else if (layerContent.Name.Contains("BackWalls"))
				tileLayerType = TileLayerType.BackWalls;
			else if (layerContent.Name.Contains("Objects"))
				tileLayerType = TileLayerType.Objects;
			else if (layerContent.Name.Contains("FrontWalls"))
				tileLayerType = TileLayerType.FrontWalls;

			TileLayer tileLayer = new TileLayer(layerContent.Name, tileLayerContent.Width, tileLayerContent.Height, tileLayerType);
			for (int i = 0; i < tileLayerContent.Data.Length; i++)
			{
				// strip out the flipped flags from the map editor to get the real tile index
				uint tileID = tileLayerContent.Data[i];
				uint flippedHorizontallyFlag = 0x80000000;
				uint flippedVerticallyFlag = 0x40000000;
				int tileIndex = (int)(tileID & ~(flippedVerticallyFlag | flippedHorizontallyFlag));
				Tile tile = CreateTile(tileIndex, tileSets, tileLayerType);
				tileLayer.AddTile(tile);
			}

			return tileLayer;
		}

		/// <summary>
		/// Creates the proper map object layer based on the layer name such as collidables and path nodes.
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="orientation"></param>
		/// <returns></returns>
		private MapObjectLayer CreateObjectLayer(LayerContent layer, Orientation orientation)
		{
			ObjectLayerContent objectLayerContent = layer as ObjectLayerContent;

			MapObjectLayerType mapObjectLayerType = MapObjectLayerType.None;
			MapObjectType mapObjectType = MapObjectType.None;

			if (objectLayerContent.Name == "DeadZones")
			{
				mapObjectLayerType = MapObjectLayerType.DeadZone;
				mapObjectType = MapObjectType.DeadZone;
			}
			else if (objectLayerContent.Name == "PathNodes")
			{
				mapObjectLayerType = MapObjectLayerType.PathNode;
			}
			else
				throw new Exception("Unknown map object layer type. Did you name your layer correctly? \"Collidables\" will be picked up as collidable objects. \"PathNodes\" will be picked up as path node objects.");

			MapObjectLayer mapObjectLayer = new MapObjectLayer(objectLayerContent.Name, mapObjectLayerType);

			foreach (ObjectContent objectContent in objectLayerContent.MapObjects)
			{
				if (mapObjectLayer.Type == MapObjectLayerType.PathNode)
				{
					PathNode pathNode = new PathNode(objectContent.Name, objectContent.Bounds, orientation);
					mapObjectLayer.AddMapObject(pathNode);
				}
				else
				{
					MapObject mapObject = new MapObject(objectContent.Name, objectContent.Bounds, orientation, mapObjectType);
					mapObjectLayer.AddMapObject(mapObject);
				}
			}

			return mapObjectLayer;
		}

		/// <summary>
		/// Build map cells based on the layers contained within. Tile layers will be used to determine z-index tiles on each map cell. Object layers
		/// will be used to determine dead zones and path nodes in the map cells.
		/// </summary>
		private void BuildMapCells()
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
						mapCell.WorldPosition = tile.WorldPosition;
						mapCell.ProjectedPosition = tile.ProjectedPosition;

						int zIndex = 0;
						if (tile.Type == TileType.Ground)
							zIndex = 0;
						else if (tile.Type == TileType.BackWall)
							zIndex = 1;
						else if (tile.Type == TileType.Object)
							zIndex = 2;
						else if (tile.Type == TileType.FrontWall)
							zIndex = 3;

						mapCell.AddTile(tile, zIndex);
					}
					else
						throw new Exception(String.Format("No map cell found at [{0},{1}].", tile.WorldGridIndex.X, tile.WorldGridIndex.Y));
				}
			}

			AddDeadZonesAndPathNodesToMapCells();
		}

		/// <summary>
		/// Adds the dead zones and path nodes to map cells. Each map cell can contains 0-4 dead zones and 0-4 path nodes based on the map's object layers.
		/// This method will loop through object layers and add the dead zone and path node objects to the map cell's collections if the map cell contains
		/// that object. Objects must be cell/axis aligned or an exception is thrown.
		/// </summary>
		private void AddDeadZonesAndPathNodesToMapCells()
		{
			foreach (var mapObjectLayer in mapObjectLayers)
			{
				foreach (MapObject mapObject in mapObjectLayer.MapObjects)
				{
					// get the map cell that contains us (where two of our edges line up with two of the map cell edges)
					MapCell mapCell = mapCells.FirstOrDefault(
						                  mc => (mc.Bounds.Left == mapObject.Bounds.Left && mc.Bounds.Top == mapObject.Bounds.Top)
						                  || (mc.Bounds.Left == mapObject.Bounds.Left && mc.Bounds.Bottom == mapObject.Bounds.Bottom)
						                  || (mc.Bounds.Right == mapObject.Bounds.Right && mc.Bounds.Top == mapObject.Bounds.Top)
						                  || (mc.Bounds.Right == mapObject.Bounds.Right && mc.Bounds.Bottom == mapObject.Bounds.Bottom));

					// if we are in a valid map cell, add the map object appropriately based on type
					if (mapCell != null)
					{
						if (mapObject.Type == MapObjectType.DeadZone)
							mapCell.AddDeadZone(mapObject);
						else if (mapObject.Type == MapObjectType.PathNode)
							mapCell.AddPathNode((PathNode)mapObject);
					}
					else
						throw new Exception("MapObjects must be map cell aligned.");
				}
			}
		}

		/// <summary>
		/// Create empty map cells based on tile counts in the Tiled Map TMX. For example, a tile map of 15x15 tiles will be translated into
		/// 15x15 map cells. These map cells are then later populated with 0-N tiles, 0-4 dead zones, and 0-4 path nodes.
		/// </summary>
		/// <param name="mapContent">Map content.</param>
		private void CreateMapCells(MapContent mapContent)
		{
			for (int x = 0; x < mapContent.Width; x++)
			{
				for (int y = 0; y < mapContent.Height; y++)
				{
					MapCell mapCell = new MapCell(CoordinateHelper.WorldGridCellWidth, CoordinateHelper.WorldGridCellHeight);
					mapCell.WorldGridIndex = new Point(x, y);
					mapCells.Add(mapCell);
				}
			}
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

		/// <summary>Based on a passed tile index, create a Tile by looking up which TileSet it belongs to, assign the proper TilSet texture,
		/// and find the bounds of the rectangle that encompasses the correct tile texture within the total tileset texture.
		/// </summary>
		/// <param name="tileIndex">Index of the tile (GID) within the map file</param>
		/// <param name="tileSets">Enumerable list of tilesets used to find out which tileset a tile belongs to</param>
		/// <returns></returns>
		private Tile CreateTile(int tileIndex, IEnumerable<TileSetContent> tileSets, TileLayerType tileLayerType)
		{
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
						source = tileSet.Tiles[(int)(tileIndex - tileSet.FirstGID)].SourceTextureBounds;
						break;
					}
				}

				TileType tileType = TileType.None;
				if (tileLayerType == TileLayerType.Ground)
					tileType = TileType.Ground;
				else if (tileLayerType == TileLayerType.BackWalls)
					tileType = TileType.BackWall;
				if (tileLayerType == TileLayerType.Objects)
					tileType = TileType.Object;
				if (tileLayerType == TileLayerType.FrontWalls)
					tileType = TileType.FrontWall;

				tile = new Tile(tileSetTexture, source, TileWidth, TileHeight, tileType);
			}

			return tile;
		}

		/// <summary>Loop through all tiles in all tile layers and calculate their X,Y coordinates. This will be used
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

						Vector projectedPosition = Vector.Zero;
						Vector worldPosition = Vector.Zero;
						if (mapOrientation == Orientation.Isometric)
						{
							Vector screenPosition = CoordinateHelper.WorldGridIndexToScreenSpace(
								                        x, y, TileWidth / 2, TileHeight,
								                        CoordinateHelper.ScreenOffset,
								                        CoordinateHelper.ScreenProjectionType.Isometric
							                        );

							projectedPosition = new Vector(screenPosition.X, screenPosition.Y);
							worldPosition = new Vector(x * TileWidth / 2, y * TileHeight);
						}
						else if (mapOrientation == Orientation.Orthogonal)
						{
							projectedPosition = new Vector(x * TileWidth, y * TileHeight);
							worldPosition = new Vector(x * TileWidth, y * TileHeight);
						}

						tile.ProjectedPosition = projectedPosition;
						tile.WorldPosition = worldPosition;
						tile.WorldGridIndex = new Point(x, y);
					}
				}
			}
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
			PathNode pathNode = pathNodes.FirstOrDefault(pn => pn.Bounds.Contains(new Point((int)worldPosition.X, (int)worldPosition.Y)));

			if (pathNode != null)
				return pathNode;
			else
				throw new Exception(String.Format("No path node found at [{0},{1}]", worldPosition.X, worldPosition.Y));
		}

		public IList<PathNode> GetPathNodes()
		{
			List<PathNode> pathNodes = new List<PathNode>();
			foreach (MapCell mapCell in mapCells)
				pathNodes.AddRange(mapCell.PathNodes);

			return pathNodes;
		}

        public IList<PathNode> GetActivePathNodes()
        {
            return GetPathNodes().Where(pn => pn.IsEnabled == true).ToList();
        }

		#region Dispose

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~TiledMap()
		{
			Dispose(false);
		}

		private void Dispose(bool isDisposing)
		{
			foreach (TileLayer tileLayer in tileLayers)
				tileLayer.Dispose();
		}

		#endregion

	}
}
