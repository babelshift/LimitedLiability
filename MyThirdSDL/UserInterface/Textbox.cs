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
			private set
			{
				iconFrame = value;
				Width = iconFrame.Width;
				Height = iconFrame.Height;
			}
		}

		public Icon IconInputBar { get; private set; }

		public Label LabelText { get; private set; }

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
				LabelText.Position = new Vector(base.Position.X + 6, base.Position.X + 8);
			}
		}

		private bool HasText
		{
			get
			{
				if (LabelText.Text == ".")
					return false;
				return true;
			}
		}

		protected bool IsHovered { get; private set; }

		protected bool IsClicked { get; private set; }

		public Textbox(ContentManager contentManager)
		{
			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(218, 218, 218);
			int fontSizeContent = 12;

			IconFrame = new Icon(contentManager.GetTexture("TextboxLongFrame"));
			IconInputBar = new Icon(contentManager.GetTexture("IconInputBar"));
			LabelText = new Label();
			LabelText.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, ".");

			Blur();
		}

		public void Focus()
		{
			hasFocus = true;
			Keyboard.StartTextInput();
			//Renderer.SetTextInputRectangle(Bounds);
		}

		public void Blur()
		{
			hasFocus = false;
			Keyboard.StopTextInput();
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
				LabelText.Update(gameTime);
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

				if(HasText)
					LabelText.Draw(gameTime, renderer);
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

		public void HandleTextInputtingEvent(string text)
		{
			if(HasText)
				LabelText.Text += text;
			else
				LabelText.Text = text;

			IconInputBar.Position += new Vector(12, 0);
		}
	}
}
