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
	public class Button : Control, IDisposable
	{
		#region Members

		#endregion

		#region Properties

		public Icon Icon { get; set; }
		public Icon IconHovered { get; set; }
		public Texture TextureFrame { get; set; }
		public Texture TextureFrameHovered { get; set; }
		public Texture TextureFrameSelected { get; set; }
		public Label Label { get; set; }
		public Tooltip Tooltip { get; set; }
		public string Text { get { return Label.Text; } set { Label.Text = value; } }
		public bool IsPressed { get; private set; }

		protected bool IsHovered { get; private set; }

		protected bool IsClicked { get; private set; }

		#endregion

		#region Events

		public event EventHandler Clicked;

		#endregion

		#region Constructors

		public Button()
		{
		}

		#endregion

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			Icon.Update(gameTime);
			IconHovered.Update(gameTime);
			Label.Update(gameTime);
			Tooltip.Update(gameTime);

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

		#endregion

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
