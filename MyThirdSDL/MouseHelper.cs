using System;
using SharpDL.Input;
using SharpDL.Graphics;
using System.Collections.Generic;

namespace MyThirdSDL
{
	public static class MouseHelper
	{
		public static Vector CurrentPosition { get; private set; }

		public static IEnumerable<MouseButtonCode> ButtonsPressed { get; private set; }

		public static IEnumerable<MouseButtonCode> PreviousButtonsPressed { get; private set; }

		public static void UpdateMousePosition(Vector currentPosition)
		{
			CurrentPosition = currentPosition;
		}

		public static void UpdateMouseState()
		{
			PreviousButtonsPressed = ButtonsPressed;

			var currentMouseState = Mouse.GetState();
			ButtonsPressed = currentMouseState.ButtonsPressed;
		}
	}
}