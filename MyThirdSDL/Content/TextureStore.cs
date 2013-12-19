using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Content
{
	public class TextureStore
	{
		private Dictionary<string, Texture> textureDictionary = new Dictionary<string, Texture>();
		private Renderer renderer;

		public TextureStore(Renderer renderer)
		{
			this.renderer = renderer;
		}

		public Texture GetTextureCopy(string texturePath)
		{
			return CreateTexture(texturePath);
		}

		public Texture GetTexture(string texturePath)
		{
			Texture texture;

			if (!textureDictionary.TryGetValue(texturePath, out texture))
			{
				texture = CreateTexture(texturePath);
				textureDictionary.Add(texturePath, texture);
			}

			return texture;
		}

		private Texture CreateTexture(string texturePath)
		{
			Surface surface = new Surface(texturePath, SurfaceType.PNG);
			Texture texture = new Texture(renderer, surface);
			return texture;
		}

		public TextureBook GetTextureBook(string contentPath)
		{
			string fileExtension = Path.GetExtension(contentPath);

			string contentTopLeftPath = contentPath.Replace(String.Format("{0}", fileExtension), String.Format("TopLeft{0}", fileExtension));
			string contentTopRightPath = contentPath.Replace(String.Format("{0}", fileExtension), String.Format("TopRight{0}", fileExtension));
			string contentBottomRightPath = contentPath.Replace(String.Format("{0}", fileExtension), String.Format("BottomRight{0}", fileExtension));
			string contentBottomLeftPath = contentPath.Replace(String.Format("{0}", fileExtension), String.Format("BottomLeft{0}", fileExtension));

			Texture textureTopLeft = null;
			if (File.Exists(contentTopLeftPath))
			{
				Surface surfaceTopLeft = new Surface(contentTopLeftPath, SurfaceType.PNG);
				textureTopLeft = new Texture(renderer, surfaceTopLeft);
			}
			else
			{
				Surface surface = new Surface(contentPath, SurfaceType.PNG);
				textureTopLeft = new Texture(renderer, surface);
			}
			
			Texture textureTopRight = null;
			Texture textureBottomRight = null;
			Texture textureBottomLeft = null;

			if (File.Exists(contentTopRightPath))
			{
				Surface surfaceTopRight = new Surface(contentTopRightPath, SurfaceType.PNG);
				textureTopRight = new Texture(renderer, surfaceTopRight);
			}

			if (File.Exists(contentBottomRightPath))
			{
				Surface surfaceBottomRight = new Surface(contentBottomRightPath, SurfaceType.PNG);
				textureBottomRight = new Texture(renderer, surfaceBottomRight);
			}

			if (File.Exists(contentBottomLeftPath))
			{
				Surface surfaceBottomLeft = new Surface(contentBottomLeftPath, SurfaceType.PNG);
				textureBottomLeft = new Texture(renderer, surfaceBottomLeft);
			}

			return new TextureBook(textureTopLeft, textureBottomLeft, textureTopRight, textureBottomRight);
		}
	}
}
