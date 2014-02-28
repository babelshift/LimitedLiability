using System;
using System.Collections.Generic;
using System.Linq;
using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using SharpDL.Input;
using MyThirdSDL.Agents;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using MyThirdSDL.Mail;
using MyThirdSDL.Simulation;
using MyThirdSDL.UserInterface;

namespace MyThirdSDL.Screens
{
	public class MainGameScreen : Screen
	{
		#region Members

		private readonly JobFactory jobFactory;
		private readonly AgentFactory agentFactory;
		private readonly RoomFactory roomFactory;
		private readonly SimulationManager simulationManager;
		private UserInterfaceManager userInterfaceManager;
		private MailManager mailManager;
		private BankAccount bankAccount;
		private TiledMap tiledMap;
		private readonly string mapPathToLoad;
		private IReadOnlyList<MapCell> hoveredMapCells;
		private BusinessManager business;

		#endregion Members

		#region Properties

		private bool IsUserInterfaceStateChangeDelayPassed { get { return userInterfaceManager.TimeSpentInCurrentState > TimeSpan.FromSeconds(0.1); } }

		#endregion Properties

		#region Public Events

		public event EventHandler ReturnToMainMenu;

		#endregion Public Events

		#region Constructor

		public MainGameScreen(Renderer renderer, ContentManager contentManager, string mapPathToLoad)
			: base(contentManager)
		{
			if (renderer == null)
				throw new ArgumentNullException("renderer");
			if (contentManager == null)
				throw new ArgumentNullException("contentManager");
			if (String.IsNullOrEmpty(mapPathToLoad))
				throw new ArgumentNullException("mapPathToLoad");

			simulationManager = new SimulationManager(DateTime.Now, contentManager.ThoughtPool);
			jobFactory = new JobFactory(contentManager);
			agentFactory = new AgentFactory(renderer, contentManager, jobFactory);
			roomFactory = new RoomFactory(renderer, contentManager);

			simulationManager.HadThought += HandleEmployeeHadThought;
			simulationManager.EmployeeThirstSatisfied += HandleEmployeeThirstSatisfied;
			simulationManager.EmployeeHungerSatisfied += HandleEmployeeHungerSatisfied;

			this.mapPathToLoad = mapPathToLoad;
		}

		#endregion Constructor

		#region User Input

		public override void HandleMouseButtonReleasedEvent(object sender, MouseButtonEventArgs e)
		{
			userInterfaceManager.HandleMouseButtonReleasedEvent(sender, e);
		}

		public override void HandleTextInputtingEvent(object sender, TextInputEventArgs e)
		{
			base.HandleTextInputtingEvent(sender, e);

			userInterfaceManager.HandleTextInputtingEvent(sender, e);
		}

		public override void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			// if the equipment being updated is an employee and that equipment is being clicked on by the user, fire the event telling subscribers of such
			// we can use this event to react to the user interacting with the employees to do things like display their inspection information
			foreach (var employee in simulationManager.TrackedEmployees)
			{
				if (IsAgentClicked(employee, e))
				{
					if (userInterfaceManager.CurrentState == UserInterfaceState.Default)
					{
						userInterfaceManager.SetEmployeeBeingInspected(employee);
					}
				}
			}

			foreach (var equipment in simulationManager.TrackedEquipment)
				if (IsAgentClicked(equipment, e))
				if (userInterfaceManager.CurrentState == UserInterfaceState.Default)
					userInterfaceManager.SetEquipmentBeingInspected(equipment);

			userInterfaceManager.HandleMouseButtonPressedEvent(sender, e);
		}

		/// <summary>
		/// Determines whether this employee is clicked based on the passed mouse state by translating the screen coordinates to world space and checking the equipment's collision box.
		/// </summary>
		/// <returns><c>true</c> if this the passed employee is clicked based on the passed mouse state; otherwise, <c>false</c>.</returns>
		/// <param name="agent"></param>
		private static bool IsAgentClicked<T>(T agent, MouseButtonEventArgs e)
					where T : Agent
		{
			if (agent == null)
				throw new ArgumentNullException("agent");

			if (e.MouseButton == MouseButtonCode.Left)
			{
				Vector worldPositionAtMousePosition = CoordinateHelper.ScreenSpaceToWorldSpace(
					                                      e.RelativeToWindowX, e.RelativeToWindowY,
					                                      CoordinateHelper.ScreenOffset,
					                                      CoordinateHelper.ScreenProjectionType.Orthogonal);

				return agent.CollisionBox.Contains(new Point((int)worldPositionAtMousePosition.X, (int)worldPositionAtMousePosition.Y));
			}

			return false;
		}

