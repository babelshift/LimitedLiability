using System;
using System.Collections.Generic;
using SharpDL.Input;

namespace MyThirdSDL
{
	public static class KeyboardHelper
	{
		public static KeyboardState KeyboardState { get; private set; }

		public static void Update()
		{
			KeyboardState = Keyboard.GetKeyboardState();
		}
	}
}