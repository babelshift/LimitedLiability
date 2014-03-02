using MyThirdSDL.Content;
using MyThirdSDL.Mail;
using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using MyThirdSDL.Content.Data;

namespace MyThirdSDL.UserInterface
{
	public class MenuMailbox : Menu
	{

		#region Members

		private Icon iconFrame;

		private enum ActiveTab
		{
			Inbox,
			Outbox,
			Archive
		}

		private ActiveTab activeTab;
		private const int itemsPerPage = 5;
		private int currentDisplayedPageInbox = 1;
		private int currentDisplayedPageOutbox = 1;
		private int currentDisplayedPageArchive = 1;
		private MailItem selectedMailItemInbox;
		private MailItem selectedMailItemOutbox;
		private MailItem selectedMailItemArchive;
		private MenuEmail menuEmail;

		#endregion Members

		#region Pages

		private Dictionary<int, MailItemPage> mailItemPages;
		private Dictionary<int, MailItemPage> mailItemInboxPages = new Dictionary<int, MailItemPage>();
		private Dictionary<int, MailItemPage> mailItemOutboxPages = new Dictionary<int, MailItemPage>();
		private Dictionary<int, MailItemPage> mailItemArchivePages = new Dictionary<int, MailItemPage>();

		#endregion Pages

		#region Labels

		private Label labelFolderHeader;
		private Label labelFrom;
		private Label labelSubject;
		private Label labelPageNumber;
		private Label labelInboxFolder;
		private Label labelOutboxFolder;
		private Label labelArchiveFolder;
		private Label labelSelectedFolderHeader;

		#endregion Labels

		#region Buttons

		private Button buttonInboxFolder;
		private Button buttonOutboxFolder;
		private Button buttonArchiveFolder;
		private Button buttonArrowLeft;
		private Button buttonArrowRight;
		private Button buttonMailOpen;
		private Button buttonMailArchive;
		private Button buttonCloseWindow;

		#endregion Buttons

		#region Icons

		private Icon iconFolderHeader;
		private Icon iconInboxFolder;
		private Icon iconOutboxFolder;
		private Icon iconArchiveFolder;
		private Icon iconSelectedFolderHeader;
		private Icon iconTopSeparator;

		#endregion Icons

		#region Properties

		private int CurrentDisplayedPageNumber
		{
			get
			{
				int currentDisplayedPage = 0;

				if (activeTab == ActiveTab.Inbox)
					currentDisplayedPage = currentDisplayedPageInbox;
				else if (activeTab == ActiveTab.Outbox)
					currentDisplayedPage = currentDisplayedPageOutbox;
				else if (activeTab == ActiveTab.Archive)
					currentDisplayedPage = currentDisplayedPageArchive;

				if (currentDisplayedPage > CurrentDisplayedPageCount)
					return CurrentDisplayedPageCount;

				return currentDisplayedPage;
			}
		}

		private int CurrentDisplayedPageCount
		{
			get
			{
				int currentDisplayedPageCount = 0;

				if (activeTab == ActiveTab.Inbox)
					currentDisplayedPageCount = mailItemInboxPages.Count();
				else if (activeTab == ActiveTab.Outbox)
					currentDisplayedPageCount = mailItemOutboxPages.Count();
				else if (activeTab == ActiveTab.Archive)
					currentDisplayedPageCount = mailItemArchivePages.Count();

				return currentDisplayedPageCount;
			}
		}

		private MailItem SelectedMailItem
		{
			get
			{
				switch (activeTab)
				{
					case ActiveTab.Inbox:
						return selectedMailItemInbox;

					case ActiveTab.Outbox:
						return selectedMailItemOutbox;

					case ActiveTab.Archive:
						return selectedMailItemArchive;

					default:
						return null;
				}
			}
			set
			{
				if (activeTab == ActiveTab.Inbox)
					selectedMailItemInbox = value;
				else if (activeTab == ActiveTab.Outbox)
					selectedMailItemOutbox = value;
				else if (activeTab == ActiveTab.Archive)
					selectedMailItemArchive = value;
			}
		}

		private int InboxPageCount { get { return mailItemInboxPages.Count(); } }

