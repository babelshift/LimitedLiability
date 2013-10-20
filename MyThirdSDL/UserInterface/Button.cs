using SharpDL;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Button : Control
	{
		private Texture textureHover;
		private bool isHovered = false;

		public Tooltip Tooltip { get; set; }

		public event EventHandler Clicked;

		public Button(Texture texture, Texture textureHover, Vector position)
			: base(texture, position)
		{
			this.textureHover = textureHover;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			MouseState mouseState = Mouse.GetState();
			if (Bounds.Contains(new Point(mouseState.X, mouseState.Y)))
			{
				if (mouseState.ButtonsPressed.Contains(MouseButtonCode.Left))
					OnClicked(EventArgs.Empty);
				isHovered = true;
			}
			else
				isHovered = false;
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (isHovered)
			{
				if (textureHover != null)
					renderer.RenderTexture(textureHover, (int)Position.X - 3, (int)Position.Y - 2);
				if (Tooltip != null)
					Tooltip.Draw(gameTime, renderer);
			}

			base.Draw(gameTime, renderer);
		}

		private void OnClicked(EventArgs e)
		{
			if (Clicked != null)
				Clicked(this, e);
		}
	}
}
