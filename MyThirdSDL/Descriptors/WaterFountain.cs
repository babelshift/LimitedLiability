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

		public int HealthEffectiveness { get; private set; }
		public int HygieneEffectiveness { get; private set; }
		public int SleepEffectiveness { get; private set; }
		public int ThirstEffectiveness { get; private set; }
		public int HungerEffectiveness { get; private set; }
		public string IconTextureKey { get { return "IconWater"; } }

		public WaterFountain(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			HealthEffectiveness = 1;
			HygieneEffectiveness = 0;
			SleepEffectiveness = 0;
			ThirstEffectiveness = 1;
			HungerEffectiveness = 0;
		}
	}
}
