using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class SnackMachine : Equipment, IPurchasable
	{
		private const int price = 50;
		private const string name = "Snack Machine";

		public int HealthEffectiveness { get; private set; }
		public int HygieneEffectiveness { get; private set; }
		public int SleepEffectiveness { get; private set; }
		public int ThirstEffectiveness { get; private set; }
		public int HungerEffectiveness { get; private set; }
		public string IconTextureKey { get { return "IconPizza"; } }

		public SnackMachine(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			HealthEffectiveness = -1;
			HygieneEffectiveness = 0;
			SleepEffectiveness = 0;
			ThirstEffectiveness = 0;
			HungerEffectiveness = 2;
		}
	}
}
