using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Label : Control, IDisposable
	{
		private TrueTypeText trueTypeText;
		private bool isOutlined;

		public Color OutlineColor { get; set; }
		public int OutlineSize { get; set; }

		public TrueTypeText TrueTypeText
		{
			get { return trueTypeText; }
			set
			{
				trueTypeText = value;
				Width = trueTypeText.Texture.Width;
				Height = trueTypeText.Texture.Height;
			}
		}

		public string Text
		{
			get { return TrueTypeText.Text; }
			set
			{
				TrueTypeText.UpdateText(value);
				Width = TrueTypeText.Texture.Width;
				Height = TrueTypeText.Texture.Height;
			}
		}

		public Label()
		{
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (TrueTypeText != null)
			{
				renderer.RenderTexture(TrueTypeText.Texture, Position.X, Position.Y);

				if (isOutlined)
				{
					Vector left = new Vector(Position.X - OutlineSize, Position.Y);
					Vector upLeft = new Vector(Position.X - OutlineSize, Position.Y - OutlineSize);
					Vector up = new Vector(Position.X, Position.Y - OutlineSize);
					Vector upRight = new Vector(Position.X + OutlineSize, Position.Y - OutlineSize);
					Vector right = new Vector(Position.X + OutlineSize, Position.Y);
					Vector downRight = new Vector(Position.X + OutlineSize, Position.Y + OutlineSize);
					Vector down = new Vector(Position.X, Position.Y + OutlineSize);
					Vector downLeft = new Vector(Position.X - OutlineSize, Position.Y + OutlineSize);

					renderer.RenderTexture(TrueTypeText.Texture, left.X, left.Y);
					renderer.RenderTexture(TrueTypeText.Texture, upLeft.X, upLeft.Y);
					renderer.RenderTexture(TrueTypeText.Texture, up.X, up.Y);
					renderer.RenderTexture(TrueTypeText.Texture, upRight.X, upRight.Y);
					renderer.RenderTexture(TrueTypeText.Texture, right.X, right.Y);
					renderer.RenderTexture(TrueTypeText.Texture, downRight.X, downRight.Y);
					renderer.RenderTexture(TrueTypeText.Texture, down.X, down.Y);
					renderer.RenderTexture(TrueTypeText.Texture, downLeft.X, downLeft.Y);
				}
			}
		}

		public void EnableOutline()
		{
			isOutlined = true;
		}

		public void DisableOutline()
		{
			isOutlined = false;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Label()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (TrueTypeText != null)
				TrueTypeText.Dispose();
		}
	}
}
