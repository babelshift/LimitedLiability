using MyThirdSDL.Simulation;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyThirdSDL.Content
{
	public class MapCell : IDrawable
	{
		private PathNode containedPathNode;

		private List<IDrawable> drawableObjects = new List<IDrawable>();

		/// <summary>
		/// A MapCell can optionally consist of a single path node. This value
		/// </summary>
		public PathNode ContainedPathNode
		{
			get { return containedPathNode; }
			set
			{
				if (Type != MapCellType.PathNode)
					throw new InvalidOperationException("MapCells can only contain a path node if the map cell is of type 'PathNode'.");

				containedPathNode = value;
			}
		}

		public Tile BaseTile { get; set; }

		public Texture BaseTexture { get { return BaseTile.Texture; } }

		public MapCellType Type { get; set; }

		public Guid ID { get; private set; }

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
			BaseTile.Draw(gameTime, renderer);

			foreach (var drawable in drawableObjects)
				drawable.Draw(gameTime, renderer);
		}

		public void AddDrawableObject(IDrawable drawable)
		{
			if (drawableObjects.Any(d => d.ID == drawable.ID))
				throw new InvalidOperationException("Cannot add the same object to the same map cell twice.");

			drawableObjects.Add(drawable);
		}
	}
}