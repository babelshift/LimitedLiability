using MyThirdSDL.Descriptors;
using SharpDL.Graphics;
using SharpTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Agents
{
	public class Wall : Room
	{
		private const int widthInTiles = 1;
		private const int heightInTiles = 1;
		private const int price = 10;
		private const string name = "Wall";
		private const string iconTexturekey = "IconWall";

		public Wall()
			: base(name, price, widthInTiles, heightInTiles, iconTexturekey)
		{
			NecessityEffect = new NecessityEffect(0, 0, 0, 0, 0);
			SkillEffect = new SkillEffect(0, 0, 0, 0);
		}
	}
}