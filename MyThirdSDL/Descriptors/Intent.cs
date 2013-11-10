using System;
using System.Collections;
using System.Collections.Generic;
using MyThirdSDL;

namespace MyThirdSDL.Descriptors
{
	/// <summary>
	/// Intents are used by MobileAgents to perform activities. These are usually
	/// stored in a queue to be performed in sequential order when the MobileAgent completes the previous Intent. For example, an Employee can be thirsty
	/// and hungry simultaneously, but can only satisfy one of them at a time. The first Intent could be "Go to a Soda Machine and Drink." and the second 
	/// Intent could be "Go to a Vending Machine and Eat." Intents should point to another Agent so that the MobileAgent can path to it and optionally
	/// set off the trigger for the Agent (if the Agent is Triggerable).
	/// </summary>
    public class Intent
    {
		public Agent WalkToAgent { get; private set; }
		public Queue<MapObject> PathNodesToAgent { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MyThirdSDL.Intent"/> class. 
		/// </summary>
		public Intent(Agent walkToAgent, Queue<MapObject> pathNodesToAgent)
        {
			WalkToAgent = walkToAgent;
			PathNodesToAgent = pathNodesToAgent;
        }
    }
}

