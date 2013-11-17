using System;
using SharpDL.Input;
using SharpDL.Graphics;

namespace MyThirdSDL
{
	public static class InputHelper
	{
		public static Vector ClickedWorldSpacePoint { get; private set; }

		public static Point ClickedMousePoint { get; private set; }

		public static MouseState CurrentMouseState { get; private set; }

		public static MouseState PreviousMouseState { get; private set; }

		public static void Update()
		{
			InputHelper.PreviousMouseState = InputHelper.CurrentMouseState;
			InputHelper.CurrentMouseState = Mouse.GetState();
			ClickedMousePoint = new Point(InputHelper.CurrentMouseState.X, InputHelper.CurrentMouseState.Y);

			ClickedWorldSpacePoint = CoordinateHelper.ScreenSpaceToWorldSpace(
                ClickedMousePoint.X, ClickedMousePoint.Y,
                CoordinateHelper.ScreenOffset,
                CoordinateHelper.ScreenProjectionType.Isometric);
		}
	}
}