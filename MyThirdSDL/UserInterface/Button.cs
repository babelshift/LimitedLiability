using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Button : Control
	{
		public Button(Texture texture, Vector position)
			: base(texture, position)
		{
		}

		public event EventHandler Clicked;
	}
}
