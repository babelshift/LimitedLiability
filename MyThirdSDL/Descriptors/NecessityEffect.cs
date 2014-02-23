using System;

namespace MyThirdSDL
{
	public class NecessityEffect
	{
		public int HealthEffectiveness { get; private set; }
		public int HygieneEffectiveness { get; private set; }
		public int SleepEffectiveness { get; private set; }
		public int ThirstEffectiveness { get; private set; }
		public int HungerEffectiveness { get; private set; }

		public NecessityEffect(int health, int hygiene, int sleep, int thirst, int hunger)
		{
			HealthEffectiveness = health;
			HygieneEffectiveness = hygiene;
			SleepEffectiveness = sleep;
			ThirstEffectiveness = thirst;
			HungerEffectiveness = hunger;
		}

		private string RatingToString(int effectiveness)
		{
			if (effectiveness <= 0)
				return "Bad";
			if (effectiveness >= 1 && effectiveness <= 4)
				return "OK";
			if (effectiveness >= 5 && effectiveness <= 8)
				return "Good";
			if (effectiveness >= 9)
				return "Excellent";
			return "???";
		}

		public string HealthEffectivenessToString()
		{
			return RatingToString(HealthEffectiveness);
		}

		public string HygieneEffectivenessToString()
		{
			return RatingToString(HygieneEffectiveness);
		}

		public string SleepEffectivenessToString()
		{
			return RatingToString(SleepEffectiveness);
		}

		public string ThirstEffectivenessToString()
		{
			return RatingToString(ThirstEffectiveness);
		}

		public string HungerEffectivenessToString()
		{
			return RatingToString(HungerEffectiveness);
		}
	}
}

