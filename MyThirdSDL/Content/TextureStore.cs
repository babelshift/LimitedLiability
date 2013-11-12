using SharpDL.Graphics;
using System;
using System.Collections.Generic;
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
	}
}
