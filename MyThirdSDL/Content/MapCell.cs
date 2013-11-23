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
	public class MapCell : IDisposable, IDrawable
	{
		private Dictionary<int, List<Tile>> tilesByIndexZ = new Dictionary<int, List<Tile>>();
		private List<MapObject> deadZones = new List<MapObject>();
		private List<PathNode> pathNodes = new List<PathNode>();

		public IEnumerable<PathNode> PathNodes { get { return pathNodes; } }

		public Point WorldGridIndex { get; internal set; }

		public Vector WorldPosition { get; internal set; }

		public Vector ProjectedPosition { get; internal set; }

		public float Depth { get { return WorldPosition.X + WorldPosition.Y; } }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public Rectangle Bounds { get { return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, Width, Height); } }

		public MapCell(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			foreach (int zIndex in tilesByIndexZ.Keys)
			{
				List<Tile> tiles = new List<Tile>();

				bool success = tilesByIndexZ.TryGetValue(zIndex, out tiles);
				if (success)
					foreach (var tile in tiles)
						tile.Draw(gameTime, renderer);
			}
		}

		public void AddDeadZone(MapObject deadZone)
		{
			if(!deadZones.Contains(deadZone))
				deadZones.Add(deadZone);
		}

		public void AddPathNode(PathNode pathNode)
		{
			if (!pathNodes.Contains(pathNode))
				pathNodes.Add(pathNode);
		}

		public void AddTile(Tile tile, int zIndex)
		{
			List<Tile> tiles = new List<Tile>();

			bool success = tilesByIndexZ.TryGetValue(zIndex, out tiles);
			if (success)
				tiles.Add(tile);
			else
			{
				tiles = new List<Tile>();
				tiles.Add(tile);
				tilesByIndexZ.Add(zIndex, tiles);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~MapCell()
		{
			Dispose(false);
		}

		private void Dispose(bool isDisposing)
		{
			foreach (var tiles in tilesByIndexZ.Values)
				foreach (var tile in tiles)
					tile.Dispose();
		}
	}
}
