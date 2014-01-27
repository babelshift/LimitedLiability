using System.Linq;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;

namespace MyThirdSDL.Agents
{
	public abstract class Equipment : Agent, IPurchasable
	{
		public int Price { get; private set; }

		public NecessityEffect NecessityEffect { get; protected set; }

		public SkillEffect SkillEffect { get; protected set; }

		public string IconTextureKey { get; private set; }

		public int HorizontalMapCellCount { get { return 1; } }

		public int VerticalMapCellCount { get { return 1; } }

		protected Equipment(TimeSpan birthTime, string agentName, Texture activeTexture, Vector startingPosition, int price, string iconTextureKey)
			: base(birthTime, agentName, startingPosition)
		{
			Price = price;
			IconTextureKey = iconTextureKey;
			ActiveTexture = activeTexture;
		}

		/// <summary>
		/// Equipment needs to be drawn offset if its height expands beyond more than one tile
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="renderer"></param>
		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			Vector drawPosition = new Vector(ProjectedPosition.X - Camera.Position.X, ProjectedPosition.Y - Camera.Position.Y);
			Vector offset = Vector.Zero;

			if (ActiveTexture.Height == CoordinateHelper.TileMapTileHeight * 2)
				offset = new Vector(0, CoordinateHelper.TileMapTileHeight);

			drawPosition -= offset;

			renderer.RenderTexture(
				ActiveTexture,
				drawPosition.X,
				drawPosition.Y
			);
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer, int x, int y)
		{
			if (isOverlappingDeadZone)
			{
				// alter the texture shaded red
				renderer.SetTextureColorMod(ActiveTexture, 150, 0, 0);
			}
			else
				renderer.SetTextureColorMod(ActiveTexture, 255, 255, 255);

			renderer.RenderTexture(
				ActiveTexture,
				x, y);

			lastDrawPositionX = x;
			lastDrawPositionY = y;
		}

		private int lastDrawPositionX;
		private int lastDrawPositionY;
		private bool isOverlappingDeadZone;

		public void CheckOverlap(IReadOnlyList<MapCell> mapCells)
		{
			if (mapCells == null) throw new ArgumentNullException("mapCells");

			if (mapCells[0] == null) return;

			Vector origin = mapCells[0].WorldPosition;

			Vector offsetPosition = new Vector(WorldPosition.X + origin.X, WorldPosition.Y + origin.Y);

			isOverlappingDeadZone =
				mapCells
					.Where(mc => mc.Type == MapCellType.DeadZone)
					.Any(mc => mc.Bounds.Contains(offsetPosition));

			int i = 0;
		}
	}
}