using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Icon : Control
	{
		public int Width { get { return Texture.Width; } }
		public int Height { get { return Texture.Height; } }

		public Icon(Texture texture, Vector position)
			: base(texture, position)
		{
		}
	}
}
