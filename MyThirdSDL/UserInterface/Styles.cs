using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public static class Styles
	{
		public static class Fonts
		{
			public static string Arcade = "Arcade";
			public static string DroidSans = "DroidSans";
			public static string DroidSansBold = "DroidSans Bold";
		}

		public static class FontSizes
		{
			public static int MainMenuTitle = 28;
			public static int Title = 14;
			public static int Content = 12;
			public static int Tooltip = 8;
		}

		public static class Colors
		{
			public static Color PaleGreen = new Color(239, 255, 205);
			public static Color Black = new Color(0, 0, 0);
			public static Color PaleYellow = new Color(238, 230, 171);
			public static Color White = new Color(218, 218, 218);
			public static Color Yellow = new Color(255, 190, 50);
			public static Color DuskBlue = new Color(66, 75, 92);
		}
	}
}
