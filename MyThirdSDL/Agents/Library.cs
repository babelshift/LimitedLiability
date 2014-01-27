using MyThirdSDL.Content;

namespace MyThirdSDL.Agents
{
	public class Library : Room
	{
		public Library(string name, int price, string iconTextureKey, TiledMap tiledMap)
			: base(name, price, iconTextureKey, tiledMap)
		{
			NecessityEffect = new NecessityEffect(0, 0, -1, 0, 0);
			SkillEffect = new SkillEffect(1, 1, 0, 0);
		}
	}
}