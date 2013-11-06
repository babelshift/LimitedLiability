using System;
using SharpDL.Graphics;

namespace MyThirdSDL.UserInterface
{
	public class SimulationLabel : Label
    {
		public SimulationMessage SimulationMessage { get; private set; }

		public SimulationLabel(Vector position, TrueTypeText trueTypeText, SimulationMessage simulationMessage)
			: base(position, trueTypeText)
        {
			SimulationMessage = simulationMessage;
        }
    }
}

