using System;

namespace MyThirdSDL
{
    public class AgentMetaData
    {
		public int Price { get; private set; }
		public string Name { get; private set; }
		public string IconKey { get; private set; }
		public NecessityEffect NecessityEffect { get; private set; }
		public SkillEffect SkillEffect { get; private set; }

		public AgentMetaData(int price, string name, string iconKey, NecessityEffect necessityEffect, SkillEffect skillEffect)
        {
			Price = price;
			Name = name;
			IconKey = iconKey;
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
        }
    }
}

