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

		#endregion

		#region Public Events

		public event EventHandler<ArchiveEventArgs> ArchiveMailButtonClicked;
		public event EventHandler<EventArgs> CloseButtonClicked;

		#endregion

		#region Constructors

		public MenuMailbox(Texture texture, Vector position, Icon iconFolderHeader, Label labelFolderHeader, Label labelFrom, Label labelSubject, Label labelPageNumber,
			Icon iconInboxFolder, Icon iconOutboxFolder, Icon iconArchiveFolder, Label labelInboxFolder, Label labelOutboxFolder, Label labelArchiveFolder,
			Button buttonInboxFolder, Button buttonOutboxFolder, Button buttonArchiveFolder, Button buttonArrowLeft, Button buttonArrowRight, Button buttonView, Button buttonArchive,
			Icon iconTopSeparator, Button buttonCloseWindow)
			: base(texture, position)
		{
			this.labelFolderHeader = labelFolderHeader;
			this.labelFrom = labelFrom;
			this.labelSubject = labelSubject;
			this.labelPageNumber = labelPageNumber;
			this.iconFolderHeader = iconFolderHeader;
			this.buttonInboxFolder = buttonInboxFolder;
			this.buttonOutboxFolder = buttonOutboxFolder;
			this.buttonArchiveFolder = buttonArchiveFolder;
			this.buttonArrowLeft = buttonArrowLeft;
			this.buttonArrowRight = buttonArrowRight;
			this.buttonView = buttonView;
			this.buttonArchive = buttonArchive;

			this.labelInboxFolder = labelInboxFolder;
			this.labelOutboxFolder = labelOutboxFolder;
			this.labelArchiveFolder = labelArchiveFolder;

			this.iconInboxFolder = iconInboxFolder;
			this.iconOutboxFolder = iconOutboxFolder;
			this.iconArchiveFolder = iconArchiveFolder;

			this.iconTopSeparator = iconTopSeparator;

			this.buttonCloseWindow = buttonCloseWindow;

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
			base.Update(gameTime);

			labelFolderHeader.Update(gameTime);
			labelFrom.Update(gameTime);
			labelSubject.Update(gameTime);
			labelPageNumber.Update(gameTime);
			iconFolderHeader.Update(gameTime);
			buttonInboxFolder.Update(gameTime);
			buttonOutboxFolder.Update(gameTime);
			buttonArchiveFolder.Update(gameTime);
			buttonArrowLeft.Update(gameTime);
			buttonArrowRight.Update(gameTime);
			buttonView.Update(gameTime);
			buttonArchive.Update(gameTime);

			iconSelectedFolderHeader.Update(gameTime);
			labelSelectedFolderHeader.Update(gameTime);

			iconTopSeparator.Update(gameTime);

			buttonCloseWindow.Update(gameTime);

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
			base.Draw(gameTime, renderer);

			labelFolderHeader.Draw(gameTime, renderer);
			labelFrom.Draw(gameTime, renderer);
			labelSubject.Draw(gameTime, renderer);
			labelPageNumber.Draw(gameTime, renderer);
			iconFolderHeader.Draw(gameTime, renderer);
			buttonInboxFolder.Draw(gameTime, renderer);
			buttonOutboxFolder.Draw(gameTime, renderer);
			buttonArchiveFolder.Draw(gameTime, renderer);
			buttonArrowLeft.Draw(gameTime, renderer);
			buttonArrowRight.Draw(gameTime, renderer);
			buttonView.Draw(gameTime, renderer);
			buttonArchive.Draw(gameTime, renderer);

			iconSelectedFolderHeader.Draw(gameTime, renderer);
			labelSelectedFolderHeader.Draw(gameTime, renderer);

			iconTopSeparator.Draw(gameTime, renderer);

			buttonCloseWindow.Draw(gameTime, renderer);

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
