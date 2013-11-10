using System;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL
{
    public interface ITriggerSubscriber
    {
		void ReactToAction(ActionType actionType, NecessityAffector affector);
    }
}

