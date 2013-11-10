using System;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL
{
    public interface ITriggerSubscriber
    {
		Guid ID { get; }
		void ReactToAction(ActionType actionType, NecessityAffector affector);
    }
}

