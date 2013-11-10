using System;
using System.Collections.Concurrent;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL
{
    public class SimulationNotificationDictionary
    {
		public Agent Agent { get; private set; }
		public ConcurrentDictionary<SimulationMessageType, SimulationMessage> Messages { get; private set; }

		public SimulationNotificationDictionary(Agent agent)
        {
			Agent = agent;
        }
    }
}

