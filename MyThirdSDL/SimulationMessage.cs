using System;
using SharpDL.Graphics;

namespace MyThirdSDL
{
	public enum SimulationMessageType
	{
		EmployeeIsSleepy,
		EmployeeIsUnhealthy,
		EmployeeIsDirty,
		EmployeeIsHungry,
		EmployeeIsThirsty,
		EmployeeIsUnhappy,
		EmployeeNeedsDesk
	}

    public class SimulationMessage
    {
		public Vector Position { get; private set; }
		public string Text { get; private set;}
		public SimulationMessageType Type { get; private set; }

		public SimulationMessage(Vector position, string text, SimulationMessageType type)
        {
			Position = position;
			Text = text;
			Type = type;
        }
    }
}