		private int OutboxPageCount { get { return mailItemOutboxPages.Count(); } }

		private int ArchivePageCount { get { return mailItemArchivePages.Count(); } }

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				iconFrame.Position = base.Position;
				iconFolderHeader.Position = new Vector(base.Position.X + 5, base.Position.Y + 5);
				labelFolderHeader.Position = new Vector(base.Position.X + 40, base.Position.Y + 15);
				labelPageNumber.Position = new Vector(base.Position.X + 450, base.Position.Y + 278);
				labelFrom.Position = new Vector(base.Position.X + 208, base.Position.Y + 50);
				labelSubject.Position = new Vector(base.Position.X + 340, base.Position.Y + 50);
				buttonInboxFolder.Position = new Vector(base.Position.X + 5, base.Position.Y + 50);
				buttonOutboxFolder.Position = new Vector(base.Position.X + 5, base.Position.Y + 100);
				buttonArchiveFolder.Position = new Vector(base.Position.X + 5, base.Position.Y + 150);
				buttonArrowLeft.Position = new Vector(base.Position.X + 600, base.Position.Y + 263);
				buttonArrowRight.Position = new Vector(base.Position.X + 650, base.Position.Y + 263);
				labelInboxFolder.Position = new Vector(base.Position.X + 185, base.Position.Y + 15);
				labelOutboxFolder.Position = new Vector(base.Position.X + 185, base.Position.Y + 15);
				labelArchiveFolder.Position = new Vector(base.Position.X + 185, base.Position.Y + 15);
				iconInboxFolder.Position = new Vector(base.Position.X + 150, base.Position.Y + 5);
				iconOutboxFolder.Position = new Vector(base.Position.X + 150, base.Position.Y + 5);
				iconArchiveFolder.Position = new Vector(base.Position.X + 150, base.Position.Y + 5);
				iconTopSeparator.Position = new Vector(base.Position.X + 156, base.Position.Y + 65);
				buttonCloseWindow.Position = new Vector(base.Position.X + Width - buttonCloseWindow.Width, base.Position.Y + Height + 5);
				buttonMailArchive.Position = new Vector(base.Position.X + Width - buttonCloseWindow.Width - 5 - buttonMailArchive.Width, base.Position.Y + Height + 5);
				buttonMailOpen.Position = new Vector(base.Position.X + Width - buttonCloseWindow.Width - 5 - buttonMailArchive.Width - 5 - buttonMailOpen.Width, base.Position.Y + Height + 5);
				buttonCloseWindow.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
				buttonMailOpen.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
				buttonMailArchive.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);

