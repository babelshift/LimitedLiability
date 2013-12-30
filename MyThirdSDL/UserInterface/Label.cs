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
				renderer.RenderTexture(TrueTypeText.Texture, Position.X, Position.Y);
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
