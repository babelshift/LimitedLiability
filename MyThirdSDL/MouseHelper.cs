using System;
using SharpDL.Input;
using SharpDL.Graphics;

namespace MyThirdSDL
{
	public static class MouseHelper
	{
		public static Vector ClickedWorldSpacePoint { get; private set; }

		public static Point ClickedMousePoint { get; private set; }

		public static MouseState CurrentMouseState { get; private set; }

		public static MouseState PreviousMouseState { get; private set; }

		public static void Update()
		{
			MouseHelper.PreviousMouseState = MouseHelper.CurrentMouseState;
			MouseHelper.CurrentMouseState = Mouse.GetState();
			ClickedMousePoint = new Point(MouseHelper.CurrentMouseState.X, MouseHelper.CurrentMouseState.Y);

			ClickedWorldSpacePoint = CoordinateHelper.ScreenSpaceToWorldSpace(
                ClickedMousePoint.X, ClickedMousePoint.Y,
                CoordinateHelper.ScreenOffset,
                CoordinateHelper.ScreenProjectionType.Isometric);
		}
	}
}