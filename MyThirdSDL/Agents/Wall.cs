using MyThirdSDL.Descriptors;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Agents
{

	public class Wall : Room, IPurchasable
	{
		private const int widthInTiles = 1;
		private const int heightInTiles = 1;
		private const int price = 10;
		private const string name = "Wall";

		public NecessityEffect NecessityEffect { get; private set; }
		public SkillEffect SkillEffect { get; private set; }

		public string IconTextureKey { get { return "IconWall"; } }

		public Wall(TimeSpan birthTime, string agentName, TextureBook textureBook, Vector position, AgentOrientation orientation)
			: base(birthTime, name, textureBook, position, orientation, widthInTiles, heightInTiles, price)
		{
			NecessityEffect = new NecessityEffect(0, 0, 0, 0, 0);
			SkillEffect = new SkillEffect(0, 0, 0, 0);
			Orientation = AgentOrientation.TopLeft;
		}
	}
}
