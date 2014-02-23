using MyThirdSDL.Content;
using MyThirdSDL.Content.Data;
using MyThirdSDL.Descriptors;
using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyThirdSDL.UserInterface
{
	public class MenuPurchase : Menu
	{
		private const int itemsPerPage = 4;
		private int currentDisplayedPage = 1;

		private Icon iconFrame;
		private Tooltip tooltipHoveredItem;

		private string defaultText;

		#region Header Controls

		private Icon iconMainMenuHeader;
		private Icon iconInfoMenuHeader;
		private Icon iconSkillsMenuHeader;
		private Label labelMainMenuHeader;
		private Label labelInfoMenuHeader;
		private Label labelSkillsMenuHeader;

		#endregion Header Controls

		#region Info Controls

		private Icon iconMoney;
		private Icon iconHealth;
		private Icon iconHygiene;
		private Icon iconSleep;
		private Icon iconThirst;
		private Icon iconHunger;
		private Icon iconCommunication;
		private Icon iconCreativity;
		private Icon iconIntelligence;
		private Icon iconLeadership;
		private Label labelMoney;
		private Label labelHealth;
		private Label labelHygiene;
		private Label labelSleep;
		private Label labelThirst;
		private Label labelHunger;
		private Label labelCommunication;
		private Label labelCreativity;
		private Label labelIntelligence;
		private Label labelLeadership;

		#endregion Info Controls

		#region Buttons

		private Button buttonArrowCircleLeft;
		private Button buttonArrowCircleRight;
		private Button buttonCloseWindow;
		private Dictionary<int, List<ButtonMenuItem>> buttonMenuItemPages = new Dictionary<int, List<ButtonMenuItem>>();

		#endregion Buttons

		private IPurchasable selectedPurchasableItem;

		#region Events

		public event EventHandler<EventArgs> ButtonCloseWindowClicked;

		public event EventHandler<ButtonConfirmWindowClickedEventArgs> ButtonConfirmWindowClicked;

		#endregion Events

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
				iconMainMenuHeader.Position = new Vector(base.Position.X + 3, base.Position.Y + 5);
				iconInfoMenuHeader.Position = new Vector(base.Position.X + 365, base.Position.Y + 5);
				iconSkillsMenuHeader.Position = new Vector(base.Position.X + 505, base.Position.Y + 5);
				labelMainMenuHeader.Position = new Vector(base.Position.X + 38, base.Position.Y + 15);
				labelInfoMenuHeader.Position = new Vector(base.Position.X + 400, base.Position.Y + 15);
				labelSkillsMenuHeader.Position = new Vector(base.Position.X + 535, base.Position.Y + 15);
				iconMoney.Position = new Vector(base.Position.X + 365, base.Position.Y + 50);
				iconHealth.Position = new Vector(base.Position.X + 365, base.Position.Y + 80);
				iconHygiene.Position = new Vector(base.Position.X + 365, base.Position.Y + 110);
				iconSleep.Position = new Vector(base.Position.X + 365, base.Position.Y + 140);
				iconThirst.Position = new Vector(base.Position.X + 365, base.Position.Y + 170);
				iconHunger.Position = new Vector(base.Position.X + 365, base.Position.Y + 200);
				labelMoney.Position = new Vector(base.Position.X + 400, base.Position.Y + 60);
				labelHealth.Position = new Vector(base.Position.X + 400, base.Position.Y + 90);
				labelHygiene.Position = new Vector(base.Position.X + 400, base.Position.Y + 120);
				labelSleep.Position = new Vector(base.Position.X + 400, base.Position.Y + 150);
				labelThirst.Position = new Vector(base.Position.X + 400, base.Position.Y + 180);
				labelHunger.Position = new Vector(base.Position.X + 400, base.Position.Y + 210);
				iconCommunication.Position = new Vector(base.Position.X + 508, base.Position.Y + 50);
				iconLeadership.Position = new Vector(base.Position.X + 508, base.Position.Y + 80);
				iconCreativity.Position = new Vector(base.Position.X + 508, base.Position.Y + 110);
				iconIntelligence.Position = new Vector(base.Position.X + 508, base.Position.Y + 140);
				labelCommunication.Position = new Vector(base.Position.X + 540, base.Position.Y + 60);
				labelLeadership.Position = new Vector(base.Position.X + 540, base.Position.Y + 90);
				labelCreativity.Position = new Vector(base.Position.X + 540, base.Position.Y + 120);
				labelIntelligence.Position = new Vector(base.Position.X + 540, base.Position.Y + 150);
				buttonCloseWindow.Position = new Vector(base.Position.X + Width - buttonCloseWindow.Width, base.Position.Y + Height + 5);
				buttonArrowCircleLeft.Position = new Vector(base.Position.X + 9, base.Position.Y + 248);
				buttonArrowCircleRight.Position = new Vector(base.Position.X + 296, base.Position.Y + 248);
				tooltipHoveredItem.Position = new Vector(base.Position.X, base.Position.Y + Height + 40);
				buttonCloseWindow.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);

				SetMenuItemButtonPositions();
			}
		}

		#region Constructor

		public MenuPurchase(ContentManager contentManager, string iconMainMenuContentPathKey, string menuTitle, IEnumerable<IPurchasable> purchasableItems)
		{
			iconFrame = new Icon(contentManager.GetTexture("MenuPurchaseFrame"));
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			defaultText = contentManager.GetString(StringReferenceKeys.DEFAULT_TEXT);

			string fontPath = contentManager.GetContentPath(Styles.Fonts.Arcade);
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorYellow = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;
			int fontSizeTooltip = Styles.FontSizes.Tooltip;

			iconMainMenuHeader = ControlFactory.CreateIcon(contentManager, iconMainMenuContentPathKey);
			iconInfoMenuHeader = ControlFactory.CreateIcon(contentManager, "IconStatistics");
			iconSkillsMenuHeader = ControlFactory.CreateIcon(contentManager, "IconPenPaper");

			labelMainMenuHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, menuTitle);
			labelMainMenuHeader.EnableShadow(contentManager, 2, 2);
			labelInfoMenuHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, "Needs");
			labelInfoMenuHeader.EnableShadow(contentManager, 2, 2);
			labelSkillsMenuHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorWhite, "Skills");
			labelSkillsMenuHeader.EnableShadow(contentManager, 2, 2);

			iconMoney = ControlFactory.CreateIcon(contentManager, "IconMoney");

			iconHealth = ControlFactory.CreateIcon(contentManager, "IconMedkit");
			iconHygiene = ControlFactory.CreateIcon(contentManager, "IconToothbrush");
			iconSleep = ControlFactory.CreateIcon(contentManager, "IconPersonTired");
			iconThirst = ControlFactory.CreateIcon(contentManager, "IconSoda");
			iconHunger = ControlFactory.CreateIcon(contentManager, "IconChicken");

			labelMoney = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);
			labelHealth = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);
			labelHygiene = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);
			labelSleep = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);
			labelThirst = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);
			labelHunger = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);

			iconCommunication = ControlFactory.CreateIcon(contentManager, "IconCommunication");
			iconLeadership = ControlFactory.CreateIcon(contentManager, "IconLeadership");
			iconCreativity = ControlFactory.CreateIcon(contentManager, "IconCreativity");
			iconIntelligence = ControlFactory.CreateIcon(contentManager, "IconIntelligence");

			labelCommunication = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);
			labelLeadership = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);
			labelCreativity = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);
			labelIntelligence = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, defaultText);

			buttonCloseWindow = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;
			buttonCloseWindow.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
				fontColorWhite, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_CLOSE_WINDOW));

			buttonArrowCircleLeft = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonArrowCircleLeft.Icon = ControlFactory.CreateIcon(contentManager, "IconArrowCircleLeft");
			buttonArrowCircleLeft.IconHovered = ControlFactory.CreateIcon(contentManager, "IconArrowCircleLeft");
			buttonArrowCircleLeft.ButtonType = ButtonType.IconOnly;

			buttonArrowCircleRight = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonArrowCircleRight.Icon = ControlFactory.CreateIcon(contentManager, "IconArrowCircleRight");
			buttonArrowCircleRight.IconHovered = ControlFactory.CreateIcon(contentManager, "IconArrowCircleRight");
			buttonArrowCircleRight.ButtonType = ButtonType.IconOnly;

			tooltipHoveredItem = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath,
				Styles.FontSizes.Tooltip, Styles.Colors.White, defaultText);

			foreach (var purchasableItem in purchasableItems)
			{
				ButtonMenuItem buttonMenuItem = new ButtonMenuItem(purchasableItem);
				buttonMenuItem.TextureFrame = contentManager.GetTexture("ButtonMenuItem");
				buttonMenuItem.TextureFrameHovered = contentManager.GetTexture("ButtonMenuItemHover");
				buttonMenuItem.IconMain = ControlFactory.CreateIcon(contentManager, purchasableItem.IconTextureKey);
				buttonMenuItem.IconMoney = ControlFactory.CreateIcon(contentManager, "IconMoney");
				buttonMenuItem.LabelMain = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, purchasableItem.Name);
				buttonMenuItem.LabelMoney = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorYellow, purchasableItem.Price.ToString());
				AddButtonMenuItem(buttonMenuItem);
			}

			Controls.Add(iconFrame);
			Controls.Add(iconMainMenuHeader);
			Controls.Add(iconInfoMenuHeader);
			Controls.Add(iconSkillsMenuHeader);
			Controls.Add(labelMainMenuHeader);
			Controls.Add(labelInfoMenuHeader);
			Controls.Add(labelSkillsMenuHeader);
			Controls.Add(iconMoney);
			Controls.Add(iconHealth);
			Controls.Add(iconHygiene);
			Controls.Add(iconSleep);
			Controls.Add(iconThirst);
			Controls.Add(iconHunger);
			Controls.Add(labelMoney);
			Controls.Add(labelHealth);
			Controls.Add(labelSleep);
			Controls.Add(labelThirst);
			Controls.Add(labelHunger);
			Controls.Add(labelHygiene);
			Controls.Add(iconCommunication);
			Controls.Add(iconCreativity);
			Controls.Add(iconIntelligence);
			Controls.Add(iconLeadership);
			Controls.Add(labelCommunication);
			Controls.Add(labelCreativity);
			Controls.Add(labelIntelligence);
			Controls.Add(labelLeadership);
			Controls.Add(buttonArrowCircleLeft);
			Controls.Add(buttonArrowCircleRight);
			Controls.Add(buttonCloseWindow);
			Controls.Add(tooltipHoveredItem);

			buttonArrowCircleLeft.Clicked += buttonArrowCircleLeft_Clicked;
			buttonArrowCircleRight.Clicked += buttonArrowCircleRight_Clicked;
			buttonCloseWindow.Clicked += buttonCloseWindow_Clicked;

			Visible = false;
		}

		#endregion Constructor

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			if (!Visible) return;

			bool isAnyButtonMenuItemHovered = false;
			List<ButtonMenuItem> buttonMenuItemsOnCurrentPage;
			bool success = buttonMenuItemPages.TryGetValue(currentDisplayedPage, out buttonMenuItemsOnCurrentPage);
			if (success)
			{
				foreach (var buttonMenuItem in buttonMenuItemsOnCurrentPage)
				{
					buttonMenuItem.Update(gameTime);
					if (buttonMenuItem.IsHovered)
						isAnyButtonMenuItemHovered = true;
				}
			}

			if (!isAnyButtonMenuItemHovered)
			{
				ClearInfoAndSkillsText();
				tooltipHoveredItem.Visible = false;
			}
			else
				tooltipHoveredItem.Visible = true;

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible) return;

			base.Draw(gameTime, renderer);

			List<ButtonMenuItem> buttonMenuItemsOnCurrentPage;
			bool success = buttonMenuItemPages.TryGetValue(currentDisplayedPage, out buttonMenuItemsOnCurrentPage);
			if (success)
				foreach (var buttonMenuItem in buttonMenuItemsOnCurrentPage)
					buttonMenuItem.Draw(gameTime, renderer);
		}

		public override void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			if (!Visible) return;

			base.HandleMouseButtonPressedEvent(sender, e);

			List<ButtonMenuItem> buttonMenuItemsOnCurrentPage;
			bool success = buttonMenuItemPages.TryGetValue(currentDisplayedPage, out buttonMenuItemsOnCurrentPage);
			if (success)
				foreach (var buttonMenuItem in buttonMenuItemsOnCurrentPage)
					buttonMenuItem.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			if (!Visible) return;

			base.HandleMouseMovingEvent(sender, e);

			List<ButtonMenuItem> buttonMenuItemsOnCurrentPage;
			bool success = buttonMenuItemPages.TryGetValue(currentDisplayedPage, out buttonMenuItemsOnCurrentPage);
			if (success)
				foreach (var buttonMenuItem in buttonMenuItemsOnCurrentPage)
					buttonMenuItem.HandleMouseMovingEvent(sender, e);
		}

		#endregion Game Loop

		#region Helper Methods

		public void AddButtonMenuItem(ButtonMenuItem buttonMenuItem)
		{
			if (buttonMenuItem == null) throw new ArgumentNullException("buttonMenuItem");

			// the last page number will indicate the key in which we check for the next items to add
			int lastPageNumber = buttonMenuItemPages.Keys.Count();
			List<ButtonMenuItem> buttonMenuItemsOnLastPage;
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
				buttonMenuItemsOnLastPage = new List<ButtonMenuItem> { buttonMenuItem };
				buttonMenuItemPages.Add(1, buttonMenuItemsOnLastPage);
			}

			int itemsOnLastPageCount = buttonMenuItemsOnLastPage.Count;
			Vector buttonMenuItemPosition = Vector.Zero;

			switch (itemsOnLastPageCount)
			{
				case 1:
					buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 50);
					break;

				case 2:
					buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 100);
					break;

				case 3:
					buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 150);
					break;

				case 4:
					buttonMenuItemPosition = new Vector(Position.X + 10, Position.Y + 200);
					break;

				default:
					if (itemsOnLastPageCount >= itemsPerPage + 1)
						return;
					break;
			}

			buttonMenuItem.Position = buttonMenuItemPosition;
			buttonMenuItem.Clicked += buttonMenuItem_Clicked;
			buttonMenuItem.Hovered += buttonMenuItem_Hovered;
		}

		private void SetMenuItemButtonPositions()
		{
			foreach (int key in buttonMenuItemPages.Keys)
			{
				List<ButtonMenuItem> currentPage = null;
				bool success = buttonMenuItemPages.TryGetValue(key, out currentPage);

				if (!success) continue;

				foreach (var buttonMenuItem in currentPage)
				{
					if (currentPage.IndexOf(buttonMenuItem) == 0)
						buttonMenuItem.Position = new Vector(Position.X + 10, Position.Y + 50);
					if (currentPage.IndexOf(buttonMenuItem) == 1)
						buttonMenuItem.Position = new Vector(Position.X + 10, Position.Y + 100);
					if (currentPage.IndexOf(buttonMenuItem) == 2)
						buttonMenuItem.Position = new Vector(Position.X + 10, Position.Y + 150);
					if (currentPage.IndexOf(buttonMenuItem) == 3)
						buttonMenuItem.Position = new Vector(Position.X + 10, Position.Y + 200);
				}
			}
		}

		private void buttonMenuItem_Clicked(object sender, PurchasableItemSelectedEventArgs e)
		{
			OnButtonConfirmWindowClicked(sender, new ButtonConfirmWindowClickedEventArgs(e.PurchasableItem));
		}

		private void buttonMenuItem_Hovered(object sender, PurchasableItemSelectedEventArgs e)
		{
			labelMoney.Text = e.PurchasableItem.Price.ToString();
			labelHealth.Text = e.PurchasableItem.NecessityEffect.HealthEffectiveness.ToString();
			labelHunger.Text = e.PurchasableItem.NecessityEffect.HungerEffectiveness.ToString();
			labelHygiene.Text = e.PurchasableItem.NecessityEffect.HygieneEffectiveness.ToString();
			labelSleep.Text = e.PurchasableItem.NecessityEffect.SleepEffectiveness.ToString();
			labelThirst.Text = e.PurchasableItem.NecessityEffect.ThirstEffectiveness.ToString();
			labelCommunication.Text = e.PurchasableItem.SkillEffect.CommunicationEffectiveness.ToString();
			labelCreativity.Text = e.PurchasableItem.SkillEffect.CreativityEffectiveness.ToString();
			labelIntelligence.Text = e.PurchasableItem.SkillEffect.IntelligenceEffectiveness.ToString();
			labelLeadership.Text = e.PurchasableItem.SkillEffect.LeadershipEffectiveness.ToString();
			tooltipHoveredItem.Label.Text = e.PurchasableItem.Description;
		}

		/// <summary>
		/// When the user is not hovering any button item, we should display default text for the info/skills labels.
		/// </summary>
		private void ClearInfoAndSkillsText()
		{
			if (labelMoney.Text != defaultText)
				labelMoney.Text = defaultText;
			if (labelHealth.Text != defaultText)
				labelHealth.Text = defaultText;
			if (labelHunger.Text != defaultText)
				labelHunger.Text = defaultText;
			if (labelHygiene.Text != defaultText)
				labelHygiene.Text = defaultText;
			if (labelSleep.Text != defaultText)
				labelSleep.Text = defaultText;
			if (labelThirst.Text != defaultText)
				labelThirst.Text = defaultText;
			if (labelCommunication.Text != defaultText)
				labelCommunication.Text = defaultText;
			if (labelCreativity.Text != defaultText)
				labelCreativity.Text = defaultText;
			if (labelIntelligence.Text != defaultText)
				labelIntelligence.Text = defaultText;
			if (labelLeadership.Text != defaultText)
				labelLeadership.Text = defaultText;
		}

		#endregion Helper Methods

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

		#endregion Event Subscriptions

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			base.Dispose();

			foreach (var key in buttonMenuItemPages.Keys)
				foreach (var buttonMenuItem in buttonMenuItemPages[key])
					buttonMenuItem.Dispose();

			buttonMenuItemPages.Clear();
		}
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