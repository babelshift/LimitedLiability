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

		private Point mouseClickPositionWorldGridIndex = CoordinateHelper.DefaultPoint;
		private Point mousePositionWorldGridIndex = CoordinateHelper.DefaultPoint;

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

		public override void Activate(Renderer renderer)
		{
			string mapPath = ContentManager.GetContentPath("Office2");
			string tileHighlightTexturePath = ContentManager.GetContentPath("TileHighlight2");
			string tileHighlightSelectedTexturePath = ContentManager.GetContentPath("TileHighlightSelected");

			tiledMap = new TiledMap(mapPath, renderer);
			simulationManager.CurrentMap = tiledMap;

			var pathNodes = tiledMap.GetPathNodes();
			Random random = new Random();
			for (int i = 0; i < 2; i++)
			{
				int x = random.Next(0, pathNodes.Count);
				var pathNode = pathNodes[x];
				Employee employee = agentFactory.CreateEmployee(TimeSpan.Zero, new Vector(pathNode.WorldPosition.X, pathNode.WorldPosition.Y));
				simulationManager.AddAgent(employee);
			}

			for (int i = 0; i < 2; i++)
			{
				int x = random.Next(0, pathNodes.Count);
				var pathNode = pathNodes[x];
				OfficeDesk officeDesk = agentFactory.CreateOfficeDesk(TimeSpan.Zero, new Vector(pathNode.WorldPosition.X, pathNode.WorldPosition.Y));
				simulationManager.AddAgent(officeDesk);
			}

//			var pathNode1 = tiledMap.GetPathNodeAtWorldGridIndex(new Point(3, 15));
//			var pathNode2 = tiledMap.GetPathNodeAtWorldGridIndex(new Point(15, 9));
//			var pathNode3 = tiledMap.GetPathNodeAtWorldGridIndex(new Point(20, 9));
//			var pathNode4 = tiledMap.GetPathNodeAtWorldGridIndex(new Point(4, 10));
//			var pathNode5 = tiledMap.GetPathNodeAtWorldGridIndex(new Point(3, 21));
//			var pathNode6 = tiledMap.GetPathNodeAtWorldGridIndex(new Point(4, 4));
//
//			SodaMachine sodaMachine = agentFactory.CreateSodaMachine(TimeSpan.Zero, new Vector(pathNode3.WorldPosition.X, pathNode3.WorldPosition.Y));
//			SodaMachine sodaMachine2 = agentFactory.CreateSodaMachine(TimeSpan.Zero, new Vector(pathNode4.WorldPosition.X, pathNode4.WorldPosition.Y));
//			SnackMachine snackMachine = agentFactory.CreateSnackMachine(TimeSpan.Zero, new Vector(pathNode2.WorldPosition.X, pathNode2.WorldPosition.Y));
//			SnackMachine snackMachine2 = agentFactory.CreateSnackMachine(TimeSpan.Zero, new Vector(pathNode1.WorldPosition.X, pathNode1.WorldPosition.Y));
//			OfficeDesk officeDesk = agentFactory.CreateOfficeDesk(TimeSpan.Zero, new Vector(pathNode5.WorldPosition.X, pathNode5.WorldPosition.Y));
//			OfficeDesk officeDesk2 = agentFactory.CreateOfficeDesk(TimeSpan.Zero, new Vector(pathNode6.WorldPosition.X, pathNode6.WorldPosition.Y));
//			simulationManager.AddAgents(employees);
//			simulationManager.AddAgent(sodaMachine);
//			simulationManager.AddAgent(sodaMachine2);
//			simulationManager.AddAgent(snackMachine);
//			simulationManager.AddAgent(snackMachine2);
//			simulationManager.AddAgent(officeDesk);
//			simulationManager.AddAgent(officeDesk2);

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

		public override void Update(GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			simulationManager.Update(gameTime);

			if (userInterfaceManager.MouseMode == MouseMode.SelectEquipment)
			{
				if (MouseHelper.CurrentMouseState.ButtonsPressed != null && MouseHelper.PreviousMouseState.ButtonsPressed != null)
				{
					if (!MouseHelper.CurrentMouseState.ButtonsPressed.Contains(MouseButtonCode.Left)
						&& MouseHelper.PreviousMouseState.ButtonsPressed.Contains(MouseButtonCode.Left))
					{
						if (selectedPurchasableItem is SodaMachine)
						{
							var clickedWorldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndexPoint(
								MouseHelper.ClickedWorldSpacePoint.X,
								MouseHelper.ClickedWorldSpacePoint.Y,
								CoordinateHelper.WorldGridCellWidth,
								CoordinateHelper.WorldGridCellHeight
							);

							var sodaMachine = agentFactory.CreateSodaMachine(simulationManager.SimulationTime, new Vector(clickedWorldGridIndex.X * CoordinateHelper.WorldGridCellWidth, clickedWorldGridIndex.Y * CoordinateHelper.WorldGridCellHeight));
							simulationManager.AddAgent(sodaMachine);
						}
					}
				}
			}

			string simulationTimeText = simulationManager.SimulationTimeDisplay;
			userInterfaceManager.Update(gameTime, simulationTimeText);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			allDrawables.Clear();
			renderer.ClearScreen();

//			DrawBaseTiles(gameTime, renderer);
			SortDrawablesByDrawDepth();
//			DrawHeightTiles(gameTime, renderer);
			foreach (var drawable in allDrawables)
				drawable.Draw(gameTime, renderer);

			userInterfaceManager.Draw(gameTime, renderer);

			if (userInterfaceManager.MouseMode == MouseMode.SelectEquipment)
			{
				renderer.RenderTexture(selectedPurchasableItem.Texture, 
					MouseHelper.ClickedMousePoint.X - selectedPurchasableItem.Texture.Width / 2, 
					MouseHelper.ClickedMousePoint.Y - selectedPurchasableItem.Texture.Height);
			}
		}

		public override void Unload()
		{
			base.Unload();
		}

		/// <summary>
		/// Selects out the non-empty height tiles from the height layer in the tile map and sorts them by their draw depth.
		/// </summary>
		/// 
		private void SortDrawablesByDrawDepth()
		{
			// select out the drawable tiles from our height layer (walls/objects on top of floor)
			//TileLayer heightLayer = tiledMap.TileLayers.First(tl => tl.Type == TileLayerType.Height);
			//IEnumerable<Tile> drawableTiles = heightLayer.Tiles.Where(t => !t.IsEmpty);
			//allDrawables.AddRange(drawableTiles);
			allDrawables.AddRange(tiledMap.MapCells);
			allDrawables.AddRange(simulationManager.TrackedAgents);
			// sort the drawables by their depth so they appear correctly on top of each other
			allDrawables.Sort((d1, d2) => d1.Depth.CompareTo(d2.Depth));
		}

		/// <summary>
		/// Draws the base tiles and optionally any tile highlights.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		private void DrawBaseTiles(GameTime gameTime, Renderer renderer)
		{
//			TileLayer baseLayer = tiledMap.TileLayers.First(tl => tl.Type == TileLayerType.Base);
//			IEnumerable<Tile> baseTiles = baseLayer.Tiles.Where(t => !t.IsEmpty);
//			foreach (Tile baseTile in baseTiles)
//			{
//				baseTile.Draw(gameTime, renderer);
//				if (baseTile.WorldGridIndex == mouseClickPositionWorldGridIndex)
//					DrawTileHighlight(tileHighlightSelectedImage, baseTile.ProjectedPosition - Camera.Position, renderer);
//				if (baseTile.WorldGridIndex == mousePositionWorldGridIndex)
//					DrawTileHighlight(tileHighlightImage, baseTile.ProjectedPosition - Camera.Position, renderer);
//			}
		}

		/// <summary>
		/// Selects out the non-empty height tiles from the height layer in the tile map and draws them (must be sorted by draw depth prior to drawing
		/// or items will appear rendered out of order)
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		private void DrawHeightTiles(GameTime gameTime, Renderer renderer)
		{
			foreach (IDrawable drawable in allDrawables)
			{
				drawable.Draw(gameTime, renderer);
				if (drawable is Tile)
				{
					if (drawable.WorldGridIndex == mouseClickPositionWorldGridIndex)
						DrawTileHighlight(tileHighlightSelectedImage, drawable.ProjectedPosition - Camera.Position, renderer);
					if (drawable.WorldGridIndex == mousePositionWorldGridIndex)
						DrawTileHighlight(tileHighlightImage, drawable.ProjectedPosition - Camera.Position, renderer);
				}
			}
		}

		private void DrawTileHighlight(Image image, Vector position, Renderer renderer)
		{
			renderer.RenderTexture(
				image.Texture,
				position.X - (image.Texture.Width * 0.5f),
				position.Y - (image.Texture.Height * 0.75f)
			);
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

		private void HandlePurchasableItemSelected(object sender, PurchasableItemSelectedEventArgs e)
		{
			selectedPurchasableItem = e.PurchasableItem;
		}
    }
}

