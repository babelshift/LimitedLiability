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
		public static class FontPaths
		{
			public static string Arcade = "Arcade";
		}

		public static class FontSizes
		{
			public static int Title = 14;
			public static int Content = 12;
			public static int Tooltip = 8;
		}

		public static class Colors
		{
			public static Color Title = new Color(218, 218, 218);
			public static Color Label = new Color(218, 218, 218);
			public static Color LabelValue = new Color(255, 190, 50);
		}
	}
}
