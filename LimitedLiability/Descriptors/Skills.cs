using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.Descriptors
{
	public class Skills
	{
		private static Random random = new Random();

		public enum Rating
		{
			Atrocious = 0,
			Awful = 1,
			Bad = 2,
			Poor = 3,
			Unsatisfactory = 4,
			Neutral = 5,
			Satisfactory = 6,
			Good = 7,
			Great = 8,
			Excellent = 9,
			Genius = 10
		}

		public Rating Intelligence { get; private set; }
		public Rating Creativity { get; private set; }
		public Rating Communication { get; private set; }
		public Rating Leadership { get; private set; }

		public Skills(Rating blanketRating)
		{
			Intelligence = blanketRating;
			Creativity = blanketRating;
			Communication = blanketRating;
			Leadership = blanketRating;
		}

		public Skills(Rating intelligence, Rating creativity, Rating communication, Rating leadership)
		{
			Intelligence = intelligence;
			Creativity = creativity;
			Communication = communication;
			Leadership = leadership;
		}

		private string RatingToString(Rating rating)
		{
			int ratingRaw = (int)rating;
			if (ratingRaw < 4)
				return "Bad";
			if (ratingRaw >= 4 && ratingRaw <= 6)
				return "OK";
			if (ratingRaw > 6)
				return "Good";
			return "???";
		}

		public string IntelligenceToString()
		{
			return RatingToString(Intelligence);
		}

		public string CreativityToString()
		{
			return RatingToString(Creativity);
		}

		public string CommunicationToString()
		{
			return RatingToString(Communication);
		}

		public string LeadershipToString()
		{
			return RatingToString(Leadership);
		}

		public void AdjustIntelligence(int intelligence)
		{
			Intelligence += intelligence;
		}

		public void AdjustCreativity(int creativity)
		{
			Creativity += creativity;
		}

		public void AdjustCommunication(int communication)
		{
			Communication += communication;
		}

		public void AdjustLeadership(int leadership)
		{
			Leadership += leadership;
		}

		public static Skills GetRandomSkills()
		{
			int randomIntelligence = random.Next(0, 10);
			int randomCreativity = random.Next(0, 10);
			int randomCommunication = random.Next(0, 10);
			int randomLeadership = random.Next(0, 10);

			return new Skills(
				(Rating)randomIntelligence,
				(Rating)randomCreativity,
				(Rating)randomCommunication,
				(Rating)randomLeadership
			);
		}
	}
}
