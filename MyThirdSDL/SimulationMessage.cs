using System;
using SharpDL.Graphics;

namespace MyThirdSDL
{
    public class SimulationMessage
    {
		public enum MessageType
		{
			EmployeeIsSleepy,
			EmployeeIsUnhealthy,
			EmployeeIsDirty,
			EmployeeIsHungry,
			EmployeeIsThirsty,
			EmployeeIsUnhappy,
			EmployeeNeedsDesk
		}

		public Vector Position { get; private set; }
		public string Text { get; private set;}
		public MessageType Type { get; private set; }

		public SimulationMessage(Vector position, string text, MessageType type)
        {
			Position = position;
			Text = text;
			Type = type;
        }
    }
}

