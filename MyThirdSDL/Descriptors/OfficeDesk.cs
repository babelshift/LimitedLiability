using System;
using SharpDL.Graphics;

namespace MyThirdSDL.Descriptors
{
	public class OfficeDesk : Equipment, IPurchasable
	{
		private const int price = 50;
		private const string name = "Office Desk";

		public int HealthEffectiveness { get; private set; }
		public int HygieneEffectiveness { get; private set; }
		public int SleepEffectiveness { get; private set; }
		public int ThirstEffectiveness { get; private set; }
		public int HungerEffectiveness { get; private set; }
		public string IconTextureKey { get { return "IconOfficeDesk"; } }

		public OfficeDesk(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			HealthEffectiveness = 0;
			HygieneEffectiveness = 0;
			SleepEffectiveness = 0;
			ThirstEffectiveness = 0;
			HungerEffectiveness = 0;
        }
    }
}

