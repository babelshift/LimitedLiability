using System;
using SharpDL.Graphics;

namespace MyThirdSDL.Simulation
{
	public static class SimulationMessageFactory
    {
		public static SimulationMessage Create(Vector position, string messageText, SimulationMessageType messageType)
		{
			return new SimulationMessage(position, messageText, messageType);
		}
    }
}

