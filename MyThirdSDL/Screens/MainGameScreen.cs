using System;
using System.Linq;
using System.Collections.Generic;
using SharpDL;
using SharpDL.Graphics;
using MyThirdSDL.Agents;
using MyThirdSDL.Content;
using MyThirdSDL.Simulation;
using MyThirdSDL.UserInterface;
using MyThirdSDL.Descriptors;
using SharpDL.Input;
using SharpDL.Events;

namespace MyThirdSDL.Screens
{
	public class MainGameScreen : Screen
	{
		#region Members

		private JobFactory jobFactory;
		private AgentFactory agentFactory;
		private AgentManager agentManager;
		private SimulationManager simulationManager;
		private UserInterfaceManager userInterfaceManager;

		private List<IDrawable> allDrawables = new List<IDrawable>();
		private Image tileHighlightImage;
		private Image tileHighlightSelectedImage;
		private TiledMap tiledMap;
		private IPurchasable selectedPurchasableItem;
		private Image redDotTexture;

		private Point mouseClickPositionWorldGridIndex = CoordinateHelper.DefaultPoint;
		private Point mousePositionWorldGridIndex = CoordinateHelper.DefaultPoint;

		private MapCell hoveredMapCell;

		#endregion

		#region Constructor

		public MainGameScreen(Renderer renderer, ContentManager contentManager)
			: base(contentManager)
		{
			simulationManager = new SimulationManager();
			jobFactory = new JobFactory();
			agentManager = new AgentManager();
			agentFactory = new AgentFactory(renderer, agentManager, contentManager, jobFactory);

			simulationManager.EmployeeIsDirty += HandleEmployeeIsDirty;
			simulationManager.EmployeeIsHungry += HandleEmployeeIsHungry;
			simulationManager.EmployeeIsSleepy += HandleEmployeeIsSleepy;
			simulationManager.EmployeeIsThirsty += HandleEmployeeIsThirsty;
			simulationManager.EmployeeIsUnhappy += HandleEmployeeIsUnhappy;
			simulationManager.EmployeeIsUnhealthy += HandleEmployeeIsUnhealthy;
			simulationManager.EmployeeNeedsOfficeDeskAssignment += HandleEmployeeNeedsOfficeDesk;
			simulationManager.EmployeeThirstSatisfied += HandleEmployeeThirstSatisfied;
			simulationManager.EmployeeHungerSatisfied += HandleEmployeeHungerSatisfied;
			simulationManager.EmployeeClicked += HandleEmployeeClicked;
		}

		#endregion

		#region User Input

		public override void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			base.HandleMouseButtonPressedEvent(sender, e);

			userInterfaceManager.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			base.HandleMouseMovingEvent(sender, e);

			userInterfaceManager.HandleMouseMovingEvent(sender, e);
		}

		public override void HandleInput(GameTime gameTime)
		{
			base.HandleInput(gameTime);
		}


