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

		private double sleepRating;
		public double healthRating;
		private double hygieneRating;
		private double hungerRating;
		private double thirstRating;

		public Rating Sleep
		{ 
			get { return GetRatingFromDouble(sleepRating); } 
			private set { sleepRating = (double)value; }
		}

		public Rating Health
		{ 
			get { return GetRatingFromDouble(healthRating); } 
			private set { healthRating = (double)value; }
		}

		public Rating Hygiene
		{ 
			get { return GetRatingFromDouble(hygieneRating); } 
			private set { hygieneRating = (double)value; }
		}

		public Rating Hunger
		{ 
			get { return GetRatingFromDouble(hungerRating); } 
			private set { hungerRating = (double)value; }
		}

		public Rating Thirst
		{ 
			get { return GetRatingFromDouble(thirstRating); } 
			private set { thirstRating = (double)value; }
		}

		private Rating GetRatingFromDouble(double rating)
		{
			return (Rating)((int)Math.Round(rating));
		}

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

		public void AdjustSleep(double sleepRating)
		{
			if (this.sleepRating + sleepRating > 0)
				this.sleepRating += sleepRating;
			else
				this.sleepRating = 0.0;
		}

		public void AdjustHealth(double healthRating)
		{
			if (this.healthRating + healthRating > 0)
				this.healthRating += healthRating;
			else
				this.healthRating = 0.0;
		}

		public void AdjustHygiene(double hygieneRating)
		{
			if (this.hygieneRating + hygieneRating > 0)
				this.hygieneRating += hygieneRating;
			else
				this.hygieneRating = 0.0;
		}

		public void AdjustHunger(double hungerRating)
		{
			if (this.hungerRating + hungerRating > 0)
				this.hungerRating += hungerRating;
			else
				this.hungerRating = 0.0;
		}

		public void AdjustThirst(double thirstRating)
		{
			if (this.thirstRating + thirstRating > 0)
				this.thirstRating += thirstRating;
			else
				this.thirstRating = 0.0;
		}
	}
}
