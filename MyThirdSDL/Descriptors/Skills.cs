using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class Skills
	{
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
	}
}
