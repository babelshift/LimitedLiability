using MyThirdSDL.Content;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.Agents
{
	public class RoomFactory
	{
		private ContentManager contentManager;

		public RoomFactory(ContentManager contentManager)
		{
			this.contentManager = contentManager;
		}

		public Library CreateLibrary(Renderer renderer, AgentFactory agentFactory)
		{
			return CreateRoom<Library>(renderer, agentFactory, "Library");
		}

		private T CreateRoom<T>(Renderer renderer, AgentFactory agentFactory, string roomKey)
		{
			RoomMetadata roomMetaData = contentManager.GetRoomMetadata(roomKey);
			string mapPath = contentManager.GetContentPath(roomMetaData.MapPathKey);
			TiledMap tiledMap = new TiledMap(mapPath, renderer, agentFactory);
			return (T)Activator.CreateInstance(typeof(T), roomMetaData.Name, roomMetaData.Price, roomMetaData.IconKey, tiledMap);
		}
	}
}