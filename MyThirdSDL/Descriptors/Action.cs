using System;
using System.Collections.Generic;

namespace MyThirdSDL
{
	public enum ActionType
	{
		DispenseDrink,
		DispenseFood
	}

    public class Action
    {
		private List<ITriggerSubscriber> subscribers = new List<ITriggerSubscriber>();

		public ActionType Type { get; private set; }
		public IEnumerable<ITriggerSubscriber> Subscribers { get { return subscribers; } }

		public Action(ActionType type)
        {
			Type = type;
        }

		public void AddSubscriber(ITriggerSubscriber subscriber)
		{
			subscribers.Add(subscriber);
		}
    }
}

