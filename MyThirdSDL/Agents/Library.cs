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

		public NecessityEffects NecessityEffects { get; private set; }
		public SkillEffects SkillEffects { get; private set; }

		public string IconTextureKey { get { return "IconLibrary"; } }

		public Library(TimeSpan birthTime, string agentName, Texture texture, Vector position)
			: base(birthTime, name, texture, position, widthInTiles, heightInTiles, price)
		{
			NecessityEffects = new NecessityEffects(0, 0, -1, 0, 0);
			SkillEffects = new SkillEffects(1, 1, 0, 0);
		}
	}
}
