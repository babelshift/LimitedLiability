using MyThirdSDL.Agents;
using Newtonsoft.Json.Linq;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyThirdSDL.Content
{
	public class ContentManager
	{
		private readonly Random random = new Random();

		private readonly Renderer renderer;

		private const string contentRoot = "Content/";
		private const string contentDataRoot = contentRoot + "Data/";
		private const string contentReferencePath = contentDataRoot + "ContentReference.json";
		private const string agentReferencePath = contentDataRoot + "AgentReference.json";
		private const string peopleNamesPath = contentDataRoot + "PeopleNames.json";
		private const string companyDataPath = contentDataRoot + "CompanyData.json";
		private const string thoughtReferencePath = contentDataRoot + "ThoughtReference.json";
		private const string stringReferencePath = contentDataRoot + "StringReference.json";

		private readonly Dictionary<string, string> contentReference = new Dictionary<string, string>();
		private readonly Dictionary<string, AgentMetadata> agentMetadataDictionary = new Dictionary<string, AgentMetadata>();
		private readonly Dictionary<string, RoomMetadata> roomMetadataDictionary = new Dictionary<string, RoomMetadata>();
		private readonly List<string> firstNames = new List<string>();
		private readonly List<string> lastNames = new List<string>();
		private readonly List<CompanyMetadata> companies = new List<CompanyMetadata>();
		private readonly List<ThoughtMetadata> thoughtPool = new List<ThoughtMetadata>();
		private readonly Dictionary<string, string> stringReference = new Dictionary<string, string>();

		public IEnumerable<ThoughtMetadata> ThoughtPool { get { return thoughtPool; } }

		public ContentManager(Renderer renderer)
		{
			this.renderer = renderer;

			string contentReferenceJson = File.ReadAllText(contentReferencePath);
			LoadContentFiles(contentReferenceJson);

			string agentReferenceJson = File.ReadAllText(agentReferencePath);
			LoadAgentMetadata(agentReferenceJson);

			string peopleNamesJson = File.ReadAllText(peopleNamesPath);
			LoadPeopleNames(peopleNamesJson);

			string companyDataJson = File.ReadAllText(companyDataPath);
			LoadCompanyData(companyDataJson);

			string thoughtReferenceJson = File.ReadAllText(thoughtReferencePath);
			LoadThoughtData(thoughtReferenceJson);

			string stringReferenceJson = File.ReadAllText(stringReferencePath);
			LoadStrings(stringReferenceJson);
		}

		public Texture GetTexture(string texturePathKey)
		{
			if (texturePathKey == null) throw new ArgumentNullException("texturePathKey");

			string texturePath = GetContentPath(texturePathKey);
			Surface surface = new Surface(texturePath, SurfaceType.PNG);
			Texture texture = new Texture(renderer, surface);
			return texture;
		}

		public TrueTypeText GetTrueTypeText(string fontPath, int fontSize, Color color, string text, int wrapLength)
		{
			if (fontPath == null) throw new ArgumentNullException("fontPath");
			if (text == null) throw new ArgumentNullException("text");

			return TrueTypeTextFactory.CreateTrueTypeText(renderer, fontPath, fontSize, color, text, wrapLength);
		}

		private void LoadCompanyData(string json)
		{
			if (json == null) throw new ArgumentNullException("json");

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
			if (json == null) throw new ArgumentNullException("json");

			JObject o = JObject.Parse(json);

			foreach (var firstName in o["firstNames"])
				firstNames.Add(firstName["name"].ToString());

			foreach (var lastName in o["lastNames"])
				lastNames.Add(lastName["name"].ToString());
		}

		private void LoadAgentMetadata(string json)
		{
			if (json == null) throw new ArgumentNullException("json");

			JObject o = JObject.Parse(json);

			foreach (var equipment in o["equipment"])
			{
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

				AgentMetadata agentMetadata = new AgentMetadata(price, name, iconKey, necessityEffectData, skillEffectData);
				agentMetadataDictionary.Add(key, agentMetadata);
			}

			foreach (var room in o["rooms"])
			{
				NecessityEffect necessityEffectData = null;
				SkillEffect skillEffectData = null;

				string key = room["key"].ToString();
				string name = room["name"].ToString();
				int price = Int32.Parse(room["price"].ToString());
				string iconKey = room["icon"].ToString();
				string mapPathKey = room["map"].ToString();

				foreach (var necessityEffect in room["necessityEffect"])
				{
					int health = Int32.Parse(necessityEffect["health"].ToString());
					int hygiene = Int32.Parse(necessityEffect["hygiene"].ToString());
					int sleep = Int32.Parse(necessityEffect["sleep"].ToString());
					int thirst = Int32.Parse(necessityEffect["thirst"].ToString());
					int hunger = Int32.Parse(necessityEffect["hunger"].ToString());

					necessityEffectData = new NecessityEffect(health, hygiene, sleep, thirst, hunger);
				}

				foreach (var skillEffect in room["skillEffect"])
				{
					int intelligence = Int32.Parse(skillEffect["intelligence"].ToString());
					int creativity = Int32.Parse(skillEffect["creativity"].ToString());
					int communication = Int32.Parse(skillEffect["communication"].ToString());
					int leadership = Int32.Parse(skillEffect["leadership"].ToString());

					skillEffectData = new SkillEffect(intelligence, creativity, communication, leadership);
				}

				RoomMetadata roomMetadata = new RoomMetadata(price, name, iconKey, necessityEffectData, skillEffectData, mapPathKey);
				roomMetadataDictionary.Add(key, roomMetadata);
			}
		}

		private void LoadContentFiles(string json)
		{
			if (json == null) throw new ArgumentNullException("json");

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
			foreach (var room in o["rooms"])
			{
				var keyValuePair = GetKeyValuePair(room);
				contentReference.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		private void LoadStrings(string json)
		{
			if (json == null) throw new ArgumentNullException("json");
			JObject o = JObject.Parse(json);
			foreach (var stringPair in o)
				stringReference.Add(stringPair.Key, stringPair.Value.ToString());
		}

		public string GetString(string key)
		{
			if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

			string returnKey = String.Empty;
			stringReference.TryGetValue(key, out returnKey);

			if (String.IsNullOrEmpty(returnKey))
				throw new Exception(String.Format("String with key {0} is missing.", key));

			return returnKey;

		}

		public CompanyMetadata GetCompany(string name)
		{
			if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

			var company = companies.FirstOrDefault(c => c.Name == name);

			if (company == null)
				throw new Exception(String.Format("Company with name {0} not found.", name));

			return company;
		}

		public AgentMetadata GetAgentMetadata(string key)
		{
			if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

			AgentMetadata agentMetaData;
			bool success = agentMetadataDictionary.TryGetValue(key, out agentMetaData);
			if (success)
				return agentMetaData;

			throw new KeyNotFoundException(String.Format("The contentManager with key '{0}' was not found.", key));
		}

		public RoomMetadata GetRoomMetadata(string key)
		{
			if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

			RoomMetadata roomMetaData;
			bool success = roomMetadataDictionary.TryGetValue(key, out roomMetaData);
			if (success)
				return roomMetaData;

			throw new KeyNotFoundException(String.Format("The contentManager with key '{0}' was not found.", key));
		}

		public string GetContentPath(string key)
		{
			if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

			string contentPath;
			if (contentReference.TryGetValue(key, out contentPath))
				return contentRoot + contentPath;

			throw new KeyNotFoundException(String.Format("The contentManager with key '{0}' was not found.", key));
		}

		private KeyValuePair<string, string> GetKeyValuePair(JToken o)
		{
			if (o == null) throw new ArgumentNullException("o");

			string key = o["key"].ToString();
			string value = o["value"].ToString();

			return new KeyValuePair<string, string>(key, value);
		}

		#region Random Methods

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

		#endregion

		private void LoadThoughtData(string json)
		{
			if (json == null) throw new ArgumentNullException("json");

			JObject o = JObject.Parse(json);

			foreach (var equipment in o["idle"])
				AddThoughtToCollection(equipment, ThoughtType.IsIdle);

			foreach (var equipment in o["unhappy"])
				AddThoughtToCollection(equipment, ThoughtType.Unhappy);

			foreach (var equipment in o["dirty"])
				AddThoughtToCollection(equipment, ThoughtType.Dirty);

			foreach (var equipment in o["unhealthy"])
				AddThoughtToCollection(equipment, ThoughtType.Unhealthy);

			foreach (var equipment in o["needsDeskAssignment"])
				AddThoughtToCollection(equipment, ThoughtType.NeedsDeskAssignment);

			foreach (var equipment in o["thirsty"])
				AddThoughtToCollection(equipment, ThoughtType.Thirsty);

			foreach (var equipment in o["hungry"])
				AddThoughtToCollection(equipment, ThoughtType.Hungry);

			foreach (var equipment in o["sleepy"])
				AddThoughtToCollection(equipment, ThoughtType.Sleepy);

			foreach (var equipment in o["miscellaneous"])
				AddThoughtToCollection(equipment, ThoughtType.Miscellaneous);

			foreach (var equipment in o["notEnoughEquipment"])
				AddThoughtToCollection(equipment, ThoughtType.NotEnoughEquipment);

			foreach (var equipment in o["notChallenged"])
				AddThoughtToCollection(equipment, ThoughtType.NotChallenged);
		}

		private void AddThoughtToCollection(JToken thoughtData, ThoughtType type)
		{
			if (thoughtData == null) throw new ArgumentNullException("thoughtData");

			string idea = thoughtData["idea"].ToString();
			ThoughtMetadata thoughtMetadata = new ThoughtMetadata(type, idea);
			thoughtPool.Add(thoughtMetadata);
		}
	}
}