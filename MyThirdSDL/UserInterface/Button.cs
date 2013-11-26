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
		private Icon icon;
		private Icon iconHover;
		private Texture textureHover;

		protected bool IsHovered { get; private set; }

		protected bool IsClicked { get; private set; }

		public Tooltip Tooltip { get; set; }

		public event EventHandler Clicked;

		public bool IsPressed { get; private set; }

		public Button(Texture texture, Texture textureHover, Vector position, Icon icon = null, Icon iconHover = null)
			: base(texture, position)
		{
			this.textureHover = textureHover;
			this.icon = icon;
			this.iconHover = iconHover;
		}

		private bool GetClicked(MouseState mouseStateCurrent, MouseState mouseStatePrevious)
		{
			if (IsHovered)
			{
				// if the curren state does not have a left click and the previous state does have a left click, then the user released the mouse
				if (!mouseStateCurrent.ButtonsPressed.Contains(MouseButtonCode.Left)
					 && mouseStatePrevious.ButtonsPressed.Contains(MouseButtonCode.Left))
					return true;
			}

			return false;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (Bounds.Contains(MouseHelper.ClickedMousePoint))
				IsHovered = true;
			else
				IsHovered = false;

			IsClicked = GetClicked(MouseHelper.CurrentMouseState, MouseHelper.PreviousMouseState);

			if (IsClicked)
				OnClicked(EventArgs.Empty);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (IsPressed || IsHovered)
			{
				if (IsHovered)
					if (Tooltip != null)
						Tooltip.Draw(gameTime, renderer);

				if (textureHover != null)
					renderer.RenderTexture(textureHover, (int)Position.X, (int)Position.Y);
				if (iconHover != null)
					iconHover.Draw(gameTime, renderer);
			}
			else
			{
				base.Draw(gameTime, renderer);
				if (icon != null)
					icon.Draw(gameTime, renderer);
			}
		}

		public void ToggleOn()
		{
			IsPressed = true;
		}

		public void ToggleOff()
		{
			IsPressed = false;
		}

		private void OnClicked(EventArgs e)
		{
			if (Clicked != null)
				Clicked(this, e);
		}
	}
}
