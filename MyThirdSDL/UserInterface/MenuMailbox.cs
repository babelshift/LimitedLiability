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
		private const int itemsPerPage = 5;

		private Label labelFolderHeader;
		private Label labelFrom;
		private Label labelSubject;
		private Label labelPageNumber;

		private Icon iconFolderHeader;
		private Icon iconInboxFolder;
		private Icon iconOutboxFolder;
		private Icon iconArchiveFolder;

		private Label labelInboxFolder;
		private Label labelOutboxFolder;
		private Label labelArchiveFolder;

		private Button buttonInboxFolder;
		private Button buttonOutboxFolder;
		private Button buttonArchiveFolder;
		private Button buttonArrowLeft;
		private Button buttonArrowRight;

		private Icon iconSelectedFolderHeader;
		private Label labelSelectedFolderHeader;

		private Dictionary<int, List<ButtonMailItem>> buttonMailItemPages;
		private Dictionary<int, List<ButtonMailItem>> buttonMailItemInboxPages = new Dictionary<int, List<ButtonMailItem>>();
		private Dictionary<int, List<ButtonMailItem>> buttonMailItemOutboxPages = new Dictionary<int, List<ButtonMailItem>>();
		private Dictionary<int, List<ButtonMailItem>> buttonMailItemArchivePages = new Dictionary<int, List<ButtonMailItem>>();

		private List<Icon> iconSeparators = new List<Icon>();

		public MenuMailbox(Texture texture, Vector position, Icon iconFolderHeader, Label labelFolderHeader, Label labelFrom, Label labelSubject, Label labelPageNumber,
			Icon iconInboxFolder, Icon iconOutboxFolder, Icon iconArchiveFolder, Label labelInboxFolder, Label labelOutboxFolder, Label labelArchiveFolder,
			Button buttonInboxFolder, Button buttonOutboxFolder, Button buttonArchiveFolder, Button buttonArrowLeft, Button buttonArrowRight)
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

			this.labelInboxFolder = labelInboxFolder;
			this.labelOutboxFolder = labelOutboxFolder;
			this.labelArchiveFolder = labelArchiveFolder;

			this.iconInboxFolder = iconInboxFolder;
			this.iconOutboxFolder = iconOutboxFolder;
			this.iconArchiveFolder = iconArchiveFolder;

			iconSelectedFolderHeader = iconInboxFolder;
			labelSelectedFolderHeader = labelInboxFolder;

			this.buttonInboxFolder.Clicked += buttonInboxFolder_Clicked;
			this.buttonOutboxFolder.Clicked += buttonOutboxFolder_Clicked;
			this.buttonArchiveFolder.Clicked += buttonArchiveFolder_Clicked;

			buttonMailItemPages = buttonMailItemInboxPages;
		}

		private int currentDisplayedPage = 1;

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

			iconSelectedFolderHeader.Update(gameTime);
			labelSelectedFolderHeader.Update(gameTime);

			List<ButtonMailItem> buttonMailItemsOnCurrentPage = new List<ButtonMailItem>();
			bool success = buttonMailItemPages.TryGetValue(currentDisplayedPage, out buttonMailItemsOnCurrentPage);
			if (success)
				foreach (var buttonMenuItem in buttonMailItemsOnCurrentPage)
					buttonMenuItem.Update(gameTime);
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
			iconSelectedFolderHeader.Draw(gameTime, renderer);
			labelSelectedFolderHeader.Draw(gameTime, renderer);

			List<ButtonMailItem> buttonMailItemsOnCurrentPage = new List<ButtonMailItem>();
			bool success = buttonMailItemPages.TryGetValue(currentDisplayedPage, out buttonMailItemsOnCurrentPage);
			if (success)
				foreach (var buttonMailItem in buttonMailItemsOnCurrentPage)
					buttonMailItem.Draw(gameTime, renderer);

			foreach (var iconSeparator in iconSeparators)
				iconSeparator.Draw(gameTime, renderer);
		}

		public void AddButtonMailItemInbox(ButtonMailItem buttonMailItem, Icon iconSeparator)
		{
			AddButtonMailItem(buttonMailItem, buttonMailItemInboxPages, iconSeparator);
		}

		public void AddButtonMailItemOutbox(ButtonMailItem buttonMailItem, Icon iconSeparator)
		{
			AddButtonMailItem(buttonMailItem, buttonMailItemOutboxPages, iconSeparator);
		}

		public void AddButtonMailItemArchive(ButtonMailItem buttonMailItem, Icon iconSeparator)
		{
			AddButtonMailItem(buttonMailItem, buttonMailItemArchivePages, iconSeparator);
		}

		private void AddButtonMailItem(ButtonMailItem buttonMailItem, Dictionary<int, List<ButtonMailItem>> pages, Icon iconSeparator)
		{
			iconSeparators.Add(iconSeparator);

			// the last page number will indicate the key in which we check for the next items to add
			int lastPageNumber = pages.Keys.Count();
			int itemsOnLastPageCount = 0;
			List<ButtonMailItem> buttonMailItemsOnLastPage = new List<ButtonMailItem>();
			bool success = pages.TryGetValue(lastPageNumber, out buttonMailItemsOnLastPage);
			if (success)
			{
				// if there are pages in this page collection and the last page contains less than 4 entries, add the new entry to that page
				if (buttonMailItemsOnLastPage.Count < itemsPerPage)
					buttonMailItemsOnLastPage.Add(buttonMailItem);
				// if there are 4 items on the last page, create a new page and add the item
				else
				{
					buttonMailItemsOnLastPage = new List<ButtonMailItem>();
					buttonMailItemsOnLastPage.Add(buttonMailItem);
					pages.Add(lastPageNumber + 1, buttonMailItemsOnLastPage);
				}
			}
			else
			{
				// if there are no pages, create an entry and add the first page
				buttonMailItemsOnLastPage = new List<ButtonMailItem>();
				buttonMailItemsOnLastPage.Add(buttonMailItem);
				pages.Add(1, buttonMailItemsOnLastPage);
			}

			itemsOnLastPageCount = buttonMailItemsOnLastPage.Count;
			Vector buttonMailItemPosition = Vector.Zero;
			Vector iconSeparatorPosition = Vector.Zero;

			if (itemsOnLastPageCount == 1)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 65);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 70);
			}
			else if (itemsOnLastPageCount == 2)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 102);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 107);
			}
			else if (itemsOnLastPageCount == 3)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 139);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 144);
			}
			else if (itemsOnLastPageCount == 4)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 176);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 181);
			}
			else if (itemsOnLastPageCount == 5)
			{
				iconSeparatorPosition = new Vector(Position.X + 156, Position.Y + 213);
				buttonMailItemPosition = new Vector(Position.X + 158, Position.Y + 218);

				Vector iconSeparatorPositionFinal = new Vector(Position.X + 156, Position.Y + 250);
				var iconSeparatorClone = iconSeparator.Clone();
				iconSeparatorClone.Position = iconSeparatorPositionFinal;
				iconSeparators.Add(iconSeparatorClone);
			}
			else if (itemsOnLastPageCount >= itemsPerPage + 1)
				return;

			iconSeparator.Position = iconSeparatorPosition;
			buttonMailItem.Position = buttonMailItemPosition;
			buttonMailItem.Clicked += buttonMailItem_Clicked;
		}

		private void buttonMailItem_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void buttonArchiveFolder_Clicked(object sender, EventArgs e)
		{
			buttonMailItemPages = buttonMailItemArchivePages;
			iconSelectedFolderHeader = iconArchiveFolder;
			labelSelectedFolderHeader = labelArchiveFolder;
		}

		private void buttonOutboxFolder_Clicked(object sender, EventArgs e)
		{
			buttonMailItemPages = buttonMailItemOutboxPages;
			iconSelectedFolderHeader = iconOutboxFolder;
			labelSelectedFolderHeader = labelOutboxFolder;
		}

		private void buttonInboxFolder_Clicked(object sender, EventArgs e)
		{
			buttonMailItemPages = buttonMailItemInboxPages;
			iconSelectedFolderHeader = iconInboxFolder;
			labelSelectedFolderHeader = labelInboxFolder;
		}
	}
}
