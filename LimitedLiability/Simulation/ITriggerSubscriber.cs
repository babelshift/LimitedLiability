using System;

namespace LimitedLiability.Simulation
{
	public interface ITriggerSubscriber
	{
		Guid ID { get; }
		void ReactToAction(ActionType actionType, NecessityEffect affector);
	}
}

