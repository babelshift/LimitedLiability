using MyThirdSDL.Content;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class Textbox : Control
	{
		private Icon iconFrame;
		private bool hasFocus;

		public Icon IconFrame
		{
			get { return iconFrame; }
			set
			{
				iconFrame = value;
				Width = iconFrame.Width;
				Height = iconFrame.Height;
			}
		}

		public Icon IconInputBar { get; set; }

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				IconFrame.Position = base.Position;
				IconInputBar.Position = new Vector(base.Position.X + 6, base.Position.X + 6);
			}
		}

		protected bool IsHovered { get; private set; }

		protected bool IsClicked { get; private set; }

		public Textbox(ContentManager contentManager)
		{
			IconFrame = new Icon(contentManager.GetTexture("TextboxLongFrame"));
			IconInputBar = new Icon(contentManager.GetTexture("IconInputBar"));
			Blur();
		}

		public void Focus()
		{
			hasFocus = true;
		}

		public void Blur()
		{
			hasFocus = false;
		}

		public override void Update(SharpDL.GameTime gameTime)
		{
			if (Bounds.Contains(MouseHelper.ClickedMousePoint))
				IsHovered = true;
			else
				IsHovered = false;

			IsClicked = GetClicked(MouseHelper.CurrentMouseState, MouseHelper.PreviousMouseState);

			if (IsClicked)
				Focus();

			iconFrame.Update(gameTime);

			if (hasFocus)
			{
				timeSinceLastUpdate += gameTime.ElapsedGameTime;
				if (timeSinceLastUpdate > TimeSpan.FromSeconds(0.5))
				{
					IconInputBar.Visible = true;

					timeSinceVisibleTrue += gameTime.ElapsedGameTime;
					if (timeSinceVisibleTrue > TimeSpan.FromSeconds(0.5))
					{
						timeSinceLastUpdate = TimeSpan.Zero;
						timeSinceVisibleTrue = TimeSpan.Zero;
					}
				}
				else
					IconInputBar.Visible = false;

				IconInputBar.Update(gameTime);
			}
		}

		private TimeSpan timeSinceVisibleTrue = TimeSpan.Zero;
		private TimeSpan timeSinceLastUpdate = TimeSpan.Zero;

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			if (Visible)
			{
				iconFrame.Draw(gameTime, renderer);

				if (hasFocus)
					IconInputBar.Draw(gameTime, renderer);
			}
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
	}
}
