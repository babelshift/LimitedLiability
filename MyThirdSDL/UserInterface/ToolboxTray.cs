using MyThirdSDL.Content;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ToolboxTray : Control
	{
		private List<Control> controls = new List<Control>();

		private Icon iconFrame;

		private Button ButtonSelectGeneral { get; set; }
		private Button ButtonSelectEquipment { get; set; }
		private Button ButtonSelectRoom { get; set; }
		private Button ButtonFinances { get; set; }
		private Button ButtonCompany { get; set; }
		private Button ButtonEmployees { get; set; }
		private Button ButtonProducts { get; set; }
		private Button ButtonMainMenu { get; set; }
		private Button ButtonMailMenu { get; set; }

		private Icon IconMoney { get; set; }
		private Icon IconTime { get; set; }

		private Label LabelMoney { get; set; }
		private Label LabelDate { get; set; }
		private Label LabelTime { get; set; }

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

		public ToolboxTray(ContentManager contentManager)
		{
			Texture textureFrame = contentManager.GetTexture("ToolboxTray");
			iconFrame = new Icon();
			iconFrame.TextureFrame = textureFrame;

			string buttonTexturePathKey = "ToolboxTrayButton";
			string buttonHoverTexturePathKey = "ToolboxTrayButtonHover";
			string tooltipTexturePathKey = "TooltipFrame";

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(218, 218, 218);
			int fontSizeContent = 12;
			int fontSizeTooltipText = 8;

			ButtonSelectGeneral = new Button();
			ButtonSelectGeneral.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonSelectGeneral.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonSelectGeneral.Icon = new Icon();
			ButtonSelectGeneral.Icon.TextureFrame = contentManager.GetTexture("IconArrowsInward");
			ButtonSelectGeneral.IconHovered = new Icon();
			ButtonSelectGeneral.IconHovered.TextureFrame = contentManager.GetTexture("IconArrowsInward");
			ButtonSelectGeneral.Tooltip = new Tooltip();
			ButtonSelectGeneral.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonSelectGeneral.Tooltip.Label = new Label();
			ButtonSelectGeneral.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "Select objects to inspect them");

			ButtonSelectEquipment = new Button();
			ButtonSelectEquipment.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonSelectEquipment.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonSelectEquipment.Icon = new Icon();
			ButtonSelectEquipment.Icon.TextureFrame = contentManager.GetTexture("IconHandTruck");
			ButtonSelectEquipment.IconHovered = new Icon();
			ButtonSelectEquipment.IconHovered.TextureFrame = contentManager.GetTexture("IconHandTruck");
			ButtonSelectEquipment.Tooltip = new Tooltip();
			ButtonSelectEquipment.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonSelectEquipment.Tooltip.Label = new Label();
			ButtonSelectEquipment.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View equipment to purchase");

			ButtonSelectRoom = new Button();
			ButtonSelectRoom.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonSelectRoom.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonSelectRoom.Icon = new Icon();
			ButtonSelectRoom.Icon.TextureFrame = contentManager.GetTexture("IconForklift");
			ButtonSelectRoom.IconHovered = new Icon();
			ButtonSelectRoom.IconHovered.TextureFrame = contentManager.GetTexture("IconForklift");
			ButtonSelectRoom.Tooltip = new Tooltip();
			ButtonSelectRoom.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonSelectRoom.Tooltip.Label = new Label();
			ButtonSelectRoom.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View rooms to purchase");

			ButtonFinances = new Button();
			ButtonFinances.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonFinances.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonFinances.Icon = new Icon();
			ButtonFinances.Icon.TextureFrame = contentManager.GetTexture("IconBarChartUp");
			ButtonFinances.IconHovered = new Icon();
			ButtonFinances.IconHovered.TextureFrame = contentManager.GetTexture("IconBarChartUp");
			ButtonFinances.Tooltip = new Tooltip();
			ButtonFinances.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonFinances.Tooltip.Label = new Label();
			ButtonFinances.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View your company's finances");

			ButtonCompany = buttonCompany;
			ButtonEmployees = buttonEmployees;
			ButtonProducts = buttonProducts;
			ButtonMainMenu = buttonMainMenu;
			ButtonMailMenu = buttonMailMenu;

			IconMoney = iconMoney;
			IconTime = iconTime;
			LabelMoney = labelMoney;
			LabelDate = labelDate;
			LabelTime = labelTime;

			controls.Add(ButtonSelectGeneral);
			controls.Add(ButtonSelectEquipment);
			controls.Add(ButtonSelectRoom);
			controls.Add(ButtonFinances);
			controls.Add(ButtonCompany);
			controls.Add(ButtonEmployees);
			controls.Add(ButtonProducts);
			controls.Add(ButtonMainMenu);
			controls.Add(ButtonMailMenu);
			controls.Add(IconMoney);
			controls.Add(IconTime);
			controls.Add(LabelMoney);
			controls.Add(LabelDate);
			controls.Add(LabelTime);

			ButtonSelectGeneral.Clicked += ButtonSelectGeneral_Clicked;
			ButtonSelectEquipment.Clicked += ButtonSelectEquipment_Clicked;
			ButtonSelectRoom.Clicked += ButtonSelectRoom_Clicked;
			ButtonFinances.Clicked += ButtonFinances_Clicked;
			ButtonCompany.Clicked += ButtonCompany_Clicked;
			ButtonEmployees.Clicked += ButtonEmployees_Clicked;
			ButtonProducts.Clicked += ButtonProducts_Clicked;
			ButtonMainMenu.Clicked += ButtonMainMenu_Clicked;
			ButtonMailMenu.Clicked += ButtonMailMenu_Clicked;
		}

		public void UpdateDisplayedDateAndTime(DateTime dateTime)
		{
			LabelTime.Text = dateTime.ToShortTimeString();
			LabelDate.Text = dateTime.ToShortDateString();
		}

		public void UpdateDisplayedUnreadMailCount(int unreadMailCount)
		{
			ButtonMailMenu.Text = unreadMailCount.ToString();
		}

		public void UpdateDisplayedBankAccountBalance(int money)
		{
			LabelMoney.Text = money.ToString();
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var control in controls)
				if(control != null)
					control.Update(gameTime);

			if (Bounds.Contains(new Point(MouseHelper.CurrentMouseState.X, MouseHelper.CurrentMouseState.Y)))
				IsHovered = true;
			else
				IsHovered = false;
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			foreach (var control in controls)
				if (control != null)
					control.Draw(gameTime, renderer);
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
