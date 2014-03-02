using LimitedLiability.Content;
using SharpDL.Graphics;
using System;

namespace LimitedLiability.Agents
{
	public class RoomFactory
	{
		private Renderer renderer;
		private ContentManager contentManager;

		public RoomFactory(Renderer renderer, ContentManager contentManager)
		{
			this.renderer = renderer;
			this.contentManager = contentManager;
		}

		public Library CreateLibrary(AgentFactory agentFactory)
		{
			return CreateRoom<Library>(agentFactory, "Library");
		}

		private T CreateRoom<T>(AgentFactory agentFactory, string roomKey)
		{
			RoomMetadata roomMetaData = contentManager.GetRoomMetadata(roomKey);
			string mapPath = contentManager.GetContentPath(roomMetaData.MapPathKey);
			TiledMap tiledMap = new TiledMap(mapPath, renderer, agentFactory);
			return (T)Activator.CreateInstance(typeof(T), roomMetaData.Name, roomMetaData.Price, roomMetaData.Description, roomMetaData.IconKey, tiledMap);
		}
	}
}