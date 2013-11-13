using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.Agents
{
	public class WaterFountain : Equipment, IPurchasable
	{
		private const int price = 25;
		private const string name = "Water Fountain";

		public NecessityEffects NecessityEffects { get; private set; }
		public string IconTextureKey { get { return "IconWater"; } }

		public WaterFountain(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityEffects = new NecessityEffects(1, 0, 0, 1, 0);
		}
	}
}
