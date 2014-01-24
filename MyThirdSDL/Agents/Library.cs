using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;

namespace MyThirdSDL.Agents
{
	public class Library : Room
	{
		private const int widthInTiles = 5;
		private const int heightInTiles = 5;
		private const int price = 500;
		private const string name = "Library";
		private const string iconTexturekey = "IconLibrary";

		public Library()
			: base(name, price, widthInTiles, heightInTiles, iconTexturekey)
		{
			NecessityEffect = new NecessityEffect(0, 0, -1, 0, 0);
			SkillEffect = new SkillEffect(1, 1, 0, 0);
		}
	}
}
