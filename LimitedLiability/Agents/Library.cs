using LimitedLiability.Content;

namespace LimitedLiability.Agents
{
	public class Library : Room
	{
		public Library(string name, int price, string description, string iconTextureKey, TiledMap tiledMap)
			: base(name, price, description, iconTextureKey, tiledMap)
		{
			NecessityEffect = new NecessityEffect(0, 0, -1, 0, 0);
			SkillEffect = new SkillEffect(1, 1, 0, 0);
		}
	}
}