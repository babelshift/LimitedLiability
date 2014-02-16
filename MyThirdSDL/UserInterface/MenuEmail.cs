using MyThirdSDL.Content;
using MyThirdSDL.Mail;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	public class MenuEmail : Menu
	{
		private string defaultText = "N/A";

		private Icon iconFrame;
		private Icon iconMainMenu;

		private Label labelMainMenu;
		private Label labelFrom;
		private Label labelSubject;
		private Label labelContent;

		private Button buttonClose;
		private Button buttonAttachment;

		private MailItem mailItem;

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

				buttonAttachment.Position = base.Position + new Vector(500, 50);
				buttonClose.Position = base.Position + new Vector(Width - buttonClose.Width, Height + 5);
			}
		}

		public MenuEmail(ContentManager contentManager)
		{
			Texture textureFrame = contentManager.GetTexture("MenuEmailFrame");
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

			buttonAttachment = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonAttachment.Icon = ControlFactory.CreateIcon(contentManager, "IconAttachment");
			buttonAttachment.IconHovered = ControlFactory.CreateIcon(contentManager, "IconAttachment");
			buttonAttachment.ButtonType = ButtonType.IconOnly;
			buttonAttachment.Clicked += ButtonAttachmentOnClicked;

			iconMainMenu = ControlFactory.CreateIcon(contentManager, "IconMailUnread");

			labelMainMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, "Read Email");
			labelMainMenu.EnableShadow(contentManager, 2, 2);

			labelFrom = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, defaultText);
			labelSubject = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, defaultText);
			labelContent = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColor, defaultText, 550);

			Controls.Add(iconFrame);
			Controls.Add(iconMainMenu);
			Controls.Add(labelContent);
			Controls.Add(labelMainMenu);
			Controls.Add(labelFrom);
			Controls.Add(labelSubject);
			Controls.Add(buttonClose);
			Controls.Add(buttonAttachment);

			Visible = false;
		}

		private void ButtonAttachmentOnClicked(object sender, EventArgs eventArgs)
		{
			if (mailItem == null) return;

			if (mailItem.Attachment == null) return;
			
			Visible = false;
			mailItem.Attachment.Open();
		}

		public void SetMailItem(MailItem mailItem)
		{
			this.mailItem = mailItem;
			labelFrom.Text = String.Format("From: {0}", this.mailItem.From);
			labelSubject.Text = String.Format("Subject: {0}", this.mailItem.Subject);
			labelContent.Text = this.mailItem.Body;
		}

		private void ButtonCloseOnClicked(object sender, EventArgs eventArgs)
		{
			Visible = false;
			if (Closed != null)
				Closed(sender, eventArgs);
		}
	}
}