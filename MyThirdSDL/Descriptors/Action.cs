using System;
using System.Collections.Generic;

namespace MyThirdSDL.Descriptors
{
	public enum ActionType
	{
		DispenseDrink,
		DispenseFood
	}

    public class Action
    {
		private List<Subscription> subscriptions = new List<Subscription>();

		public ActionType Type { get; private set; }
		public IEnumerable<Subscription> Subscriptions { get { return subscriptions; } }

		public Action(ActionType type)
        {
			Type = type;
        }

		public void AddSubscription(ITriggerSubscriber subscriber, SubscriptionType subscriptionType)
		{
			subscriptions.Add(new Subscription(subscriber, subscriptionType));
		}

		public void RemoveSubscription(ITriggerSubscriber subscriber)
		{
			subscriptions.RemoveAll(s => s.Subscriber.ID == subscriber.ID);
		}
    }
}

