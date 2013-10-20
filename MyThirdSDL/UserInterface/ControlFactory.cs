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
