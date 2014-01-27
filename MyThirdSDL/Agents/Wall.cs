using MyThirdSDL.Content;

namespace MyThirdSDL.Agents
{
	public class Wall : Room
	{
		public Wall(string name, int price, string iconTextureKey, TiledMap tiledMap)
			: base(name, price, iconTextureKey, tiledMap)
		{
			NecessityEffect = new NecessityEffect(0, 0, 0, 0, 0);
			SkillEffect = new SkillEffect(0, 0, 0, 0);
		}
	}
}