using LimitedLiability.Content;
using LimitedLiability.Content.Data;
using LimitedLiability.Descriptors;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;

namespace LimitedLiability.UserInterface
{
	public class MenuPurchase : Menu
	{
		private const int itemsPerPage = 4;
		private int currentDisplayedPage = 1;

		private Icon iconFrame;

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
		private Label labelDescription;

		#endregion Info Controls

		#region Buttons

		private Button buttonCloseWindow;

		#endregion Buttons

		private TabContainer tabContainer;
		private ListBox<IPurchasable> listBoxEquipment;
		private ListBox<IPurchasable> listBoxRooms;

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
				iconMainMenuHeader.Position = base.Position + new Vector(10, 10);
				iconInfoMenuHeader.Position = base.Position + new Vector(250, 10);
				iconSkillsMenuHeader.Position = base.Position + new Vector(425, 10);
				labelMainMenuHeader.Position = iconMainMenuHeader.Position + new Vector(iconMainMenuHeader.Width + 10, 8);
				labelInfoMenuHeader.Position = iconInfoMenuHeader.Position + new Vector(iconInfoMenuHeader.Width + 10, 8);
				labelSkillsMenuHeader.Position = iconSkillsMenuHeader.Position + new Vector(iconSkillsMenuHeader.Width + 8, 8);
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
				labelLeadership.Position = new Vector(base.Position.X + 540, base.Position.Y + 90);
				labelCreativity.Position = new Vector(base.Position.X + 540, base.Position.Y + 120);
				labelIntelligence.Position = new Vector(base.Position.X + 540, base.Position.Y + 150);
				buttonCloseWindow.Position = base.Position + new Vector(Width - buttonCloseWindow.Width - 15, Height - buttonCloseWindow.Height - 14);
				labelDescription.Position = base.Position + new Vector(15, 353);

