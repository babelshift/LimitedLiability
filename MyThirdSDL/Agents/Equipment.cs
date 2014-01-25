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

		public IReadOnlyList<Texture> ActiveTextures { get { return new List<Texture>() { ActiveTexture }; } }

		public Equipment(TimeSpan birthTime, string agentName, Texture activeTexture, Vector startingPosition, int price, string iconTextureKey)
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
	}
}