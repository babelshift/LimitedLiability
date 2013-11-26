using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MyThirdSDL.Content
{
	public class ContentManager
	{
		private Dictionary<string, string> contentReference = new Dictionary<string, string>();
		private const string contentRoot = "Content/";
		private const string contentReferencePath = contentRoot + "ContentReference.json";

		public ContentManager()
		{
			string json = File.ReadAllText(contentReferencePath);
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

		public string GetContentPath(string contentKey)
		{
			string contentPath = String.Empty;
			if (contentReference.TryGetValue(contentKey, out contentPath))
				return contentPath;
			else
				throw new KeyNotFoundException(String.Format("The content with key '{0}' was not found.", contentKey));
		}

		private KeyValuePair<string, string> GetKeyValuePair(JToken o)
		{
			string key = o["key"].ToString();
			string value = contentRoot + o["value"].ToString();
			return new KeyValuePair<string, string>(key, value);
		}
	}
}
