using System;

namespace MyThirdSDL.Agents
{
	public class RoomMetadata : AgentMetadata
	{
		public string MapPathKey { get; private set; }

		public RoomMetadata(int price, string name, string iconKey, NecessityEffect necessityEffect, SkillEffect skillEffect, string mapPathKey)
			: base(price, name, iconKey, necessityEffect, skillEffect)
		{
			MapPathKey = mapPathKey;
		}
	}
}

