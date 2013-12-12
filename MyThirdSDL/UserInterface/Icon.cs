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
		public Icon(Texture texture, Vector position)
			: base(texture, position)
		{
		}

		public Icon Clone()
		{
			return new Icon(Texture, Position);
		}
	}
}
