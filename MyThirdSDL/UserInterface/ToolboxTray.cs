using MyThirdSDL.Content;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	public class ToolboxTray : Menu
	{
		private Icon iconFrame;

		//private Button buttonSelectGeneral;
		private Button buttonSelectEquipment;
		private Button buttonSelectRoom;
		private Button buttonFinances;
		private Button buttonCompany;
		private Button buttonEmployees;
		private Button buttonProducts;
		private Button buttonMainMenu;
		//private Button buttonMailMenu;

		//private Icon iconMoney;
		//private Icon iconTime;

		//private Label labelMoney;
		//private Label labelDate;
		//private Label labelTime;

		public event EventHandler ButtonSelectGeneralClicked;

		public event EventHandler ButtonSelectEquipmentClicked;

		public event EventHandler ButtonSelectRoomClicked;

		public event EventHandler ButtonFinancesClicked;

		public event EventHandler ButtonCompanyClicked;

		public event EventHandler ButtonEmployeesClicked;

		public event EventHandler ButtonProductsClicked;

		public event EventHandler ButtonMainMenuClicked;

		public event EventHandler ButtonMailMenuClicked;

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				int spacerWidth = 14;
				int spacerHeight = 15;

				iconFrame.Position = base.Position;
				buttonSelectEquipment.Position = new Vector(iconFrame.Position.X + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonSelectRoom.Position = new Vector(buttonSelectEquipment.Position.X + buttonSelectEquipment.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonFinances.Position = new Vector(buttonSelectRoom.Position.X + buttonSelectRoom.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonCompany.Position = new Vector(buttonFinances.Position.X + buttonFinances.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonEmployees.Position = new Vector(buttonCompany.Position.X + buttonCompany.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonProducts.Position = new Vector(buttonEmployees.Position.X + buttonEmployees.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonMainMenu.Position = new Vector(buttonProducts.Position.X + buttonProducts.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				//buttonMailMenu.Position = buttonNinePosition;
				//iconMoney.Position = iconOnePosition;
				//iconTime.Position = iconTwoPosition;
				//labelMoney.Position = iconOnePosition + new Vector(35, 10);
				//labelDate.Position = iconTwoPosition + new Vector(32, 3);
				//labelTime.Position = iconTwoPosition + new Vector(55, 18);

				//buttonSelectEquipment.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				//buttonSelectRoom.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				//buttonFinances.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				//buttonCompany.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				//buttonEmployees.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				//buttonProducts.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				//buttonMainMenu.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				//buttonMailMenu.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
			}
		}

		public ToolboxTray(ContentManager contentManager, int unreadMailCount, int money)
		{
			Texture textureFrame = contentManager.GetTexture("BottomBar");
			iconFrame = new Icon(textureFrame);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = Styles.Colors.PaleYellow;
			int fontSizeContent = 12;
			int fontSizeTooltipText = 8;

			//buttonSelectGeneral = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			//buttonSelectGeneral.Icon = ControlFactory.CreateIcon(contentManager, "IconArrowsInward");
			//buttonSelectGeneral.IconHovered = ControlFactory.CreateIcon(contentManager, "IconArrowsInward");
			//buttonSelectGeneral.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "Select objects to inspect them");
			//buttonSelectGeneral.ButtonType = ButtonType.IconOnly;

			buttonSelectEquipment = ControlFactory.CreateButton(contentManager, "ButtonItems", "ButtonItemsHover");
			buttonSelectEquipment.ButtonType = ButtonType.FrameOnly;

			buttonSelectRoom = ControlFactory.CreateButton(contentManager, "ButtonRooms", "ButtonRoomsHover");
			buttonSelectRoom.ButtonType = ButtonType.FrameOnly;

			buttonFinances = ControlFactory.CreateButton(contentManager, "ButtonFinances", "ButtonFinancesHover");
			buttonFinances.ButtonType = ButtonType.FrameOnly;

			buttonCompany = ControlFactory.CreateButton(contentManager, "ButtonStatistics", "ButtonStatisticsHover");
			buttonCompany.ButtonType = ButtonType.FrameOnly;

			buttonEmployees = ControlFactory.CreateButton(contentManager, "ButtonEmployees", "ButtonEmployeesHover");
			buttonEmployees.ButtonType = ButtonType.FrameOnly;

			buttonProducts = ControlFactory.CreateButton(contentManager, "ButtonProducts", "ButtonProductsHover");
			buttonProducts.ButtonType = ButtonType.FrameOnly;

			buttonMainMenu = ControlFactory.CreateButton(contentManager, "ButtonGameMenu", "ButtonGameMenuHover");
			buttonMainMenu.ButtonType = ButtonType.FrameOnly;

			//buttonMailMenu = ControlFactory.CreateButton(contentManager, "ToolboxTrayButtonMail", "ToolboxTrayButtonMailHover");
			//buttonMailMenu.Icon = ControlFactory.CreateIcon(contentManager, "IconMailUnread");
			//buttonMailMenu.IconHovered = ControlFactory.CreateIcon(contentManager, "IconMailUnread");
			//buttonMailMenu.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "View your e-mail messages");
			//buttonMailMenu.Label = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, unreadMailCount.ToString());
			//buttonMailMenu.ButtonType = ButtonType.IconAndText;

			//iconMoney = ControlFactory.CreateIcon(contentManager, "IconMoney");

			//iconTime = ControlFactory.CreateIcon(contentManager, "IconTime");

			//labelMoney = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, money.ToString());

			//labelDate = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, DateTime.Now.ToShortDateString());

			//labelTime = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, DateTime.Now.ToShortTimeString());

			Controls.Add(iconFrame);
			Controls.Add(buttonSelectEquipment);
			Controls.Add(buttonSelectRoom);
			Controls.Add(buttonFinances);
			Controls.Add(buttonCompany);
			Controls.Add(buttonEmployees);
			Controls.Add(buttonProducts);
			Controls.Add(buttonMainMenu);
			//Controls.Add(buttonMailMenu);
			//Controls.Add(iconMoney);
			//Controls.Add(iconTime);
			//Controls.Add(labelMoney);
			//Controls.Add(labelDate);
			//Controls.Add(labelTime);

			buttonSelectEquipment.Clicked += ButtonSelectEquipment_Clicked;
			buttonSelectRoom.Clicked += ButtonSelectRoom_Clicked;
			buttonFinances.Clicked += ButtonFinances_Clicked;
			buttonCompany.Clicked += ButtonCompany_Clicked;
			buttonEmployees.Clicked += ButtonEmployees_Clicked;
			buttonProducts.Clicked += ButtonProducts_Clicked;
			buttonMainMenu.Clicked += ButtonMainMenu_Clicked;
			//buttonMailMenu.Clicked += ButtonMailMenu_Clicked;
		}

		public void UpdateDisplayedDateAndTime(DateTime dateTime)
		{
			//labelTime.Text = dateTime.ToShortTimeString();
			//labelDate.Text = dateTime.ToShortDateString();
		}

		public void UpdateDisplayedUnreadMailCount(int unreadMailCount)
		{
			//buttonMailMenu.Text = unreadMailCount.ToString();
		}

		public void UpdateDisplayedBankAccountBalance(int money)
		{
			//labelMoney.Text = money.ToString();
		}

		private void ButtonMailMenu_Clicked(object sender, EventArgs e)
		{
			OnButtonMailMenuClicked(sender, e);
		}

		private void ButtonMainMenu_Clicked(object sender, EventArgs e)
		{
			OnButtonMainMenuClicked(sender, e);
		}

		private void ButtonProducts_Clicked(object sender, EventArgs e)
		{
			OnButtonProductsClicked(sender, e);
		}

		private void ButtonEmployees_Clicked(object sender, EventArgs e)
		{
			OnButtonEmployeesClicked(sender, e);
		}

		private void ButtonCompany_Clicked(object sender, EventArgs e)
		{
			OnButtonCompanyClicked(sender, e);
		}

		private void ButtonFinances_Clicked(object sender, EventArgs e)
		{
			OnButtonFinancesClicked(sender, e);
		}

		private void ButtonSelectRoom_Clicked(object sender, EventArgs e)
		{
			OnButtonSelectRoomClicked(sender, e);
		}

		private void ButtonSelectEquipment_Clicked(object sender, EventArgs e)
		{
			OnButtonSelectEquipmentClicked(sender, e);
		}

		private void ButtonSelectGeneral_Clicked(object sender, EventArgs e)
		{
			OnButtonSelectGeneralClicked(sender, e);
		}

		private void OnButtonSelectGeneralClicked(object sender, EventArgs e)
		{
			if (ButtonSelectGeneralClicked != null)
				ButtonSelectGeneralClicked(sender, e);
		}

		private void OnButtonSelectEquipmentClicked(object sender, EventArgs e)
		{
			if (ButtonSelectEquipmentClicked != null)
				ButtonSelectEquipmentClicked(sender, e);
		}

		private void OnButtonSelectRoomClicked(object sender, EventArgs e)
		{
			if (ButtonSelectRoomClicked != null)
				ButtonSelectRoomClicked(sender, e);
		}

		private void OnButtonFinancesClicked(object sender, EventArgs e)
		{
			if (ButtonFinancesClicked != null)
				ButtonFinancesClicked(sender, e);
		}

		private void OnButtonCompanyClicked(object sender, EventArgs e)
		{
			if (ButtonCompanyClicked != null)
				ButtonCompanyClicked(sender, e);
		}

		private void OnButtonEmployeesClicked(object sender, EventArgs e)
		{
			if (ButtonEmployeesClicked != null)
				ButtonEmployeesClicked(sender, e);
		}

		private void OnButtonProductsClicked(object sender, EventArgs e)
		{
			if (ButtonProductsClicked != null)
				ButtonProductsClicked(sender, e);
		}

		private void OnButtonMainMenuClicked(object sender, EventArgs e)
		{
			if (ButtonMainMenuClicked != null)
				ButtonMainMenuClicked(sender, e);
		}

		private void OnButtonMailMenuClicked(object sender, EventArgs e)
		{
			if (ButtonMailMenuClicked != null)
				ButtonMailMenuClicked(sender, e);
		}
	}
}