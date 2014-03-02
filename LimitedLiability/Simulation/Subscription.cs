using System;

namespace LimitedLiability.Simulation
{
	public enum SubscriptionType
	{
		Once,
		Permanent
	}

	public class Subscription
	{
		public ITriggerSubscriber Subscriber { get; private set; }
		public SubscriptionType Type { get; private set; }

		public Subscription(ITriggerSubscriber subscriber, SubscriptionType type)
		{
			Subscriber = subscriber;
			Type = type;
		}
	}
}

