using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using SharpDL.Graphics;

namespace MyThirdSDL.Content
{
	public class ContentManager
	{
		private Random random = new Random();

		private const string contentRoot = "Content/";
		private const string contentDataRoot = contentRoot + "Data/";
		private const string contentReferencePath = contentDataRoot + "ContentReference.json";
		private const string agentReferencePath = contentDataRoot + "AgentReference.json";
		private const string peopleNamesPath = contentDataRoot + "PeopleNames.json";
		private const string companyDataPath = contentDataRoot + "CompanyData.json";

		private Dictionary<string, string> contentReference = new Dictionary<string, string>();
		private Dictionary<string, AgentMetadata> agentMetadataDictionary = new Dictionary<string, AgentMetadata>();
		private List<string> firstNames = new List<string>();
		private List<string> lastNames = new List<string>();
		private List<CompanyMetadata> companies = new List<CompanyMetadata>();

		public ContentManager()
		{
			string contentReferenceJson = File.ReadAllText(contentReferencePath);
			LoadContentFiles(contentReferenceJson);

			string agentReferenceJson = File.ReadAllText(agentReferencePath);
			LoadAgentMetadata(agentReferenceJson);

			string peopleNamesJson = File.ReadAllText(peopleNamesPath);
			LoadPeopleNames(peopleNamesJson);

			string companyDataJson = File.ReadAllText(companyDataPath);
			LoadCompanyData(companyDataJson);
		}

		private void LoadCompanyData(string json)
		{
			JObject o = JObject.Parse(json);

			foreach (var company in o["companies"])
			{
				string name = company["name"].ToString();
				string mailAddressType = company["type"].ToString();
				string domain = company["domain"].ToString();
				CompanyMetadata companyMetadata = new CompanyMetadata(name, mailAddressType, domain);
				companies.Add(companyMetadata);
			}
		}

		private void LoadPeopleNames(string json)
		{
			JObject o = JObject.Parse(json);

			foreach (var firstName in o["firstNames"])
				firstNames.Add(firstName["name"].ToString());

			foreach (var lastName in o["lastNames"])
				lastNames.Add(lastName["name"].ToString());
		}

		private void LoadAgentMetadata(string json)
		{
			JObject o = JObject.Parse(json);

			foreach (var equipment in o["equipment"])
			{
				AgentMetadata agentMetadata = null;
				NecessityEffect necessityEffectData = null;
				SkillEffect skillEffectData = null;

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

					necessityEffectData = new NecessityEffect(health, hygiene, sleep, thirst, hunger);
				}

				foreach (var skillEffect in equipment["skillEffect"])
				{
					int intelligence = Int32.Parse(skillEffect["intelligence"].ToString());
					int creativity = Int32.Parse(skillEffect["creativity"].ToString());
					int communication = Int32.Parse(skillEffect["communication"].ToString());
					int leadership = Int32.Parse(skillEffect["leadership"].ToString());

					skillEffectData = new SkillEffect(intelligence, creativity, communication, leadership);
				}

				agentMetadata = new AgentMetadata(price, name, iconKey, necessityEffectData, skillEffectData);
				agentMetadataDictionary.Add(key, agentMetadata);
			}
		}

		private void LoadContentFiles(string json)
		{
			JObject o = JObject.Parse(json);
			foreach (var font in o["fonts"])
			{
				var keyValuePair = GetKeyValuePair(font);
				contentReference.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (var texture in o["textures"])
			{
				var keyValuePair = GetKeyValuePair(texture);
				contentReference.Add(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (var map in o["maps"])
			{
				var keyValuePair = GetKeyValuePair(map);
				contentReference.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public CompanyMetadata GetCompany(string name)
		{
			var company = companies.FirstOrDefault(c => c.Name == name);

			if (company == null)
				throw new Exception(String.Format("Company with name {0} not found.", name));

			return company;
		}

		public AgentMetadata GetAgentMetadata(string key)
		{
			AgentMetadata agentMetaData;
			bool success = agentMetadataDictionary.TryGetValue(key, out agentMetaData);
			if (success)
				return agentMetaData;
			else
				throw new KeyNotFoundException(String.Format("The content with key '{0}' was not found.", key));
		}

		public string GetContentPath(string contentKey)
		{
			string contentPath = String.Empty;
			if (contentReference.TryGetValue(contentKey, out contentPath))
				return contentRoot + contentPath;
			else
				throw new KeyNotFoundException(String.Format("The content with key '{0}' was not found.", contentKey));
		}

		private KeyValuePair<string, string> GetKeyValuePair(JToken o)
		{
			string key = o["key"].ToString();
			string value = o["value"].ToString();
			return new KeyValuePair<string, string>(key, value);
		}

		public string GetRandomFirstName()
		{
			int index = random.Next(0, firstNames.Count() - 1);
			return firstNames[index];
		}

		public string GetRandomLastName()
		{
			int index = random.Next(0, lastNames.Count() - 1);
			return lastNames[index];
		}
	}
}
