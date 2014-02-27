using SharpDL;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	public class Tooltip : Control
	{
		private Texture textureFrame;

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

				Label.Position = base.Position + new Vector(7, Height / 2 - Label.Height / 2);
			}
		}

		public Tooltip(Texture textureFrame)
		{
			this.textureFrame = textureFrame;
			Width = textureFrame.Width;
			Height = textureFrame.Height;
		}

		public override void Update(GameTime gameTime)
		{
			if (!Visible) return;

			base.Update(gameTime);

			if (Label != null)
				Label.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible) return;

			if (textureFrame != null)
				textureFrame.Draw(Position.X, Position.Y);
			if (Label != null)
				Label.Draw(gameTime, renderer);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (textureFrame != null)
				textureFrame.Dispose();
			if (Label != null)
				Label.Dispose();
		}
	}
}