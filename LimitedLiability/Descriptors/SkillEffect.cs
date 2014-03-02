using System;

namespace LimitedLiability
{
	public class SkillEffect
	{
		public int IntelligenceEffectiveness { get; private set; }
		public int CreativityEffectiveness { get; private set; }
		public int CommunicationEffectiveness { get; private set; }
		public int LeadershipEffectiveness { get; private set; }

		public SkillEffect(int intelligence, int creativity, int communication, int leadership)
		{
			IntelligenceEffectiveness = intelligence;
			CreativityEffectiveness = creativity;
			CommunicationEffectiveness = communication;
			LeadershipEffectiveness = leadership;
		}

		private string RatingToString(int effectiveness)
		{
			if (effectiveness == 0)
				return "Bad";
			if (effectiveness >= 1 && effectiveness <= 4)
				return "OK";
			if (effectiveness >= 5 && effectiveness <= 8)
				return "Good";
			if (effectiveness >= 9)
				return "Excellent";
			return "???";
		}

		public string IntelligenceEffectivenessToString()
		{
			return RatingToString(IntelligenceEffectiveness);
		}

		public string CreativityEffectivenessToString()
		{
			return RatingToString(CreativityEffectiveness);
		}

		public string CommunicationEffectivenessToString()
		{
			return RatingToString(CommunicationEffectiveness);
		}

		public string LeadershipEffectivenessToString()
		{
			return RatingToString(LeadershipEffectiveness);
		}
	}
}

