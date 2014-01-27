using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using SharpDL;
using SharpDL.Graphics;

namespace MyThirdSDL.Agents
{
	public class Room : IPurchasable
	{
		private TiledMap tiledMap;

		public NecessityEffect NecessityEffect { get; protected set; }

		public SkillEffect SkillEffect { get; protected set; }

		public string Name { get; private set; }

		public int Price { get; private set; }

		public string IconTextureKey { get; private set; }

		public int Width { get { return tiledMap.PixelWidth; } }

		public int Height { get { return tiledMap.PixelHeight; } }

		public Room(string name, int price, string iconTextureKey, TiledMap tiledMap)
		{
			Name = name;
			Price = price;
			IconTextureKey = iconTextureKey;
			this.tiledMap = tiledMap;
		}

		public void Draw(GameTime gameTime, Renderer renderer, int x, int y)
		{
			tiledMap.Draw(gameTime, renderer, x, y);
		}
	}
}