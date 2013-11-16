using System;

namespace MyThirdSDL.Simulation
{
    public interface ITriggerSubscriber
    {
		Guid ID { get; }
		void ReactToAction(ActionType actionType, NecessityEffect affector);
    }
}

