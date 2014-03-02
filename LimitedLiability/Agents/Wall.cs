using LimitedLiability.Content;

namespace LimitedLiability.Agents
{
	public class Wall : Room
	{
		public Wall(string name, int price, string description, string iconTextureKey, TiledMap tiledMap)
			: base(name, price, description, iconTextureKey, tiledMap)
		{
			NecessityEffect = new NecessityEffect(0, 0, 0, 0, 0);
			SkillEffect = new SkillEffect(0, 0, 0, 0);
		}
	}
}