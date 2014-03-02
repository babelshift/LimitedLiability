using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimitedLiability.UserInterface;

namespace LimitedLiability
{
	public static class Camera
	{
		private static int MoveSpeedY = 8;
		private static int MoveSpeedX = 8;

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
		public static void Update(IEnumerable<MouseOverScreenEdge> mouseOverScreenEdges)
		{
			if (mouseOverScreenEdges.Any(m => m == MouseOverScreenEdge.Top))
				MoveUp();

			if (mouseOverScreenEdges.Any(m => m == MouseOverScreenEdge.Bottom))
				MoveDown();

			if (mouseOverScreenEdges.Any(m => m == MouseOverScreenEdge.Left))
				MoveLeft();

			if (mouseOverScreenEdges.Any(m => m == MouseOverScreenEdge.Right))
				MoveRight();
		}
	}
}
