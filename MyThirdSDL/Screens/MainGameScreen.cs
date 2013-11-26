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

            //for (int i = 0; i < 5; i++)
            //{
            //    int x = random.Next(0, pathNodes.Count);
            //    var pathNode = pathNodes[x];
            //    OfficeDesk officeDesk = agentFactory.CreateOfficeDesk(TimeSpan.Zero, new Vector(pathNode.WorldPosition.X, pathNode.WorldPosition.Y));
            //    simulationManager.AddAgent(officeDesk);
            //}

            //for (int i = 0; i < 3; i++)
            //{
            //    int x = random.Next(0, pathNodes.Count);
            //    var pathNode = pathNodes[x];
            //    SnackMachine snackMachine = agentFactory.CreateSnackMachine(TimeSpan.Zero, new Vector(pathNode.WorldPosition.X, pathNode.WorldPosition.Y));
            //    simulationManager.AddAgent(snackMachine);
            //}

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

        private Image redDotTexture;

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

        private MapCell hoveredMapCell;

        public override void Update(GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

            simulationManager.Update(gameTime);

            if (userInterfaceManager.MouseMode == MouseMode.SelectEquipment)
            {
                hoveredMapCell = GetHoveredMapCell();

                if (hoveredMapCell != null)
                {
                    if (MouseHelper.CurrentMouseState.ButtonsPressed != null && MouseHelper.PreviousMouseState.ButtonsPressed != null)
                    {
                        if (!MouseHelper.CurrentMouseState.ButtonsPressed.Contains(MouseButtonCode.Left)
                            && MouseHelper.PreviousMouseState.ButtonsPressed.Contains(MouseButtonCode.Left))
                        {
                            if (selectedPurchasableItem is SodaMachine)
                            {
                                var sodaMachine = agentFactory.CreateSodaMachine(simulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
                                simulationManager.AddAgent(sodaMachine);
                                hoveredMapCell.AddDrawable(sodaMachine, (int)TileType.Object);
                            }
                            else if (selectedPurchasableItem is SnackMachine)
                            {
                                var snackMachine = agentFactory.CreateSnackMachine(simulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
                                simulationManager.AddAgent(snackMachine);
                                hoveredMapCell.AddDrawable(snackMachine, (int)TileType.Object);
                            }
                            else if (selectedPurchasableItem is OfficeDesk)
                            {
                                var officeDesk = agentFactory.CreateOfficeDesk(simulationManager.SimulationTime, new Vector(hoveredMapCell.WorldPosition.X, hoveredMapCell.WorldPosition.Y));
                                simulationManager.AddAgent(officeDesk);
                                hoveredMapCell.AddDrawable(officeDesk, (int)TileType.Object);
                            }
                        }
                    }
                }
            }
            else
                hoveredMapCell = null;

            string simulationTimeText = simulationManager.SimulationTimeDisplay;
            userInterfaceManager.Update(gameTime, simulationTimeText);
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

        public override void Draw(GameTime gameTime, Renderer renderer)
        {
            base.Draw(gameTime, renderer);

            allDrawables.Clear();
            renderer.ClearScreen();

            SortDrawablesByDrawDepth();
            foreach (var drawable in allDrawables)
                drawable.Draw(gameTime, renderer);

            if (hoveredMapCell != null)
                renderer.RenderTexture(tileHighlightImage.Texture,
                    hoveredMapCell.ProjectedPosition.X - Camera.Position.X - CoordinateHelper.TileMapTileWidth * 0.5f,
                    hoveredMapCell.ProjectedPosition.Y - Camera.Position.Y - CoordinateHelper.TileMapTileHeight);

            //DrawActiveNodeCenters(renderer);

            userInterfaceManager.Draw(gameTime, renderer);

            if (userInterfaceManager.MouseMode == MouseMode.SelectEquipment)
            {
                if (hoveredMapCell != null)
                    renderer.RenderTexture(selectedPurchasableItem.Texture,
                    hoveredMapCell.ProjectedPosition.X - Camera.Position.X - CoordinateHelper.TileMapTileWidth * 0.5f,
                    hoveredMapCell.ProjectedPosition.Y - Camera.Position.Y - CoordinateHelper.TileMapTileHeight);
            }
        }

        private void DrawActiveNodeCenters(Renderer renderer)
        {
            var pathNodes = tiledMap.GetActivePathNodes();
            foreach (var pathNode in pathNodes)
            {
                var pathNodeProjectedPosition1 = CoordinateHelper.WorldSpaceToScreenSpace(pathNode.Bounds.Center.X, pathNode.Bounds.Center.Y,
                    CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);

                renderer.RenderTexture(redDotTexture.Texture, pathNodeProjectedPosition1.X, pathNodeProjectedPosition1.Y);
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
            allDrawables.AddRange(tiledMap.MapCells);

            foreach (var trackedAgent in simulationManager.TrackedAgents)
                if (!allDrawables.Any(d => d.ID == trackedAgent.ID))
                    allDrawables.Add(trackedAgent);

            allDrawables.Sort((d1, d2) => d1.Depth.CompareTo(d2.Depth));
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

