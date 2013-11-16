using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MyThirdSDL
{
    public class AgentManager
	{
		private const string agentRoot = "Agents/";
		private const string agentReferencePath = agentRoot + "AgentReference.json";

		private Dictionary<string, AgentMetaData> agentMetaDataDictionary = new Dictionary<string, AgentMetaData>();

        public AgentManager()
        {
			string json = File.ReadAllText(agentReferencePath);

			JObject o = JObject.Parse(json);
			foreach (var equipment in o["equipment"])
			{
				AgentMetaData agentMetaData = null;
				NecessityEffects necessityEffectData = null;
				SkillEffects skillEffectData = null;

				string key = equipment["key"].ToString();
				string name = equipment["name"].ToString();
				int price = Int32.Parse(equipment["price"].ToString());
				string iconKey = equipment["icon"].ToString();

				foreach (var necessityEffect in equipment["necessityEffect"])
				{
					int health = Int32.Parse(necessityEffect["health"].ToString());
					int hygiene = Int32.Parse(necessityEffect["hygiene"].ToString());
					int sleep = Int32.Parse(necessityEffect["sleep"].ToString());
					int thirst = Int32.Parse(necessityEffect["thirst"].ToString());
					int hunger = Int32.Parse(necessityEffect["hunger"].ToString());

					necessityEffectData = new NecessityEffects(health, hygiene, sleep, thirst, hunger);
				}

				foreach (var skillEffect in equipment["skillEffect"])
				{
					int intelligence = Int32.Parse(skillEffect["intelligence"].ToString());
					int creativity = Int32.Parse(skillEffect["creativity"].ToString());
					int communication = Int32.Parse(skillEffect["communication"].ToString());
					int leadership = Int32.Parse(skillEffect["leadership"].ToString());

					skillEffectData = new SkillEffects(intelligence, creativity, communication, leadership);
				}

				agentMetaData = new AgentMetaData(price, name, iconKey, necessityEffectData, skillEffectData);
				agentMetaDataDictionary.Add(key, agentMetaData);
			}
        }

		public AgentMetaData GetAgentMetaData(string key)
		{
			AgentMetaData agentMetaData;
			bool success = agentMetaDataDictionary.TryGetValue(key, out agentMetaData);
			if (success)
				return agentMetaData;
			else
				throw new KeyNotFoundException(String.Format("The content with key '{0}' was not found.", key));
		}
    }
}

