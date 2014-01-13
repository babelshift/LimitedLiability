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

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				Label.Position = base.Position + new Vector(7, 8);
			}
		}

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
			if (Visible)
			{
				if (TextureFrame != null)
					renderer.RenderTexture(TextureFrame, Position.X, Position.Y);
				if (Label != null)
					Label.Draw(gameTime, renderer);
			}
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (TextureFrame != null)
				TextureFrame.Dispose();
			if (Label != null)
				Label.Dispose();
		}
	}
}
