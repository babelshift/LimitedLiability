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
		#region Members

		private Icon icon;
		private Icon iconHover;
		private Texture textureHover;
		private Label label;

		#endregion

		#region Properties

		protected bool IsHovered { get; private set; }

		protected bool IsClicked { get; private set; }

		public Tooltip Tooltip { get; set; }

		public bool IsPressed { get; private set; }

		#endregion

		#region Events

		public event EventHandler Clicked;

		#endregion

		#region Constructors

		public Button(Texture texture, Texture textureHover, Vector position, Icon icon = null, Icon iconHover = null, Label label = null)
			: base(texture, position)
		{
			this.textureHover = textureHover;
			this.icon = icon;
			this.iconHover = iconHover;
			this.label = label;
		}

		#endregion

		#region Game Loop

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
				else if (icon != null)
					icon.Draw(gameTime, renderer);
			}
			else
			{
				base.Draw(gameTime, renderer);
				if (icon != null)
					icon.Draw(gameTime, renderer);
			}

			if (label != null)
				label.Draw(gameTime, renderer);
		}

		#endregion

		#region Behaviors

		public void ToggleOn()
		{
			IsPressed = true;
		}

		public void ToggleOff()
		{
			IsPressed = false;
		}

		private bool GetClicked(MouseState mouseStateCurrent, MouseState mouseStatePrevious)
		{
			if (IsHovered)
			{
				if(mouseStateCurrent.ButtonsPressed != null && mouseStatePrevious.ButtonsPressed != null)
					// if the curren state does not have a left click and the previous state does have a left click, then the user released the mouse
					if (!mouseStateCurrent.ButtonsPressed.Contains(MouseButtonCode.Left)
						 && mouseStatePrevious.ButtonsPressed.Contains(MouseButtonCode.Left))
						return true;
			}

			return false;
		}

		private void OnClicked(EventArgs e)
		{
			if (Clicked != null)
				Clicked(this, e);
		}

		#endregion
	}
}
