using MyThirdSDL.Descriptors;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class MenuEquipment : Control
	{
		private const int itemsPerPage = 4;

		#region Header Controls

		private Icon iconMainMenu;
		private Icon iconInfoMenu;
		private Label labelMainMenu;
		private Label labelInfoMenu;

		#endregion

		#region Info Controls

		private Icon iconMoney;
		private Icon iconHealth;
		private Icon iconHygiene;
		private Icon iconSleepy;
		private Icon iconThirst;
		private Icon iconHunger;
		private Label labelMoney;
		private Label labelHealth;
		private Label labelHygiene;
		private Label labelSleep;
		private Label labelThirst;
		private Label labelHunger;

		#endregion

		#region Buttons

		private Button buttonArrowCircleLeft;
		private Button buttonArrowCircleRight;
		private Button buttonCloseWindow;
		private Button buttonConfirmWindow;
		private Dictionary<int, List<ButtonMenuItem>> buttonMenuItemPages = new Dictionary<int, List<ButtonMenuItem>>();

		#endregion
		
		private IPurchasable selectedPurchasableItem;

		#region Events

		public event EventHandler<EventArgs> ButtonCloseWindowClicked;
		public event EventHandler<ButtonConfirmWindowClickedEventArgs> ButtonConfirmWindowClicked;

		#endregion

		#region Constructor

		public MenuEquipment(Texture texture, Vector position, Icon iconMainMenu, Icon iconInfoMenu, Label labelMainMenu, Label labelInfoMenu,
			Icon iconMoney, Icon iconHealth, Icon iconHygiene, Icon iconSleepy, Icon iconThirst, Icon iconHunger, Label labelMoney,
			Label labelHealth, Label labelHygiene, Label labelSleepy, Label labelThirst, Label labelHunger,
			Button buttonArrowCircleLeft, Button buttonArrowCircleRight, Button buttonCloseWindow, Button buttonConfirmWindow)
			: base(texture, position)
		{
			this.iconMainMenu = iconMainMenu;
			this.iconInfoMenu = iconInfoMenu;
			this.labelMainMenu = labelMainMenu;
			this.labelInfoMenu = labelInfoMenu;
			this.iconMoney = iconMoney;
			this.iconHealth = iconHealth;
			this.iconHygiene = iconHygiene;
			this.iconSleepy = iconSleepy;
			this.iconThirst = iconThirst;
			this.iconHunger = iconHunger;
			this.labelMoney = labelMoney;
			this.labelHealth = labelHealth;
			this.labelHygiene = labelHygiene;
			this.labelSleep = labelSleepy;
			this.labelThirst = labelThirst;
			this.labelHunger = labelHunger;
			this.buttonArrowCircleLeft = buttonArrowCircleLeft;
			this.buttonArrowCircleRight = buttonArrowCircleRight;
			this.buttonCloseWindow = buttonCloseWindow;
			this.buttonConfirmWindow = buttonConfirmWindow;

			this.buttonArrowCircleLeft.Clicked += buttonArrowCircleLeft_Clicked;
			this.buttonArrowCircleRight.Clicked += buttonArrowCircleRight_Clicked;
			this.buttonCloseWindow.Clicked += buttonCloseWindow_Clicked;
			this.buttonConfirmWindow.Clicked += buttonConfirmWindow_Clicked;
		}

		#endregion

		#region Game Loop

		private int currentDisplayedPage = 1;

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			List<ButtonMenuItem> buttonMenuItemsOnCurrentPage = new List<ButtonMenuItem>();
			bool success = buttonMenuItemPages.TryGetValue(currentDisplayedPage, out buttonMenuItemsOnCurrentPage);
			if (success)
				foreach (var buttonMenuItem in buttonMenuItemsOnCurrentPage)
					buttonMenuItem.Update(gameTime);

			buttonArrowCircleLeft.Update(gameTime);
			buttonArrowCircleRight.Update(gameTime);
			buttonCloseWindow.Update(gameTime);
			buttonConfirmWindow.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			iconMainMenu.Draw(gameTime, renderer);
			iconInfoMenu.Draw(gameTime, renderer);
			labelMainMenu.Draw(gameTime, renderer);
			labelInfoMenu.Draw(gameTime, renderer);
			iconMoney.Draw(gameTime, renderer);
			iconHealth.Draw(gameTime, renderer);
			iconHygiene.Draw(gameTime, renderer);
			iconSleepy.Draw(gameTime, renderer);
			iconThirst.Draw(gameTime, renderer);
			iconHunger.Draw(gameTime, renderer);
			labelMoney.Draw(gameTime, renderer);
			labelHealth.Draw(gameTime, renderer);
			labelHygiene.Draw(gameTime, renderer);
			labelSleep.Draw(gameTime, renderer);
			labelThirst.Draw(gameTime, renderer);
			labelHunger.Draw(gameTime, renderer);
			buttonArrowCircleLeft.Draw(gameTime, renderer);
			buttonArrowCircleRight.Draw(gameTime, renderer);
			buttonCloseWindow.Draw(gameTime, renderer);
			buttonConfirmWindow.Draw(gameTime, renderer);

			List<ButtonMenuItem> buttonMenuItemsOnCurrentPage = new List<ButtonMenuItem>();
			bool success = buttonMenuItemPages.TryGetValue(currentDisplayedPage, out buttonMenuItemsOnCurrentPage);
			if (success)
				foreach (var buttonMenuItem in buttonMenuItemsOnCurrentPage)
					buttonMenuItem.Draw(gameTime, renderer);
		}

		#endregion

		#region Helper Methods

		public void AddButtonMenuItem(ButtonMenuItem buttonMenuItem)
		{
			// the last page number will indicate the key in which we check for the next items to add
			int lastPageNumber = buttonMenuItemPages.Keys.Count();
			int itemsOnLastPageCount = 0;
			List<ButtonMenuItem> buttonMenuItemsOnLastPage = new List<ButtonMenuItem>();
			bool success = buttonMenuItemPages.TryGetValue(lastPageNumber, out buttonMenuItemsOnLastPage);
			if (success)
			{
				// if there are pages in this page collection and the last page contains less than 4 entries, add the new entry to that page
				if (buttonMenuItemsOnLastPage.Count < itemsPerPage)
					buttonMenuItemsOnLastPage.Add(buttonMenuItem);
				// if there are 4 items on the last page, create a new page and add the item
				else
				{
					buttonMenuItemsOnLastPage = new List<ButtonMenuItem>();
					buttonMenuItemsOnLastPage.Add(buttonMenuItem);
					buttonMenuItemPages.Add(lastPageNumber + 1, buttonMenuItemsOnLastPage);
				}
			}
			else
			{
				// if there are no pages, create an entry and add the first page
				buttonMenuItemsOnLastPage = new List<ButtonMenuItem>();
				buttonMenuItemsOnLastPage.Add(buttonMenuItem);
				buttonMenuItemPages.Add(1, buttonMenuItemsOnLastPage);
			}

			itemsOnLastPageCount = buttonMenuItemsOnLastPage.Count;
			Vector buttonMenuItemPosition = Vector.Zero;

			if (itemsOnLastPageCount == 1)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 50);
			else if (itemsOnLastPageCount == 2)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 100);
			else if (itemsOnLastPageCount == 3)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 150);
			else if (itemsOnLastPageCount == 4)
				buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 200);
			else if (itemsOnLastPageCount >= itemsPerPage + 1)
				return;

			buttonMenuItem.Position = buttonMenuItemPosition;
			buttonMenuItem.Clicked += buttonMenuItem_Clicked;
		}

		private void buttonMenuItem_Clicked(object sender, PurchasableItemSelectedEventArgs e)
		{
			selectedPurchasableItem = e.PurchasableItem;
			ButtonMenuItem clickedButtonMenuItem = sender as ButtonMenuItem;
			if (clickedButtonMenuItem != null)
			{
				foreach (var buttonMenuItemPage in buttonMenuItemPages.Values)
					foreach (var buttonMenuItem in buttonMenuItemPage)
						buttonMenuItem.ToggleOff();
				clickedButtonMenuItem.ToggleOn();
			}

			labelMoney.Text = e.PurchasableItem.Price.ToString();
			labelHealth.Text = e.PurchasableItem.NecessityEffect.HealthEffectiveness.ToString();
			labelHunger.Text = e.PurchasableItem.NecessityEffect.HungerEffectiveness.ToString();
			labelHygiene.Text = e.PurchasableItem.NecessityEffect.HygieneEffectiveness.ToString();
			labelSleep.Text = e.PurchasableItem.NecessityEffect.SleepEffectiveness.ToString();
			labelThirst.Text = e.PurchasableItem.NecessityEffect.ThirstEffectiveness.ToString();
		}

		#endregion

		#region Event Subscriptions

		private void OnButtonConfirmWindowClicked(object sender, ButtonConfirmWindowClickedEventArgs e)
		{
			if (ButtonConfirmWindowClicked != null)
				ButtonConfirmWindowClicked(sender, e);
		}

		private void OnButtonCloseWindowClicked(object sender, EventArgs e)
		{
			if (ButtonCloseWindowClicked != null)
				ButtonCloseWindowClicked(sender, e);
		}

		private void buttonCloseWindow_Clicked(object sender, EventArgs e)
		{
			OnButtonCloseWindowClicked(sender, e);
		}

		private void buttonConfirmWindow_Clicked(object sender, EventArgs e)
		{
			ButtonConfirmWindowClickedEventArgs e2 = new ButtonConfirmWindowClickedEventArgs(selectedPurchasableItem);
			OnButtonConfirmWindowClicked(sender, e2);
		}

		private void buttonArrowCircleRight_Clicked(object sender, EventArgs e)
		{
			if (currentDisplayedPage < buttonMenuItemPages.Keys.Count)
				currentDisplayedPage++;
		}

		private void buttonArrowCircleLeft_Clicked(object sender, EventArgs e)
		{
			if (currentDisplayedPage > 1)
				currentDisplayedPage--;
		}

		#endregion
	}

	public class ButtonConfirmWindowClickedEventArgs : EventArgs
	{
		public IPurchasable PurchasableItem { get; private set; }

		public ButtonConfirmWindowClickedEventArgs(IPurchasable purchasableItem)
		{
			PurchasableItem = purchasableItem;
		}
	}
}
