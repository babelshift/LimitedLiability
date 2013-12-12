using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ButtonMailFolder : Button
	{
		public ButtonMailFolder(Texture texture, Texture textureHover, Vector position, Icon icon, Label label)
			: base(texture, textureHover, position, icon, null, label)
		{

		}
	}
}
