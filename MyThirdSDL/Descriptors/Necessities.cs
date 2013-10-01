using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class Necessities
	{
		public enum Rating
		{
			Critical = 0,
			Severe = 1,
			Bad = 2,
			Poor = 3,
			Unsatisfactory = 4,
			Neutral = 5,
			Satisfactory = 6,
			Good = 7,
			Great = 8,
			Excellent = 9,
			Full = 10
		}

		public Rating Sleep { get; private set; }
		public Rating Health { get; private set; }
		public Rating Hygiene { get; private set; }
		public Rating Hunger { get; private set; }
		public Rating Thirst { get; private set; }

		public Necessities(Rating blanketRating)
		{
			Sleep = blanketRating;
			Health = blanketRating;
			Hygiene = blanketRating;
			Hunger = blanketRating;
			Thirst = blanketRating;
		}

		public Necessities(Rating sleep, Rating health, Rating hygiene, Rating hunger, Rating thirst)
		{
			Sleep = sleep;
			Health = health;
			Hygiene = hygiene;
			Hunger = hunger;
			Thirst = thirst;
		}

		public void AdjustSleep(int sleep)
		{
			Sleep += sleep;
		}

		public void AdjustHealth(int health)
		{
			Health += health;
		}

		public void AdjustHygiene(int Hygiene)
		{
			Hygiene += Hygiene;
		}

		public void AdjustHunger(int Hunger)
		{
			Hunger += Hunger;
		}

		public void AdjustThirst(int Thirst)
		{
			Thirst += Thirst;
		}
	}
}
