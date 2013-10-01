using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public static class Camera
	{
		private static int MoveSpeedY = 5;
		private static int MoveSpeedX = 5;

		public static Vector Position { get; set; }

		public static void MoveTo(Vector position)
		{
			Position = position;
		}

		public static void MoveUp()
		{
			Position -= new Vector(0, MoveSpeedY);
		}

		public static void MoveDown()
		{
			Position += new Vector(0, MoveSpeedY);
		}

		public static void MoveLeft()
		{
			Position -= new Vector(MoveSpeedX, 0);
		}

		public static void MoveRight()
		{
			Position += new Vector(MoveSpeedY, 0);
		}
	}
}
