using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace MyThirdSDL.Descriptors
{
    public class Trigger
    {
		private ConcurrentDictionary<ActionType, Action> actions = new ConcurrentDictionary<ActionType, Action>();

		public Guid ID { get; private set; }

		public Trigger()
		{
			ID = Guid.NewGuid();
		}

		public void AddAction(Action action)
		{
			if(!actions.ContainsKey(action.Type))
				actions.TryAdd(action.Type, action);
		}

		public void Execute(NecessityAffector affector)
		{
			foreach (var actionKey in actions.Keys)
			{
				Action action;
				bool success = actions.TryGetValue(actionKey, out action);
				if (success)
					foreach (var subscriber in action.Subscribers)
						subscriber.ReactToAction(action.Type, affector);
			}
		}

		public void AddSubscriberToActionByType(ITriggerSubscriber subscriber, ActionType actionType)
		{
			Action action;
			bool success = actions.TryGetValue(actionType, out action);
			if (success)
				action.AddSubscriber(subscriber);
		}
    }
}