				SetMailItemButtonPositions();
			}
		}

		#endregion Properties

		#region Public Events

		public event EventHandler<SelectedMailItemActionEventArgs> ArchiveMailButtonClicked;
		public event EventHandler<EventArgs> CloseButtonClicked;

		#endregion Public Events

		#region Constructors

		public MenuMailbox(ContentManager contentManager, IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			Texture texture = contentManager.GetTexture("MenuMailboxFrame");
			iconFrame = new Icon(texture);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorPaleYellow = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;
			int fontSizeTooltip = Styles.FontSizes.Tooltip;

			iconFolderHeader = ControlFactory.CreateIcon(contentManager, "IconFolderOpen");
			labelFolderHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, "Folder");
			labelFolderHeader.EnableShadow(contentManager, 2, 2);
			labelPageNumber = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, "N/A");
			labelFrom = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "From");
			labelSubject = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, "Subject");

			buttonInboxFolder = ControlFactory.CreateButton(contentManager, "ButtonMailFolder", "ButtonMailFolderHover");
			buttonInboxFolder.Icon = ControlFactory.CreateIcon(contentManager, "IconFolderInbox");
			buttonInboxFolder.Label = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, "Inbox");
			buttonInboxFolder.ButtonType = ButtonType.IconAndText;

			buttonOutboxFolder = ControlFactory.CreateButton(contentManager, "ButtonMailFolder", "ButtonMailFolderHover");
			buttonOutboxFolder.Icon = ControlFactory.CreateIcon(contentManager, "IconFolderOutbox");
			buttonOutboxFolder.Label = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, "Outbox");
			buttonOutboxFolder.ButtonType = ButtonType.IconAndText;

			buttonArchiveFolder = ControlFactory.CreateButton(contentManager, "ButtonMailFolder", "ButtonMailFolderHover");
			buttonArchiveFolder.Icon = ControlFactory.CreateIcon(contentManager, "IconFolderArchive");
			buttonArchiveFolder.Label = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorPaleYellow, "Archive");
			buttonArchiveFolder.ButtonType = ButtonType.IconAndText;

			buttonArrowLeft = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonArrowLeft.Icon = ControlFactory.CreateIcon(contentManager, "IconArrowCircleLeft");
			buttonArrowLeft.IconHovered = ControlFactory.CreateIcon(contentManager, "IconArrowCircleLeft");
			buttonArrowLeft.ButtonType = ButtonType.IconOnly;

			buttonArrowRight = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonArrowRight.Icon = ControlFactory.CreateIcon(contentManager, "IconArrowCircleRight");
			buttonArrowRight.IconHovered = ControlFactory.CreateIcon(contentManager, "IconArrowCircleRight");
			buttonArrowRight.ButtonType = ButtonType.IconOnly;

			labelInboxFolder = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, "Inbox");
			labelInboxFolder.EnableShadow(contentManager, 2, 2);
			labelOutboxFolder = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, "Outbox");
			labelOutboxFolder.EnableShadow(contentManager, 2, 2);
			labelArchiveFolder = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, "Archive");
			labelArchiveFolder.EnableShadow(contentManager, 2, 2);

			iconInboxFolder = ControlFactory.CreateIcon(contentManager, "IconFolderInbox");
			iconOutboxFolder = ControlFactory.CreateIcon(contentManager, "IconFolderOutbox");
			iconArchiveFolder = ControlFactory.CreateIcon(contentManager, "IconFolderArchive");

			buttonMailOpen = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonMailOpen.Icon = ControlFactory.CreateIcon(contentManager, "IconMailOpen");
			buttonMailOpen.ButtonType = ButtonType.IconOnly;
			buttonMailOpen.Visible = false;
			buttonMailOpen.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColorWhite, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_OPEN_MAIL));

			buttonMailArchive = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonMailArchive.Icon = ControlFactory.CreateIcon(contentManager, "IconMailArchive");
			buttonMailArchive.ButtonType = ButtonType.IconOnly;
			buttonMailArchive.Visible = true;
			buttonMailArchive.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColorWhite, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_ARCHIVE_MAIL));

			iconTopSeparator = ControlFactory.CreateIcon(contentManager, "IconSeparator");

			buttonCloseWindow = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;
			buttonCloseWindow.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColorWhite, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_CLOSE_WINDOW));

			menuEmail = new MenuEmail(contentManager);
			menuEmail.Closed += MenuEmailOnClosed;

			AddButtonMailItems(contentManager, inbox, outbox, archive);

			Controls.Add(iconFrame);
			Controls.Add(iconFolderHeader);
			Controls.Add(labelFolderHeader);
			Controls.Add(labelPageNumber);
			Controls.Add(labelFrom);
			Controls.Add(labelSubject);
			Controls.Add(buttonInboxFolder);
			Controls.Add(buttonOutboxFolder);
			Controls.Add(buttonArchiveFolder);
			Controls.Add(buttonArrowLeft);
			Controls.Add(buttonArrowRight);
			Controls.Add(buttonMailOpen);
			Controls.Add(buttonMailArchive);
			Controls.Add(iconTopSeparator);
			Controls.Add(buttonCloseWindow);

			buttonInboxFolder.Clicked += buttonInboxFolder_Clicked;
			buttonOutboxFolder.Clicked += buttonOutboxFolder_Clicked;
			buttonArchiveFolder.Clicked += buttonArchiveFolder_Clicked;
			buttonArrowLeft.Clicked += buttonArrowLeft_Clicked;
			buttonArrowRight.Clicked += buttonArrowRight_Clicked;
			buttonMailOpen.Clicked += buttonView_Clicked;
			buttonMailArchive.Clicked += buttonArchive_Clicked;
			buttonCloseWindow.Clicked += buttonCloseWindow_Clicked;

			currentDisplayedPageInbox = 1;
			SetActiveTab(ActiveTab.Inbox);

			Visible = false;
		}

		private void MenuEmailOnClosed(object sender, EventArgs eventArgs)
		{
			Visible = true;
		}

		#endregion Constructors

		#region Button Events

		private void buttonCloseWindow_Clicked(object sender, EventArgs e)
		{
			if (CloseButtonClicked != null)
				CloseButtonClicked(sender, e);
		}

		private void buttonArchive_Clicked(object sender, EventArgs e)
		{
			OnArchive(sender);
		}

		private void buttonView_Clicked(object sender, EventArgs e)
		{
			OnViewButtonClicked();
		}

		private void OnViewButtonClicked()
		{
			Visible = false;
			menuEmail.SetMailItem(SelectedMailItem);
			menuEmail.Position = Position;
			menuEmail.Visible = true;
		}

		private void buttonArrowRight_Clicked(object sender, EventArgs e)
		{
			NextPage();
		}

		private void buttonArrowLeft_Clicked(object sender, EventArgs e)
		{
			PreviousPage();
		}

		private void buttonMailItem_Clicked(object sender, EventArgs e)
		{
			ButtonMailItem clickedButtonMailItem = sender as ButtonMailItem;
			if (clickedButtonMailItem != null)
			{
				//foreach (var page in mailItemPages.Values)
				//	foreach (var button in page.Buttons)
				//		button.ToggleOff();
				//clickedButtonMailItem.ToggleOn();
				SelectedMailItem = clickedButtonMailItem.MailItem;
				buttonMailOpen.Visible = true;
				buttonMailArchive.Visible = true;
			}
		}

		private void buttonArchiveFolder_Clicked(object sender, EventArgs e)
		{
			SetActiveTab(ActiveTab.Archive);
		}

		private void buttonOutboxFolder_Clicked(object sender, EventArgs e)
		{
			SetActiveTab(ActiveTab.Outbox);
		}

		private void buttonInboxFolder_Clicked(object sender, EventArgs e)
		{
			SetActiveTab(ActiveTab.Inbox);
		}

		private void OnArchive(object sender)
		{
			if (SelectedMailItem != null)
			if (ArchiveMailButtonClicked != null)
				ArchiveMailButtonClicked(sender, new SelectedMailItemActionEventArgs(SelectedMailItem));
		}

		public void ArchiveSelectedMailItem(object sender)
		{
			if (ArchiveMailButtonClicked != null)
				ArchiveMailButtonClicked(sender, new SelectedMailItemActionEventArgs(SelectedMailItem));
		}

		#endregion Button Events

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			if (Visible)
			{
				base.Update(gameTime);

				if (iconSelectedFolderHeader != null)
					iconSelectedFolderHeader.Update(gameTime);

				if (labelSelectedFolderHeader != null)
					labelSelectedFolderHeader.Update(gameTime);

				MailItemPage currentPage;
				bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
				if (success)
				{
					foreach (var button in currentPage.Buttons)
						button.Update(gameTime);
					foreach (var separator in currentPage.Separators)
						separator.Update(gameTime);
				}
			}
			else
				menuEmail.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (Visible)
			{
				base.Draw(gameTime, renderer);

				if (iconSelectedFolderHeader != null)
					iconSelectedFolderHeader.Draw(gameTime, renderer);

				if (labelSelectedFolderHeader != null)
					labelSelectedFolderHeader.Draw(gameTime, renderer);

				MailItemPage currentPage;
				bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
				if (success)
				{
					foreach (var button in currentPage.Buttons)
						button.Draw(gameTime, renderer);
					foreach (var separator in currentPage.Separators)
						separator.Draw(gameTime, renderer);
				}
			}
			else
				menuEmail.Draw(gameTime, renderer);
		}

		public override void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			if (Visible)
			{
				base.HandleMouseButtonPressedEvent(sender, e);

				MailItemPage currentPage;
				bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
				if (success)
					foreach (var button in currentPage.Buttons)
						button.HandleMouseButtonPressedEvent(sender, e);
			}
			else
				menuEmail.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			if (Visible)
			{
				base.HandleMouseMovingEvent(sender, e);

				MailItemPage currentPage;
				bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
				if (success)
					foreach (var button in currentPage.Buttons)
						button.HandleMouseMovingEvent(sender, e);
			}
			else
				menuEmail.HandleMouseMovingEvent(sender, e);
		}

		#endregion Game Loop

		#region Methods

		public void AddButtonMailItems(ContentManager contentManager, IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			string fontPath = contentManager.GetContentPath(Styles.Fonts.Arcade);
			Color fontColorTitle = Styles.Colors.PaleGreen;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;

			foreach (MailItem mailItem in inbox)
			{
				ButtonMailItem buttonMailItem = new ButtonMailItem(mailItem);
				buttonMailItem.TextureFrame = contentManager.GetTexture("ButtonMailItem");
				//buttonMailItem.TextureFrameHovered = contentManager.GetTexture("ButtonMailItemHover");
				//buttonMailItem.TextureFrameSelected = contentManager.GetTexture("ButtonMailItemSelected");
				buttonMailItem.IconMailUnread = ControlFactory.CreateIcon(contentManager, "IconMailUnread");
				buttonMailItem.IconMailRead = ControlFactory.CreateIcon(contentManager, "IconMailRead");
				buttonMailItem.LabelFrom = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorTitle, mailItem.From);
				buttonMailItem.LabelSubject = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorTitle, mailItem.Subject);
				Icon iconSeparator = ControlFactory.CreateIcon(contentManager, "IconSeparator");
				AddButtonMailItemInbox(buttonMailItem, iconSeparator);
			}

			foreach (MailItem mailItem in outbox)
			{
				ButtonMailItem buttonMailItem = new ButtonMailItem(mailItem);
				buttonMailItem.TextureFrame = contentManager.GetTexture("ButtonMailItem");
				//buttonMailItem.TextureFrameHovered = contentManager.GetTexture("ButtonMailItemHover");
				//buttonMailItem.TextureFrameSelected = contentManager.GetTexture("ButtonMailItemSelected");
				buttonMailItem.IconMailUnread = ControlFactory.CreateIcon(contentManager, "IconMailUnread");
				buttonMailItem.IconMailRead = ControlFactory.CreateIcon(contentManager, "IconMailRead");
				buttonMailItem.LabelFrom = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorTitle, mailItem.From);
				buttonMailItem.LabelSubject = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorTitle, mailItem.Subject);
				Icon iconSeparator = ControlFactory.CreateIcon(contentManager, "IconSeparator");
				AddButtonMailItemOutbox(buttonMailItem, iconSeparator);
			}

			foreach (MailItem mailItem in archive)
			{
				ButtonMailItem buttonMailItem = new ButtonMailItem(mailItem);
				buttonMailItem.TextureFrame = contentManager.GetTexture("ButtonMailItem");
				//buttonMailItem.TextureFrameHovered = contentManager.GetTexture("ButtonMailItemHover");
				//buttonMailItem.TextureFrameSelected = contentManager.GetTexture("ButtonMailItemSelected");
				buttonMailItem.IconMailUnread = ControlFactory.CreateIcon(contentManager, "IconMailUnread");
				buttonMailItem.IconMailRead = ControlFactory.CreateIcon(contentManager, "IconMailRead");
				buttonMailItem.LabelFrom = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorTitle, mailItem.From);
				buttonMailItem.LabelSubject = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorTitle, mailItem.Subject);
				Icon iconSeparator = ControlFactory.CreateIcon(contentManager, "IconSeparator");
				AddButtonMailItemArchive(buttonMailItem, iconSeparator);
			}

			buttonMailOpen.Visible = false;
			buttonMailArchive.Visible = false;
		}

		public void AddButtonMailItemInbox(ButtonMailItem buttonMailItem, Icon iconSeparator)
		{
			AddButtonMailItem(buttonMailItem, mailItemInboxPages, iconSeparator);
		}

		public void AddButtonMailItemOutbox(ButtonMailItem buttonMailItem, Icon iconSeparator)
		{
			AddButtonMailItem(buttonMailItem, mailItemOutboxPages, iconSeparator);
		}

		public void AddButtonMailItemArchive(ButtonMailItem buttonMailItem, Icon iconSeparator)
		{
			AddButtonMailItem(buttonMailItem, mailItemArchivePages, iconSeparator);
		}

		private void AddButtonMailItem(ButtonMailItem buttonMailItem, IDictionary<int, MailItemPage> mailItemPages, Icon iconSeparator)
		{
			// the last page number will indicate the key in which we check for the next items to add
			int lastPageNumber = mailItemPages.Keys.Count();
			int buttonsOnLastPageCount = 0;
			MailItemPage lastPage = null;
			bool success = mailItemPages.TryGetValue(lastPageNumber, out lastPage);
			if (success)
			{
				// if there are pages in this page collection and the last page contains less than 4 entries, add the new entry to that page
				if (lastPage.Buttons.Count() < itemsPerPage)
				{
					lastPage.AddButton(buttonMailItem);
					lastPage.AddSeparator(iconSeparator);
				}
				// if there are 4 items on the last page, create a new page and add the item
				else
				{
					lastPage = new MailItemPage();
					lastPage.AddButton(buttonMailItem);
					lastPage.AddSeparator(iconSeparator);
					mailItemPages.Add(lastPageNumber + 1, lastPage);
				}
			}
			else
			{
				// if there are no pages, create an entry and add the first page
				lastPage = new MailItemPage();
				lastPage.AddButton(buttonMailItem);
				lastPage.AddSeparator(iconSeparator);
				mailItemPages.Add(1, lastPage);
			}

			buttonsOnLastPageCount = lastPage.Buttons.Count();
			Vector buttonMailItemPosition = Vector.Zero;
			Vector iconSeparatorPosition = Vector.Zero;

			if (buttonsOnLastPageCount == 1)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 102);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 70);
			}
			else if (buttonsOnLastPageCount == 2)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 139);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 107);
			}
			else if (buttonsOnLastPageCount == 3)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 176);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 144);
			}
			else if (buttonsOnLastPageCount == 4)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 213);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 181);
			}
			else if (buttonsOnLastPageCount == 5)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 250);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 218);
			}
			else if (buttonsOnLastPageCount >= itemsPerPage + 1)
				return;

			iconSeparator.Position = iconSeparatorPosition;
			buttonMailItem.Position = buttonMailItemPosition;
			buttonMailItem.Clicked += buttonMailItem_Clicked;

			SetActiveTab(activeTab);
			SetLabelPageNumber();
		}

		private void SetMailItemButtonPositions()
		{
			foreach (int key in mailItemPages.Keys)
			{
				MailItemPage currentPage = null;
				bool success = mailItemPages.TryGetValue(key, out currentPage);
				if (success)
				{
					foreach (var button in currentPage.Buttons)
					{
						if (currentPage.Buttons.IndexOf(button) == 0)
							button.Position = new Vector(Position.X + 158, Position.Y + 70);
						else if (currentPage.Buttons.IndexOf(button) == 1)
							button.Position = new Vector(Position.X + 158, Position.Y + 107);
						else if (currentPage.Buttons.IndexOf(button) == 2)
							button.Position = new Vector(Position.X + 158, Position.Y + 144);
						else if (currentPage.Buttons.IndexOf(button) == 3)
							button.Position = new Vector(Position.X + 158, Position.Y + 181);
						else if (currentPage.Buttons.IndexOf(button) == 4)
							button.Position = new Vector(Position.X + 158, Position.Y + 218);
					}
					foreach (var separator in currentPage.Separators)
					{
						if (currentPage.Separators.IndexOf(separator) == 0)
							separator.Position = new Vector(Position.X + 156, Position.Y + 102);
						else if (currentPage.Separators.IndexOf(separator) == 1)
							separator.Position = new Vector(Position.X + 156, Position.Y + 139);
						else if (currentPage.Separators.IndexOf(separator) == 2)
							separator.Position = new Vector(Position.X + 156, Position.Y + 176);
						else if (currentPage.Separators.IndexOf(separator) == 3)
							separator.Position = new Vector(Position.X + 156, Position.Y + 213);
						else if (currentPage.Separators.IndexOf(separator) == 4)
							separator.Position = new Vector(Position.X + 156, Position.Y + 250);
					}
				}
			}
		}

		private void SetActiveTab(ActiveTab activeTab)
		{
			if (activeTab == ActiveTab.Inbox)
			{
				mailItemPages = mailItemInboxPages;
				iconSelectedFolderHeader = iconInboxFolder;
				labelSelectedFolderHeader = labelInboxFolder;
				this.activeTab = activeTab;
			}
			else if (activeTab == ActiveTab.Outbox)
			{
				mailItemPages = mailItemOutboxPages;
				iconSelectedFolderHeader = iconOutboxFolder;
				labelSelectedFolderHeader = labelOutboxFolder;
				this.activeTab = activeTab;
			}
			else if (activeTab == ActiveTab.Archive)
			{
				mailItemPages = mailItemArchivePages;
				iconSelectedFolderHeader = iconArchiveFolder;
				labelSelectedFolderHeader = labelArchiveFolder;
				this.activeTab = activeTab;
			}

			SetLabelPageNumber();
		}

		private void SetLabelPageNumber()
		{
			labelPageNumber.Text = String.Format("Page {0} of {1}", CurrentDisplayedPageNumber, CurrentDisplayedPageCount);
		}

		private void NextPage()
		{
			if (activeTab == ActiveTab.Inbox)
			{
				if (currentDisplayedPageInbox < CurrentDisplayedPageCount)
					currentDisplayedPageInbox++;
			}
			else if (activeTab == ActiveTab.Outbox)
			{
				if (currentDisplayedPageOutbox < CurrentDisplayedPageCount)
					currentDisplayedPageOutbox++;
			}
			else if (activeTab == ActiveTab.Archive)
			{
				if (currentDisplayedPageArchive < CurrentDisplayedPageCount)
					currentDisplayedPageArchive++;
			}

			SetLabelPageNumber();
		}

		private void PreviousPage()
		{
			if (activeTab == ActiveTab.Inbox)
			{
				if (currentDisplayedPageInbox > 1)
					currentDisplayedPageInbox--;
			}
			else if (activeTab == ActiveTab.Outbox)
			{
				if (currentDisplayedPageOutbox > 1)
					currentDisplayedPageOutbox--;
			}
			else if (activeTab == ActiveTab.Archive)
			{
				if (currentDisplayedPageArchive > 1)
					currentDisplayedPageArchive--;
			}

			SetLabelPageNumber();
		}

		public void ClearButtonsAndSeparators()
		{
			foreach (var page in mailItemInboxPages.Values)
				page.Clear();
			mailItemInboxPages.Clear();

			foreach (var page in mailItemOutboxPages.Values)
				page.Clear();
			mailItemOutboxPages.Clear();

			foreach (var page in mailItemArchivePages.Values)
				page.Clear();
			mailItemArchivePages.Clear();

			mailItemPages = new Dictionary<int, MailItemPage>();
		}

		#endregion Methods

		#region Dispose

		private void Dispose(bool disposing)
		{
			base.Dispose();

			if (iconSelectedFolderHeader != null)
				iconSelectedFolderHeader.Dispose();

			if (labelSelectedFolderHeader != null)
				labelSelectedFolderHeader.Dispose();

			foreach (var key in mailItemPages.Keys)
			{
				foreach (var button in mailItemPages[key].Buttons)
					if (button != null)
						button.Dispose();

				mailItemPages[key].Buttons.Clear();

				foreach (var separator in mailItemPages[key].Separators)
					if (separator != null)
						separator.Dispose();

				mailItemPages[key].Separators.Clear();
			}

			mailItemPages.Clear();
		}

		#endregion Dispose

	}

	public class SelectedMailItemActionEventArgs : EventArgs
	{
		public MailItem SelectedMailItem { get; private set; }

		public SelectedMailItemActionEventArgs(MailItem selectedMailItem)
		{
			SelectedMailItem = selectedMailItem;
		}
	}
}