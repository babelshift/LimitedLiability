using MyThirdSDL.Content;
using MyThirdSDL.Mail;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	public class MenuEmail : Menu
	{
		private Icon iconFrame;
		private Icon iconMainMenu;

		private Label labelMainMenu;
		private Label labelFrom;
		private Label labelSubject;
		private Label labelContent;

		private Button buttonClose;

		public event EventHandler Closed;

		public override Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				iconFrame.Position = base.Position;
				iconMainMenu.Position = base.Position + new Vector(3, 5);
				labelMainMenu.Position = base.Position + new Vector(38, 15);
				labelFrom.Position = base.Position + new Vector(5, 50);
				labelSubject.Position = base.Position + new Vector(5, 70);
				labelContent.Position = base.Position + new Vector(5, 110);

				buttonClose.Position = base.Position + new Vector(Width - buttonClose.Width, Height + 5);
			}
		}

		public MenuEmail(ContentManager contentManager, MailItem mailItem)
		{
			Texture textureFrame = contentManager.GetTexture("MenuResumeFrame");
			iconFrame = new Icon(textureFrame);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = Styles.Colors.White;
			Color fontColorValue = Styles.Colors.PaleYellow;
			int fontSizeTitle = 14;
			int fontSizeContent = 12;

			buttonClose = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonClose.Icon = ControlFactory.CreateIcon(contentManager, "IconAccept");
			buttonClose.IconHovered = ControlFactory.CreateIcon(contentManager, "IconAccept");
			buttonClose.ButtonType = ButtonType.IconOnly;
			buttonClose.Clicked += ButtonCloseOnClicked;

			iconMainMenu = ControlFactory.CreateIcon(contentManager, "IconMailUnread");

			labelMainMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, "Read Email");
			labelMainMenu.EnableShadow(contentManager, 2, 2);

			labelFrom = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, String.Format("From: {0}", mailItem.From));
			labelSubject = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor,
				String.Format("Subject: {0}", mailItem.Subject));
			labelContent = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, mailItem.Body, 550);

			Controls.Add(iconFrame);
			Controls.Add(iconMainMenu);
			Controls.Add(labelContent);
			Controls.Add(labelMainMenu);
			Controls.Add(labelFrom);
			Controls.Add(labelSubject);
			Controls.Add(buttonClose);

			Visible = false;
		}

		private void ButtonCloseOnClicked(object sender, EventArgs eventArgs)
		{
			Visible = false;
			if (Closed != null)
				Closed(sender, eventArgs);
		}
	}
}