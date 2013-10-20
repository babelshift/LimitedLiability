using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Tooltip : Control
	{
		private Label tooltipLabel;

		public Tooltip(Texture texture, Vector position, Label tooltipLabel)
			: base(texture, position)
		{
			this.tooltipLabel = tooltipLabel;
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			tooltipLabel.Draw(gameTime, renderer);
		}
	}
}
