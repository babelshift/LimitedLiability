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
	public enum ButtonType
	{
		TextOnly,
		IconOnly,
		IconAndText
	}

	public class Button : Control, IDisposable
	{
		#region Members

		private Texture textureFrame;
		private ButtonType buttonType;

		#endregion

		#region Properties

		public Icon Icon { get; set; }
		public Icon IconHovered { get; set; }
		public Texture TextureFrame
		{
			get { return textureFrame; }
			set
			{
				textureFrame = value;
				Width = textureFrame.Width;
				Height = textureFrame.Height;
			}
		}
		public Texture TextureFrameHovered { get; set; }
		public Texture TextureFrameSelected { get; set; }
		public Label Label { get; set; }
		public Tooltip Tooltip { get; set; }
		public string Text { get { return Label.Text; } set { Label.Text = value; } }
		public bool IsPressed { get; private set; }
		public ButtonType ButtonType
		{
			get { return buttonType; }
			set
			{
				buttonType = value;
				if (buttonType == UserInterface.ButtonType.TextOnly)
				{
					if (Label != null)
						Label.Position = new Vector(base.Position.X + TextureFrame.Width / 2 - Label.Width / 2, base.Position.Y + TextureFrame.Height / 2 - Label.Height / 2);
				}
				else if (buttonType == UserInterface.ButtonType.IconOnly)
				{
					if (Icon != null)
						Icon.Position = new Vector(base.Position.X + (TextureFrame.Width / 2 - Icon.Width / 2), base.Position.Y + (TextureFrame.Height / 2 - Icon.Height / 2));

					if (IconHovered != null)
						IconHovered.Position = new Vector(base.Position.X + (TextureFrame.Width / 2 - IconHovered.Width / 2), base.Position.Y + (TextureFrame.Height / 2 - IconHovered.Height / 2));
				}
				else if (buttonType == UserInterface.ButtonType.IconAndText)
				{
					if (Icon != null)
						Icon.Position = new Vector(base.Position.X + 5, base.Position.Y + (TextureFrame.Height / 2 - Icon.Height / 2));

					if (IconHovered != null)
						IconHovered.Position = new Vector(base.Position.X + 5, base.Position.Y + (TextureFrame.Height / 2 - IconHovered.Height / 2));

					if (Label != null)
						Label.Position = new Vector(Icon.Position.X + Icon.Width + 3, Icon.Position.Y + (TextureFrame.Height / 2 - Label.Height / 2));
				}
			}
		}

		protected bool IsHovered { get; private set; }

		protected bool IsClicked { get; private set; }

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				if (buttonType == UserInterface.ButtonType.TextOnly)
				{
					if (Label != null)
						Label.Position = new Vector(base.Position.X + (TextureFrame.Width / 2 - Label.Width / 2), base.Position.Y + (TextureFrame.Height / 2 - Label.Height / 2));
				}
				else if (buttonType == UserInterface.ButtonType.IconOnly)
				{
					if (Icon != null)
						Icon.Position = new Vector(base.Position.X + (TextureFrame.Width / 2 - Icon.Width / 2), base.Position.Y + (TextureFrame.Height / 2 - Icon.Height / 2));

					if (IconHovered != null)
						IconHovered.Position = new Vector(base.Position.X + (TextureFrame.Width / 2 - IconHovered.Width / 2), base.Position.Y + (TextureFrame.Height / 2 - IconHovered.Height / 2));
				}
				else if (buttonType == UserInterface.ButtonType.IconAndText)
				{
					if (Icon != null)
						Icon.Position = new Vector(base.Position.X + 5, base.Position.Y + (TextureFrame.Height / 2 - Icon.Height / 2));

					if (IconHovered != null)
						IconHovered.Position = new Vector(base.Position.X + 5, base.Position.Y + (TextureFrame.Height / 2 - IconHovered.Height / 2));

					if (Label != null)
						Label.Position = new Vector(Icon.Position.X + Icon.Width + 3, Icon.Position.Y + (TextureFrame.Height / 2 - Label.Height / 2));
				}
			}
		}

		#endregion

		#region Events

		public event EventHandler Clicked;
		public event EventHandler Hovered;

		#endregion

		#region Constructors

		public Button()
		{
			Visible = true;
		}

		#endregion

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			if (Icon != null)
				Icon.Update(gameTime);

			if (IconHovered != null)
				IconHovered.Update(gameTime);

			if (Label != null)
				Label.Update(gameTime);

			if (Tooltip != null)
				Tooltip.Update(gameTime);

			if (Bounds.Contains(MouseHelper.ClickedMousePoint))
				IsHovered = true;
			else
				IsHovered = false;

			if (IsHovered)
				OnHovered(EventArgs.Empty);

			IsClicked = GetClicked(MouseHelper.CurrentMouseState, MouseHelper.PreviousMouseState);

			if (IsClicked)
				OnClicked(EventArgs.Empty);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (Visible)
			{
				renderer.RenderTexture(TextureFrame, (int)Position.X, (int)Position.Y);

				if (IsPressed || IsHovered)
				{
					if (IsHovered)
					{
						if (Tooltip != null)
							Tooltip.Draw(gameTime, renderer);
						if (TextureFrameHovered != null)
							renderer.RenderTexture(TextureFrameHovered, (int)Position.X, (int)Position.Y);
						if (IconHovered != null)
							IconHovered.Draw(gameTime, renderer);
					}
					else if (IsPressed)
					{
						if (TextureFrameSelected != null)
							renderer.RenderTexture(TextureFrameSelected, (int)Position.X, (int)Position.Y);
						else if (TextureFrameHovered != null)
							renderer.RenderTexture(TextureFrameHovered, (int)Position.X, (int)Position.Y);
					}

					if (Icon != null)
						Icon.Draw(gameTime, renderer);
				}
				else
				{
					if (Icon != null)
						Icon.Draw(gameTime, renderer);
				}

				if (Label != null)
					Label.Draw(gameTime, renderer);
			}
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
				if (mouseStateCurrent.ButtonsPressed != null && mouseStatePrevious.ButtonsPressed != null)
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

		private void OnHovered(EventArgs e)
		{
			if (Hovered != null)
				Hovered(this, e);
		}

		#endregion

		public void Focus()
		{
		}

		public void Blur()
		{
		}

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Button()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (Icon != null)
				Icon.Dispose();
			if (IconHovered != null)
				IconHovered.Dispose();
			if (TextureFrameHovered != null)
				TextureFrameHovered.Dispose();
			if (TextureFrameSelected != null)
				TextureFrameHovered.Dispose();
			if (Label != null)
				Label.Dispose();
		}
	}
}
