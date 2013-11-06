using System;
using SharpDL.Graphics;

namespace MyThirdSDL
{
	public static class SimulationMessageFactory
    {
		public static SimulationMessage Create(Vector position, string messageText, SimulationMessage.MessageType messageType)
		{
			return new SimulationMessage(position, messageText, messageType);
		}
    }
}

