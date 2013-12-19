using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;

namespace MyThirdSDL.Agents
{
	public class Library : Room, IPurchasable
	{
		private const int widthInTiles = 5;
		private const int heightInTiles = 5;
		private const int price = 500;
		private const string name = "Library";

		public NecessityEffect NecessityEffect { get; private set; }
		public SkillEffect SkillEffect { get; private set; }

		public string IconTextureKey { get { return "IconLibrary"; } }

		public Library(TimeSpan birthTime, string agentName, TextureBook textureBook, Vector startingPosition, AgentOrientation orientation)
			: base(birthTime, name, textureBook, startingPosition, orientation, widthInTiles, heightInTiles, price)
		{
			NecessityEffect = new NecessityEffect(0, 0, -1, 0, 0);
			SkillEffect = new SkillEffect(1, 1, 0, 0);
		}
	}
}
