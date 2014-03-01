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
		//private Tooltip tooltipHoveredItem;

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

		private ListBox<IPurchasable> listBox;

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
				labelMainMenuHeader.Position = iconMainMenuHeader.Position + new Vector(iconMainMenuHeader.Width + 10, 5);
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
				//labelCommunication.Position = new Vector(base.Position.X + 540, base.Position.Y + 60);
				labelLeadership.Position = new Vector(base.Position.X + 540, base.Position.Y + 90);
				labelCreativity.Position = new Vector(base.Position.X + 540, base.Position.Y + 120);
				labelIntelligence.Position = new Vector(base.Position.X + 540, base.Position.Y + 150);
				buttonCloseWindow.Position = base.Position + new Vector(Width - buttonCloseWindow.Width - 15, Height - buttonCloseWindow.Height - 15);
				//tooltipHoveredItem.Position = new Vector(base.Position.X, base.Position.Y + Height + 40);
				//buttonCloseWindow.Tooltip.Position = new Vector(Position.X, buttonCloseWindow.Position.Y + buttonCloseWindow.Height + 5);
				labelDescription.Position = base.Position + new Vector(15, 295);

				listBox.Position = base.Position + new Vector(12, 47);
			}
		}

		#region Constructor

		public MenuPurchase(ContentManager contentManager, string iconMainMenuContentPathKey, string menuTitle, IEnumerable<IPurchasable> purchasableItems)
		{
			iconFrame = new Icon(contentManager.GetTexture("MenuPurchaseFrame"));
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			//defaultText = contentManager.GetString(StringReferenceKeys.DEFAULT_TEXT);

			string fontPath = contentManager.GetContentPath(Styles.Fonts.DroidSansBold);
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorYellow = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;
			int fontSizeTooltip = Styles.FontSizes.Tooltip;

			iconMainMenuHeader = ControlFactory.CreateIcon(contentManager, iconMainMenuContentPathKey);
			iconInfoMenuHeader = ControlFactory.CreateIcon(contentManager, "IconStatistics");
			iconSkillsMenuHeader = ControlFactory.CreateIcon(contentManager, "IconPenPaper");

			labelMainMenuHeader = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorYellow, menuTitle);
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

			buttonCloseWindow = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonCloseWindow.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowClose");
			buttonCloseWindow.ButtonType = ButtonType.IconOnly;
			//buttonCloseWindow.Tooltip = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath, fontSizeTooltip,
			//	fontColorWhite, contentManager.GetString(StringReferenceKeys.TOOLTIP_BUTTON_CLOSE_WINDOW));

			labelDescription = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorWhite, contentManager.GetString(StringReferenceKeys.DEFAULT_TEXT), 570);

			//tooltipHoveredItem = ControlFactory.CreateTooltip(contentManager, "TooltipFrame", fontPath,
			//	Styles.FontSizes.Tooltip, Styles.Colors.White, defaultText);

			Texture textureListBoxTargetFrame = contentManager.GetTexture("MenuPurchaseListBoxTarget");
			Texture textureListItemHighlight = contentManager.GetTexture("ListItemHighlight");
			Icon iconScrollbar = ControlFactory.CreateIcon(contentManager, "IconScrollbarMenuPurchase");
			Icon iconScroller = ControlFactory.CreateIcon(contentManager, "IconScroller");

			listBox = new ListBox<IPurchasable>(contentManager, textureListBoxTargetFrame, iconScrollbar, iconScroller);
			listBox.ItemHovered += listBox_ItemHovered;
			listBox.ItemSelected += listBox_ItemSelected;

			foreach (var purchasableItem in purchasableItems)
			{
				ListItem<IPurchasable> listItem = new ListItem<IPurchasable>(purchasableItem, textureListItemHighlight);
				Icon iconItem = ControlFactory.CreateIcon(contentManager, purchasableItem.IconTextureKey);
				Label labelPurchasableItemName = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, purchasableItem.Name);
				Label labelPurchasableItemPrice = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, "$" + purchasableItem.Price);
				Label labelPurchasableItemHealth = ControlFactory.CreateLabel(contentManager, fontPath, 13, Styles.Colors.White, purchasableItem.NecessityEffect.HealthEffectiveness.ToString());
				listItem.AddColumn(iconItem);
				listItem.AddColumn(labelPurchasableItemName);
				listItem.AddColumn(labelPurchasableItemPrice);
				listItem.AddColumn(labelPurchasableItemHealth);
				listBox.AddItem(listItem);
			}

			Controls.Add(iconFrame);
			Controls.Add(iconMainMenuHeader);
			Controls.Add(iconInfoMenuHeader);
			Controls.Add(iconSkillsMenuHeader);
			Controls.Add(labelMainMenuHeader);
			Controls.Add(labelInfoMenuHeader);
			Controls.Add(labelSkillsMenuHeader);
			Controls.Add(buttonCloseWindow);
			Controls.Add(labelDescription);
			//Controls.Add(tooltipHoveredItem);
			Controls.Add(listBox);

			buttonCloseWindow.Clicked += buttonCloseWindow_Clicked;

			Visible = false;
		}

		private void listBox_ItemSelected(object sender, ListItemSelectedEventArgs<IPurchasable> e)
		{
			if (ButtonConfirmWindowClicked != null)
				ButtonConfirmWindowClicked(sender, new ButtonConfirmWindowClickedEventArgs(e.Value));
		}

		private void listBox_ItemHovered(object sender, ListItemHoveredEventArgs<IPurchasable> e)
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

			if (!listBox.IsAnyItemHovered)
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