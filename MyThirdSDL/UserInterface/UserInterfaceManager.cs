using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.UserInterface
{
	public enum MouseModeType
	{
		SelectGeneral,
		SelectEquipment,
		SelectRoom
	}

	public class UserInterfaceManager
	{
		private Point bottomRightPointOfWindow;
		private ControlFactory controlFactory;

		private Label labelMousePositionAbsolute;
		private Label labelMousePositionIsometric;
		private Label labelSimulationTime;

		private ToolboxTray toolboxTray;
		private MenuEquipment menuEquipment;
		private bool isEquipmentMenuOpen = false;

		private IEnumerable<IPurchasable> purchasableItems;

		public MouseModeType MouseMode { get; private set; }

		public UserInterfaceManager(Renderer renderer, ContentManager contentManager, Point bottomRightPointOfWindow, IEnumerable<IPurchasable> purchasableItems)
		{
			this.bottomRightPointOfWindow = bottomRightPointOfWindow;

			MouseMode = MouseModeType.SelectGeneral;

			controlFactory = new ControlFactory(renderer, contentManager);
			toolboxTray = controlFactory.CreateToolboxTray(new Vector(bottomRightPointOfWindow.X / 2 - 300, bottomRightPointOfWindow.Y - 50));
			toolboxTray.ButtonSelectGeneralClicked += ToolboxTray_ButtonSelectGeneralClicked;
			toolboxTray.ButtonSelectEquipmentClicked += ToolboxTray_ButtonSelectEquipmentClicked;
			toolboxTray.ButtonSelectRoomClicked += ToolboxTray_ButtonSelectRoomClicked;
			toolboxTray.ButtonFinancesClicked += ToolboxTray_ButtonFinancesClicked;
			toolboxTray.ButtonCompanyClicked += ToolboxTray_ButtonCompanyClicked;
			toolboxTray.ButtonEmployeesClicked += ToolboxTray_ButtonEmployeesClicked;
			toolboxTray.ButtonProductsClicked += ToolboxTray_ButtonProductsClicked;
			toolboxTray.ButtonMainMenuClicked += ToolboxTray_ButtonMainMenuClicked;

			this.purchasableItems = purchasableItems;
		}

		private void ToolboxTray_ButtonMainMenuClicked(object sender, EventArgs e)
		{
			// show main menu
		}

		private void ToolboxTray_ButtonProductsClicked(object sender, EventArgs e)
		{
			// show products menu
		}

		private void ToolboxTray_ButtonEmployeesClicked(object sender, EventArgs e)
		{
			// show employees menu
		}

		private void ToolboxTray_ButtonCompanyClicked(object sender, EventArgs e)
		{
			// show company menu
		}

		private void ToolboxTray_ButtonFinancesClicked(object sender, EventArgs e)
		{
			// show finances menu
		}

		private void ToolboxTray_ButtonSelectRoomClicked(object sender, EventArgs e)
		{
			MouseMode = MouseModeType.SelectRoom;
		}

		private void ToolboxTray_ButtonSelectEquipmentClicked(object sender, EventArgs e)
		{
			if (!isEquipmentMenuOpen)
			{
				if (menuEquipment == null)
				{
					menuEquipment = controlFactory.CreateMenuEquipment(new Vector(100, 100), purchasableItems);
					menuEquipment.ButtonCloseWindowClicked += menuEquipment_ButtonCloseWindowClicked;
				}

				MouseMode = MouseModeType.SelectEquipment;

				isEquipmentMenuOpen = true;
			}
		}

		private void menuEquipment_ButtonCloseWindowClicked(object sender, EventArgs e)
		{
			isEquipmentMenuOpen = false;
		}

		private void ToolboxTray_ButtonSelectGeneralClicked(object sender, EventArgs e)
		{
			MouseMode = MouseModeType.SelectGeneral;
		}
		
		public void Update(GameTime gameTime)
		{
			toolboxTray.Update(gameTime);

			if(isEquipmentMenuOpen)
				menuEquipment.Update(gameTime);
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			toolboxTray.Draw(gameTime, renderer);

			if (isEquipmentMenuOpen)
				menuEquipment.Draw(gameTime, renderer);
		}
	}
}
