using LimitedLiability.Content;
using SharpDL.Graphics;
using System;

namespace LimitedLiability.UserInterface
{
	public class ToolboxTray : Menu
	{
		private Icon iconFrame;

		private Button buttonSelectEquipment;

		private Button buttonSelectRoom;
		private Button buttonFinances;
		private Button buttonCompany;
		private Button buttonEmployees;
		private Button buttonProducts;

		public event EventHandler ButtonSelectGeneralClicked;

		public event EventHandler ButtonSelectEquipmentClicked;

		public event EventHandler ButtonSelectRoomClicked;

		public event EventHandler ButtonFinancesClicked;

		public event EventHandler ButtonCompanyClicked;

		public event EventHandler ButtonEmployeesClicked;

		public event EventHandler ButtonProductsClicked;

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				int spacerWidth = 10;
				int spacerHeight = 20;

				int totalButtonWidth = buttonSelectEquipment.Width * 6 + spacerWidth * 5;

				iconFrame.Position = base.Position;
				buttonSelectEquipment.Position = new Vector(iconFrame.Position.X + iconFrame.Width / 2 - totalButtonWidth / 2, iconFrame.Position.Y - spacerHeight);
				buttonSelectRoom.Position = new Vector(buttonSelectEquipment.Position.X + buttonSelectEquipment.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonFinances.Position = new Vector(buttonSelectRoom.Position.X + buttonSelectRoom.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonCompany.Position = new Vector(buttonFinances.Position.X + buttonFinances.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonEmployees.Position = new Vector(buttonCompany.Position.X + buttonCompany.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
				buttonProducts.Position = new Vector(buttonEmployees.Position.X + buttonEmployees.Width + spacerWidth, iconFrame.Position.Y - spacerHeight);
			}
		}

		public ToolboxTray(ContentManager content)
		{
			iconFrame = ControlFactory.CreateIcon(content, "BottomBar");
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPathDroid = content.GetContentPath("DroidSans Bold");
			Color fontColorWhite = Styles.Colors.White;
			int fontSizeContent = 16;
			//int fontSizeTooltipText = 8;

			buttonSelectEquipment = ControlFactory.CreateButton(content, "ButtonBottomBar", "ButtonBottomBarHover");
			buttonSelectEquipment.Icon = ControlFactory.CreateIcon(content, "IconHandTruck");
			buttonSelectEquipment.Label = ControlFactory.CreateLabel(content, fontPathDroid, fontSizeContent, fontColorWhite, "Equipment");
			buttonSelectEquipment.ButtonType = ButtonType.IconAndText;

			buttonSelectRoom = ControlFactory.CreateButton(content, "ButtonBottomBar", "ButtonBottomBarHover");
			buttonSelectRoom.Icon = ControlFactory.CreateIcon(content, "IconForklift");
			buttonSelectRoom.Label = ControlFactory.CreateLabel(content, fontPathDroid, fontSizeContent, fontColorWhite, "Rooms");
			buttonSelectRoom.ButtonType = ButtonType.IconAndText;

			buttonFinances = ControlFactory.CreateButton(content, "ButtonBottomBar", "ButtonBottomBarHover");
			buttonFinances.Icon = ControlFactory.CreateIcon(content, "IconMoney");
			buttonFinances.Label = ControlFactory.CreateLabel(content, fontPathDroid, fontSizeContent, fontColorWhite, "Finances");
			buttonFinances.ButtonType = ButtonType.IconAndText;

			buttonCompany = ControlFactory.CreateButton(content, "ButtonBottomBar", "ButtonBottomBarHover");
			buttonCompany.Icon = ControlFactory.CreateIcon(content, "IconBarChartUp");
			buttonCompany.Label = ControlFactory.CreateLabel(content, fontPathDroid, fontSizeContent, fontColorWhite, "Company");
			buttonCompany.ButtonType = ButtonType.IconAndText;

			buttonEmployees = ControlFactory.CreateButton(content, "ButtonBottomBar", "ButtonBottomBarHover");
			buttonEmployees.Icon = ControlFactory.CreateIcon(content, "IconNametag");
			buttonEmployees.Label = ControlFactory.CreateLabel(content, fontPathDroid, fontSizeContent, fontColorWhite, "Employees");
			buttonEmployees.ButtonType = ButtonType.IconAndText;

			buttonProducts = ControlFactory.CreateButton(content, "ButtonBottomBar", "ButtonBottomBarHover");
			buttonProducts.Icon = ControlFactory.CreateIcon(content, "IconOpenBox");
			buttonProducts.Label = ControlFactory.CreateLabel(content, fontPathDroid, fontSizeContent, fontColorWhite, "Products");
			buttonProducts.ButtonType = ButtonType.IconAndText;

			Controls.Add(iconFrame);
			Controls.Add(buttonSelectEquipment);
			Controls.Add(buttonSelectRoom);
			Controls.Add(buttonFinances);
			Controls.Add(buttonCompany);
			Controls.Add(buttonEmployees);
			Controls.Add(buttonProducts);

			buttonSelectEquipment.Clicked += ButtonSelectEquipment_Clicked;
			buttonSelectRoom.Clicked += ButtonSelectRoom_Clicked;
			buttonFinances.Clicked += ButtonFinances_Clicked;
			buttonCompany.Clicked += ButtonCompany_Clicked;
			buttonEmployees.Clicked += ButtonEmployees_Clicked;
			buttonProducts.Clicked += ButtonProducts_Clicked;
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
	}
}