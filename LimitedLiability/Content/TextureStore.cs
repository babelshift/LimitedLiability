using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace LimitedLiability.Content
{
	public class TextureStore
	{
		private Renderer renderer;

		public TextureStore(Renderer renderer)
		{
			this.renderer = renderer;
		}

		public Texture CreateTexture(string texturePath)
		{
			Surface surface = new Surface(texturePath, SurfaceType.PNG);
			Texture texture = new Texture(renderer, surface);
			return texture;
		}

		public TextureBook GetTextureBook(string contentPath)
		{
			if (String.IsNullOrEmpty(contentPath)) throw new ArgumentNullException("contentPath");

			string fileExtension = Path.GetExtension(contentPath);

			if (String.IsNullOrEmpty(fileExtension)) throw new ArgumentException("contentPath", String.Format("The path '{0}' does not have a valid file extension.", contentPath));

			string contentTopLeftPath = contentPath.Replace(fileExtension, String.Format("_FacingLeft{0}", fileExtension));
			string contentTopRightPath = contentPath.Replace(fileExtension, String.Format("_FacingRight{0}", fileExtension));
			string contentBottomRightPath = contentPath.Replace(fileExtension, String.Format("_FacingUp{0}", fileExtension));
			string contentBottomLeftPath = contentPath.Replace(fileExtension, String.Format("_FacingDown{0}", fileExtension));

			Texture textureTopLeft;
			if (File.Exists(contentTopLeftPath))
				textureTopLeft = CreateTexture(contentTopLeftPath);
			else
				textureTopLeft = CreateTexture(contentPath);

			Texture textureTopRight;
			Texture textureBottomRight;
			Texture textureBottomLeft;

			if (!File.Exists(contentTopRightPath))
				throw new IOException(String.Format("The file at path '{0}' does not exist.", contentTopRightPath));

			textureTopRight = CreateTexture(contentTopRightPath);

			if (!File.Exists(contentBottomRightPath))
				throw new IOException(String.Format("The file at path '{0}' does not exist.", contentBottomRightPath));

			textureBottomRight = CreateTexture(contentBottomRightPath);


			if (!File.Exists(contentBottomLeftPath))
				throw new IOException(String.Format("The file at path '{0}' does not exist.", contentBottomLeftPath));

			textureBottomLeft = CreateTexture(contentBottomLeftPath);

			return new TextureBook(textureTopLeft, textureBottomLeft, textureTopRight, textureBottomRight);
		}
	}
}