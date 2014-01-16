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
		private TimeSpan timeSinceVisibleTrue = TimeSpan.Zero;
		private TimeSpan timeSinceLastUpdate = TimeSpan.Zero;

		private Icon iconFrame;

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
				IconInputBar.Position = new Vector(base.Position.X + 6, base.Position.Y + 6);
				LabelText.Position = new Vector(base.Position.X + 6, base.Position.Y + 8);
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

		private bool IsTextboxFull
		{
			get
			{
				int maxCharacterCount = (Width / LabelText.TrueTypeText.Font.PointSize) - 1;

				if (LabelText == null)
					return false;
				else if (LabelText.Text.Length < maxCharacterCount)
					return false;
				else
					return true;
			}
		}

		protected bool IsHovered { get; private set; }

		protected bool IsClicked { get; private set; }

		public Textbox(ContentManager contentManager)
		{
			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = Styles.Colors.MainMenuTitleText;
			int fontSizeContent = 12;

			IconFrame = new Icon(contentManager.GetTexture("TextboxLongFrame"));
			IconInputBar = new Icon(contentManager.GetTexture("IconInputBar"));
			LabelText = new Label();
			LabelText.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, ".");

			Blur();
		}

		public override void Focus()
		{
			base.Focus();
			Keyboard.StartTextInput();
		}

		public override void Blur()
		{
			base.Blur();
			Keyboard.StopTextInput();
		}

		public override void Update(SharpDL.GameTime gameTime)
		{
			if (IsClicked)
				Focus();

			iconFrame.Update(gameTime);

			if (HasFocus)
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

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			if (Visible)
			{
				iconFrame.Draw(gameTime, renderer);

				if (HasFocus)
					IconInputBar.Draw(gameTime, renderer);

				if (HasText)
					LabelText.Draw(gameTime, renderer);
			}
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			IsClicked = GetClicked(e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			if (Bounds.Contains(new Point(e.RelativeToWindowX, e.RelativeToWindowY)))
				IsHovered = true;
			else
				IsHovered = false;
		}

		private bool GetClicked(SharpDL.Events.MouseButtonEventArgs e)
		{
			if (IsHovered)
			{
				if (e.MouseButton == MouseButtonCode.Left)
					//if (e.MouseButton != null && Mouse.PreviousButtonsPressed != null)
					//	// if the curren state does not have a left click and the previous state does have a left click, then the user released the mouse
					//	if (!Mouse.ButtonsPressed.Contains(MouseButtonCode.Left)
					//		 && Mouse.PreviousButtonsPressed.Contains(MouseButtonCode.Left))
					return true;
			}

			return false;
		}

		public override void HandleTextInput(string text)
		{
			base.HandleTextInput(text);

			if (!IsTextboxFull)
			{
				if (HasText)
					LabelText.Text += text;
				else
					LabelText.Text = text;

				IconInputBar.Position += new Vector(12, 0);
			}
		}

		public override void HandleKeyPressed(KeyInformation key)
		{
			base.HandleKeyPressed(key);

			if (key.VirtualKey == VirtualKeyCode.Backspace)
			{
				if (HasText)
				{
					if (LabelText.Text.Length > 1)
						LabelText.Text = LabelText.Text.Remove(LabelText.Text.Length - 1);
					else
						LabelText.Text = ".";

					IconInputBar.Position -= new Vector(12, 0);
				}
			}
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (IconFrame != null)
				IconFrame.Dispose();
			if (IconInputBar != null)
				IconInputBar.Dispose();
			if (LabelText != null)
				LabelText.DisableOutline();
		}
	}
}
