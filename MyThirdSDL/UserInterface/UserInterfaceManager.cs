using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MyThirdSDL.Descriptors;
using SharpDL.Events;
using SharpDL.Input;

namespace MyThirdSDL.UserInterface
{
	public enum MouseOverScreenEdge
	{
		None,
		Top,
		Bottom,
		Left,
		Right
	}

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

		#region Diagnostic Items

		private List<Label> diagnosticLabels = new List<Label>();
		private Label labelMousePositionAbsolute;
		private Label labelMousePositionIsometric;
		private Label labelSimulationTime;
		private Label labelWorldGridIndex;

		private Label labelEmployeeHealthRaw;
		private Label labelEmployeeHealthRating;

		#endregion

		public void SetEmployeeHealth(double raw, MyThirdSDL.Descriptors.Necessities.Rating rating)
		{
			labelEmployeeHealthRaw.Text = raw.ToString();
			labelEmployeeHealthRating.Text = rating.ToString();
		}

		#region Controls

		private ToolboxTray toolboxTray;
		private MenuEquipment menuEquipment;
		private IEnumerable<IPurchasable> purchasableItems;
		private bool isEquipmentMenuOpen = false;

		#endregion

		public MouseModeType MouseMode { get; private set; }

		public UserInterfaceManager(Renderer renderer, ContentManager contentManager, Point bottomRightPointOfWindow, IEnumerable<IPurchasable> purchasableItems)
		{
			this.bottomRightPointOfWindow = bottomRightPointOfWindow;
			this.purchasableItems = purchasableItems;

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

			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColor = new Color(255, 165, 0);
			int fontSizeContent = 16;

			labelMousePositionAbsolute = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelMousePositionIsometric = controlFactory.CreateLabel(Vector.Zero + new Vector(0, 18), fontPath, fontSizeContent, fontColor, ".");
			labelSimulationTime = controlFactory.CreateLabel(Vector.Zero + new Vector(0, 36), fontPath, fontSizeContent, fontColor, ".");

			labelEmployeeHealthRaw = controlFactory.CreateLabel(Vector.Zero + new Vector(0, 54), fontPath, fontSizeContent, fontColor, ".");
			labelEmployeeHealthRating = controlFactory.CreateLabel(Vector.Zero + new Vector(0, 72), fontPath, fontSizeContent, fontColor, ".");
		}

		#region ToolboxTray Events

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
					Vector menuPosition = new Vector(bottomRightPointOfWindow.X, bottomRightPointOfWindow.Y);
					menuEquipment = controlFactory.CreateMenuEquipment(menuPosition, purchasableItems);
					menuEquipment.ButtonCloseWindowClicked += menuEquipment_ButtonCloseWindowClicked;
				}

				MouseMode = MouseModeType.SelectEquipment;

				isEquipmentMenuOpen = true;
			}
		}

		#endregion

		#region Menu Equipment Events

		private void menuEquipment_ButtonCloseWindowClicked(object sender, EventArgs e)
		{
			isEquipmentMenuOpen = false;
		}

		private void ToolboxTray_ButtonSelectGeneralClicked(object sender, EventArgs e)
		{
			MouseMode = MouseModeType.SelectGeneral;
		}

		#endregion

		#region Game Loop

		public void Update(GameTime gameTime, string simulationTimeText)
		{
			labelSimulationTime.Text = String.Format("Simulation Time: {0}", simulationTimeText);

			toolboxTray.Update(gameTime);

			if (isEquipmentMenuOpen)
				menuEquipment.Update(gameTime);
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			labelMousePositionAbsolute.Draw(gameTime, renderer);
			labelMousePositionIsometric.Draw(gameTime, renderer);
			labelSimulationTime.Draw(gameTime, renderer);

			labelEmployeeHealthRating.Draw(gameTime, renderer);
			labelEmployeeHealthRaw.Draw(gameTime, renderer);

			toolboxTray.Draw(gameTime, renderer);

			if (isEquipmentMenuOpen)
				menuEquipment.Draw(gameTime, renderer);
		}

		#endregion

		#region User Input Events

		public void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{

		}

		public void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			var mousePositionAbsolute = new Vector(e.RelativeToWindowX, e.RelativeToWindowY);
			var mousePositionIsometric = CoordinateHelper.ScreenSpaceToWorldSpace(e.RelativeToWindowX, e.RelativeToWindowY, 
				                             CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);

			labelMousePositionAbsolute.Text = String.Format("Mouse Position (Absolute): ({0}, {1})", mousePositionAbsolute.X, mousePositionAbsolute.Y);
			labelMousePositionIsometric.Text = String.Format("Mouse Position (Isometric): ({0}, {1})", mousePositionIsometric.X, mousePositionIsometric.Y);
		}

		#endregion

	}
}
