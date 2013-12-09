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
		private TiledMap tiledMap;
		private IPurchasable selectedPurchasableItem;
		private MapCell hoveredMapCell;
		private Cursor cursor;

		private bool IsValidMapCellHovered { get { return hoveredMapCell != null; } }

		private bool IsLeftMouseButtonClicked
		{ 
			get
			{ 
				if (MouseHelper.CurrentMouseState.ButtonsPressed != null && MouseHelper.PreviousMouseState.ButtonsPressed != null)
				{
					if (!MouseHelper.CurrentMouseState.ButtonsPressed.Contains(MouseButtonCode.Left) && MouseHelper.PreviousMouseState.ButtonsPressed.Contains(MouseButtonCode.Left))
						return true;
				}

				return false;
			} 
		}

		private bool IsUserInterfaceStateChangeDelayPassed { get { return userInterfaceManager.TimeSpentInCurrentState > TimeSpan.FromSeconds(1.0); } }

		#endregion

		#region Constructor

		public MainGameScreen(Renderer renderer, ContentManager contentManager)
			: base(contentManager)
		{
			simulationManager = new SimulationManager(DateTime.Now);
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

		public override void HandleInput(GameTime gameTime, bool isMouseInsideWindowBounds)
		{
			base.HandleInput(gameTime, isMouseInsideWindowBounds);

			// we only want to scroll the screen when the user isn't hovering the toolbox tray
			if (!userInterfaceManager.IsToolboxTrayHovered)
			{
				var mouseOverScreenEdge = GetMouseOverScreenEdge();
				cursor.Update(isMouseInsideWindowBounds, mouseOverScreenEdge);
				Camera.Update(mouseOverScreenEdge);
			}

			if (userInterfaceManager.CurrentState == UserInterfaceState.Default)
			{
				// no menu is active
				ClearHoveredMapCell();
			}
			else if (userInterfaceManager.CurrentState == UserInterfaceState.SelectEquipmentMenuActive)
			{
				// select equipment menu is active
				ClearHoveredMapCell();
			}
			else if (userInterfaceManager.CurrentState == UserInterfaceState.PlaceEquipmentActive)
			{
				HandlePlaceEquipment();
			}
		}

		private void HandlePlaceEquipment()
		{
			// get the map cell that the user's mouse is hovering over
			hoveredMapCell = GetHoveredMapCell();

			// if the user is not hovering over a valid map cell, don't take any action (this happens when the mouse is outside the bounds of the world)
			if (IsValidMapCellHovered)
			{
				// place equipment active only if we have been in this state for more than a second to prevent super fast action being taken
				if (IsUserInterfaceStateChangeDelayPassed)
				{
					// if the user has clicked or released any mouse buttons while in this mode, try to place the equipment
					if (IsLeftMouseButtonClicked)
					{
						if (selectedPurchasableItem is SodaMachine)
						{
							var agentToAdd = agentFactory.CreateSodaMachine(SimulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
							AddEquipmentToSimulationAndHoveredMapCell(agentToAdd);
						}
						else if (selectedPurchasableItem is SnackMachine)
						{
							var agentToAdd = agentFactory.CreateSnackMachine(SimulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
							AddEquipmentToSimulationAndHoveredMapCell(agentToAdd);
						}
						else if (selectedPurchasableItem is OfficeDesk)
						{
							var agentToAdd = agentFactory.CreateOfficeDesk(SimulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
							AddEquipmentToSimulationAndHoveredMapCell(agentToAdd);
						}
					}
				}
			}
		}

		private void HandleSelectEquipment(object sender, PurchasableItemSelectedEventArgs e)
		{
			selectedPurchasableItem = e.PurchasableItem;
		}

		#endregion

		public override void Activate(Renderer renderer)
		{
			string mapPath = ContentManager.GetContentPath("Office1");
			string tileHighlightTexturePath = ContentManager.GetContentPath("TileHighlight");

			tiledMap = new TiledMap(mapPath, renderer);
			simulationManager.CurrentMap = tiledMap;

			var pathNodes = tiledMap.GetPathNodes();
			Random random = new Random();
			for (int i = 0; i < 5; i++)
			{
				int x = random.Next(0, pathNodes.Count);
				var pathNode = pathNodes[x];
				Employee employee = agentFactory.CreateEmployee(SimulationManager.SimulationTime, simulationManager.WorldDateTime, new Vector(pathNode.WorldPosition.X, pathNode.WorldPosition.Y));
				simulationManager.AddAgent(employee);
			}

			Surface tileHighlightSurface = new Surface(tileHighlightTexturePath, SurfaceType.PNG);
			tileHighlightImage = new Image(renderer, tileHighlightSurface, ImageFormat.PNG);

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
			userInterfaceManager.PurchasableItemSelected += HandleSelectEquipment;

			cursor = new Cursor(ContentManager, renderer);
		}

		public override void Deactivate()
		{
			base.Deactivate();
		}

		public override void Update(GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			simulationManager.Update(gameTime);

			string simulationTimeText = simulationManager.SimulationTimeDisplay;
			userInterfaceManager.Update(gameTime, simulationTimeText);
			userInterfaceManager.UpdateDisplayedDateAndTime(simulationManager.WorldDateTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			renderer.ClearScreen();
			allDrawables.Clear();
			AddAndSortDrawablesByDrawDepth();

			foreach (var drawable in allDrawables)
				drawable.Draw(gameTime, renderer);

			if (userInterfaceManager.CurrentState == UserInterfaceState.PlaceEquipmentActive)
			{
				if (hoveredMapCell != null)
				{
					Vector drawPosition = CoordinateHelper.ProjectedPositionToDrawPosition(hoveredMapCell.ProjectedPosition);

					renderer.RenderTexture(tileHighlightImage.Texture, drawPosition.X, drawPosition.Y);

					renderer.RenderTexture(selectedPurchasableItem.Texture, drawPosition.X, drawPosition.Y);
				}
			}

			DrawActiveNodeCenters(renderer);
			DrawEmployeCollisionBoxes(renderer);

			userInterfaceManager.Draw(gameTime, renderer);

			cursor.Draw(renderer);
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
			if(userInterfaceManager.CurrentState == UserInterfaceState.Default)
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
		private void AddAndSortDrawablesByDrawDepth()
		{
			allDrawables.AddRange(tiledMap.MapCells);
			allDrawables.Sort((d1, d2) => d1.Depth.CompareTo(d2.Depth));
		}

		private MapCell GetHoveredMapCell()
		{
			int mousePositionX = MouseHelper.CurrentMouseState.X;
			int mousePositionY = MouseHelper.CurrentMouseState.Y;

			Vector worldPositionAtMousePosition = CoordinateHelper.ScreenSpaceToWorldSpace(
				                                      mousePositionX, mousePositionY,
				                                      CoordinateHelper.ScreenOffset, 
				                                      CoordinateHelper.ScreenProjectionType.Isometric);

			return tiledMap.GetMapCellAtWorldPosition(worldPositionAtMousePosition);
		}

		private void ClearHoveredMapCell()
		{
			hoveredMapCell = null;
		}

		/// <summary>
		/// Adds the passed agent to the simulation by registering it with the simulation manager and adding it as a drawable object on the clicked map cell. Does nothing if
		/// the passed agent is null.
		/// </summary>
		/// <param name="agent">Agent.</param>
		private void AddEquipmentToSimulationAndHoveredMapCell<T>(T agent)
			where T : Agent
		{
			if (agent != null)
			{
				simulationManager.AddAgent(agent);
				hoveredMapCell.AddDrawable(agent, (int)TileType.Object);
			}
		}

		/// <summary>
		/// Draws the employees' collision boxes based on world position. Use this to debug any pathing or collision problems.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		private void DrawEmployeCollisionBoxes(Renderer renderer)
		{
			foreach (var employee in simulationManager.TrackedEmployees)
			{
				Vector projected1 = CoordinateHelper.WorldSpaceToScreenSpace(employee.CollisionBox.X, employee.CollisionBox.Y, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
				Vector projected2 = CoordinateHelper.WorldSpaceToScreenSpace(employee.CollisionBox.Right, employee.CollisionBox.Y, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
				Vector projected3 = CoordinateHelper.WorldSpaceToScreenSpace(employee.CollisionBox.X, employee.CollisionBox.Bottom, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
				Vector projected4 = CoordinateHelper.WorldSpaceToScreenSpace(employee.CollisionBox.Right, employee.CollisionBox.Bottom, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);

				renderer.SetDrawColor(8, 255, 8, 255);

				Primitive.DrawLine(renderer, (int)projected1.X, (int)projected1.Y, (int)projected2.X, (int)projected2.Y); // top left to top right
				Primitive.DrawLine(renderer, (int)projected1.X, (int)projected1.Y, (int)projected3.X, (int)projected3.Y); // top left to bottom left
				Primitive.DrawLine(renderer, (int)projected2.X, (int)projected2.Y, (int)projected4.X, (int)projected4.Y); // top right to bottom right
				Primitive.DrawLine(renderer, (int)projected3.X, (int)projected3.Y, (int)projected4.X, (int)projected4.Y); // bottom left to bottom right

//				Primitive.DrawLine(renderer, employee.CollisionBox.X, employee.CollisionBox.Y, employee.CollisionBox.Right, employee.CollisionBox.Y);
//				Primitive.DrawLine(renderer, employee.CollisionBox.X, employee.CollisionBox.Y, employee.CollisionBox.X, employee.CollisionBox.Bottom);
//				Primitive.DrawLine(renderer, employee.CollisionBox.Right, employee.CollisionBox.Y, employee.CollisionBox.Right, employee.CollisionBox.Bottom);
//				Primitive.DrawLine(renderer, employee.CollisionBox.X, employee.CollisionBox.Bottom, employee.CollisionBox.Right, employee.CollisionBox.Bottom);
				renderer.SetDrawColor(0, 0, 0, 255);
			}
		}

		/// <summary>
		/// Draws the outlines of active path nodes in the map based on their world position. Use this to debug any pathing problems.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		private void DrawActiveNodeCenters(Renderer renderer)
		{
			var pathNodes = tiledMap.GetActivePathNodes();
			foreach (var pathNode in pathNodes)
			{
				Vector projected1 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.X, pathNode.Bounds.Y, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
				Vector projected2 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.Right, pathNode.Bounds.Y, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
				Vector projected3 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.X, pathNode.Bounds.Bottom, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
				Vector projected4 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.Right, pathNode.Bounds.Bottom, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);

				renderer.SetDrawColor(255, 8, 8, 255);

				Primitive.DrawLine(renderer, (int)projected1.X, (int)projected1.Y, (int)projected2.X, (int)projected2.Y); // top left to top right
				Primitive.DrawLine(renderer, (int)projected1.X, (int)projected1.Y, (int)projected3.X, (int)projected3.Y); // top left to bottom left
				Primitive.DrawLine(renderer, (int)projected2.X, (int)projected2.Y, (int)projected4.X, (int)projected4.Y); // top right to bottom right
				Primitive.DrawLine(renderer, (int)projected3.X, (int)projected3.Y, (int)projected4.X, (int)projected4.Y); // bottom left to bottom right

//				Primitive.DrawLine(renderer, pathNode.Bounds.X, pathNode.Bounds.Y, pathNode.Bounds.Right, pathNode.Bounds.Y);
//				Primitive.DrawLine(renderer, pathNode.Bounds.X, pathNode.Bounds.Y, pathNode.Bounds.X, pathNode.Bounds.Bottom);
//				Primitive.DrawLine(renderer, pathNode.Bounds.Right, pathNode.Bounds.Y, pathNode.Bounds.Right, pathNode.Bounds.Bottom);
//				Primitive.DrawLine(renderer, pathNode.Bounds.X, pathNode.Bounds.Bottom, pathNode.Bounds.Right, pathNode.Bounds.Bottom);
				renderer.SetDrawColor(0, 0, 0, 255);
			}
		}
	
		private MouseOverScreenEdge GetMouseOverScreenEdge()
		{
			MouseOverScreenEdge mouseOverScreenEdge = MouseOverScreenEdge.Unknown;
			if (MouseHelper.CurrentMouseState.X < 50 && MouseHelper.CurrentMouseState.X > 0)
				mouseOverScreenEdge = MouseOverScreenEdge.Left;
			else if (MouseHelper.CurrentMouseState.X > MainGame.SCREEN_WIDTH - 50 && MouseHelper.CurrentMouseState.X < MainGame.SCREEN_WIDTH - 1)
				mouseOverScreenEdge = MouseOverScreenEdge.Right;
			else if (MouseHelper.CurrentMouseState.Y < 50 && MouseHelper.CurrentMouseState.Y > 0)
				mouseOverScreenEdge = MouseOverScreenEdge.Top;
			else if (MouseHelper.CurrentMouseState.Y > MainGame.SCREEN_HEIGHT - 50 && MouseHelper.CurrentMouseState.Y < MainGame.SCREEN_HEIGHT - 1)
				mouseOverScreenEdge = MouseOverScreenEdge.Bottom;
			else
				mouseOverScreenEdge = MouseOverScreenEdge.None;
			return mouseOverScreenEdge;
		}
	}
}

