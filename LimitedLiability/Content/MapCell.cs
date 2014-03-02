using System.Runtime.InteropServices;
using LimitedLiability.Agents;
using LimitedLiability.Simulation;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LimitedLiability.Content
{
	public class MapCell : IDrawable
	{
		private PathNode containedPathNode;
		private Vector worldPosition;

		private List<Tile> tileObjects = new List<Tile>();

		public IEnumerable<Tile> TileObjects { get { return tileObjects; } }

		public Equipment OccupantEquipment { get; set; }

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

		public Tile FloorTile { get; set; }

		public MapCellType Type { get; set; }

		public Guid ID { get; private set; }

		public Point WorldGridIndex { get; internal set; }

		public Vector WorldPosition
		{
			get { return worldPosition; }
			internal set
			{
				worldPosition = value;

				if (FloorTile != null)
				{
					FloorTile.WorldGridIndex = WorldGridIndex;
					FloorTile.WorldPosition = worldPosition;
				}

				if (OccupantEquipment != null)
					OccupantEquipment.WorldPosition = worldPosition;

				foreach (var tileObject in tileObjects)
				{
					tileObject.WorldGridIndex = WorldGridIndex;
					tileObject.WorldPosition = worldPosition;
				}
			}
		}

		public Vector ProjectedPosition
		{
			get
			{
				Vector projectedPosition = CoordinateHelper.WorldSpaceToScreenSpace(
					WorldPosition.X,
					WorldPosition.Y,
					CoordinateHelper.ScreenOffset,
					CoordinateHelper.ScreenProjectionType.Orthogonal
				);

				return projectedPosition;
			}
		}

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
			FloorTile.Draw(gameTime, renderer);

			if (OccupantEquipment != null)
				OccupantEquipment.Draw(gameTime, renderer);

			foreach (var tileObject in tileObjects)
				tileObject.Draw(gameTime, renderer);
		}

		public void Draw(GameTime gameTime, Renderer renderer, int x, int y, bool isOverlappingDeadZoneOverride)
		{
			x += (int)ProjectedPosition.X;
			y += (int)ProjectedPosition.Y;

			FloorTile.Draw(gameTime, renderer, x, y, isOverlappingDeadZoneOverride);

			if (OccupantEquipment != null)
				OccupantEquipment.Draw(gameTime, renderer, x, y, isOverlappingDeadZoneOverride);

			foreach (var tileObject in tileObjects)
				tileObject.Draw(gameTime, renderer, x, y, isOverlappingDeadZoneOverride);
		}

		public void AddTileObject(Tile tileObject)
		{
			if (tileObjects.Any(d => d.ID == tileObject.ID))
				throw new InvalidOperationException("Cannot add the same object to the same map cell twice.");

			tileObjects.Add(tileObject);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (FloorTile != null)
				FloorTile.Dispose();
			if (OccupantEquipment != null)
				OccupantEquipment.Dispose();
			foreach (var tileObject in tileObjects)
				tileObject.Dispose();
		}
	}
}