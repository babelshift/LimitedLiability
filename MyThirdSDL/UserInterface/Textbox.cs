using MyThirdSDL.Content;
using SharpDL.Graphics;
using SharpDL.Input;
using System;

namespace MyThirdSDL.UserInterface
{
	public sealed class Textbox : Control
	{
		private TimeSpan timeSinceVisibleTrue = TimeSpan.Zero;
		private TimeSpan timeSinceLastUpdate = TimeSpan.Zero;

		private Icon iconFrame;
		private const string defaultText = ".";

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

		private bool HasText { get { return !String.IsNullOrEmpty(Text); } }

		private bool IsTextboxFull
		{
			get
			{
				int maxCharacterCount = (Width / LabelText.TrueTypeText.Font.PointSize) - 1;

				if (LabelText == null)
					return false;
				if (LabelText.Text.Length < maxCharacterCount)
					return false;
				return true;
			}
		}

		public string Text
		{
			get
			{
				if (LabelText.Text == defaultText)
					return String.Empty;
				else
					return LabelText.Text;
			}
		}

		public Textbox(ContentManager contentManager)
		{
			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = Styles.Colors.PaleGreen;
			const int fontSizeContent = 12;

			IconFrame = ControlFactory.CreateIcon(contentManager, "TextboxLongFrame");
			IconInputBar = ControlFactory.CreateIcon(contentManager, "IconInputBar");
			LabelText = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, ".");

			Clicked += OnClicked;

			Blur();
		}

		private void OnClicked(object sender, EventArgs eventArgs)
		{
			Focus();
		}

		public void Clear()
		{
			LabelText.Text = defaultText;
			IconInputBar.Position = new Vector(Position.X + 6, Position.Y + 6);
		}

		public override void Focus()
		{
			base.Focus();
			Keyboard.StartTextInput();
		}

		public override void Blur()
		{
			Keyboard.StopTextInput();
			base.Blur();
		}

		public override void Update(SharpDL.GameTime gameTime)
		{
			if (!Visible) return;
			
			iconFrame.Update(gameTime);

			if (IsFocused)
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

			base.Update(gameTime);
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			if (!Visible) return;
			
			iconFrame.Draw(gameTime, renderer);

			if (IsFocused)
				IconInputBar.Draw(gameTime, renderer);

			if (HasText)
				LabelText.Draw(gameTime, renderer);
		}

		public override void HandleTextInput(string text)
		{
			if (!IsFocused) return;

			base.HandleTextInput(text);

			if (IsTextboxFull) return;

			if (HasText)
				LabelText.Text += text;
			else
				LabelText.Text = text;

			IconInputBar.Position += new Vector(12, 0);
		}

		public override void HandleKeyPressed(KeyInformation key)
		{
			if (!IsFocused) return;

			base.HandleKeyPressed(key);

			if (key.VirtualKey == VirtualKeyCode.Backspace)
			{
				if (!HasText) return;

				if (LabelText.Text.Length > 1)
					LabelText.Text = LabelText.Text.Remove(LabelText.Text.Length - 1);
				else
					LabelText.Text = ".";

				IconInputBar.Position -= new Vector(12, 0);
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
				LabelText.Dispose();
		}
	}
}