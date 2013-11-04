using System;

namespace MyThirdSDL
{
	public static class EventHelper
    {
		public static void FireEvent<T>(EventHandler<T> eventHandler, object sender, T e)
		{
			if (eventHandler != null)
				eventHandler(sender, e);
		}
    }
}

