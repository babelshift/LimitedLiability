using System.Collections.Generic;
using MyThirdSDL.Content;
using SharpDL;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	public enum MessageBoxType
	{
		Information,
		Warning,
		Error
	}

	public class MessageBox : Control
	{
		private const string defaultText = "NO TEXT DEFINED";

		private readonly List<Control> controls = new List<Control>(); 

		private Button buttonOK;
		private Icon iconFrame;
		private Icon iconMain;
		private Label labelTextTitle;
		private Label labelTextContent;

		public override Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				iconFrame.Position = base.Position;
				iconMain.Position = base.Position + new Vector(10, 10);
				labelTextTitle.Position = base.Position + new Vector(40, 15);
				labelTextContent.Position = base.Position + new Vector(10, 45);
				buttonOK.Position = base.Position + new Vector(iconFrame.Width - buttonOK.Width, iconFrame.Height + 5);
			}
		}

		public MessageBox(ContentManager contentManager, MessageBoxType type)
		{
			iconFrame = ControlFactory.CreateIcon(contentManager, "MessageBoxFrame");
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath(Styles.FontPaths.Arcade);
			Color fontColorTitle = Styles.Colors.White;
			Color fontColorLabelValue = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;

			labelTextTitle = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorTitle, defaultText);
			labelTextContent = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorLabelValue,
				defaultText);

			buttonOK = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonOK.Icon = ControlFactory.CreateIcon(contentManager, "IconAccept");
			buttonOK.ButtonType = ButtonType.IconOnly;

			if (type == MessageBoxType.Information)
				iconMain = ControlFactory.CreateIcon(contentManager, "IconInfo");

			controls.Add(iconFrame);
			controls.Add(iconMain);
			controls.Add(labelTextTitle);
			controls.Add(labelTextContent);
			controls.Add(buttonOK);

			buttonOK.Clicked += ButtonOK_Clicked;
			
			Hide();
		}

		public void UpdateLabels(ContentManager contentManager, string title, string content)
		{
			labelTextContent.TrueTypeText.WrapLength = 325;

			if (String.IsNullOrEmpty(title))
				labelTextTitle.Text = defaultText;
			else
				labelTextTitle.Text = title;

			if (String.IsNullOrEmpty(content))
				labelTextContent.Text = defaultText;
			else
				labelTextContent.Text = content;

			labelTextTitle.EnableShadow(contentManager, 2, 2);
		}

		public void Show()
		{
			Visible = true;
		}

		public void Hide()
		{
			Visible = false;
		}

		public override void Update(GameTime gameTime)
		{
			if (Visible)
			{
				base.Update(gameTime);

				foreach(var control in controls)
					control.Update(gameTime);
			}
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (Visible)
			{
				foreach (var control in controls)
					control.Draw(gameTime, renderer);
			}
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			if (Visible)
			{
				foreach (var control in controls)
					control.HandleMouseButtonPressedEvent(sender, e);
			}
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			if (Visible)
			{
				foreach (var control in controls)
					control.HandleMouseMovingEvent(sender, e);
			}
		}

		private void ButtonOK_Clicked(object sender, EventArgs e)
		{
			Hide();
			Dispose();
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach (var control in controls)
				control.Dispose();
		}
	}
}