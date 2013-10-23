using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ButtonMenuItem : Button
	{
		private Icon iconMain;
		private Label labelMain;
		private Icon iconMoney;
		private Label labelMoney;

		public ButtonMenuItem(Texture texture, Texture textureHover, Vector position, Icon iconMain, Label labelMain, 
			Icon iconMoney, Label labelMoney)
			: base(texture, textureHover, position)
		{
			this.iconMain = iconMain;
			this.labelMain = labelMain;
			this.iconMoney = iconMoney;
			this.labelMoney = labelMoney;
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);
		}
	}
}
