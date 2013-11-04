using System;
using SharpDL.Graphics;

namespace MyThirdSDL
{
    public class SimulationMessage
    {
		public Vector Position { get; private set; }
		public string Text { get; private set;}

		public SimulationMessage(Vector position, string text)
        {
			Position = position;
			Text = text;
        }
    }
}

