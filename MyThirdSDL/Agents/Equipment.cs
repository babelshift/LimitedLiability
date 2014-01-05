using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Agents
{
	public abstract class Equipment : Agent
	{
		public int Price { get; private set; }

		public Equipment(TimeSpan birthTime, string agentName, TextureBook textureBook, Vector startingPosition, AgentOrientation orientation, int price)
			: base(birthTime, agentName, textureBook, startingPosition, orientation)
		{
			Price = price;
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
