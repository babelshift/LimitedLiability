using MyThirdSDL.Content;
using MyThirdSDL.Mail;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class MenuMailbox : Control
	{
		#region Members

		private List<Control> controls = new List<Control>();

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

		#endregion

		#region Pages

		private Dictionary<int, MailItemPage> mailItemPages;
		private Dictionary<int, MailItemPage> mailItemInboxPages = new Dictionary<int, MailItemPage>();
		private Dictionary<int, MailItemPage> mailItemOutboxPages = new Dictionary<int, MailItemPage>();
		private Dictionary<int, MailItemPage> mailItemArchivePages = new Dictionary<int, MailItemPage>();

		#endregion

		#region Labels

		private Label labelFolderHeader;
		private Label labelFrom;
		private Label labelSubject;
		private Label labelPageNumber;
		private Label labelInboxFolder;
		private Label labelOutboxFolder;
		private Label labelArchiveFolder;
		private Label labelSelectedFolderHeader;

		#endregion

		#region Buttons

		private Button buttonInboxFolder;
		private Button buttonOutboxFolder;
		private Button buttonArchiveFolder;
		private Button buttonArrowLeft;
		private Button buttonArrowRight;
		private Button buttonView;
		private Button buttonArchive;
		private Button buttonCloseWindow;

		#endregion

		#region Icons

		private Icon iconFolderHeader;
		private Icon iconInboxFolder;
		private Icon iconOutboxFolder;
		private Icon iconArchiveFolder;
		private Icon iconSelectedFolderHeader;
		private Icon iconTopSeparator;

		#endregion

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
				else
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
				if (activeTab == ActiveTab.Inbox)
					return selectedMailItemInbox;
				else if (activeTab == ActiveTab.Outbox)
					return selectedMailItemOutbox;
				else if (activeTab == ActiveTab.Archive)
					return selectedMailItemArchive;
				else
					return null;
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
				buttonView.Position = new Vector(base.Position.X + 155, base.Position.Y + 272);
				buttonArchive.Position = new Vector(base.Position.X + 255, base.Position.Y + 272);
				iconTopSeparator.Position = new Vector(base.Position.X + 156, base.Position.Y + 65);
				buttonCloseWindow.Position = new Vector(base.Position.X + 656, base.Position.Y - 47);
				SetMailItemButtonPositions();
			}
		}

		#endregion

		#region Public Events

		public event EventHandler<ArchiveEventArgs> ArchiveMailButtonClicked;
		public event EventHandler<EventArgs> CloseButtonClicked;

		#endregion

		#region Constructors

		public MenuMailbox(ContentManager contentManager, IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			Texture texture = contentManager.GetTexture("MenuMailboxFrame");
			iconFrame = new Icon(texture);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = Styles.Colors.MainMenuTitleText;
			int fontSizeTitle = 14;
			int fontSizeContent = 12;

			iconFolderHeader = new Icon(contentManager.GetTexture("IconFolderOpen"));
			labelFolderHeader = new Label();
			labelFolderHeader.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColor, "Folder");
			labelFolderHeader.EnableShadow(contentManager, 2, 2);
			labelPageNumber = new Label();
			labelPageNumber.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "N/A");
			labelFrom = new Label();
			labelFrom.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "From");
			labelSubject = new Label();
			labelSubject.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Subject");

			buttonInboxFolder = new Button();
			buttonInboxFolder.TextureFrame = contentManager.GetTexture("ButtonMailFolder");
			buttonInboxFolder.TextureFrameHovered = contentManager.GetTexture("ButtonMailFolderHover");
			buttonInboxFolder.Icon = new Icon(contentManager.GetTexture("IconMailInbox"));
			buttonInboxFolder.Label = new Label();
			buttonInboxFolder.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Inbox");
			buttonInboxFolder.ButtonType = ButtonType.IconAndText;

			buttonOutboxFolder = new Button();
			buttonOutboxFolder.TextureFrame = contentManager.GetTexture("ButtonMailFolder");
			buttonOutboxFolder.TextureFrameHovered = contentManager.GetTexture("ButtonMailFolderHover");
			buttonOutboxFolder.Icon = new Icon(contentManager.GetTexture("IconMailOutbox"));
			buttonOutboxFolder.Label = new Label();
			buttonOutboxFolder.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Outbox");
			buttonOutboxFolder.ButtonType = ButtonType.IconAndText;

			buttonArchiveFolder = new Button();
			buttonArchiveFolder.TextureFrame = contentManager.GetTexture("ButtonMailFolder");
			buttonArchiveFolder.TextureFrameHovered = contentManager.GetTexture("ButtonMailFolderHover");
			buttonArchiveFolder.Icon = new Icon(contentManager.GetTexture("IconMailArchive"));
			buttonArchiveFolder.Label = new Label();
			buttonArchiveFolder.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Archive");
			buttonArchiveFolder.ButtonType = ButtonType.IconAndText;

			buttonArrowLeft = new Button();
			buttonArrowLeft.TextureFrame = contentManager.GetTexture("ButtonSquare");
			buttonArrowLeft.TextureFrameHovered = contentManager.GetTexture("ButtonSquareHover");
			buttonArrowLeft.Icon = new Icon(contentManager.GetTexture("IconArrowCircleLeft"));
			buttonArrowLeft.IconHovered = new Icon(contentManager.GetTexture("IconArrowCircleLeft"));
			buttonArrowLeft.ButtonType = ButtonType.IconOnly;

			buttonArrowRight = new Button();
			buttonArrowRight.TextureFrame = contentManager.GetTexture("ButtonSquare");
			buttonArrowRight.TextureFrameHovered = contentManager.GetTexture("ButtonSquareHover");
			buttonArrowRight.Icon = new Icon(contentManager.GetTexture("IconArrowCircleRight"));
			buttonArrowRight.IconHovered = new Icon(contentManager.GetTexture("IconArrowCircleRight"));
			buttonArrowRight.ButtonType = ButtonType.IconOnly;

			labelInboxFolder = new Label();
			labelInboxFolder.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColor, "Inbox");
			labelInboxFolder.EnableShadow(contentManager, 2, 2);
			labelOutboxFolder = new Label();
			labelOutboxFolder.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColor, "Outbox");
			labelOutboxFolder.EnableShadow(contentManager, 2, 2);
			labelArchiveFolder = new Label();
			labelArchiveFolder.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColor, "Archive");
			labelArchiveFolder.EnableShadow(contentManager, 2, 2);

			iconInboxFolder = new Icon(contentManager.GetTexture("IconMailInbox"));
			iconOutboxFolder = new Icon(contentManager.GetTexture("IconMailOutbox"));
			iconArchiveFolder = new Icon(contentManager.GetTexture("IconMailArchive"));

			buttonView = new Button();
			buttonView.TextureFrame = contentManager.GetTexture("ButtonMailAction");
			buttonView.TextureFrameHovered = contentManager.GetTexture("ButtonMailActionHover");
			buttonView.Label = new Label();
			buttonView.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "View");
			buttonView.ButtonType = ButtonType.TextOnly;
			buttonView.Visible = false;

			buttonArchive = new Button();
			buttonArchive.TextureFrame = contentManager.GetTexture("ButtonMailAction");
			buttonArchive.TextureFrameHovered = contentManager.GetTexture("ButtonMailActionHover");
			buttonArchive.Label = new Label();
			buttonArchive.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, "Archive");
			buttonArchive.ButtonType = ButtonType.TextOnly;
			buttonArchive.Visible = true;

			iconTopSeparator = new Icon(contentManager.GetTexture("IconSeparator"));

			buttonCloseWindow = new Button();
			buttonCloseWindow.TextureFrame = contentManager.GetTexture("ButtonSquare");
			buttonCloseWindow.TextureFrameHovered = contentManager.GetTexture("ButtonSquareHover");
			buttonCloseWindow.Icon = new Icon(contentManager.GetTexture("IconWindowClose"));
			buttonCloseWindow.IconHovered = new Icon(contentManager.GetTexture("IconWindowClose"));
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;

			AddButtonMailItems(contentManager, inbox, outbox, archive);

			controls.Add(iconFrame);
			controls.Add(iconFolderHeader);
			controls.Add(labelFolderHeader);
			controls.Add(labelPageNumber);
			controls.Add(labelFrom);
			controls.Add(labelSubject);
			controls.Add(buttonInboxFolder);
			controls.Add(buttonOutboxFolder);
			controls.Add(buttonArchiveFolder);
			controls.Add(buttonArrowLeft);
			controls.Add(buttonArrowRight);
			controls.Add(buttonView);
			controls.Add(buttonArchive);
			controls.Add(iconTopSeparator);
			controls.Add(buttonCloseWindow);

			this.buttonInboxFolder.Clicked += buttonInboxFolder_Clicked;
			this.buttonOutboxFolder.Clicked += buttonOutboxFolder_Clicked;
			this.buttonArchiveFolder.Clicked += buttonArchiveFolder_Clicked;
			this.buttonArrowLeft.Clicked += buttonArrowLeft_Clicked;
			this.buttonArrowRight.Clicked += buttonArrowRight_Clicked;
			this.buttonView.Clicked += buttonView_Clicked;
			this.buttonArchive.Clicked += buttonArchive_Clicked;
			this.buttonCloseWindow.Clicked += buttonCloseWindow_Clicked;

			this.currentDisplayedPageInbox = 1;
			SetActiveTab(ActiveTab.Inbox);
		}

		#endregion

		public void AddButtonMailItems(ContentManager contentManager, IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			string fontPath = contentManager.GetContentPath(Styles.FontPaths.Arcade);
			Color fontColorTitle = Styles.Colors.MainMenuTitleText;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;

			foreach (MailItem mailItem in inbox)
			{
				ButtonMailItem buttonMailItem = new ButtonMailItem(mailItem);
				buttonMailItem.TextureFrame = contentManager.GetTexture("ButtonMailItem");
				buttonMailItem.TextureFrameHovered = contentManager.GetTexture("ButtonMailItemHover");
				buttonMailItem.TextureFrameSelected = contentManager.GetTexture("ButtonMailItemSelected");
				buttonMailItem.IconMailUnread = new Icon(contentManager.GetTexture("IconMailUnread"));
				buttonMailItem.IconMailRead = new Icon(contentManager.GetTexture("IconMailRead"));
				buttonMailItem.LabelFrom = new Label();
				buttonMailItem.LabelFrom.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorTitle, mailItem.From);
				buttonMailItem.LabelSubject = new Label();
				buttonMailItem.LabelSubject.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorTitle, mailItem.Subject);
				Icon iconSeparator = new Icon(contentManager.GetTexture("IconSeparator"));
				AddButtonMailItemInbox(buttonMailItem, iconSeparator);
			}

			foreach (MailItem mailItem in outbox)
			{
				ButtonMailItem buttonMailItem = new ButtonMailItem(mailItem);
				buttonMailItem.TextureFrame = contentManager.GetTexture("ButtonMailItem");
				buttonMailItem.TextureFrameHovered = contentManager.GetTexture("ButtonMailItemHover");
				buttonMailItem.TextureFrameSelected = contentManager.GetTexture("ButtonMailItemSelected");
				buttonMailItem.IconMailUnread = new Icon(contentManager.GetTexture("IconMailUnread"));
				buttonMailItem.IconMailRead = new Icon(contentManager.GetTexture("IconMailRead"));
				buttonMailItem.LabelFrom = new Label();
				buttonMailItem.LabelFrom.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorTitle, mailItem.From);
				buttonMailItem.LabelSubject = new Label();
				buttonMailItem.LabelSubject.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorTitle, mailItem.Subject);
				Icon iconSeparator = new Icon(contentManager.GetTexture("IconSeparator"));
				AddButtonMailItemOutbox(buttonMailItem, iconSeparator);
			}

			foreach (MailItem mailItem in archive)
			{
				ButtonMailItem buttonMailItem = new ButtonMailItem(mailItem);
				buttonMailItem.TextureFrame = contentManager.GetTexture("ButtonMailItem");
				buttonMailItem.TextureFrameHovered = contentManager.GetTexture("ButtonMailItemHover");
				buttonMailItem.TextureFrameSelected = contentManager.GetTexture("ButtonMailItemSelected");
				buttonMailItem.IconMailUnread = new Icon(contentManager.GetTexture("IconMailUnread"));
				buttonMailItem.IconMailRead = new Icon(contentManager.GetTexture("IconMailRead"));
				buttonMailItem.LabelFrom = new Label();
				buttonMailItem.LabelFrom.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorTitle, mailItem.From);
				buttonMailItem.LabelSubject = new Label();
				buttonMailItem.LabelSubject.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorTitle, mailItem.Subject);
				Icon iconSeparator = new Icon(contentManager.GetTexture("IconSeparator"));
				AddButtonMailItemArchive(buttonMailItem, iconSeparator);
			}

			buttonView.Visible = false;
			buttonArchive.Visible = false;
		}

		#region Button Events

		private void buttonCloseWindow_Clicked(object sender, EventArgs e)
		{
			if (CloseButtonClicked != null)
				CloseButtonClicked(sender, e);
		}

		private void buttonArchive_Clicked(object sender, EventArgs e)
		{
			if (SelectedMailItem != null)
				if (ArchiveMailButtonClicked != null)
					ArchiveMailButtonClicked(sender, new ArchiveEventArgs(SelectedMailItem));
		}

		private void buttonView_Clicked(object sender, EventArgs e)
		{

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
				foreach (var page in mailItemPages.Values)
					foreach (var button in page.Buttons)
						button.ToggleOff();
				clickedButtonMailItem.ToggleOn();
				SelectedMailItem = clickedButtonMailItem.MailItem;
				buttonView.Visible = true;
				buttonArchive.Visible = true;
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

		#endregion

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			foreach (var control in controls)
				if (control != null)
					control.Update(gameTime);

			if (iconSelectedFolderHeader != null)
				iconSelectedFolderHeader.Update(gameTime);

			if (labelSelectedFolderHeader != null)
				labelSelectedFolderHeader.Update(gameTime);

			MailItemPage currentPage = null;
			bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
			if (success)
			{
				foreach (var button in currentPage.Buttons)
					button.Update(gameTime);
				foreach (var separator in currentPage.Separators)
					separator.Update(gameTime);
			}
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			foreach (var control in controls)
				if (control != null)
					control.Draw(gameTime, renderer);

			if (iconSelectedFolderHeader != null)
				iconSelectedFolderHeader.Draw(gameTime, renderer);

			if (labelSelectedFolderHeader != null)
				labelSelectedFolderHeader.Draw(gameTime, renderer);

			MailItemPage currentPage = null;
			bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
			if (success)
			{
				foreach (var button in currentPage.Buttons)
					button.Draw(gameTime, renderer);
				foreach (var separator in currentPage.Separators)
					separator.Draw(gameTime, renderer);
			}
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			foreach (var control in controls)
				if (control != null)
					control.HandleMouseButtonPressedEvent(sender, e);

			MailItemPage currentPage = null;
			bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
			if (success)
			{
				foreach (var button in currentPage.Buttons)
					button.HandleMouseButtonPressedEvent(sender, e);
			}
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			foreach (var control in controls)
				if (control != null)
					control.HandleMouseMovingEvent(sender, e);

			MailItemPage currentPage = null;
			bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
			if (success)
			{
				foreach (var button in currentPage.Buttons)
					button.HandleMouseMovingEvent(sender, e);
			}
		}

		#endregion

		#region Methods

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

		#endregion

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach (var control in controls)
				if (control != null)
					control.Dispose();

			if (iconSelectedFolderHeader != null)
				iconSelectedFolderHeader.Dispose();

			if (labelSelectedFolderHeader != null)
				labelSelectedFolderHeader.Dispose();

			MailItemPage currentPage = null;
			bool success = mailItemPages.TryGetValue(CurrentDisplayedPageNumber, out currentPage);
			if (success)
			{
				foreach (var button in currentPage.Buttons)
					if (button != null)
						button.Dispose();
				currentPage.Buttons.Clear();

				foreach (var separator in currentPage.Separators)
					if (separator != null)
						separator.Dispose();
				currentPage.Separators.Clear();
			}

			controls.Clear();
			mailItemPages.Clear();
		}
	}

	public class ArchiveEventArgs : EventArgs
	{
		public MailItem SelectedMailItem { get; private set; }

		public ArchiveEventArgs(MailItem selectedMailItem)
		{
			SelectedMailItem = selectedMailItem;
		}
	}
}
