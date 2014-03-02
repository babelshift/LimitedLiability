using System;

namespace LimitedLiability.Agents
{
	public class RoomMetadata : AgentMetadata
	{
		public string MapPathKey { get; private set; }

		public RoomMetadata(int price, string name, string description, string iconKey, NecessityEffect necessityEffect, SkillEffect skillEffect, string mapPathKey)
			: base(price, name, description, iconKey, necessityEffect, skillEffect)
		{
			MapPathKey = mapPathKey;
		}
	}
}

