using System;

namespace MyThirdSDL
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
    }
}

