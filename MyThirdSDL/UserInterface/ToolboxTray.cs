using MyThirdSDL.Content;
using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ToolboxTray : Menu
	{
		private Icon iconFrame;

		private Button buttonSelectGeneral;
		private Button buttonSelectEquipment;
		private Button buttonSelectRoom;
		private Button buttonFinances;
		private Button buttonCompany;
		private Button buttonEmployees;
		private Button buttonProducts;
		private Button buttonMainMenu;
		private Button buttonMailMenu;

		private Icon iconMoney;
		private Icon iconTime;

		private Label labelMoney;
		private Label labelDate;
		private Label labelTime;

		public bool IsHovered { get; private set; }

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

				int toolboxTraySpaceBetweenButtons = 41;
				int toolboxTrayOuterEdgeWidth = 4;
				int toolboxTrayButtonImageOffsetX = 0;
				int toolboxTrayButtonImageOffsetY = 4;

				Vector buttonOnePosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX, toolboxTrayButtonImageOffsetY);
				Vector buttonTwoPosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons, toolboxTrayButtonImageOffsetY);
				Vector buttonThreePosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 2, toolboxTrayButtonImageOffsetY);
				Vector buttonFourPosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 3, toolboxTrayButtonImageOffsetY);
				Vector buttonFivePosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 4, toolboxTrayButtonImageOffsetY);
				Vector buttonSixPosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 5, toolboxTrayButtonImageOffsetY);
				Vector buttonSevenPosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 6, toolboxTrayButtonImageOffsetY);
				Vector buttonEightPosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 7, toolboxTrayButtonImageOffsetY);
				Vector buttonNinePosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 8, toolboxTrayButtonImageOffsetY);
				Vector iconOnePosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 9 + 35, toolboxTrayButtonImageOffsetY + 2);
				Vector iconTwoPosition = base.Position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 9 + 165, toolboxTrayButtonImageOffsetY + 2);

				iconFrame.Position = base.Position;
				buttonSelectGeneral.Position = buttonOnePosition;
				buttonSelectEquipment.Position = buttonTwoPosition;
				buttonSelectRoom.Position = buttonThreePosition;
				buttonFinances.Position = buttonFourPosition;
				buttonCompany.Position = buttonFivePosition;
				buttonEmployees.Position = buttonSixPosition;
				buttonProducts.Position = buttonSevenPosition;
				buttonMainMenu.Position = buttonEightPosition;
				buttonMailMenu.Position = buttonNinePosition;
				iconMoney.Position = iconOnePosition;
				iconTime.Position = iconTwoPosition;
				labelMoney.Position = iconOnePosition + new Vector(35, 10);
				labelDate.Position = iconTwoPosition + new Vector(32, 3);
				labelTime.Position = iconTwoPosition + new Vector(55, 18);

				buttonSelectGeneral.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				buttonSelectEquipment.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				buttonSelectRoom.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				buttonFinances.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				buttonCompany.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				buttonEmployees.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				buttonProducts.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				buttonMainMenu.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				buttonMailMenu.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
			}
		}

		public ToolboxTray(ContentManager contentManager, int unreadMailCount, int money)
		{
			Texture textureFrame = contentManager.GetTexture("ToolboxTray");
			iconFrame = new Icon(textureFrame);
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string buttonTexturePathKey = "ToolboxTrayButton";
			string buttonHoverTexturePathKey = "ToolboxTrayButtonHover";
			string tooltipTexturePathKey = "TooltipFrame";

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = Styles.Colors.PaleYellow;
			int fontSizeContent = 12;
			int fontSizeTooltipText = 8;

			buttonSelectGeneral = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			buttonSelectGeneral.Icon = ControlFactory.CreateIcon(contentManager, "IconArrowsInward");
			buttonSelectGeneral.IconHovered =ControlFactory.CreateIcon(contentManager, "IconArrowsInward");
			buttonSelectGeneral.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "Select objects to inspect them");
			buttonSelectGeneral.ButtonType = ButtonType.IconOnly;

			buttonSelectEquipment = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			buttonSelectEquipment.Icon = ControlFactory.CreateIcon(contentManager, "IconHandTruck");
			buttonSelectEquipment.IconHovered = ControlFactory.CreateIcon(contentManager, "IconHandTruck");
			buttonSelectEquipment.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "View equipment to purchase");
			buttonSelectEquipment.ButtonType = ButtonType.IconOnly;

			buttonSelectRoom = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			buttonSelectRoom.Icon = ControlFactory.CreateIcon(contentManager, "IconForklift");
			buttonSelectRoom.IconHovered = ControlFactory.CreateIcon(contentManager, "IconForklift");
			buttonSelectRoom.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey,fontPath, fontSizeTooltipText, fontColor, "View rooms to purchase");
			buttonSelectRoom.ButtonType = ButtonType.IconOnly;

			buttonFinances = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			buttonFinances.Icon = ControlFactory.CreateIcon(contentManager, "IconBarChartUp");
			buttonFinances.IconHovered = ControlFactory.CreateIcon(contentManager, "IconBarChartUp");
			buttonFinances.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "View your company's finances");
			buttonFinances.ButtonType = ButtonType.IconOnly;

			buttonCompany = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			buttonCompany.Icon = ControlFactory.CreateIcon(contentManager, "IconPenPaper");
			buttonCompany.IconHovered = ControlFactory.CreateIcon(contentManager, "IconPenPaper");
			buttonCompany.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "View your company's health and statistics");
			buttonCompany.ButtonType = ButtonType.IconOnly;

			buttonEmployees = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			buttonEmployees.Icon = ControlFactory.CreateIcon(contentManager, "IconPersonPlain");
			buttonEmployees.IconHovered = ControlFactory.CreateIcon(contentManager, "IconPersonPlain");
			buttonEmployees.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "View your employee information");
			buttonEmployees.ButtonType = ButtonType.IconOnly;

			buttonProducts = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			buttonProducts.Icon = ControlFactory.CreateIcon(contentManager, "IconOpenBox");
			buttonProducts.IconHovered = ControlFactory.CreateIcon(contentManager, "IconOpenBox");
			buttonProducts.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "View your products and services");
			buttonProducts.ButtonType = ButtonType.IconOnly;

			buttonMainMenu = ControlFactory.CreateButton(contentManager, buttonTexturePathKey, buttonHoverTexturePathKey);
			buttonMainMenu.Icon = ControlFactory.CreateIcon(contentManager, "IconWindow");
			buttonMainMenu.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindow");
			buttonMainMenu.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "View the main menu");
			buttonMainMenu.ButtonType = ButtonType.IconOnly;

			buttonMailMenu = ControlFactory.CreateButton(contentManager, "ToolboxTrayButtonMail", "ToolboxTrayButtonMailHover");
			buttonMailMenu.Icon = ControlFactory.CreateIcon(contentManager, "IconMailUnread");
			buttonMailMenu.IconHovered = ControlFactory.CreateIcon(contentManager, "IconMailUnread");
			buttonMailMenu.Tooltip = ControlFactory.CreateTooltip(contentManager, tooltipTexturePathKey, fontPath, fontSizeTooltipText, fontColor, "View your e-mail messages");
			buttonMailMenu.Label = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, unreadMailCount.ToString());
			buttonMailMenu.ButtonType = ButtonType.IconAndText;

			iconMoney = ControlFactory.CreateIcon(contentManager, "IconMoney");
			
			iconTime = ControlFactory.CreateIcon(contentManager, "IconTime");

			labelMoney = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, money.ToString());

			labelDate = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, DateTime.Now.ToShortDateString());

			labelTime = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, DateTime.Now.ToShortTimeString());

			Controls.Add(iconFrame);
			Controls.Add(buttonSelectGeneral);
			Controls.Add(buttonSelectEquipment);
			Controls.Add(buttonSelectRoom);
			Controls.Add(buttonFinances);
			Controls.Add(buttonCompany);
			Controls.Add(buttonEmployees);
			Controls.Add(buttonProducts);
			Controls.Add(buttonMainMenu);
			Controls.Add(buttonMailMenu);
			Controls.Add(iconMoney);
			Controls.Add(iconTime);
			Controls.Add(labelMoney);
			Controls.Add(labelDate);
			Controls.Add(labelTime);

			buttonSelectGeneral.Clicked += ButtonSelectGeneral_Clicked;
			buttonSelectEquipment.Clicked += ButtonSelectEquipment_Clicked;
			buttonSelectRoom.Clicked += ButtonSelectRoom_Clicked;
			buttonFinances.Clicked += ButtonFinances_Clicked;
			buttonCompany.Clicked += ButtonCompany_Clicked;
			buttonEmployees.Clicked += ButtonEmployees_Clicked;
			buttonProducts.Clicked += ButtonProducts_Clicked;
			buttonMainMenu.Clicked += ButtonMainMenu_Clicked;
			buttonMailMenu.Clicked += ButtonMailMenu_Clicked;
		}

		public void UpdateDisplayedDateAndTime(DateTime dateTime)
		{
			labelTime.Text = dateTime.ToShortTimeString();
			labelDate.Text = dateTime.ToShortDateString();
		}

		public void UpdateDisplayedUnreadMailCount(int unreadMailCount)
		{
			buttonMailMenu.Text = unreadMailCount.ToString();
		}

		public void UpdateDisplayedBankAccountBalance(int money)
		{
			labelMoney.Text = money.ToString();
		}

		public override void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			if (Bounds.Contains(new Point(e.RelativeToWindowX, e.RelativeToWindowY)))
				IsHovered = true;
			else
				IsHovered = false;

			base.HandleMouseMovingEvent(sender, e);
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
