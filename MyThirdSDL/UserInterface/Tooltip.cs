using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Tooltip : Control
	{
		public Texture TextureFrame { get; set; }
		public Label Label { get; set; }

		public Tooltip()
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (Label != null)
				Label.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (TextureFrame != null)
				renderer.RenderTexture(TextureFrame, Position.X, Position.Y);
			if(Label != null)
				Label.Draw(gameTime, renderer);
		}
	}
}
