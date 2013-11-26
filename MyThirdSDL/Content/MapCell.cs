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
using MyThirdSDL.Agents;

namespace MyThirdSDL.Content
{
	public class MapCell : IDrawable
	{
        private SortedDictionary<int, List<IDrawable>> drawablesByZIndex = new SortedDictionary<int, List<IDrawable>>();
		private List<MapObject> deadZones = new List<MapObject>();
        private List<PathNode> pathNodes = new List<PathNode>();

        public Guid ID { get; private set; }

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
            ID = Guid.NewGuid();
			Width = width;
			Height = height;
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
            List<Texture> orderedTexturesToDraw = new List<Texture>();

            foreach (int zIndex in drawablesByZIndex.Keys)
            {
                List<IDrawable> drawables = new List<IDrawable>();
                bool success = drawablesByZIndex.TryGetValue(zIndex, out drawables);
                if (success)
                    foreach (var drawable in drawables)
                        drawable.Draw(gameTime, renderer);
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

		public void AddDrawable(IDrawable drawable, int zIndex)
		{
			List<IDrawable> drawables = new List<IDrawable>();

			bool success = drawablesByZIndex.TryGetValue(zIndex, out drawables);
			if (success)
				drawables.Add(drawable);
			else
			{
				drawables = new List<IDrawable>();
				drawables.Add(drawable);
				drawablesByZIndex.Add(zIndex, drawables);
			}
		}
	}
}
