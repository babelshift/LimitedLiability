using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using MyThirdSDL.Simulation;

namespace MyThirdSDL.UserInterface
{
	public class ControlFactory
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

		public MenuInspectEmployee CreateMenuInspectEmployee(Vector bottomRightPointOfWindow)
		{
			Texture texture = GetTexture("MenuInspectEmployeeFrame");
			Vector position = new Vector(bottomRightPointOfWindow.X / 2 - texture.Width / 2, bottomRightPointOfWindow.Y / 2 - texture.Height / 2);
			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(218, 218, 218);
			int fontSizeTitle = 14;
			int fontSizeContent = 12;

			Icon iconMainMenu = CreateIcon(new Vector(position.X + 5, position.Y + 5), "IconPersonPlain");
			Icon iconNeedsMenu = CreateIcon(new Vector(position.X + 362, position.Y + 5), "IconStatistics");
			Icon iconSkillsMenu = CreateIcon(new Vector(position.X + 505, position.Y + 5), "IconPenPaper");

			Label labelMainMenu = CreateLabel(new Vector(position.X + 38, position.Y + 15), fontPath, fontSizeTitle, fontColor, "Inspect Employee");
			Label labelNeedsMenu = CreateLabel(new Vector(position.X + 400, position.Y + 15), fontPath, fontSizeTitle, fontColor, "Needs");
			Label labelSkillsMenu = CreateLabel(new Vector(position.X + 538, position.Y + 15), fontPath, fontSizeTitle, fontColor, "Skills");

			Icon iconHealth = CreateIcon(new Vector(position.X + 362, position.Y + 50), "IconMedkit");
			Icon iconHygiene = CreateIcon(new Vector(position.X + 362, position.Y + 80), "IconToothbrush");
			Icon iconSleep = CreateIcon(new Vector(position.X + 362, position.Y + 110), "IconPersonTired");
			Icon iconThirst = CreateIcon(new Vector(position.X + 362, position.Y + 140), "IconSoda");
			Icon iconHunger = CreateIcon(new Vector(position.X + 362, position.Y + 170), "IconChicken");

			Label labelHealth = CreateLabel(new Vector(position.X + 395, position.Y + 60), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelHygiene = CreateLabel(new Vector(position.X + 395, position.Y + 90), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelSleep = CreateLabel(new Vector(position.X + 395, position.Y + 120), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelThirst = CreateLabel(new Vector(position.X + 395, position.Y + 150), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelHunger = CreateLabel(new Vector(position.X + 395, position.Y + 180), fontPath, fontSizeContent, fontColor, "N/A");

			Icon iconCommunication = CreateIcon(new Vector(position.X + 508, position.Y + 50), "IconCommunication");
			Icon iconLeadership = CreateIcon(new Vector(position.X + 508, position.Y + 80), "IconLeadership");
			Icon iconCreativity = CreateIcon(new Vector(position.X + 508, position.Y + 110), "IconCreativity");
			Icon iconIntelligence = CreateIcon(new Vector(position.X + 508, position.Y + 140), "IconIntelligence");

			Label labelCommunication = CreateLabel(new Vector(position.X + 540, position.Y + 60), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelLeadership = CreateLabel(new Vector(position.X + 540, position.Y + 90), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelCreativity = CreateLabel(new Vector(position.X + 540, position.Y + 120), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelIntelligence = CreateLabel(new Vector(position.X + 540, position.Y + 150), fontPath, fontSizeContent, fontColor, "N/A");

			Label labelName = CreateLabel(new Vector(position.X + 15, position.Y + 60), fontPath, fontSizeContent, fontColor, "Name:");
			Label labelAge = CreateLabel(new Vector(position.X + 15, position.Y + 90), fontPath, fontSizeContent, fontColor, "Age:");
			Label labelJob = CreateLabel(new Vector(position.X + 15, position.Y + 120), fontPath, fontSizeContent, fontColor, "Job:");
			Label labelSalary = CreateLabel(new Vector(position.X + 15, position.Y + 150), fontPath, fontSizeContent, fontColor, "Salary:");
			Label labelStatus = CreateLabel(new Vector(position.X + 15, position.Y + 180), fontPath, fontSizeContent, fontColor, "Status:");
			Label labelBirth = CreateLabel(new Vector(position.X + 15, position.Y + 210), fontPath, fontSizeContent, fontColor, "Birth:");
			Label labelMood = CreateLabel(new Vector(position.X + 15, position.Y + 240), fontPath, fontSizeContent, fontColor, "Mood:");

			Label labelNameValue = CreateLabel(new Vector(position.X + 110, position.Y + 60), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelAgeValue = CreateLabel(new Vector(position.X + 110, position.Y + 90), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelJobValue = CreateLabel(new Vector(position.X + 110, position.Y + 120), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelSalaryValue = CreateLabel(new Vector(position.X + 110, position.Y + 150), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelStatusValue = CreateLabel(new Vector(position.X + 110, position.Y + 180), fontPath, fontSizeContent, fontColor, "N/A");
			Label labelBirthValue = CreateLabel(new Vector(position.X + 110, position.Y + 210), fontPath, fontSizeContent, fontColor, "N/A");

			Icon iconMoodHappy = CreateIcon(new Vector(position.X + 110, position.Y + 230), "IconPersonHappy");
			Icon iconMoodAngry = CreateIcon(new Vector(position.X + 110, position.Y + 230), "IconPersonAngry");

			Button buttonCloseWindow = CreateButton(new Vector(position.X + 600, position.Y - 47), "ButtonSquare", "ButtonSquareHover", "IconWindowClose", "IconWindowClose");

			MenuInspectEmployee menuInspectEmployee = new MenuInspectEmployee(texture, position, buttonCloseWindow,
				labelNameValue, labelAgeValue, labelJobValue, labelSalaryValue, labelStatusValue, labelBirthValue, iconMoodHappy, iconMoodAngry,
				labelHealth, labelHygiene, labelSleep, labelThirst, labelHunger, labelCommunication, labelCreativity, labelLeadership, labelIntelligence);

			menuInspectEmployee.AddControl(iconMainMenu);
			menuInspectEmployee.AddControl(iconNeedsMenu);
			menuInspectEmployee.AddControl(iconSkillsMenu);

			menuInspectEmployee.AddControl(labelMainMenu);
			menuInspectEmployee.AddControl(labelNeedsMenu);
			menuInspectEmployee.AddControl(labelSkillsMenu);

			menuInspectEmployee.AddControl(iconHealth);
			menuInspectEmployee.AddControl(iconHygiene);
			menuInspectEmployee.AddControl(iconSleep);
			menuInspectEmployee.AddControl(iconThirst);
			menuInspectEmployee.AddControl(iconHunger);

			menuInspectEmployee.AddControl(iconCommunication);
			menuInspectEmployee.AddControl(iconLeadership);
			menuInspectEmployee.AddControl(iconCreativity);
			menuInspectEmployee.AddControl(iconIntelligence);

			menuInspectEmployee.AddControl(labelName);
			menuInspectEmployee.AddControl(labelAge);
			menuInspectEmployee.AddControl(labelJob);
			menuInspectEmployee.AddControl(labelSalary);
			menuInspectEmployee.AddControl(labelStatus);
			menuInspectEmployee.AddControl(labelBirth);
			menuInspectEmployee.AddControl(labelMood);

			return menuInspectEmployee;
		}

		public MenuEquipment CreateMenuEquipment(Vector bottomRightPointOfWindow, IEnumerable<IPurchasable> purchasableItems)
		{
			Texture texture = GetTexture("MenuEquipmentFrame");

			Vector position = new Vector(bottomRightPointOfWindow.X / 2 - texture.Width / 2, bottomRightPointOfWindow.Y / 2 - texture.Height / 2);

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(218, 218, 218);
			int fontSizeTitle = 14;
			int fontSizeContent = 12;

			Icon iconMainMenu = CreateIcon(new Vector(position.X + 3, position.Y + 5), "IconHandTruck");
			Icon iconInfoMenu = CreateIcon(new Vector(position.X + 365, position.Y + 5), "IconStatistics");
			Label labelMainMenu = CreateLabel(new Vector(position.X + 38, position.Y + 15), fontPath, fontSizeTitle, fontColor, "Equipment");
			Label labelInfoMenu = CreateLabel(new Vector(position.X + 400, position.Y + 15), fontPath, fontSizeTitle, fontColor, "Info");

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

			Button buttonCloseWindow = CreateButton(new Vector(position.X + 454, position.Y - 47), "ButtonSquare", "ButtonSquareHover", "IconWindowClose", "IconWindowClose");
			Button buttonConfirmWindow = CreateButton(new Vector(position.X + 454, position.Y + 303), "ButtonSquare", "ButtonSquareHover", "IconWindowConfirm", "IconWindowConfirm");

			Button buttonArrowCircleLeft = CreateButton(new Vector(position.X + 9, position.Y + 248), "ButtonSquare", "ButtonSquareHover", "IconArrowCircleLeft", "IconArrowCircleLeft");
			Button buttonArrowCircleRight = CreateButton(new Vector(position.X + 296, position.Y + 248), "ButtonSquare", "ButtonSquareHover", "IconArrowCircleRight", "IconArrowCircleRight");

			MenuEquipment menuEquipment = new MenuEquipment(texture, position, iconMainMenu, iconInfoMenu, labelMainMenu, labelInfoMenu,
				iconMoney, iconHealth, iconHygiene, iconSleep, iconThirst, iconHunger, labelMoney, labelHealth, labelHygiene, labelSleep, labelThirst,
				labelHunger, buttonArrowCircleLeft, buttonArrowCircleRight, buttonCloseWindow, buttonConfirmWindow);

			foreach (var purchasableItem in purchasableItems)
			{
				ButtonMenuItem buttonMenuItem = CreateButtonMenuItem(position, "ButtonMenuItem", "ButtonMenuItemHover", purchasableItem);
				menuEquipment.AddButtonMenuItem(buttonMenuItem);
			}

			return menuEquipment;
		}

		public ToolboxTray CreateToolboxTray(Vector position)
		{
			Texture texture = GetTexture("ToolboxTray");

			int toolboxTraySpaceBetweenButtons = 41;
			int toolboxTrayOuterEdgeWidth = 4;
			int toolboxTrayButtonImageOffsetX = 0;
			int toolboxTrayButtonImageOffsetY = 4;

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

			string buttonTexturePathKey = "ToolboxTrayButton";
			string buttonHoverTexturePathKey = "ToolboxTrayButtonHover";
			Button buttonSelectGeneral = CreateButton(buttonOnePosition, buttonTexturePathKey, buttonHoverTexturePathKey, "IconArrowsInward", "IconArrowsInward");
			Button buttonSelectEquipment = CreateButton(buttonTwoPosition, buttonTexturePathKey, buttonHoverTexturePathKey, "IconHandTruck", "IconHandTruck");
			Button buttonSelectRoom = CreateButton(buttonThreePosition, buttonTexturePathKey, buttonHoverTexturePathKey, "IconForklift", "IconForklift");
			Button buttonFinances = CreateButton(buttonFourPosition, buttonTexturePathKey, buttonHoverTexturePathKey, "IconBarChartUp", "IconBarChartUp");
			Button buttonCompany = CreateButton(buttonFivePosition, buttonTexturePathKey, buttonHoverTexturePathKey, "IconPenPaper", "IconPenPaper");
			Button buttonEmployees = CreateButton(buttonSixPosition, buttonTexturePathKey, buttonHoverTexturePathKey, "IconPersonPlain", "IconPersonPlain");
			Button buttonProducts = CreateButton(buttonSevenPosition, buttonTexturePathKey, buttonHoverTexturePathKey, "IconOpenBox", "IconOpenBox");
			Button buttonMainMenu = CreateButton(buttonEightPosition, buttonTexturePathKey, buttonHoverTexturePathKey, "IconWindow", "IconWindow");
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

		public ButtonMenuItem CreateButtonMenuItem(Vector position, string texturePathKey, string textureHoverPathKey, IPurchasable purchasableItem)
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

			Icon iconItem = CreateIcon(iconItemPosition, purchasableItem.IconTextureKey);
			Label labelItem = CreateLabel(labelItemPosition, fontPath, fontSizeContent, fontColor, purchasableItem.Name);
			Icon iconMoney = CreateIcon(iconMoneyPosition, "IconMoney");
			Label labelMoney = CreateLabel(labelMoneyPosition, fontPath, fontSizeContent, fontColor, purchasableItem.Price.ToString());

			return new ButtonMenuItem(position, texture, textureHover, iconItem, labelItem, iconMoney, labelMoney, purchasableItem);
		}

		public Button CreateButton(Vector position, string texturePathKey, string textureHoverPathKey,
			string iconTexturePathKey, string iconTextureHoverPathKey)
		{
			Texture texture = GetTexture(texturePathKey);
			Texture textureHover = GetTexture(textureHoverPathKey);
			Icon icon = CreateIcon(position, iconTexturePathKey);
			Icon iconHover = CreateIcon(position, iconTextureHoverPathKey);
			icon.Position = new Vector(position.X + (texture.Width / 2 - icon.Width / 2), position.Y + (texture.Height / 2 - icon.Height / 2));
			iconHover.Position = new Vector(position.X + (texture.Width / 2 - icon.Width / 2), position.Y + (texture.Height / 2 - icon.Height / 2));

			return new Button(texture, textureHover, position, icon, iconHover);
		}

		public Icon CreateIcon(Vector position, string texturePathKey)
		{
			Texture texture = GetTexture(texturePathKey);
			return new Icon(texture, position);
		}

		public Label CreateLabel(Vector position, string fontPath, int fontSize, Color color, string text)
		{
			if (log.IsDebugEnabled)
				log.Debug(String.Format("Creating label at ({0},{1}) with text: {2}", position.X, position.Y, text));
			TrueTypeText trueTypeText = TrueTypeTextFactory.CreateTrueTypeText(renderer, fontPath, fontSize, color, text);
			return new Label(position, trueTypeText);
		}

		public SimulationLabel CreateSimulationLabel(Vector position, string fontPath, int fontSize, Color color, SimulationMessage simulationMessage)
		{
			if (log.IsDebugEnabled)
				log.Debug(String.Format("Creating simulation label at ({0},{1}) with type: {2} and text: {3}", position.X, position.Y, simulationMessage.Type, simulationMessage.Text));
			TrueTypeText trueTypeText = TrueTypeTextFactory.CreateTrueTypeText(renderer, fontPath, fontSize, color, simulationMessage.Text);
			return new SimulationLabel(position, trueTypeText, simulationMessage);
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