		private void HandleMouseModeSelectEquipment()
		{
			// when we are in mouse mode select equipment, the user can place equipment into the world
			if (userInterfaceManager.MouseMode == MouseMode.SelectEquipment)
			{
				// get the map cell that the user's mouse is hovering over
				hoveredMapCell = GetHoveredMapCell();

				// if the user is not hovering over a valid map cell, don't take any action (this happens when the mouse is outside the bounds of the world)
				if (hoveredMapCell != null)
				{
					// if the user has clicked or released any mouse buttons while in this mode, try to place the equipment
					if (MouseHelper.CurrentMouseState.ButtonsPressed != null && MouseHelper.PreviousMouseState.ButtonsPressed != null)
					{
						// only place the equipment when the user releases the left mouse button
						if (!MouseHelper.CurrentMouseState.ButtonsPressed.Contains(MouseButtonCode.Left)
							&& MouseHelper.PreviousMouseState.ButtonsPressed.Contains(MouseButtonCode.Left))
						{
							if (selectedPurchasableItem is SodaMachine)
							{
								var sodaMachine = agentFactory.CreateSodaMachine(simulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
								AddEquipmentToSimulationAndHoveredMapCell(sodaMachine);
							}
							else if (selectedPurchasableItem is SnackMachine)
							{
								var snackMachine = agentFactory.CreateSnackMachine(simulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
								AddEquipmentToSimulationAndHoveredMapCell(snackMachine);
							}
							else if (selectedPurchasableItem is OfficeDesk)
							{
								var officeDesk = agentFactory.CreateOfficeDesk(simulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
								AddEquipmentToSimulationAndHoveredMapCell(officeDesk);
							}
						}
					}
				}
			}
			else
				hoveredMapCell = null;
		}

		private void HandlePurchasableItemSelected(object sender, PurchasableItemSelectedEventArgs e)
		{
			selectedPurchasableItem = e.PurchasableItem;
		}

		#endregion

		public override void Activate(Renderer renderer)
		{
			string mapPath = ContentManager.GetContentPath("Office1");

			tiledMap = new TiledMap(mapPath, renderer);
			simulationManager.CurrentMap = tiledMap;

			var pathNodes = tiledMap.GetPathNodes();
			Random random = new Random();
			for (int i = 0; i < 5; i++)
			{
				int x = random.Next(0, pathNodes.Count);
				var pathNode = pathNodes[x];
				Employee employee = agentFactory.CreateEmployee(TimeSpan.Zero, new Vector(pathNode.WorldPosition.X, pathNode.WorldPosition.Y));
				simulationManager.AddAgent(employee);
			}

			string redDotTexturePath = ContentManager.GetContentPath("RedDot");
			Surface redDotSurface = new Surface(redDotTexturePath, SurfaceType.PNG);
			redDotTexture = new Image(renderer, redDotSurface, ImageFormat.PNG);

			string tileHighlightTexturePath = ContentManager.GetContentPath("TileHighlight3");
			string tileHighlightSelectedTexturePath = ContentManager.GetContentPath("TileHighlightSelected");

			Surface tileHighlightSurface = new Surface(tileHighlightTexturePath, SurfaceType.PNG);
			tileHighlightImage = new Image(renderer, tileHighlightSurface, ImageFormat.PNG);

			Surface tileHightlightSelectedSurface = new Surface(tileHighlightSelectedTexturePath, SurfaceType.PNG);
			tileHighlightSelectedImage = new Image(renderer, tileHightlightSelectedSurface, ImageFormat.PNG);

			List<IPurchasable> purchasableItems = new List<IPurchasable>();
			purchasableItems.Add(agentFactory.CreateSnackMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateSodaMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateWaterFountain(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateOfficeDesk(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateTrashBin(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateSnackMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateSodaMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateWaterFountain(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateOfficeDesk(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateTrashBin(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateSodaMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateWaterFountain(TimeSpan.Zero));
			userInterfaceManager = new UserInterfaceManager(renderer, ContentManager, new Point(MainGame.SCREEN_WIDTH, MainGame.SCREEN_HEIGHT), purchasableItems);
			userInterfaceManager.PurchasableItemSelected += HandlePurchasableItemSelected;
		}

		public override void Deactivate()
		{
			base.Deactivate();
		}

		public override void Update(GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			simulationManager.Update(gameTime);

			HandleMouseModeSelectEquipment();

			string simulationTimeText = simulationManager.SimulationTimeDisplay;
			userInterfaceManager.Update(gameTime, simulationTimeText);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			renderer.ClearScreen();

			allDrawables.Clear();
			SortDrawablesByDrawDepth();

			foreach (var drawable in allDrawables)
				drawable.Draw(gameTime, renderer);

			if (hoveredMapCell != null)
			{
				Vector drawPosition = CoordinateHelper.ProjectedPositionToDrawPosition(hoveredMapCell.ProjectedPosition);

				renderer.RenderTexture(tileHighlightImage.Texture, drawPosition.X, drawPosition.Y);

				if (userInterfaceManager.MouseMode == MouseMode.SelectEquipment)
				{
					renderer.RenderTexture(selectedPurchasableItem.Texture, drawPosition.X, drawPosition.Y);
				}
			}

			//DrawActiveNodeCenters(renderer);

			userInterfaceManager.Draw(gameTime, renderer);
		}

		public override void Unload()
		{
			base.Unload();
		}

		#region Employee Events

		private Employee GetEmployeeFromEventSender(object sender)
		{
			var employee = sender as Employee;
			if (employee == null)
				throw new ArgumentException("HandleEmployee handlers can only work with Employee objects!");
			return employee;
		}

		private void HandleEmployeeClicked(object sender, EmployeeClickedEventArgs e)
		{
			userInterfaceManager.SetEmployeeBeingInspected(e.Employee);
		}

		private void HandleEmployeeHungerSatisfied(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			userInterfaceManager.RemoveMessageForAgentByType(employee.ID, SimulationMessageType.EmployeeIsHungry);
		}

		private void HandleEmployeeThirstSatisfied(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			userInterfaceManager.RemoveMessageForAgentByType(employee.ID, SimulationMessageType.EmployeeIsThirsty);
		}

		private void SendEmployeeMessageToUserInterface(Employee employee, string messageText, SimulationMessageType messageType)
		{
			var message = SimulationMessageFactory.Create(employee.ProjectedPosition, messageText, messageType);
			userInterfaceManager.AddMessageForAgent(employee.ID, message);
		}

		private void HandleEmployeeIsUnhappy(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			SendEmployeeMessageToUserInterface(employee, String.Format("{0} is unhappy!", employee.FullName), SimulationMessageType.EmployeeIsUnhappy);
		}

		private void HandleEmployeeNeedsOfficeDesk(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			//SendEmployeeMessageToUserInterface(employee, String.Format("{0} needs and office desk to work!", employee.FullName), SimulationMessageType.EmployeeNeedsDesk);
		}

		private void HandleEmployeeIsUnhealthy(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			SendEmployeeMessageToUserInterface(employee, String.Format("{0} is unhealthy!", employee.FullName), SimulationMessageType.EmployeeIsUnhealthy);
		}

		private void HandleEmployeeIsSleepy(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			SendEmployeeMessageToUserInterface(employee, String.Format("{0} is sleepy!", employee.FullName), SimulationMessageType.EmployeeIsSleepy);
		}

		private void HandleEmployeeIsThirsty(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			//SendEmployeeMessageToUserInterface(employee, String.Format("{0} is thirsty!", employee.FullName), SimulationMessageType.EmployeeIsThirsty);
		}

		private void HandleEmployeeIsHungry(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			//SendEmployeeMessageToUserInterface(employee, String.Format("{0} is hungry!", employee.FullName), SimulationMessageType.EmployeeIsHungry);
		}

		private void HandleEmployeeIsDirty(object sender, EventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);
			SendEmployeeMessageToUserInterface(employee, String.Format("{0} is dirty!", employee.FullName), SimulationMessageType.EmployeeIsDirty);
		}

		#endregion

		/// <summary>
		/// Selects out the non-empty height tiles from the height layer in the tile map and sorts them by their draw depth.
		/// </summary>
		/// 
		private void SortDrawablesByDrawDepth()
		{
			allDrawables.AddRange(tiledMap.MapCells);

			allDrawables.AddRange(simulationManager.TrackedEmployees);

			allDrawables.Sort((d1, d2) => d1.Depth.CompareTo(d2.Depth));
		}

		private MapCell GetHoveredMapCell()
		{
			int mousePositionX = MouseHelper.CurrentMouseState.X;
			int mousePositionY = MouseHelper.CurrentMouseState.Y;

			Vector worldPositionAtMousePosition = CoordinateHelper.ScreenSpaceToWorldSpace(
				mousePositionX, mousePositionY,
				CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);

			return tiledMap.GetMapCellAtWorldPosition(worldPositionAtMousePosition);
		}

		private void AddEquipmentToSimulationAndHoveredMapCell<T>(T agent)
			where T : Agent
		{
			simulationManager.AddAgent(agent);
			hoveredMapCell.AddDrawable(agent, (int)TileType.Object);
		}

		private void DrawActiveNodeCenters(Renderer renderer)
		{
			var pathNodes = tiledMap.GetActivePathNodes();
			foreach (var pathNode in pathNodes)
			{
				var pathNodeProjectedPosition1 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.Center.X, pathNode.Bounds.Center.Y,
					CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);

				renderer.RenderTexture(redDotTexture.Texture, pathNodeProjectedPosition1.X - Camera.Position.X, pathNodeProjectedPosition1.Y - Camera.Position.Y);
			}
		}

	}
}

