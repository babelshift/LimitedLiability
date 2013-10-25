using MyThirdSDL.Descriptors;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ControlFactory
	{
		private Renderer renderer;
		private ContentManager contentManager;
		private TextureStore textureStore;

		public ControlFactory(Renderer renderer, ContentManager contentManager)
		{
			this.renderer = renderer;
			this.contentManager = contentManager;
			this.textureStore = new TextureStore(renderer);
		}

		private Texture GetTexture(string texturePathKey)
		{
			string texturePath = contentManager.GetContentPath(texturePathKey);
			Texture texture = textureStore.GetTexture(texturePath);
			return texture;
		}

		public MessageBox CreateMessageBox(Vector position, string text, MessageBoxType type)
		{
			Texture textureFrame = GetTexture("MessageBoxFrame");
			Texture textureIcon = GetTexture("IconWarning");
			MessageBox messageBox = new MessageBox(textureFrame, position, textureIcon, text);
			return messageBox;
		}

		public MenuEquipment CreateMenuEquipment(Vector position)
		{
			Texture texture = GetTexture("MenuEquipmentFrame");
			
			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(218, 218, 218);
			int fontSizeTitle = 14;
			int fontSizeContent = 12;

			Icon iconMainMenu = CreateIcon(new Vector(position.X + 3, position.Y + 5), "IconHandTruck");
			Icon iconInfoMenu = CreateIcon(new Vector(position.X + 365, position.Y + 5), "IconStatistics");
			Label labelMainMenu = CreateLabel(new Vector(position.X + 38, position.Y + 15), fontPath, fontSizeTitle, fontColor, "Equipment");
			Label labelInfoMenu = CreateLabel(new Vector(position.X + 400, position.Y + 15), fontPath, fontSizeContent, fontColor, "Info");

			Icon iconMoney = CreateIcon(new Vector(position.X + 365, position.Y + 50), "IconMoney");
			Icon iconHealth = CreateIcon(new Vector(position.X + 365, position.Y + 80), "IconMedkit");
			Icon iconHygiene = CreateIcon(new Vector(position.X + 365, position.Y + 110), "IconToothbrush");
			Icon iconSleep = CreateIcon(new Vector(position.X + 365, position.Y + 140), "IconPersonTired");
			Icon iconThirst = CreateIcon(new Vector(position.X + 365, position.Y + 170), "IconSoda");
			Icon iconHunger = CreateIcon(new Vector(position.X + 365, position.Y + 200), "IconChicken");

			Label labelMoney = CreateLabel(new Vector(position.X + 400, position.Y + 60), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelHealth = CreateLabel(new Vector(position.X + 400, position.Y + 90), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelHygiene = CreateLabel(new Vector(position.X + 400, position.Y + 120), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelSleep = CreateLabel(new Vector(position.X + 400, position.Y + 150), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelThirst = CreateLabel(new Vector(position.X + 400, position.Y + 180), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelHunger = CreateLabel(new Vector(position.X + 400, position.Y + 210), fontPath, fontSizeContent, fontColor, "N/A");

			Button buttonCloseWindow = CreateButton(new Vector(position.X + 460, position.Y - 40), "IconWindowClose", "IconWindowCloseHover");
			Button buttonConfirmWindow = CreateButton(new Vector(position.X + 460, position.Y + 295), "IconWindowConfirm", "IconWindowCloseHover");

			Button buttonArrowCircleLeft = CreateButton(new Vector(position.X + 10, position.Y + 245), "IconArrowCircleLeft", "IconArrowCircleLeftHover");
			Button buttonArrowCircleRight = CreateButton(new Vector(position.X + 310, position.Y + 245), "IconArrowCircleRight", "IconArrowCircleRightHover");

			MenuEquipment menuEquipment = new MenuEquipment(texture, position, iconMainMenu, iconInfoMenu, labelMainMenu, labelInfoMenu,
				iconMoney, iconHealth, iconHygiene, iconSleep, iconThirst, iconHunger, labelMoney, labelHealth, labelHygiene, labelSleep, labelThirst,
				labelHunger, buttonArrowCircleLeft, buttonArrowCircleRight, buttonCloseWindow, buttonConfirmWindow);

			ButtonMenuItem buttonMenuItem1 = CreateButtonMenuItem(position, "ButtonMenuItem", "ButtonMenuItemHover", "Snack Machine", "IconPizza", 100);
			ButtonMenuItem buttonMenuItem2 = CreateButtonMenuItem(position, "ButtonMenuItem", "ButtonMenuItemHover", "Water Fountain", "IconWater", 50);

			menuEquipment.AddButtonMenuItem(buttonMenuItem1);
			menuEquipment.AddButtonMenuItem(buttonMenuItem2);

			return menuEquipment;
		}

		public ToolboxTray CreateToolboxTray(Vector position)
		{
			Texture texture = GetTexture("ToolboxTray");

			int toolboxTraySpaceBetweenButtons = 41;
			int toolboxTrayOuterEdgeWidth = 4;
			int toolboxTrayButtonImageOffsetX = 3;
			int toolboxTrayButtonImageOffsetY = 6;

			Vector buttonOnePosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX, toolboxTrayButtonImageOffsetY);
			Vector buttonTwoPosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons, toolboxTrayButtonImageOffsetY);
			Vector buttonThreePosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 2, toolboxTrayButtonImageOffsetY);
			Vector buttonFourPosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 3, toolboxTrayButtonImageOffsetY);
			Vector buttonFivePosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 4, toolboxTrayButtonImageOffsetY);
			Vector buttonSixPosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 5, toolboxTrayButtonImageOffsetY);
			Vector buttonSevenPosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 6, toolboxTrayButtonImageOffsetY);
			Vector buttonEightPosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 7, toolboxTrayButtonImageOffsetY);
			Vector iconOnePosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 8, toolboxTrayButtonImageOffsetY);
			Vector iconTwoPosition = position + new Vector(toolboxTrayOuterEdgeWidth + toolboxTrayButtonImageOffsetX + toolboxTraySpaceBetweenButtons * 8 + 135, toolboxTrayButtonImageOffsetY);

			string buttonHoverTexturePathKey = "ToolboxTrayButtonHover";
			Button buttonSelectGeneral = CreateButton(buttonOnePosition, "ButtonSelectGeneral", buttonHoverTexturePathKey);
			Button buttonSelectEquipment = CreateButton(buttonTwoPosition, "ButtonSelectEquipment", buttonHoverTexturePathKey);
			Button buttonSelectRoom = CreateButton(buttonThreePosition, "ButtonSelectRoom", buttonHoverTexturePathKey);
			Button buttonFinances = CreateButton(buttonFourPosition, "ButtonFinances", buttonHoverTexturePathKey);
			Button buttonCompany = CreateButton(buttonFivePosition, "ButtonCompany", buttonHoverTexturePathKey);
			Button buttonEmployees = CreateButton(buttonSixPosition, "ButtonEmployees", buttonHoverTexturePathKey);
			Button buttonProducts = CreateButton(buttonSevenPosition, "ButtonProducts", buttonHoverTexturePathKey);
			Button buttonMainMenu = CreateButton(buttonEightPosition, "ButtonMenu", buttonHoverTexturePathKey);
			Icon iconMoney = CreateIcon(iconOnePosition, "IconMoney");
			Icon iconTime = CreateIcon(iconTwoPosition, "IconTime");

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(218, 218, 218);
			Label labelMoney = CreateLabel(iconOnePosition + new Vector(35, 10), fontPath, 12, fontColor, "0");
			Label labelDate = CreateLabel(iconTwoPosition + new Vector(32, 3), fontPath, 12, fontColor, DateTime.Now.ToShortDateString());
			Label labelTime = CreateLabel(iconTwoPosition + new Vector(55, 18), fontPath, 12, fontColor, DateTime.Now.ToShortTimeString());

			Tooltip tooltipSelectGeneral = CreateTooltip(new Vector(position.X, position.Y - 25), "TooltipFrame", fontPath, 8, fontColor, "Select objects to inspect them");
			Tooltip tooltipSelectEquipment = CreateTooltip(new Vector(position.X, position.Y - 25), "TooltipFrame", fontPath, 8, fontColor, "View equipment to purchase");
			Tooltip tooltipSelectRoom = CreateTooltip(new Vector(position.X, position.Y - 25), "TooltipFrame", fontPath, 8, fontColor, "View rooms to purchase");
			Tooltip tooltipFinances = CreateTooltip(new Vector(position.X, position.Y - 25), "TooltipFrame", fontPath, 8, fontColor, "View your company's finances");
			Tooltip tooltipCompany = CreateTooltip(new Vector(position.X, position.Y - 25), "TooltipFrame", fontPath, 8, fontColor, "View your company's health and statistics");
			Tooltip tooltipEmployees = CreateTooltip(new Vector(position.X, position.Y - 25), "TooltipFrame", fontPath, 8, fontColor, "View your employee information");
			Tooltip tooltipProducts = CreateTooltip(new Vector(position.X, position.Y - 25), "TooltipFrame", fontPath, 8, fontColor, "View your products and services");
			Tooltip tooltipMainMenu = CreateTooltip(new Vector(position.X, position.Y - 25), "TooltipFrame", fontPath, 8, fontColor, "View the main menu");

			buttonSelectGeneral.Tooltip = tooltipSelectGeneral;
			buttonSelectEquipment.Tooltip = tooltipSelectEquipment;
			buttonSelectRoom.Tooltip = tooltipSelectRoom;
			buttonFinances.Tooltip = tooltipFinances;
			buttonCompany.Tooltip = tooltipCompany;
			buttonEmployees.Tooltip = tooltipEmployees;
			buttonProducts.Tooltip = tooltipProducts;
			buttonMainMenu.Tooltip = tooltipMainMenu;

			ToolboxTray toolboxTray = new ToolboxTray(texture, position, buttonSelectGeneral, buttonSelectEquipment, buttonSelectRoom,
				buttonFinances, buttonCompany, buttonEmployees, buttonProducts, buttonMainMenu, iconMoney, iconTime, labelMoney, labelDate, labelTime);

			return toolboxTray;
		}

		public ButtonMenuItem CreateButtonMenuItem(Vector position, string texturePathKey, string textureHoverPathKey, 
			string buttonText, string textureIconPathKey, int price)
		{
			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(218, 218, 218);
			int fontSizeContent = 12;

			Texture texture = GetTexture(texturePathKey);
			Texture textureHover = GetTexture(textureHoverPathKey);

			Vector iconItemPosition = new Vector(position.X + 5, position.Y + 5);
			Vector labelItemPosition = new Vector(position.X + 40, position.Y + 15);
			Vector iconMoneyPosition = new Vector(position.X + 245, position.Y + 5);
			Vector labelMoneyPosition = new Vector(position.X + 280, position.Y + 15);

			Icon iconItem = CreateIcon(iconItemPosition, textureIconPathKey);
			Label labelItem = CreateLabel(labelItemPosition, fontPath, fontSizeContent, fontColor, buttonText);
			Icon iconMoney = CreateIcon(iconMoneyPosition, "IconMoney");
			Label labelMoney = CreateLabel(labelMoneyPosition, fontPath, fontSizeContent, fontColor, price.ToString());

			return new ButtonMenuItem(position, texture, textureHover, iconItem, labelItem, iconMoney, labelMoney);
		}

		public Button CreateButton(Vector position, string texturePathKey, string textureHoverPathKey)
		{
			Texture texture = GetTexture(texturePathKey);
			Texture textureHover = GetTexture(textureHoverPathKey);
			return new Button(texture, textureHover, position);
		}

		public Icon CreateIcon(Vector position, string texturePathKey)
		{
			Texture texture = GetTexture(texturePathKey);
			return new Icon(texture, position);
		}

		public Label CreateLabel(Vector position, string fontPath, int fontSize, Color color, string text)
		{
			TrueTypeText trueTypeText = TrueTypeTextFactory.CreateTrueTypeText(renderer, fontPath, fontSize, color, text);
			return new Label(position, trueTypeText);
		}

		public Tooltip CreateTooltip(Vector position, string tooltipFrameTexturePathKey, string labelFontPath, 
			int labelFontSize, Color labelColor, string labelText)
		{
			Label label = CreateLabel(new Vector(position.X + 7, position.Y + 8), labelFontPath, labelFontSize, labelColor, labelText);
			Texture texture = GetTexture(tooltipFrameTexturePathKey);
			return new Tooltip(texture, position, label);
		}
	}
}
