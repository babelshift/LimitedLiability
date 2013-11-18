using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.UserInterface;

namespace MyThirdSDL
{
	public static class Camera
	{
		private static int MoveSpeedY = 5;
		private static int MoveSpeedX = 5;

		public static Vector Position { get; set; }

		private static void MoveTo(Vector position)
		{
			Position = position;
		}

		private static void MoveUp()
		{
			Position -= new Vector(0, MoveSpeedY);
		}

		private static void MoveDown()
		{
			Position += new Vector(0, MoveSpeedY);
		}

		private static void MoveLeft()
		{
			Position -= new Vector(MoveSpeedX, 0);
		}

		private static void MoveRight()
		{
			Position += new Vector(MoveSpeedY, 0);
		}

		/// <summary>
		/// Update the camera's position based on the passed moused over edge.
		/// </summary>
		/// <param name="edge">Edge.</param>
		public static void Update(MouseOverScreenEdge edge)
		{
			if (edge == MouseOverScreenEdge.Top)
				MoveUp();
			else if (edge == MouseOverScreenEdge.Bottom)
				MoveDown();
			else if (edge == MouseOverScreenEdge.Left)
				MoveLeft();
			else if (edge == MouseOverScreenEdge.Right)
				MoveRight();
		}
	}
}
