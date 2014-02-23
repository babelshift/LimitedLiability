using MyThirdSDL.Content;
using MyThirdSDL.Mail;
using SharpDL.Graphics;
using System;
using MyThirdSDL.Content.Data;

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
		private Button buttonCloseWindow;
		private Button buttonOpenAttachment;
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

				buttonCloseWindow.Position = base.Position + new Vector(Width - buttonCloseWindow.Width, Height + 5);
				buttonCloseWindow.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
				buttonOpenAttachment.Position = base.Position + new Vector(Width - buttonCloseWindow.Width - 5 - buttonOpenAttachment.Width, Height + 5);
				buttonOpenAttachment.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
			}
		}

		public MenuEmail(ContentManager contentManager)
		{
			Texture textureFrame = contentManager.GetTexture("MenuEmailFrame");
			iconFrame = new Icon(textureFrame);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColorWhite = Styles.Colors.White;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeTooltip = Styles.FontSizes.Tooltip;

			buttonCloseWindow = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;
			buttonCloseWindow.Clicked += ButtonCloseOnClicked;
			buttonCloseWindow.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColorWhite, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_CLOSE_WINDOW));

			buttonOpenAttachment = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonOpenAttachment.Icon = ControlFactory.CreateIcon(contentManager, "IconAttachment");
			buttonOpenAttachment.IconHovered = ControlFactory.CreateIcon(contentManager, "IconAttachment");
			buttonOpenAttachment.ButtonType = ButtonType.IconOnly;
			buttonOpenAttachment.Clicked += ButtonAttachmentOnClicked;
			buttonOpenAttachment.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColorWhite, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_OPEN_ATTACHMENT));

			iconMainMenu = ControlFactory.CreateIcon(contentManager, "IconMailUnread");

			labelMainMenu = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, "Read Email");
			labelMainMenu.EnableShadow(contentManager, 2, 2);

			labelFrom = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, defaultText);
			labelSubject = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, defaultText);
			labelContent = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, defaultText, 550);

			Controls.Add(iconFrame);
			Controls.Add(iconMainMenu);
			Controls.Add(labelContent);
			Controls.Add(labelMainMenu);
			Controls.Add(labelFrom);
			Controls.Add(labelSubject);
			Controls.Add(buttonCloseWindow);
			Controls.Add(buttonOpenAttachment);

			Visible = false;
		}

		private void ButtonAttachmentOnClicked(object sender, EventArgs eventArgs)
		{
			if (mailItem == null)
				return;

			if (mailItem.Attachment == null)
				return;
			
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