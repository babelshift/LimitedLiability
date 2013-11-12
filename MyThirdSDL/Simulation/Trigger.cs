using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MyThirdSDL.Simulation
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
				{
					List<Subscription> subscriptionsToRemove = new List<Subscription>();
					foreach (var subscription in action.Subscriptions)
					{
						var subscriber = subscription.Subscriber;
						subscriber.ReactToAction(action.Type, affector);

						// if the subscriber only subscribed to be notified once, queue them to be removed once the notifications are complete
						if (subscription.Type == SubscriptionType.Once)
							subscriptionsToRemove.Add(subscription);
					}

					foreach(var subscription in subscriptionsToRemove)
						action.RemoveSubscription(subscription.Subscriber);
				}
			}
		}

		public void AddSubscriptionToActionByType(ITriggerSubscriber subscriber, SubscriptionType subscriptionType, ActionType actionType)
		{
			Action action;
			bool success = actions.TryGetValue(actionType, out action);
			if (success)
				action.AddSubscription(subscriber, subscriptionType);
		}
    }
}

