using System;

namespace MyThirdSDL
{
    public class AgentMetaData
    {
		public int Price { get; private set; }
		public string Name { get; private set; }
		public string IconKey { get; private set; }
		public NecessityEffects NecessityEffect { get; private set; }
		public SkillEffects SkillEffect { get; private set; }

		public AgentMetaData(int price, string name, string iconKey, NecessityEffects necessityEffect, SkillEffects skillEffect)
        {
			Price = price;
			Name = name;
			IconKey = iconKey;
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
        }
    }
}