				tabContainer.Position = base.Position + new Vector(15, 51);
			}
		}

		#region Constructor

		public MenuPurchase(ContentManager contentManager, IEnumerable<IPurchasable> purchasableEquipment, IEnumerable<IPurchasable> purchasableRooms)
		{
			iconFrame = new Icon(contentManager.GetTexture("MenuPurchaseFrame"));
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath(Styles.Fonts.DroidSansBold);
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorYellow = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;
			int fontSizeTooltip = Styles.FontSizes.Tooltip;

			iconMainMenuHeader = ControlFactory.CreateIcon(contentManager, "IconShoppingCart");
			iconInfoMenuHeader = ControlFactory.CreateIcon(contentManager, "IconStatistics");
			iconSkillsMenuHeader = ControlFactory.CreateIcon(contentManager, "IconPenPaper");

			labelMainMenuHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorYellow, "Shopping");
			labelMainMenuHeader.EnableShadow(contentManager, 2, 2);
			labelInfoMenuHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorYellow, "Needs");
			labelInfoMenuHeader.EnableShadow(contentManager, 2, 2);
			labelSkillsMenuHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorYellow, "Skills");
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

			buttonCloseWindow = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover", "ButtonSquareSelected");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;
			buttonCloseWindow.Released += buttonCloseWindow_Clicked;

			labelDescription = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, contentManager.GetString(StringReferenceKeys.DEFAULT_TEXT), 570);

			Texture textureListBoxTargetFrame = contentManager.GetTexture("MenuPurchaseListBoxTarget");
			Texture textureListItemHighlight = contentManager.GetTexture("ListItemHighlight");
			Icon iconScrollbar = ControlFactory.CreateIcon(contentManager, "IconScrollbarMenuPurchase");
			Icon iconScroller = ControlFactory.CreateIcon(contentManager, "IconScroller");

			listBoxEquipment = CreateListBox<IPurchasable>(contentManager, textureListBoxTargetFrame, iconScrollbar, iconScroller);
			listBoxEquipment.ItemHovered += listBoxEquipment_ItemHovered;
			listBoxEquipment.ItemSelected += listBoxEquipment_ItemSelected;

			listBoxRooms = CreateListBox<IPurchasable>(contentManager, textureListBoxTargetFrame, iconScrollbar, iconScroller);
			listBoxRooms.ItemHovered += listBoxRooms_ItemHovered;
			listBoxRooms.ItemSelected += listBoxRooms_ItemSelected;

			CreateTabContainer(contentManager);

			PopulateListBox(contentManager, listBoxEquipment, purchasableEquipment, fontPath, textureListItemHighlight);
			PopulateListBox(contentManager, listBoxRooms, purchasableRooms, fontPath, textureListItemHighlight);

			Controls.Add(iconFrame);
			Controls.Add(iconMainMenuHeader);
			Controls.Add(iconInfoMenuHeader);
			Controls.Add(iconSkillsMenuHeader);
			Controls.Add(labelMainMenuHeader);
			Controls.Add(labelInfoMenuHeader);
			Controls.Add(labelSkillsMenuHeader);
			Controls.Add(buttonCloseWindow);
			Controls.Add(tabContainer);
			Controls.Add(labelDescription);

			Visible = false;
		}

		private void PopulateListBox<T>(ContentManager contentManager, ListBox<T> listBox, IEnumerable<T> purchasableItems, string fontPath, Texture textureListItemHighlight)
			where T : IPurchasable
		{
			foreach (var purchasableItem in purchasableItems)
			{
				ListItem<T> listItem = new ListItem<T>(purchasableItem, textureListItemHighlight);
				Icon iconItem = ControlFactory.CreateIcon(contentManager, purchasableItem.IconTextureKey);
				Label labelPurchasableItemName = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, purchasableItem.Name);
				Label labelPurchasableItemPrice = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, "$" + purchasableItem.Price);
				Label labelPurchasableItemHealth = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, purchasableItem.NecessityEffect.HealthEffectivenessToString());
				Label labelPurchasableItemHunger = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, purchasableItem.NecessityEffect.HungerEffectivenessToString());
				Label labelPurchasableItemThirst = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, purchasableItem.NecessityEffect.ThirstEffectivenessToString());
				Label labelPurchasableItemHygiene = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, purchasableItem.NecessityEffect.HygieneEffectivenessToString());
				Label labelPurchasableItemSleep = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, purchasableItem.NecessityEffect.SleepEffectivenessToString());
				
				listItem.AddColumn(iconItem);
				listItem.AddColumn(labelPurchasableItemName);
				listItem.AddColumn(labelPurchasableItemPrice);
				listItem.AddColumn(labelPurchasableItemHealth);
				listItem.AddColumn(labelPurchasableItemHunger);
				listItem.AddColumn(labelPurchasableItemThirst);
				listItem.AddColumn(labelPurchasableItemSleep);
				listBox.AddItem(listItem);
			}
		}

		private void CreateTabContainer(ContentManager contentManager)
		{
			Button buttonTab1 = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover", "ButtonSquareSelected");
			buttonTab1.Icon = ControlFactory.CreateIcon(contentManager, "IconHandTruck");
			buttonTab1.ButtonType = ButtonType.IconOnly;
			
			Button buttonTab2 = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover", "ButtonSquareSelected");
			buttonTab2.Icon = ControlFactory.CreateIcon(contentManager, "IconForklift");
			buttonTab2.ButtonType = ButtonType.IconOnly;
			
			tabContainer = new TabContainer();
			TabPanel tab1 = new TabPanel(buttonTab1);
			tab1.AddControl(listBoxEquipment);
			TabPanel tab2 = new TabPanel(buttonTab2);
			tab2.AddControl(listBoxRooms);
			tabContainer.AddTab(tab1);
			tabContainer.AddTab(tab2);
		}

		private ListBox<T> CreateListBox<T>(ContentManager contentManager, Texture textureListBoxTargetFrame, Icon iconScrollbar, Icon iconScroller)
		{
			ListBox<T> listBox = new ListBox<T>(contentManager, textureListBoxTargetFrame, iconScrollbar, iconScroller);
			return listBox;
		}

		private void listBoxEquipment_ItemSelected(object sender, ListItemSelectedEventArgs<IPurchasable> e)
		{
			if (ButtonConfirmWindowClicked != null)
				ButtonConfirmWindowClicked(sender, new ButtonConfirmWindowClickedEventArgs(e.Value));
		}

		private void listBoxEquipment_ItemHovered(object sender, ListItemHoveredEventArgs<IPurchasable> e)
		{
			labelDescription.Text = e.Value.Description;
		}

		private void listBoxRooms_ItemSelected(object sender, ListItemSelectedEventArgs<IPurchasable> e)
		{
			if (ButtonConfirmWindowClicked != null)
				ButtonConfirmWindowClicked(sender, new ButtonConfirmWindowClickedEventArgs(e.Value));
		}

		private void listBoxRooms_ItemHovered(object sender, ListItemHoveredEventArgs<IPurchasable> e)
		{
			labelDescription.Text = e.Value.Description;
		}

		#endregion Constructor

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			if (!Visible)
				return;
			base.Update(gameTime);

			if (!listBoxEquipment.IsAnyItemHovered)
				labelDescription.Visible = false;
			else
				labelDescription.Visible = true;
		}

		#endregion Game Loop

		#region Helper Methods

		#endregion Helper Methods

		#region Event Subscriptions

		private void OnButtonCloseWindowClicked(object sender, EventArgs e)
		{
			if (ButtonCloseWindowClicked != null)
				ButtonCloseWindowClicked(sender, e);
		}

		private void buttonCloseWindow_Clicked(object sender, EventArgs e)
		{
			OnButtonCloseWindowClicked(sender, e);
		}

		#endregion Event Subscriptions
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