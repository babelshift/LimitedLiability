﻿using MyThirdSDL.Content;
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
		private TrueTypeText trueTypeTextShadow;

		private int shadowOffsetX = 2;
		private int shadowOffsetY = 2;

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
				TrueTypeText.UpdateText(value, TrueTypeText.WrapLength);
				Width = TrueTypeText.Texture.Width;
				Height = TrueTypeText.Texture.Height;
			}
		}

		public void EnableShadow(ContentManager contentManager, int shadowOffsetX, int shadowOffsetY)
		{
			this.shadowOffsetX = shadowOffsetX;
			this.shadowOffsetY = shadowOffsetY;
			trueTypeTextShadow = contentManager.GetTrueTypeText(TrueTypeText.Font.FilePath, TrueTypeText.Font.PointSize, Styles.Colors.Black, Text, TrueTypeText.WrapLength);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (Visible)
			{
				if (TrueTypeText != null)
				{
					if (trueTypeTextShadow != null)
						renderer.RenderTexture(trueTypeTextShadow.Texture, Position.X + shadowOffsetX, Position.Y + shadowOffsetY);

					renderer.RenderTexture(TrueTypeText.Texture, Position.X, Position.Y);
				}
			}
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (TrueTypeText != null)
				TrueTypeText.Dispose();
		}
	}
}
