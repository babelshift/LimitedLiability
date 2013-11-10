using System;

namespace MyThirdSDL
{
    public class NecessityAffector
	{
		public int HealthEffectiveness { get; private set; }
		public int HygieneEffectiveness { get; private set; }
		public int SleepEffectiveness { get; private set; }
		public int ThirstEffectiveness { get; private set; }
		public int HungerEffectiveness { get; private set; }

		public NecessityAffector(int health, int hygiene, int sleep, int thirst, int hunger)
		{
			HealthEffectiveness = health;
			HygieneEffectiveness = hygiene;
			SleepEffectiveness = sleep;
			ThirstEffectiveness = thirst;
			HungerEffectiveness = hunger;
        }
    }
}

