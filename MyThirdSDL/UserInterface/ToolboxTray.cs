using MyThirdSDL.Content;
using SharpDL;
using SharpDL.Graphics;
using SharpDL.Input;
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
				ButtonSelectGeneral.Position = buttonOnePosition;
				ButtonSelectEquipment.Position = buttonTwoPosition;
				ButtonSelectRoom.Position = buttonThreePosition;
				ButtonFinances.Position = buttonFourPosition;
				ButtonCompany.Position = buttonFivePosition;
				ButtonEmployees.Position = buttonSixPosition;
				ButtonProducts.Position = buttonSevenPosition;
				ButtonMainMenu.Position = buttonEightPosition;
				ButtonMailMenu.Position = buttonNinePosition;
				IconMoney.Position = iconOnePosition;
				IconTime.Position = iconTwoPosition;
				LabelMoney.Position = iconOnePosition + new Vector(35, 10);
				LabelDate.Position = iconTwoPosition + new Vector(32, 3);
				LabelTime.Position = iconTwoPosition + new Vector(55, 18);

				ButtonSelectGeneral.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				ButtonSelectEquipment.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				ButtonSelectRoom.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				ButtonFinances.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				ButtonCompany.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				ButtonEmployees.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				ButtonProducts.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				ButtonMainMenu.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
				ButtonMailMenu.Tooltip.Position = new Vector(base.Position.X, base.Position.Y - 25);
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
			Color fontColor = new Color(218, 218, 218);
			int fontSizeContent = 12;
			int fontSizeTooltipText = 8;

			ButtonSelectGeneral = new Button();
			ButtonSelectGeneral.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonSelectGeneral.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonSelectGeneral.Icon = new Icon(contentManager.GetTexture("IconArrowsInward"));
			ButtonSelectGeneral.IconHovered = new Icon(contentManager.GetTexture("IconArrowsInward"));
			ButtonSelectGeneral.Tooltip = new Tooltip();
			ButtonSelectGeneral.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonSelectGeneral.Tooltip.Label = new Label();
			ButtonSelectGeneral.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "Select objects to inspect them");
			ButtonSelectGeneral.ButtonType = ButtonType.IconOnly;

			ButtonSelectEquipment = new Button();
			ButtonSelectEquipment.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonSelectEquipment.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonSelectEquipment.Icon = new Icon(contentManager.GetTexture("IconHandTruck"));
			ButtonSelectEquipment.IconHovered = new Icon(contentManager.GetTexture("IconHandTruck"));
			ButtonSelectEquipment.Tooltip = new Tooltip();
			ButtonSelectEquipment.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonSelectEquipment.Tooltip.Label = new Label();
			ButtonSelectEquipment.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View equipment to purchase");
			ButtonSelectEquipment.ButtonType = ButtonType.IconOnly;

			ButtonSelectRoom = new Button();
			ButtonSelectRoom.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonSelectRoom.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonSelectRoom.Icon = new Icon(contentManager.GetTexture("IconForklift"));
			ButtonSelectRoom.IconHovered = new Icon(contentManager.GetTexture("IconForklift"));
			ButtonSelectRoom.Tooltip = new Tooltip();
			ButtonSelectRoom.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonSelectRoom.Tooltip.Label = new Label();
			ButtonSelectRoom.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View rooms to purchase");
			ButtonSelectRoom.ButtonType = ButtonType.IconOnly;

			ButtonFinances = new Button();
			ButtonFinances.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonFinances.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonFinances.Icon = new Icon(contentManager.GetTexture("IconBarChartUp"));
			ButtonFinances.IconHovered = new Icon(contentManager.GetTexture("IconBarChartUp"));
			ButtonFinances.Tooltip = new Tooltip();
			ButtonFinances.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonFinances.Tooltip.Label = new Label();
			ButtonFinances.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View your company's finances");
			ButtonFinances.ButtonType = ButtonType.IconOnly;

			ButtonCompany = new Button();
			ButtonCompany.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonCompany.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonCompany.Icon = new Icon(contentManager.GetTexture("IconPenPaper"));
			ButtonCompany.IconHovered = new Icon(contentManager.GetTexture("IconPenPaper"));
			ButtonCompany.Tooltip = new Tooltip();
			ButtonCompany.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonCompany.Tooltip.Label = new Label();
			ButtonCompany.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View your company's health and statistics");
			ButtonCompany.ButtonType = ButtonType.IconOnly;

			ButtonEmployees = new Button();
			ButtonEmployees.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonEmployees.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonEmployees.Icon = new Icon(contentManager.GetTexture("IconPersonPlain"));
			ButtonEmployees.IconHovered = new Icon(contentManager.GetTexture("IconPersonPlain"));
			ButtonEmployees.Tooltip = new Tooltip();
			ButtonEmployees.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonEmployees.Tooltip.Label = new Label();
			ButtonEmployees.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View your employee information");
			ButtonEmployees.ButtonType = ButtonType.IconOnly;

			ButtonProducts = new Button();
			ButtonProducts.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonProducts.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonProducts.Icon = new Icon(contentManager.GetTexture("IconOpenBox"));
			ButtonProducts.IconHovered = new Icon(contentManager.GetTexture("IconOpenBox"));
			ButtonProducts.Tooltip = new Tooltip();
			ButtonProducts.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonProducts.Tooltip.Label = new Label();
			ButtonProducts.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View your products and services");
			ButtonProducts.ButtonType = ButtonType.IconOnly;

			ButtonMainMenu = new Button();
			ButtonMainMenu.TextureFrame = contentManager.GetTexture(buttonTexturePathKey);
			ButtonMainMenu.TextureFrameHovered = contentManager.GetTexture(buttonHoverTexturePathKey);
			ButtonMainMenu.Icon = new Icon(contentManager.GetTexture("IconWindow"));
			ButtonMainMenu.IconHovered = new Icon(contentManager.GetTexture("IconWindow"));
			ButtonMainMenu.Tooltip = new Tooltip();
			ButtonMainMenu.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonMainMenu.Tooltip.Label = new Label();
			ButtonMainMenu.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View the main menu");
			ButtonMainMenu.ButtonType = ButtonType.IconOnly;
			
			ButtonMailMenu = new Button();
			ButtonMailMenu.TextureFrame = contentManager.GetTexture("ToolboxTrayButtonMail");
			ButtonMailMenu.TextureFrameHovered = contentManager.GetTexture("ToolboxTrayButtonMailHover");
			ButtonMailMenu.Icon = new Icon(contentManager.GetTexture("IconMailUnread"));
			ButtonMailMenu.IconHovered = new Icon(contentManager.GetTexture("IconMailUnread"));
			ButtonMailMenu.Tooltip = new Tooltip();
			ButtonMailMenu.Tooltip.TextureFrame = contentManager.GetTexture(tooltipTexturePathKey);
			ButtonMailMenu.Tooltip.Label = new Label();
			ButtonMailMenu.Tooltip.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeTooltipText, fontColor, "View your e-mail messages");
			ButtonMailMenu.Label = new Label();
			ButtonMailMenu.Label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, unreadMailCount.ToString());
			ButtonMailMenu.ButtonType = ButtonType.IconAndText;

			Texture textureIconMoney = contentManager.GetTexture("IconMoney");
			IconMoney = new Icon(textureIconMoney);

			Texture textureIconTime = contentManager.GetTexture("IconTime");
			IconTime = new Icon(textureIconTime);

			LabelMoney = new Label();
			LabelMoney.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, money.ToString());

			LabelDate = new Label();
			LabelDate.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, DateTime.Now.ToShortDateString());

			LabelTime = new Label();
			LabelTime.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColor, DateTime.Now.ToShortTimeString());

			controls.Add(iconFrame);
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

			if (Bounds.Contains(new Point(Mouse.X, Mouse.Y)))
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

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach (var control in controls)
				if (control != null)
					control.Dispose();
			controls.Clear();
		}
	}
}
