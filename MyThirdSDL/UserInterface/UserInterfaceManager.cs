using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public MouseModeType MouseMode { get; private set; }
		public ToolboxTray ToolboxTray { get; private set; }

		public UserInterfaceManager(Renderer renderer, ContentManager contentManager, Point bottomRightPointOfWindow)
		{
			this.bottomRightPointOfWindow = bottomRightPointOfWindow;

			MouseMode = MouseModeType.SelectGeneral;

			controlFactory = new ControlFactory(renderer, contentManager);
			ToolboxTray = controlFactory.CreateToolboxTray(new Vector(bottomRightPointOfWindow.X / 2 - 300, bottomRightPointOfWindow.Y - 50));
			ToolboxTray.ButtonSelectGeneralClicked += ToolboxTray_ButtonSelectGeneralClicked;
			ToolboxTray.ButtonSelectEquipmentClicked += ToolboxTray_ButtonSelectEquipmentClicked;
			ToolboxTray.ButtonSelectRoomClicked += ToolboxTray_ButtonSelectRoomClicked;
			ToolboxTray.ButtonFinancesClicked += ToolboxTray_ButtonFinancesClicked;
			ToolboxTray.ButtonCompanyClicked += ToolboxTray_ButtonCompanyClicked;
			ToolboxTray.ButtonEmployeesClicked += ToolboxTray_ButtonEmployeesClicked;
			ToolboxTray.ButtonProductsClicked += ToolboxTray_ButtonProductsClicked;
			ToolboxTray.ButtonMainMenuClicked += ToolboxTray_ButtonMainMenuClicked;
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
			MouseMode = MouseModeType.SelectEquipment;
		}

		private void ToolboxTray_ButtonSelectGeneralClicked(object sender, EventArgs e)
		{
			MouseMode = MouseModeType.SelectGeneral;
		}
		
		public void Update(GameTime gameTime)
		{
			ToolboxTray.Update(gameTime);
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			ToolboxTray.Draw(gameTime, renderer);
		}
	}
}
