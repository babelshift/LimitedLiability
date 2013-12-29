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
		public TrueTypeText TrueTypeText { get; set; }

		public string Text
		{
			get { return TrueTypeText.Text; }
			set { TrueTypeText.UpdateText(value); }
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
