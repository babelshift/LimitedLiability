using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class Necessities
	{
		public enum Type
		{
			Sleep,
			Health,
			Hygiene,
			Hunger,
			Thirst,
			Happiness
		}

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
		private double healthRating;
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

		private string RatingToString(Rating rating)
		{
			int ratingRaw = (int)rating;
			if (ratingRaw < 4)
				return "Bad";
			else if (ratingRaw >= 4 && ratingRaw <= 6)
				return "OK";
			else if (ratingRaw > 6)
				return "Good";
			else
				return "???";
		}

		public string SleepToString()
		{
			return RatingToString(Sleep);
		}

		public string HealthToString()
		{
			return RatingToString(Health);
		}

		public string HygieneToString()
		{
			return RatingToString(Hygiene);
		}

		public string HungerToString()
		{
			return RatingToString(Hunger);
		}

		public string ThirstToString()
		{
			return RatingToString(Thirst);
		}

		public void AdjustSleep(double sleepRatingAdjustment)
		{
			this.sleepRating = AdjustRating(this.sleepRating, sleepRatingAdjustment);
		}

		public void AdjustHealth(double healthRatingAdjustment)
		{
			this.healthRating = AdjustRating(this.healthRating, healthRatingAdjustment);
		}

		public void AdjustHygiene(double hygieneRatingAdjustment)
		{
			this.hygieneRating = AdjustRating(this.hygieneRating, hygieneRatingAdjustment);
		}

		public void AdjustHunger(double hungerRatingAdjustment)
		{
			this.hungerRating = AdjustRating(this.hungerRating, hungerRatingAdjustment);
		}

		public void AdjustThirst(double thirstRatingAdjustment)
		{
			this.thirstRating = AdjustRating(this.thirstRating, thirstRatingAdjustment);
		}

		private double AdjustRating(double currentRating, double ratingAdjustment)
		{
			double possibleReturnValue = currentRating + ratingAdjustment;

			//log.Debug(String.Format("Current Rating: {0}, Rating Adjustment: {1}, New Rating: {2},", currentRating, ratingAdjustment, possibleReturnValue));

			if (possibleReturnValue >= 0.0 && possibleReturnValue <= 10.0)
				return possibleReturnValue;
			else if (possibleReturnValue <= 0.0)
				return 0.0;
			else if (possibleReturnValue >= 10.0)
				return 10.0;
			else
				throw new Exception(String.Format("The rating value was adjusted to a position that has confused the simulation. Value is: {0}", possibleReturnValue));
		}

		private Rating GetRatingFromDouble(double rating)
		{
			return (Rating)((int)Math.Round(rating));
		}
	}
}
