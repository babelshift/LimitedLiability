using System;
using System.Collections.Generic;
using System.Linq;
using SharpDL;
using SharpDL.Graphics;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.Agents
{
	public abstract class Equipment : Agent, IPurchasable
	{
		private double conditionDecayRate = -0.005;
		private double condition = 10;

		public int Price { get; private set; }

		public string Description { get; private set; }

		public NecessityEffect NecessityEffect { get; protected set; }

		public SkillEffect SkillEffect { get; protected set; }

		public string IconTextureKey { get; private set; }

		public int HorizontalMapCellCount { get { return 1; } }

		public int VerticalMapCellCount { get { return 1; } }	

		public EquipmentCondition Condition 
		{ 
			get
			{
				if (condition >= 0 && condition <= 3)
					return EquipmentCondition.Broken;
				if (condition > 3 && condition <= 7)
					return EquipmentCondition.Used;
				if (condition > 7 && condition <= 10)
					return EquipmentCondition.New;
				return EquipmentCondition.Unknown;
			}
		}

		protected Equipment(TimeSpan birthTime, string agentName, Texture activeTexture, Vector startingPosition, int price, string description, string iconTextureKey)
			: base(birthTime, agentName, startingPosition)
		{
			Price = price;
			IconTextureKey = iconTextureKey;
			ActiveTexture = activeTexture;
			Description = description;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			UpdateCondition(conditionDecayRate);
		}

		/// <summary>
		/// Equipment needs to be drawn offset if its height expands beyond more than one tile
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="renderer"></param>
		public override void Draw(GameTime gameTime, Renderer renderer)
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

		/// <summary>
		/// Draw used for exact positioning when the equipment is being hovered in the map for placement (before purchase).
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="renderer"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="isOverlappingDeadZone"></param>
		public override void Draw(GameTime gameTime, Renderer renderer, int x, int y, bool isOverlappingDeadZone)
		{
			// overlapping a deadzone shades the texture red
			if (isOverlappingDeadZone)
				renderer.SetTextureColorMod(ActiveTexture, 255, 0, 0);
			else
				renderer.SetTextureColorMod(ActiveTexture, 255, 255, 255);

			renderer.RenderTexture(
				ActiveTexture,
				x, y);
		}

		public bool IsOverlappingDeadZone(IReadOnlyList<MapCell> mapCells)
		{
			if (mapCells == null) throw new ArgumentNullException("mapCells");

			if (mapCells[0] == null) return true;

			Vector origin = mapCells[0].WorldPosition;

			Vector offsetPosition = new Vector(WorldPosition.X + origin.X, WorldPosition.Y + origin.Y);

			return mapCells
					.Where(mc => mc.Type == MapCellType.DeadZone)
					.Any(mc => mc.Bounds.Contains(offsetPosition));
		}

		private void UpdateCondition(double conditionDecayRate)
		{
			if (condition + conditionDecayRate >= 0)
				condition += conditionDecayRate;
		}
	}
}