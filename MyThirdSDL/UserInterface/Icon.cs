﻿using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Icon : Control, IDisposable
	{
		private Texture textureFrame;

		public Texture TextureFrame
		{
			get { return textureFrame; }
			set
			{
				textureFrame = value;
				Width = textureFrame.Width;
				Height = textureFrame.Height;
			}
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			renderer.RenderTexture(textureFrame, Position.X, Position.Y);
		}

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Icon()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (textureFrame != null)
				textureFrame.Dispose();
		}
	}
}
