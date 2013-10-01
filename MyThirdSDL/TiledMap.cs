using SharpDL;
using SharpDL.Graphics;
using SharpTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public enum TileLayerType
	{
		Base,
		Height
	}

	public enum MapObjectLayerType
	{
		None,
		PathNode,
		Collidable
	}

	public enum MapObjectType
	{
		None,
		PathNode,
		Collidable
	}

	/// <summary>A Tile is a representation of a Tile in a .tmx file. These tiles contain textures and positions in order to render the map properly.
	/// </summary>
	public class Tile : IDisposable, IDrawable
	{
		public static int EmptyTileID = 0;

		/// <summary>
		/// World grid is the world space tile grid. This is the index within the world grid that the tile is positioned.
		/// </summary>
		public Vector WorldGridIndex { get; internal set; }

		/// <summary>
		/// World position is the position within the world space
		/// </summary>
		public Vector WorldPosition { get; internal set; }

		/// <summary>
		/// Projected position is the calculated position for rendering based on world space position
		/// </summary>
		public Vector ProjectedPosition { get; internal set; }

		/// <summary>
		/// Depth determines the rendering order of the tile as compared to other drawable objects with depth
		/// </summary>
		public float Depth { get { return WorldPosition.X + WorldPosition.Y; } }

		/// <summary>
		/// Width of the tile in pixels
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// Height of the tile in pixels
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Texture from which to select a rectangle source texture (similar to selecting from a sprite sheet)
		/// </summary>
		public Texture Texture { get; private set; }

		/// <summary>
		/// Rectangle determining where in the Texture to select the specific texture for our sprite or frame
		/// </summary>
		public Rectangle SourceTextureBounds { get; private set; }

		/// <summary>
		/// Tiles are empty if they have no texture assigned within Tiled Map Editor
		/// </summary>
		public bool IsEmpty { get; private set; }

		/// <summary>
		/// Default empty constructor creates an empty tile
		/// </summary>
		public Tile()
		{
			IsEmpty = true;
		}

		/// <summary>
		/// Main constructor used to instantiate a tile when data is known at import
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="source"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Tile(Texture texture, Rectangle source, int width, int height)
		{
			IsEmpty = false;
			Texture = texture;
			SourceTextureBounds = source;
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Draws the tile to the passed renderer if the tile is not empty. The draw will occur at the center of the tile's texture.
		/// TODO: Allow passing of origin?
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="renderer"></param>
		public void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!IsEmpty)
			{
				renderer.RenderTexture(
					Texture,
					ProjectedPosition.X - Width * 0.5f - Camera.Position.X,
					ProjectedPosition.Y - Height * 0.5f - Camera.Position.Y,
					SourceTextureBounds
				);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Tile()
		{
			Dispose(false);
		}

		private void Dispose(bool isDisposing)
		{
			if (Texture != null)
				Texture.Dispose();
		}
	}

	/// <summary>A TileLayer is a representation of a tile layer in a .tmx file. These layers contain Tile objects which can be accessed
	/// in order to render the textures associated with the tiles.
	/// </summary>
	public class TileLayer : IDisposable
	{
		private List<Tile> tiles = new List<Tile>();

		public string Name { get; private set; }
		public TileLayerType Type { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public IReadOnlyList<Tile> Tiles { get { return tiles; } }

		public TileLayer(string name, int width, int height)
		{
			Name = name;
			Width = width;
			Height = height;

			if (Name == "Floor")
				Type = TileLayerType.Base;
			else if (Name == "Walls")
				Type = TileLayerType.Height;
			else
				throw new ArgumentOutOfRangeException("Unknown layer type");
		}

		public void AddTile(Tile tile)
		{
			tiles.Add(tile);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~TileLayer()
		{
			Dispose(false);
		}

		private void Dispose(bool isDisposing)
		{
			foreach (Tile tile in tiles)
				tile.Dispose();
		}
	}

	public class MapObject : ICollidable, INode, IHasNeighbors<MapObject>
	{
		private List<MapObject> neighbors = new List<MapObject>();

		public string Name { get; private set; }
		public MapObjectType Type { get; private set; }
		public Rectangle Bounds { get; set; }
		/// <summary>
		/// World position is the position within world space that the object exists. This position is defined within the .tmx tiled map file. When in isometric,
		/// it seems that the first rows of the collection are actually shifted by -32 in both X and Y directions.
		/// For example, in an isometric map with tile dimensions 64x32:
		///		The tile at [0,0] is positioned at [-32,-32].
		///		The tile at [0,1] is positioned at [-32,0].
		///		The tile at [1,0] is positioned at [0,-32].
		///	Because of that craziness, I am forced to shift the X and Y coordinates in the positive direction by the bounds and height, respectively.
		///	For example:
		///		The tile at [0,0] will be positioned (when shifted) at [0, 0].
		///		The tile at [0,1] will be positioned (when shifted) at [0, 32].
		///		The tile at [1,0] will be positioned (when shifted) at [32, 0].
		/// </summary>
		public Vector WorldPosition { get { return new Vector(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height); } }
		public Orientation Orientation { get; set; }
		public Rectangle CollisionBox { get { return Bounds; } }

		public Vector WorldGridIndex
		{
			get
			{
				Vector worldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(WorldPosition.X, WorldPosition.Y, Bounds.Width, Bounds.Height);
				int worldGridIndexX = (int)Math.Round(worldGridIndex.X);
				int worldGridIndexY = (int)Math.Round(worldGridIndex.Y);
				//return new Vector(worldGridIndexX + 1, worldGridIndexY + 1);
				return new Vector(worldGridIndexX, worldGridIndexY);
			}
		}

		public IEnumerable<MapObject> Neighbors { get { return neighbors; } }

		public MapObject(string name, Rectangle bounds, Orientation orientation, MapObjectType type)
		{
			Name = name;
			Type = type;
			Bounds = bounds;
			Orientation = orientation;
		}

		public void ResolveCollision(ICollidable i) { }

		public void AddNeighbor(MapObject mapObject)
		{
			if(mapObject != null)
				neighbors.Add(mapObject);
		}
	}

	public class MapObjectLayer
	{
		private List<MapObject> mapObjects = new List<MapObject>();

		public string Name { get; private set; }
		public MapObjectLayerType Type { get; private set; }
		public IEnumerable<MapObject> MapObjects { get { return mapObjects; } }

		public MapObjectLayer(string name, MapObjectLayerType type)
		{
			Name = name;
			Type = type;
		}

		public MapObject GetMapObjectAtWorldGridIndex(Vector worldGridIndex)
		{
			MapObject mapObject = MapObjects.FirstOrDefault(mo => mo.WorldGridIndex == worldGridIndex);
			return mapObject;
		}

		public void AddMapObject(MapObject mapObject)
		{
			mapObjects.Add(mapObject);
		}
	}

	/// <summary>A TiledMap is a representation of a .tmx map created with the Tiled Map Editor. You can access layers and tiles within
	/// layers by accessing the properties of this class after instantiation.
	/// </summary>
	public class TiledMap : IDisposable
	{
		private List<TileLayer> tileLayers = new List<TileLayer>();
		private List<MapObjectLayer> mapObjectLayers = new List<MapObjectLayer>();

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

			foreach (LayerContent layer in mapContent.Layers)
			{
				if (layer is TileLayerContent)
				{
					TileLayerContent tileLayerContent = layer as TileLayerContent;
					TileLayer tileLayer = new TileLayer(layer.Name, tileLayerContent.Width, tileLayerContent.Height);

					for (int i = 0; i < tileLayerContent.Data.Length; i++)
					{
						// strip out the flipped flags from the map editor to get the real tile index
						uint tileID = tileLayerContent.Data[i];
						uint flippedHorizontallyFlag = 0x80000000;
						uint flippedVerticallyFlag = 0x40000000;
						int tileIndex = (int)(tileID & ~(flippedVerticallyFlag | flippedHorizontallyFlag));

						Tile tile = CreateTile(tileIndex, mapContent.TileSets);

						tileLayer.AddTile(tile);
					}

					tileLayers.Add(tileLayer);
				}
				else if (layer is ObjectLayerContent)
				{
					MapObjectLayer mapObjectLayer = CreateObjectLayer(layer, mapContent.Orientation);
					mapObjectLayers.Add(mapObjectLayer);
				}
			}

			CalculateTilePositions(mapContent.Orientation);
			CalculatePathNodeNeighbors();
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
					Vector pathNodeWorldGridIndex = pathNode.WorldGridIndex;

					// get left neighbor
					Vector leftNeighborIndex = new Vector(pathNodeWorldGridIndex.X - 1, pathNodeWorldGridIndex.Y);
					MapObject leftNeighbor = pathNodeLayer.GetMapObjectAtWorldGridIndex(leftNeighborIndex);
					pathNode.AddNeighbor(leftNeighbor);


					// get right neighbor
					Vector rightNeighborIndex = new Vector(pathNodeWorldGridIndex.X + 1, pathNodeWorldGridIndex.Y);
					MapObject rightNeighbor = pathNodeLayer.GetMapObjectAtWorldGridIndex(rightNeighborIndex);
					pathNode.AddNeighbor(rightNeighbor);

					// get top neighbor
					Vector topNeighborIndex = new Vector(pathNodeWorldGridIndex.X, pathNodeWorldGridIndex.Y - 1);
					MapObject topNeighbor = pathNodeLayer.GetMapObjectAtWorldGridIndex(topNeighborIndex);
					pathNode.AddNeighbor(topNeighbor);

					// get bottom neighbor
					Vector bottomNeighborIndex = new Vector(pathNodeWorldGridIndex.X, pathNodeWorldGridIndex.Y + 1);
					MapObject bottomNeighbor = pathNodeLayer.GetMapObjectAtWorldGridIndex(bottomNeighborIndex);
					pathNode.AddNeighbor(bottomNeighbor);
				}
			}
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
			if (objectLayerContent.Name == "Collidables")
			{
				mapObjectLayerType = MapObjectLayerType.Collidable;
				mapObjectType = MapObjectType.Collidable;
			}
			else if (objectLayerContent.Name == "PathNodes")
			{
				mapObjectLayerType = MapObjectLayerType.PathNode;
				mapObjectType = MapObjectType.PathNode;
			}
			else
				throw new Exception("Unknown map object layer type. Did you name your layer correctly? \"Collidables\" will be picked up as collidable objects. \"PathNodes\" will be picked up as path node objects.");

			MapObjectLayer mapObjectLayer = new MapObjectLayer(objectLayerContent.Name, mapObjectLayerType);

			foreach (ObjectContent objectContent in objectLayerContent.MapObjects)
			{
				MapObject mapObject = new MapObject(objectContent.Name, objectContent.Bounds, orientation, mapObjectType);
				mapObjectLayer.AddMapObject(mapObject);
			}

			return mapObjectLayer;
		}

		/// <summary>Based on a passed tile index, create a Tile by looking up which TileSet it belongs to, assign the proper TilSet texture,
		/// and find the bounds of the rectangle that encompasses the correct tile texture within the total tileset texture.
		/// </summary>
		/// <param name="tileIndex">Index of the tile (GID) within the map file</param>
		/// <param name="tileSets">Enumerable list of tilesets used to find out which tileset a tile belongs to</param>
		/// <returns></returns>
		private Tile CreateTile(int tileIndex, IEnumerable<TileSetContent> tileSets)
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

				tile = new Tile(tileSetTexture, source, TileWidth, TileHeight);
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
						tile.WorldGridIndex = new Vector(x, y);
					}
				}
			}
		}

		/// <summary>
		/// Attempts to find a path node at the passed world grid index in the path node layers and returns the path node
		/// at that position. Throws exception if no path node is found.
		/// </summary>
		/// <param name="worldGridIndex"></param>
		/// <returns></returns>
		public MapObject GetPathNodeAtWorldGridIndex(Vector worldGridIndex)
		{
			IEnumerable<MapObjectLayer> pathNodeLayers = mapObjectLayers.Where(mol => mol.Type == MapObjectLayerType.PathNode);
			foreach (var pathNodeLayer in pathNodeLayers)
				foreach (var pathNode in pathNodeLayer.MapObjects)
					if (pathNode.WorldGridIndex == worldGridIndex)
						return pathNode;

			throw new Exception(String.Format("No path node found at [{0},{1}]", worldGridIndex.X, worldGridIndex.Y));
		}

		/// <summary>
		/// Finds the nodes at the passed world grid indices and returns a queue of map objects to travel along in order to get from start
		/// to end.
		/// </summary>
		/// <param name="startWorldGridIndex"></param>
		/// <param name="endWorldGridIndex"></param>
		/// <returns></returns>
		public Queue<MapObject> FindBestPath(Vector startWorldGridIndex, Vector endWorldGridIndex)
		{
			MapObject start = GetPathNodeAtWorldGridIndex(startWorldGridIndex);
			MapObject end = GetPathNodeAtWorldGridIndex(endWorldGridIndex);
			Path<MapObject> bestPath = FindPath<MapObject>(start, end, ExactDistance, ManhattanDistance);
			IEnumerable<MapObject> bestPathReversed = bestPath.Reverse();
			Queue<MapObject> result = new Queue<MapObject>();
			foreach (var bestPathNode in bestPathReversed)
				result.Enqueue(bestPathNode);
			return result;
		}

		/// <summary>
		/// An implementation of the A* path finding algorithm. Finds the best path betwen the passed start and end nodes while utilizing
		/// the passed exact distance function and estimated heuristic distance function.
		/// </summary>
		/// <typeparam name="Node"></typeparam>
		/// <param name="start"></param>
		/// <param name="destination"></param>
		/// <param name="distance"></param>
		/// <param name="estimate"></param>
		/// <returns></returns>
		private Path<Node> FindPath<Node>(
			Node start,							// starting node
			Node destination,					// destination node
			Func<Node, Node, double> distance,	// takes two nodes and calculates a distance cost between them
			Func<Node, Node, double> estimate)		// takes a node and calculates an estimated distance between current node
			where Node : IHasNeighbors<Node>
		{
			var closed = new HashSet<Node>();
			var queue = new PriorityQueue<double, Path<Node>>();

			queue.Enqueue(0, new Path<Node>(start));

			while (!queue.IsEmpty)
			{
				var path = queue.Dequeue();

				if (closed.Contains(path.LastStep))
					continue;

				if (path.LastStep.Equals(destination))
					return path;

				closed.Add(path.LastStep);

				foreach (Node n in path.LastStep.Neighbors)
				{
					double d = distance(path.LastStep, n);
					var newPath = path.AddStep(n, d);
					queue.Enqueue(newPath.TotalCost + estimate(n, destination), newPath);
				}
			}

			return null;
		}

		/// <summary>
		/// The exact distance between two nodes in this game is a single node (1).
		/// </summary>
		/// <typeparam name="Node"></typeparam>
		/// <param name="node1"></param>
		/// <param name="node2"></param>
		/// <returns></returns>
		private double ExactDistance<Node>(Node node1, Node node2)
			where Node : INode
		{
			return 1.0;
		}

		/// <summary>
		/// The manhattan distance between two nodes is the distance traveled on a taxicab-like grid where diagonal movement is not allowed.
		/// </summary>
		/// <typeparam name="Node"></typeparam>
		/// <param name="node1"></param>
		/// <param name="node2"></param>
		/// <returns></returns>
		private double ManhattanDistance<Node>(Node node1, Node node2)
			where Node : INode
		{
			return Math.Abs(node1.WorldGridIndex.X - node2.WorldGridIndex.X) + Math.Abs(node1.WorldGridIndex.Y - node2.WorldGridIndex.Y);
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