		public override void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			// get the map cell that the user's mouse is hovering over
			if (userInterfaceManager.CurrentState == UserInterfaceState.PlaceEquipmentActive || userInterfaceManager.CurrentState == UserInterfaceState.PlaceRoomActive)
			{
				IPurchasable selectedPurchasable = userInterfaceManager.SelectedPurchasableItem;
				hoveredMapCells = GetHoveredMapCellAndNeighbors(e.RelativeToWindowX, e.RelativeToWindowY, selectedPurchasable.HorizontalMapCellCount, selectedPurchasable.VerticalMapCellCount);
				userInterfaceManager.SetHoveredMapCells(hoveredMapCells);
			}

			userInterfaceManager.HandleMouseMovingEvent(sender, e);
		}

		public override void HandleKeyStates(IEnumerable<KeyInformation> keysPressed, IEnumerable<KeyInformation> keysReleased)
		{
			base.HandleKeyStates(keysPressed, keysReleased);

			foreach (var keyPressed in keysPressed)
				if (keyPressed.VirtualKey == VirtualKeyCode.Escape)
					ScreenManager.AddScreen(CreatePauseMenuScreen());

			userInterfaceManager.HandleKeyStates(keysPressed, keysReleased);
		}

		public override void HandleInput(GameTime gameTime, bool isMouseInsideWindowBounds)
		{
			base.HandleInput(gameTime, isMouseInsideWindowBounds);

			// we only want to scroll the screen when the user isn't hovering the toolbox tray
			if (!userInterfaceManager.IsToolboxTrayHovered)
			{
				var mouseOverScreenEdges = GetMouseOverScreenEdges();
				//cursor.Update(isMouseInsideWindowBounds, mouseOverScreenEdges);
				Camera.Update(mouseOverScreenEdges);
			}
		}

		private void UserInterfaceManagerOnPurchasableItemPlaced(object sender, PurchasableItemPlacedEventArgs e)
		{
			// place equipment active only if we have been in this state for more than a second to prevent super fast action being taken
			if (!IsUserInterfaceStateChangeDelayPassed)
				return;

			if (e.PurchasableItem is SodaMachine)
			{
				var agentToAdd = agentFactory.CreateSodaMachine(SimulationManager.SimulationTime, new Vector(e.HoveredMapCells[0].WorldPosition.X, e.HoveredMapCells[0].WorldPosition.Y));
				AddEquipmentToGame(agentToAdd, e.HoveredMapCells[0]);
			}
			else if (e.PurchasableItem is SnackMachine)
			{
				var agentToAdd = agentFactory.CreateSnackMachine(SimulationManager.SimulationTime, new Vector(e.HoveredMapCells[0].WorldPosition.X, e.HoveredMapCells[0].WorldPosition.Y));
				AddEquipmentToGame(agentToAdd, e.HoveredMapCells[0]);
			}
			else if (e.PurchasableItem is OfficeDesk)
			{
				var agentToAdd = agentFactory.CreateOfficeDesk(SimulationManager.SimulationTime, new Vector(e.HoveredMapCells[0].WorldPosition.X, e.HoveredMapCells[0].WorldPosition.Y));
				AddEquipmentToGame(agentToAdd, e.HoveredMapCells[0]);
			}
			else if (e.PurchasableItem is Library)
			{
				var agentToAdd = roomFactory.CreateLibrary(agentFactory);
				AddRoomToGame(agentToAdd, e.HoveredMapCells);
			}
		}

		#endregion User Input

		#region Screen Game Loop

		public override void Activate(Renderer renderer)
		{
			if (renderer == null)
				throw new ArgumentNullException("renderer");

			Point bottomRightPointOfScreen = new Point(MainGame.SCREEN_WIDTH_LOGICAL, MainGame.SCREEN_HEIGHT_LOGICAL);

			string mapPath = ContentManager.GetContentPath(mapPathToLoad);
			tiledMap = new TiledMap(mapPath, renderer, agentFactory);

			simulationManager.SetCurrentMap(tiledMap);

			bankAccount = new BankAccount(1000);
			bankAccount.AmountDeposited += bankAccount_AmountDeposited;
			bankAccount.AmountWithdrawn += bankAccount_AmountWithdrawn;

			mailManager = new MailManager();
			mailManager.UnreadMailCountChanged += mailbox_UnreadMailCountChanged;

			Employee employee = agentFactory.CreateEmployee(SimulationManager.SimulationTime, simulationManager.WorldDateTime, GetRandomEmployeePosition());
			Resume resume = new Resume(r => userInterfaceManager.ShowMenuResume(r), employee, "At my past job, I spent a lot of time sleeping at my desk. I promise not to do that if you hire me. Also, I need money. That said, I can't promise that my narcolepsy is completely cured. I have a doctor's note if you need one.");
			Employee employee2 = agentFactory.CreateEmployee(SimulationManager.SimulationTime, simulationManager.WorldDateTime, GetRandomEmployeePosition());
			Resume resume2 = new Resume(r => userInterfaceManager.ShowMenuResume(r), employee2, "At my past job, I spent a lot of time sleeping at my desk. I promise not to do that if you hire me. Also, I need money. That said, I can't promise that my narcolepsy is completely cured. I have a doctor's note if you need one.");
			Employee employee3 = agentFactory.CreateEmployee(SimulationManager.SimulationTime, simulationManager.WorldDateTime, GetRandomEmployeePosition());
			Resume resume3 = new Resume(r => userInterfaceManager.ShowMenuResume(r), employee3, "At my past job, I spent a lot of time sleeping at my desk. I promise not to do that if you hire me. Also, I need money. That said, I can't promise that my narcolepsy is completely cured. I have a doctor's note if you need one.");
			
			mailManager.SendMail(new MailItem("first.last@recruiters.com", "first.last@company.com", "Test Subject 1", "Test Body 1", resume, MailState.Unread));
			mailManager.SendMail(new MailItem("first.last@recruiters.com", "first.last@company.com", "Test Subject 2", "Test Body 2", resume2, MailState.Unread));
			mailManager.SendMail(new MailItem("first.last@recruiters.com", "first.last@company.com", "Test Subject 3", "Test Body 3", resume3, MailState.Unread));
			mailManager.SendMail(new MailItem("first.last@recruiters.com", "first.last@company.com", "Test Subject 4", "Test Body 4", resume, MailState.Unread));
			mailManager.SendMail(new MailItem("first.last@recruiters.com", "first.last@company.com", "Test Subject 5", "Test Body 5", resume, MailState.Unread));

			IEnumerable<IPurchasable> purchasableEquipment = GetPurchasableEquipment();
			IEnumerable<IPurchasable> purchasableRooms = GetPurchasableRooms();

			userInterfaceManager = new UserInterfaceManager(ContentManager, bottomRightPointOfScreen,
				purchasableEquipment,
				purchasableRooms,
				mailManager.PlayerInbox,
				mailManager.PlayerOutbox,
				mailManager.PlayerArchive,
				mailManager.PlayerUnreadMailCount,
				bankAccount.Balance,
				simulationManager.TrackedEmployees.Count());

			userInterfaceManager.PurchasableItemSelected += UserInterfaceManagerOnPurchasableItemSelected;
			userInterfaceManager.PurchasableItemPlaced += UserInterfaceManagerOnPurchasableItemPlaced;
			userInterfaceManager.MailArchived += UserInterfaceManagerOnMailArchived;
			userInterfaceManager.MainMenuButtonClicked += (sender, e) => ScreenManager.AddScreen(CreatePauseMenuScreen());
			userInterfaceManager.ResumeAccepted += (sender, e) => simulationManager.AddAgent(e.Employee);
			userInterfaceManager.EmployeeFired += UserInterfaceManagerOnEmployeeFired;
			userInterfaceManager.EmployeePromoted += UserInterfaceManagerOnEmployeePromoted;
			userInterfaceManager.EmployeeDisciplined += UserInterfaceManagerOnEmployeeDisciplined;
			userInterfaceManager.EquipmentSold += HandleEquipmentSold;
			userInterfaceManager.EquipmentRepaired += HandleEquipmentRepaired;

			business = new BusinessManager();
		}

		/// <summary>
		/// Adds the room to game.
		/// </summary>
		/// <param name="room">Room.</param>
		/// <param name="hoveredMapCells">Hovered map cells.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private void AddRoomToGame<T>(T room, IReadOnlyList<MapCell> hoveredMapCells)
			where T : Room
		{
			if (room == null)
				throw new ArgumentNullException("room");
			if (hoveredMapCells == null)
				throw new ArgumentNullException("hoveredMapCells");

			foreach (var equipmentOccupant in room.EquipmentOccupants)
				simulationManager.AddAgent(equipmentOccupant);

			// for each map cell in the room that's being placed, replace whatever map cell it's hovering in the main tile map
			Vector origin = hoveredMapCells[0].WorldPosition;
			foreach (var roomMapCell in room.MapCells)
			{
				Vector offsetPosition = new Vector(roomMapCell.WorldPosition.X + origin.X, roomMapCell.WorldPosition.Y + origin.Y);
				tiledMap.ReplaceMapCellAtPosition(roomMapCell, offsetPosition);
			}

			bankAccount.Withdraw(room.Price);
		}

		/// <summary>
		/// Adds the passed equipment to the simulation by registering it with the simulation manager and adding it as a drawable object on the clicked map cell. Does nothing if
		/// the passed equipment is null.
		/// </summary>
		/// <param name="equipment">Equipment.</param>
		/// <param name="hoveredMapCell">Hovered map cell.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private void AddEquipmentToGame<T>(T equipment, MapCell hoveredMapCell)
			where T : Equipment
		{
			if (equipment == null)
				throw new ArgumentNullException("equipment");
			if (hoveredMapCell == null)
				throw new ArgumentNullException("hoveredMapCell");

			simulationManager.AddAgent(equipment);
			hoveredMapCell.OccupantEquipment = equipment;
			bankAccount.Withdraw(equipment.Price);
		}

		/// <summary>
		/// Removes the equipment from game.
		/// </summary>
		/// <param name="equipment">Equipment.</param>
		private void RemoveEquipmentFromGame(Equipment equipment)
		{
			int salePrice = equipment.Price / 2;
			bankAccount.Deposit(salePrice);
			simulationManager.RemoveAgent<Equipment>(equipment.ID);
			tiledMap.RemoveEquipment(equipment.ID);
			equipment.Dispose();
		}

		/// <summary>
		/// When the user interface manager tells us that the user has clicked to sell an equipment, get that equipment object from
		/// the simulation manager and remove it from the game.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void HandleEquipmentSold(object sender, UserInterfaceEquipmentEventArgs e)
		{
			Equipment equipment = simulationManager.GetTrackedAgent<Equipment>(e.EquipmentId);
			RemoveEquipmentFromGame(equipment);
		}

		private void HandleEquipmentRepaired(object sender, UserInterfaceEquipmentEventArgs e)
		{

		}

		private void UserInterfaceManagerOnEmployeePromoted(object sender, UserInterfaceEmployeeEventArgs e)
		{
			simulationManager.PromoteEmployee(e.EmployeeId);
		}

		private void UserInterfaceManagerOnEmployeeDisciplined(object sender, UserInterfaceEmployeeEventArgs e)
		{
			simulationManager.DemoteEmployee(e.EmployeeId);
		}

		private void UserInterfaceManagerOnEmployeeFired(object sender, UserInterfaceEmployeeEventArgs e)
		{
			simulationManager.RemoveAgent<Employee>(e.EmployeeId);
		}

		private Vector GetRandomEmployeePosition()
		{
			var pathNodes = tiledMap.GetPathNodes();
			Random random = new Random();
			var pathNode = pathNodes[random.Next(0, pathNodes.Count)];
			return new Vector(pathNode.WorldPosition.X, pathNode.WorldPosition.Y);
		}

		public override void Update(GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			// if we are covered by another screen, attempt to pause it, otherwise attempt to unpause it
			if (coveredByOtherScreen)
				simulationManager.State = SimulationState.Paused;
			else
				simulationManager.State = SimulationState.Running;

			// only update our states if we are not covered by another screen (such as a pause menu)
			if (!coveredByOtherScreen)
			{
				base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

				simulationManager.Update(gameTime);

				userInterfaceManager.Update(gameTime, simulationManager.WorldDateTime);
				userInterfaceManager.UpdateTrackedEmployeeCount(simulationManager.TrackedEmployees.Count());
			}
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			renderer.ClearScreen();

			tiledMap.Draw(gameTime, renderer);

			foreach (var agent in simulationManager.TrackedAgents)
				agent.Draw(gameTime, renderer);

			DrawActiveNodeCenters(renderer);
			DrawEmployeCollisionBoxes(renderer);

			userInterfaceManager.Draw(gameTime, renderer);
		}

		#endregion Screen Game Loop

		#region General Methods

		private void pauseMenuScreen_QuitButtonClicked(object sender, EventArgs e)
		{
			if (ReturnToMainMenu != null)
				ReturnToMainMenu(sender, e);
		}

		private PauseMenuScreen CreatePauseMenuScreen()
		{
			var pauseMenuScreen = new PauseMenuScreen(ContentManager);
			pauseMenuScreen.QuitButtonClicked += pauseMenuScreen_QuitButtonClicked;
			return pauseMenuScreen;
		}

		private void bankAccount_AmountWithdrawn(object sender, BankAccountTransactionEventArgs e)
		{
			userInterfaceManager.UpdateDisplayedBankAccountBalance(e.NewBalance);
		}

		private void bankAccount_AmountDeposited(object sender, BankAccountTransactionEventArgs e)
		{
			userInterfaceManager.UpdateDisplayedBankAccountBalance(e.NewBalance);
		}

		private IEnumerable<IPurchasable> GetPurchasableRooms()
		{
			List<IPurchasable> purchasableRooms = new List<IPurchasable> { roomFactory.CreateLibrary(agentFactory) };

			return purchasableRooms;
		}

		private IEnumerable<IPurchasable> GetPurchasableEquipment()
		{
			List<IPurchasable> purchasableItems = new List<IPurchasable>
			{
				agentFactory.CreateSnackMachine(TimeSpan.Zero),
				agentFactory.CreateSodaMachine(TimeSpan.Zero),
				agentFactory.CreateWaterFountain(TimeSpan.Zero),
				agentFactory.CreateOfficeDesk(TimeSpan.Zero),
				agentFactory.CreateTrashBin(TimeSpan.Zero),
				agentFactory.CreateSnackMachine(TimeSpan.Zero),
				agentFactory.CreateSodaMachine(TimeSpan.Zero),
				agentFactory.CreateWaterFountain(TimeSpan.Zero),
				agentFactory.CreateOfficeDesk(TimeSpan.Zero),
				agentFactory.CreateTrashBin(TimeSpan.Zero),
				agentFactory.CreateSodaMachine(TimeSpan.Zero),
				agentFactory.CreateWaterFountain(TimeSpan.Zero)
			};

			return purchasableItems;
		}

		private void SendEmployeeMessageToUserInterface(Employee employee, string messageText, SimulationMessageType messageType)
		{
			var message = SimulationMessageFactory.Create(employee.ProjectedPosition, messageText, messageType);
			userInterfaceManager.AddMessageForAgent(employee.ID, message);
		}

		private IReadOnlyList<MapCell> GetHoveredMapCellAndNeighbors(int mapCellPositionX, int mapCellPositionY, int tileCountRight, int tileCountDown)
		{
			Vector worldPositionAtMousePosition = CoordinateHelper.ScreenSpaceToWorldSpace(
				                                      mapCellPositionX, mapCellPositionY,
				                                      CoordinateHelper.ScreenOffset,
				                                      CoordinateHelper.ScreenProjectionType.Orthogonal);

			return tiledMap.GetMapCellAtWorldPositionAndNeighbors(worldPositionAtMousePosition, tileCountRight, tileCountDown);
		}

		/// <summary>
		/// Draws the employees' collision boxes based on world position. Use this to debug any pathing or collision problems.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		private void DrawEmployeCollisionBoxes(Renderer renderer)
		{
			foreach (var employee in simulationManager.TrackedEmployees)
			{
				Vector projected1 = CoordinateHelper.WorldSpaceToScreenSpace(employee.CollisionBox.X, employee.CollisionBox.Y, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);
				Vector projected2 = CoordinateHelper.WorldSpaceToScreenSpace(employee.CollisionBox.Right, employee.CollisionBox.Y, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);
				Vector projected3 = CoordinateHelper.WorldSpaceToScreenSpace(employee.CollisionBox.X, employee.CollisionBox.Bottom, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);
				Vector projected4 = CoordinateHelper.WorldSpaceToScreenSpace(employee.CollisionBox.Right, employee.CollisionBox.Bottom, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);

				projected1 -= Camera.Position;
				projected2 -= Camera.Position;
				projected3 -= Camera.Position;
				projected4 -= Camera.Position;

				renderer.SetDrawColor(8, 255, 8, 255);

				Primitive.DrawLine(renderer, (int)projected1.X, (int)projected1.Y, (int)projected2.X, (int)projected2.Y); // top left to top right
				Primitive.DrawLine(renderer, (int)projected1.X, (int)projected1.Y, (int)projected3.X, (int)projected3.Y); // top left to bottom left
				Primitive.DrawLine(renderer, (int)projected2.X, (int)projected2.Y, (int)projected4.X, (int)projected4.Y); // top right to bottom right
				Primitive.DrawLine(renderer, (int)projected3.X, (int)projected3.Y, (int)projected4.X, (int)projected4.Y); // bottom left to bottom right

				//Primitive.DrawLine(renderer, employee.CollisionBox.X, employee.CollisionBox.Y, employee.CollisionBox.Right, employee.CollisionBox.Y);
				//Primitive.DrawLine(renderer, employee.CollisionBox.X, employee.CollisionBox.Y, employee.CollisionBox.X, employee.CollisionBox.Bottom);
				//Primitive.DrawLine(renderer, employee.CollisionBox.Right, employee.CollisionBox.Y, employee.CollisionBox.Right, employee.CollisionBox.Bottom);
				//Primitive.DrawLine(renderer, employee.CollisionBox.X, employee.CollisionBox.Bottom, employee.CollisionBox.Right, employee.CollisionBox.Bottom);
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
				Vector projected1 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.X, pathNode.Bounds.Y, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);
				Vector projected2 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.Right, pathNode.Bounds.Y, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);
				Vector projected3 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.X, pathNode.Bounds.Bottom, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);
				Vector projected4 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.Right, pathNode.Bounds.Bottom, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);

				projected1 -= Camera.Position;
				projected2 -= Camera.Position;
				projected3 -= Camera.Position;
				projected4 -= Camera.Position;

				renderer.SetDrawColor(255, 8, 8, 255);

				Primitive.DrawLine(renderer, (int)projected1.X, (int)projected1.Y, (int)projected2.X, (int)projected2.Y); // top left to top right
				Primitive.DrawLine(renderer, (int)projected1.X, (int)projected1.Y, (int)projected3.X, (int)projected3.Y); // top left to bottom left
				Primitive.DrawLine(renderer, (int)projected2.X, (int)projected2.Y, (int)projected4.X, (int)projected4.Y); // top right to bottom right
				Primitive.DrawLine(renderer, (int)projected3.X, (int)projected3.Y, (int)projected4.X, (int)projected4.Y); // bottom left to bottom right

				//Primitive.DrawLine(renderer, pathNode.Bounds.X, pathNode.Bounds.Y, pathNode.Bounds.Right, pathNode.Bounds.Y);
				//Primitive.DrawLine(renderer, pathNode.Bounds.X, pathNode.Bounds.Y, pathNode.Bounds.X, pathNode.Bounds.Bottom);
				//Primitive.DrawLine(renderer, pathNode.Bounds.Right, pathNode.Bounds.Y, pathNode.Bounds.Right, pathNode.Bounds.Bottom);
				//Primitive.DrawLine(renderer, pathNode.Bounds.X, pathNode.Bounds.Bottom, pathNode.Bounds.Right, pathNode.Bounds.Bottom);
				renderer.SetDrawColor(0, 0, 0, 255);
			}
		}

		private IEnumerable<MouseOverScreenEdge> GetMouseOverScreenEdges()
		{
			List<MouseOverScreenEdge> mouseOverScreenEdges = new List<MouseOverScreenEdge>();

			if (Mouse.X <= 1 && Mouse.X >= 0)
				mouseOverScreenEdges.Add(MouseOverScreenEdge.Left);

			if (Mouse.X >= MainGame.SCREEN_WIDTH_LOGICAL - 1 && Mouse.X <= MainGame.SCREEN_WIDTH_LOGICAL)
				mouseOverScreenEdges.Add(MouseOverScreenEdge.Right);

			if (Mouse.Y <= 1 && Mouse.Y >= 0)
				mouseOverScreenEdges.Add(MouseOverScreenEdge.Top);

			if (Mouse.Y >= MainGame.SCREEN_HEIGHT_LOGICAL - 1 && Mouse.Y <= MainGame.SCREEN_HEIGHT_LOGICAL)
				mouseOverScreenEdges.Add(MouseOverScreenEdge.Bottom);

			if (mouseOverScreenEdges.Count == 0)
				mouseOverScreenEdges.Add(MouseOverScreenEdge.None);

			return mouseOverScreenEdges;
		}

		#endregion General Methods

		#region Employee Events

		private Employee GetEmployeeFromEventSender(object sender)
		{
			if (sender == null)
				throw new ArgumentNullException("sender");

			var employee = sender as Employee;
			if (employee == null)
				throw new ArgumentException("HandleEmployee handlers can only work with Employee objects!");

			return employee;
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

		private void HandleEmployeeHadThought(object sender, ThoughtEventArgs e)
		{
			var employee = GetEmployeeFromEventSender(sender);

			//if (e.Type == ThoughtType.Hungry)
			//	SendEmployeeMessageToUserInterface(employee, String.Format("{0} is hungry!", employee.FullName), SimulationMessageType.EmployeeIsHungry);
			//else if (e.Type == ThoughtType.Thirsty)
			//	SendEmployeeMessageToUserInterface(employee, String.Format("{0} is thirsty!", employee.FullName), SimulationMessageType.EmployeeIsThirsty);
			//else if (e.Type == ThoughtType.Dirty)
			//	SendEmployeeMessageToUserInterface(employee, String.Format("{0} is dirty!", employee.FullName), SimulationMessageType.EmployeeIsDirty);
			//else if (e.Type == ThoughtType.NeedsDeskAssignment)
			//	SendEmployeeMessageToUserInterface(employee, String.Format("{0} needs and office desk to work!", employee.FullName), SimulationMessageType.EmployeeNeedsDesk);
			//else if (e.Type == ThoughtType.Sleepy)
			//	SendEmployeeMessageToUserInterface(employee, String.Format("{0} is sleepy!", employee.FullName), SimulationMessageType.EmployeeIsSleepy);
			//else if (e.Type == ThoughtType.Unhappy)
			//	SendEmployeeMessageToUserInterface(employee, String.Format("{0} is unhappy!", employee.FullName), SimulationMessageType.EmployeeIsUnhappy);
			//else if (e.Type == ThoughtType.Unhealthy)
			//	SendEmployeeMessageToUserInterface(employee, String.Format("{0} is unhealthy!", employee.FullName), SimulationMessageType.EmployeeIsUnhealthy);
		}

		#endregion Employee Events

		#region UI Manager Events

		private void UserInterfaceManagerOnPurchasableItemSelected(object sender, PurchasableItemSelectedEventArgs e)
		{
			hoveredMapCells = GetHoveredMapCellAndNeighbors(Mouse.X, Mouse.Y, e.PurchasableItem.HorizontalMapCellCount, e.PurchasableItem.VerticalMapCellCount);
			userInterfaceManager.SetHoveredMapCells(hoveredMapCells);
		}

		private void UserInterfaceManagerOnMailArchived(object sender, SelectedMailItemActionEventArgs e)
		{
			mailManager.ArchiveMail(e.SelectedMailItem);
			userInterfaceManager.UpdateMenuMailBox(mailManager.PlayerInbox, mailManager.PlayerOutbox, mailManager.PlayerArchive);
		}

		private void mailbox_UnreadMailCountChanged(object sender, EventArgs e)
		{
			if (userInterfaceManager != null)
				userInterfaceManager.UpdateUnreadMailCount(mailManager.PlayerUnreadMailCount);
		}

		#endregion UI Manager Events

		#region Dispose

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			userInterfaceManager.Dispose();
			tiledMap.Dispose();
		}

		#endregion Dispose

	}
}