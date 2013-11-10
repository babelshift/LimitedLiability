using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class WaterFountain : Equipment, IPurchasable
	{
		private const int price = 25;
		private const string name = "Water Fountain";

		public NecessityAffector NecessityAffector { get; private set; }
		public string IconTextureKey { get { return "IconWater"; } }

		public WaterFountain(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityAffector = new NecessityAffector(1, 0, 0, 1, 0);
		}
	}
}
