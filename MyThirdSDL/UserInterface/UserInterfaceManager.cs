using MyThirdSDL.Agents;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using MyThirdSDL.Mail;
using MyThirdSDL.Simulation;
using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyThirdSDL.UserInterface
{
	public class UserInterfaceManager : IDisposable
	{
		#region Members

		private Point bottomRightPointOfWindow;
		private readonly ContentManager contentManager;
		private TimeSpan timeOfStatusChange = TimeSpan.Zero;

		#endregion Members

		#region Properties

		/// <summary>
		/// Gets the mouse mode.
		/// </summary>
		/// <value>The mouse mode.</value>
		public UserInterfaceState CurrentState { get; private set; }

		public TimeSpan TimeSpentInCurrentState { get; private set; }

		#endregion Properties

		#region Diagnostic Items

		private Label labelMousePositionAbsolute;
		private Label labelMousePositionIsometric;
		private Label labelSimulationTime;
		private Label labelState;

		#endregion Diagnostic Items

		#region Message List

		private Dictionary<Guid, Dictionary<SimulationMessageType, SimulationLabel>> labelMessagesForMultipleAgents
			= new Dictionary<Guid, Dictionary<SimulationMessageType, SimulationLabel>>();

		private List<Label> labels = new List<Label>();

		#endregion Message List

		#region Controls

		private Control focusedControl;

		private Control FocusedControl
		{
			get { return focusedControl; }
			set
			{
				if (focusedControl != null)
					focusedControl.Blur();
				focusedControl = value;
			}
		}

		private ToolboxTray toolboxTray;

		private IEnumerable<IPurchasable> purchasableEquipment;
		private MenuPurchase menuPurchaseEquipment;
		private bool isMenuEquipmentOpen = false;

		private IEnumerable<IPurchasable> purchasableRooms;
		private MenuPurchase menuPurchaseRooms;
		private bool isMenuRoomsOpen = false;

		private MenuInspectEmployee menuInspectEmployee;
		private bool isMenuInspectEmployeeOpen = false;

		private bool IsMenuInspectEmployeeOpen
		{
			get { return isMenuInspectEmployeeOpen; }
			set { isMenuInspectEmployeeOpen = value; }
		}

		private MenuMailbox menuMailbox;
		private bool isMenuMailboxOpen = false;

		private MenuCompany menuCompany;
		private bool isMenuCompanyOpen = false;

		#endregion Controls

		public event EventHandler MainMenuButtonClicked;

		public event EventHandler<ArchiveEventArgs> ArchiveMailButtonClicked;

		public event EventHandler<PurchasableItemPlacedEventArgs> PurchasableItemPlaced;

		public event EventHandler PurchasableItemSelected;

		public bool IsToolboxTrayHovered
		{
			get
			{
				if (toolboxTray != null)
					return toolboxTray.IsHovered;
				return false;
			}
		}

		private void ClearMenusOpen()
		{
			HideMenuInspectEmployee();
			HideMenuEquipment();
			HideMenuMailbox();
			HideMenuRooms();
			HideMenuCompany();
		}

		public void SetHoveredMapCell(MapCell hoveredMapCell)
		{
			this.hoveredMapCell = hoveredMapCell;
		}

		private MapCell hoveredMapCell;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MyThirdSDL.UserInterface.UserInterfaceManager"/> class.
		/// </summary>
		/// <param name="renderer"></param>
		/// <param name="contentManager">Content manager.</param>
		/// <param name="bottomRightPointOfWindow">Bottom right point of window.</param>
		/// <param name="purchasableEquipment">Purchasable items.</param>
		/// <param name="purchasableRooms"></param>
		/// <param name="inbox"></param>
		/// <param name="outbox"></param>
		/// <param name="archive"></param>
		/// <param name="unreadMailCount"></param>
		/// <param name="money"></param>
		/// <param name="employeeCount"></param>
		public UserInterfaceManager(Renderer renderer, ContentManager contentManager, Point bottomRightPointOfWindow,
			IEnumerable<IPurchasable> purchasableEquipment,
			IEnumerable<IPurchasable> purchasableRooms,
			IEnumerable<MailItem> inbox,
			IEnumerable<MailItem> outbox,
			IEnumerable<MailItem> archive,
			int unreadMailCount,
			int money,
			int employeeCount)
		{
			this.bottomRightPointOfWindow = bottomRightPointOfWindow;
			this.contentManager = contentManager;

			this.purchasableEquipment = purchasableEquipment;
			this.purchasableRooms = purchasableRooms;

			Vector toolboxTrayPosition = new Vector(bottomRightPointOfWindow.X / 2 - 350, bottomRightPointOfWindow.Y - 50);
			toolboxTray = new ToolboxTray(contentManager, unreadMailCount, money);// controlFactory.CreateToolboxTray(toolboxTrayPosition, unreadMailCount, money);
			toolboxTray.Position = toolboxTrayPosition;
			toolboxTray.ButtonSelectGeneralClicked += ToolboxTray_ButtonSelectGeneralClicked;
			toolboxTray.ButtonSelectEquipmentClicked += ToolboxTray_ButtonSelectEquipmentClicked;
			toolboxTray.ButtonSelectRoomClicked += ToolboxTray_ButtonSelectRoomClicked;
			toolboxTray.ButtonFinancesClicked += ToolboxTray_ButtonFinancesClicked;
			toolboxTray.ButtonCompanyClicked += ToolboxTray_ButtonCompanyClicked;
			toolboxTray.ButtonEmployeesClicked += ToolboxTray_ButtonEmployeesClicked;
			toolboxTray.ButtonProductsClicked += ToolboxTray_ButtonProductsClicked;
			toolboxTray.ButtonMainMenuClicked += ToolboxTray_ButtonMainMenuClicked;
			toolboxTray.ButtonMailMenuClicked += ToolboxTray_ButtonMailMenuClicked;

			Color fontColor;
			int fontSizeContent;
			string fontPath = GetLabelFontDetails(out fontColor, out fontSizeContent);

			labelMousePositionAbsolute = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, ".");
			labelMousePositionAbsolute.Position = Vector.Zero;

			labelMousePositionIsometric = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, ".");
			labelMousePositionIsometric.Position = Vector.Zero;

			labelSimulationTime = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, ".");
			labelSimulationTime.Position = Vector.Zero;

			labelState = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColor, ".");
			labelState.Position = Vector.Zero;

			labels.Add(labelMousePositionAbsolute);
			labels.Add(labelMousePositionIsometric);
			labels.Add(labelSimulationTime);
			labels.Add(labelState);

			CreateMenuRooms();
			CreateMenuEquipment();
			CreateMenuInspectEmployee();
			CreateMenuMailbox(inbox, outbox, archive);
			CreateMenuCompany(employeeCount);

			ChangeState(UserInterfaceState.Default);
		}

		/// <summary>
		/// Gets the label font details.
		/// </summary>
		/// <returns>The label font details.</returns>
		/// <param name="contentManager">Content manager.</param>
		/// <param name="fontColor">Font color.</param>
		/// <param name="fontSizeContent">Font size content.</param>
		private string GetLabelFontDetails(out Color fontColor, out int fontSizeContent)
		{
			string fontPath = contentManager.GetContentPath("Arcade");
			fontColor = new Color(255, 165, 0);
			fontSizeContent = 16;
			return fontPath;
		}

		#endregion Constructors

		#region Message Events

		/// <summary>
		/// Adds the message passed message to the agent's message collection identified byt he passed agent id.
		/// </summary>
		/// <param name="agentId">Agent identifier.</param>
		/// <param name="message">Message.</param>
		public void AddMessageForAgent(Guid agentId, SimulationMessage message)
		{
			// try to get any messages already added for this agent
			var labelMessagesForSingleAgent = GetMessagesForAgent(agentId);

			// if there are no messages for this agent in the collection, create a message collection for this agent
			if (!labelMessagesForMultipleAgents.ContainsKey(agentId))
				labelMessagesForMultipleAgents.Add(agentId, labelMessagesForSingleAgent);

			// if there are no messages of the passed type in the agent's message collection, add it to his collection
			if (!labelMessagesForSingleAgent.ContainsKey(message.Type))
			{
				Color fontColor;
				int fontSizeContent;
				string fontPath = GetLabelFontDetails(out fontColor, out fontSizeContent);
				//SimulationLabel labelMessage = controlFactory.CreateSimulationLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, message);
				//labelMessagesForSingleAgent.Add(message.Type, labelMessage);
			}
		}

		/// <summary>
		/// Removes a message belonging to the passed agent (by id) where the message type is equal to the passed message type. This method does nothing
		/// if the agent does not have any messages of the passed message type.
		/// </summary>
		/// <param name="agentId">Agent identifier.</param>
		/// <param name="messageType">Message type.</param>
		public void RemoveMessageForAgentByType(Guid agentId, SimulationMessageType messageType)
		{
			var labelMessagesForSingleAgent = GetMessagesForAgent(agentId);
			SimulationLabel labelToRemove;
			bool success = labelMessagesForSingleAgent.TryGetValue(messageType, out labelToRemove);
			if (success)
			{
				labelToRemove.Dispose();
				labelMessagesForSingleAgent.Remove(messageType);
			}
		}

		/// <summary>
		/// Gets the messages by agent identifier.
		/// </summary>
		/// <returns>The messages by agent identifier.</returns>
		/// <param name="agentId">Agent identifier.</param>
		private Dictionary<SimulationMessageType, SimulationLabel> GetMessagesForAgent(Guid agentId)
		{
			Dictionary<SimulationMessageType, SimulationLabel> labelMessagesForSingleAgent;
			bool success = labelMessagesForMultipleAgents.TryGetValue(agentId, out labelMessagesForSingleAgent);

			if (success)
				return labelMessagesForSingleAgent;
			else
				return new Dictionary<SimulationMessageType, SimulationLabel>();
		}

		#endregion Message Events

		#region ToolboxTray Events

		private void ToolboxTray_ButtonMailMenuClicked(object sender, EventArgs e)
		{
			if (!isMenuMailboxOpen)
			{
				ClearMenusOpen();
				ShowMenuMailbox();
			}
		}

		private void ToolboxTray_ButtonMainMenuClicked(object sender, EventArgs e)
		{
			if (MainMenuButtonClicked != null)
				MainMenuButtonClicked(sender, e);
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
			if (!isMenuCompanyOpen)
			{
				ClearMenusOpen();
				ShowMenuCompany();
			}
		}

		private void ToolboxTray_ButtonFinancesClicked(object sender, EventArgs e)
		{
			// show finances menu
		}

		private void ToolboxTray_ButtonSelectRoomClicked(object sender, EventArgs e)
		{
			if (!isMenuRoomsOpen)
			{
				ClearMenusOpen();
				ShowMenuRooms();
			}
		}

		private void ToolboxTray_ButtonSelectEquipmentClicked(object sender, EventArgs e)
		{
			if (!isMenuEquipmentOpen)
			{
				ClearMenusOpen();
				ShowMenuEquipment();
			}
		}

		private void ToolboxTray_ButtonSelectGeneralClicked(object sender, EventArgs e)
		{
			ChangeState(UserInterfaceState.Default);
		}

		#endregion ToolboxTray Events

		#region Menu Mailbox Events

		public void UpdateUnreadMailCount(int unreadMailCount)
		{
			toolboxTray.UpdateDisplayedUnreadMailCount(unreadMailCount);
		}

		public void UpdateMenuMailBox(IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			menuMailbox.ClearButtonsAndSeparators();
			menuMailbox.AddButtonMailItems(contentManager, inbox, outbox, archive);
		}

		public void UpdateDisplayedBankAccountBalance(int balance)
		{
			toolboxTray.UpdateDisplayedBankAccountBalance(balance);
		}

		private void ShowMenuMailbox()
		{
			isMenuMailboxOpen = true;
			ChangeState(UserInterfaceState.MailboxMenuActive);
		}

		private void HideMenuMailbox()
		{
			isMenuMailboxOpen = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void CreateMenuMailbox(IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			menuMailbox = new MenuMailbox(contentManager, inbox, outbox, archive); //controlFactory.CreateMenuMailbox(menuPosition, inbox, outbox, archive);
			menuMailbox.Position = new Vector(bottomRightPointOfWindow.X / 2 - menuMailbox.Width / 2, bottomRightPointOfWindow.Y / 2 - menuMailbox.Height / 2);
			menuMailbox.ArchiveMailButtonClicked += menuMailbox_ArchiveMailButtonClicked;
			menuMailbox.CloseButtonClicked += menuMailbox_CloseButtonClicked;
		}

		private void menuMailbox_CloseButtonClicked(object sender, EventArgs e)
		{
			HideMenuMailbox();
		}

		private void menuMailbox_ArchiveMailButtonClicked(object sender, ArchiveEventArgs e)
		{
			if (ArchiveMailButtonClicked != null)
				ArchiveMailButtonClicked(sender, e);
		}

		#endregion Menu Mailbox Events

		#region Menu Inspect Employee Events

		private void CreateMenuInspectEmployee()
		{
			menuInspectEmployee = new MenuInspectEmployee(contentManager);
			menuInspectEmployee.Position = new Vector(bottomRightPointOfWindow.X / 2 - menuInspectEmployee.Width / 2, bottomRightPointOfWindow.Y / 2 - menuInspectEmployee.Height / 2);
			menuInspectEmployee.ButtonCloseWindowClicked += menuInspectEmployee_ButtonCloseWindowClicked;
		}

		public void SetEmployeeBeingInspected(Employee employee)
		{
			ClearMenusOpen();
			ShowMenuInspectEmployee();
			menuInspectEmployee.SetInfoValues(employee);
			menuInspectEmployee.SetNeedsValues(employee.Necessities);
			menuInspectEmployee.SetSkillsValues(employee.Skills);
		}

		private void ShowMenuInspectEmployee()
		{
			IsMenuInspectEmployeeOpen = true;
			ChangeState(UserInterfaceState.InspectEmployeeMenuActive);
		}

		private void HideMenuInspectEmployee()
		{
			IsMenuInspectEmployeeOpen = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void menuInspectEmployee_ButtonCloseWindowClicked(object sender, EventArgs e)
		{
			HideMenuInspectEmployee();
		}

		#endregion Menu Inspect Employee Events

		#region Menu Rooms Events

		private void CreateMenuRooms()
		{
			menuPurchaseRooms = new MenuPurchase(contentManager, "IconForklift", "Rooms", purchasableRooms); // controlFactory.CreateMenuPurchase(menuPosition, "IconForklift", "Rooms", purchasableRooms);
			menuPurchaseRooms.Position = new Vector(bottomRightPointOfWindow.X / 2 - menuPurchaseRooms.Width / 2, bottomRightPointOfWindow.Y / 2 - menuPurchaseRooms.Height / 2);
			menuPurchaseRooms.ButtonCloseWindowClicked += menuPurchaseRooms_ButtonCloseWindowClicked;
			menuPurchaseRooms.ButtonConfirmWindowClicked += menuPurchaseRooms_ButtonConfirmWindowClicked;
		}

		private void ShowMenuRooms()
		{
			isMenuRoomsOpen = true;
			ChangeState(UserInterfaceState.SelectRoomMenuActive);
		}

		private void HideMenuRooms()
		{
			isMenuRoomsOpen = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void menuPurchaseRooms_ButtonConfirmWindowClicked(object sender, ButtonConfirmWindowClickedEventArgs e)
		{
			if (PurchasableItemPlaced != null)
				PurchasableItemPlaced(sender, new PurchasableItemPlacedEventArgs(e.PurchasableItem, hoveredMapCell));

			HideMenuRooms();

			ChangeState(UserInterfaceState.PlaceRoomActive);
		}

		private void menuPurchaseRooms_ButtonCloseWindowClicked(object sender, EventArgs e)
		{
			HideMenuRooms();
		}

		#endregion Menu Rooms Events

		#region Menu Equipment Events

		private void CreateMenuEquipment()
		{
			menuPurchaseEquipment = new MenuPurchase(contentManager, "IconHandTruck", "Equipment", purchasableEquipment); // controlFactory.CreateMenuPurchase(menuPosition, "IconHandTruck", "Equipment", purchasableEquipment);
			menuPurchaseEquipment.Position = new Vector(bottomRightPointOfWindow.X / 2 - menuPurchaseEquipment.Width / 2, bottomRightPointOfWindow.Y / 2 - menuPurchaseEquipment.Height / 2);
			menuPurchaseEquipment.ButtonCloseWindowClicked += menuEquipment_ButtonCloseWindowClicked;
			menuPurchaseEquipment.ButtonConfirmWindowClicked += menuEquipment_PurchasableItemSelected;
		}

		private void ShowMenuEquipment()
		{
			isMenuEquipmentOpen = true;
			ChangeState(UserInterfaceState.SelectEquipmentMenuActive);
		}

		private void HideMenuEquipment()
		{
			isMenuEquipmentOpen = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void menuEquipment_PurchasableItemSelected(object sender, ButtonConfirmWindowClickedEventArgs e)
		{
			selectedPurchasableItem = e.PurchasableItem;

			HideMenuEquipment();

			ChangeState(UserInterfaceState.PlaceEquipmentActive);

			if (PurchasableItemSelected != null)
				PurchasableItemSelected(this, EventArgs.Empty);
		}

		private void menuEquipment_ButtonCloseWindowClicked(object sender, EventArgs e)
		{
			HideMenuEquipment();
		}

		#endregion Menu Equipment Events

		#region Menu Company

		public void UpdateTrackedEmployeeCount(int trackedEmployeeCount)
		{
			menuCompany.UpdateEmployeeCount(trackedEmployeeCount);
		}

		private void CreateMenuCompany(int employeeCount)
		{
			menuCompany = new MenuCompany(contentManager, "Skiles Inc.", employeeCount, 10, 15, "Energy", 500000);
			menuCompany.Position = new Vector(bottomRightPointOfWindow.X / 2 - menuCompany.Width / 2, bottomRightPointOfWindow.Y / 2 - menuCompany.Height / 2);
			menuCompany.CloseButtonClicked += menuCompany_CloseButtonClicked;
		}

		private void ShowMenuCompany()
		{
			isMenuCompanyOpen = true;
			ChangeState(UserInterfaceState.CompanyMenuActive);
		}

		private void HideMenuCompany()
		{
			isMenuCompanyOpen = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void menuCompany_CloseButtonClicked(object sender, EventArgs e)
		{
			HideMenuCompany();
		}

		#endregion Menu Company

		#region Game Loop

		public void Update(GameTime gameTime, DateTime worldDateTime)
		{
			string simulationTimeDisplay = String.Format("{0} minutes, {1} seconds, {2} milliseconds",
				SimulationManager.SimulationTime.Minutes.ToString(), SimulationManager.SimulationTime.Seconds.ToString(), SimulationManager.SimulationTime.Milliseconds.ToString());
			labelSimulationTime.Text = String.Format("Simulation Time: {0}", simulationTimeDisplay);

			toolboxTray.Update(gameTime);

			if (isMenuEquipmentOpen)
				menuPurchaseEquipment.Update(gameTime);

			if (IsMenuInspectEmployeeOpen)
				menuInspectEmployee.Update(gameTime);

			if (isMenuMailboxOpen)
				menuMailbox.Update(gameTime);

			if (isMenuRoomsOpen)
				menuPurchaseRooms.Update(gameTime);

			if (isMenuCompanyOpen)
				menuCompany.Update(gameTime);

			if (CurrentState == UserInterfaceState.PlaceEquipmentActive || CurrentState == UserInterfaceState.PlaceRoomActive)
				if (Mouse.ButtonsPressed.Contains(MouseButtonCode.Right))
					ChangeState(UserInterfaceState.Default);

			TimeSpentInCurrentState = SimulationManager.SimulationTime.Subtract(timeOfStatusChange);

			UpdateDisplayedDateAndTime(worldDateTime);
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			if (CurrentState == UserInterfaceState.PlaceEquipmentActive)
			{
				if (hoveredMapCell != null)
				{
					Vector drawPosition = CoordinateHelper.ProjectedPositionToDrawPosition(hoveredMapCell.ProjectedPosition);

					foreach (var activeTexture in selectedPurchasableItem.ActiveTextures)
						renderer.RenderTexture(activeTexture, drawPosition.X, drawPosition.Y);
				}
			}

			int i = 0;

			foreach (var label in labels)
			{
				label.Position = new Vector(0, i * 18);
				label.Draw(gameTime, renderer);
				i++;
			}

			foreach (var labelMessagesForSingleAgent in labelMessagesForMultipleAgents.Values)
			{
				foreach (var labelMessage in labelMessagesForSingleAgent.Values)
				{
					labelMessage.Position = new Vector(0, i * 18);
					labelMessage.Draw(gameTime, renderer);
					i++;
				}
			}

			toolboxTray.Draw(gameTime, renderer);

			if (isMenuEquipmentOpen)
				menuPurchaseEquipment.Draw(gameTime, renderer);

			if (IsMenuInspectEmployeeOpen)
				menuInspectEmployee.Draw(gameTime, renderer);

			if (isMenuMailboxOpen)
				menuMailbox.Draw(gameTime, renderer);

			if (isMenuRoomsOpen)
				menuPurchaseRooms.Draw(gameTime, renderer);

			if (isMenuCompanyOpen)
				menuCompany.Draw(gameTime, renderer);
		}

		private void UpdateDisplayedDateAndTime(DateTime dateTime)
		{
			if (toolboxTray != null)
				toolboxTray.UpdateDisplayedDateAndTime(dateTime);
		}

		#endregion Game Loop

		#region User Input Events

		public void HandleTextInputtingEvent(object sender, TextInputEventArgs e)
		{
			focusedControl.HandleTextInput(e.Text);
		}

		private IPurchasable selectedPurchasableItem;

		public void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			if (CurrentState == UserInterfaceState.PlaceEquipmentActive)
				if (e.MouseButton == MouseButtonCode.Left)
					if (PurchasableItemPlaced != null)
						PurchasableItemPlaced(this, new PurchasableItemPlacedEventArgs(selectedPurchasableItem, hoveredMapCell));

			toolboxTray.HandleMouseButtonPressedEvent(sender, e);

			if (isMenuEquipmentOpen)
				menuPurchaseEquipment.HandleMouseButtonPressedEvent(sender, e);

			if (IsMenuInspectEmployeeOpen)
				menuInspectEmployee.HandleMouseButtonPressedEvent(sender, e);

			if (isMenuMailboxOpen)
				menuMailbox.HandleMouseButtonPressedEvent(sender, e);

			if (isMenuRoomsOpen)
				menuPurchaseRooms.HandleMouseButtonPressedEvent(sender, e);

			if (isMenuCompanyOpen)
				menuCompany.HandleMouseButtonPressedEvent(sender, e);
		}

		public void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			var mousePositionAbsolute = new Vector(e.RelativeToWindowX, e.RelativeToWindowY);
			var mousePositionIsometric = CoordinateHelper.ScreenSpaceToWorldSpace(e.RelativeToWindowX, e.RelativeToWindowY,
											 CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);

			labelMousePositionAbsolute.Text = String.Format("Mouse Position (Absolute): ({0}, {1})", mousePositionAbsolute.X, mousePositionAbsolute.Y);
			labelMousePositionIsometric.Text = String.Format("Mouse Position (Isometric): ({0}, {1})", mousePositionIsometric.X, mousePositionIsometric.Y);

			toolboxTray.HandleMouseMovingEvent(sender, e);

			if (isMenuEquipmentOpen)
				menuPurchaseEquipment.HandleMouseMovingEvent(sender, e);

			if (IsMenuInspectEmployeeOpen)
				menuInspectEmployee.HandleMouseMovingEvent(sender, e);

			if (isMenuMailboxOpen)
				menuMailbox.HandleMouseMovingEvent(sender, e);

			if (isMenuRoomsOpen)
				menuPurchaseRooms.HandleMouseMovingEvent(sender, e);

			if (isMenuCompanyOpen)
				menuCompany.HandleMouseMovingEvent(sender, e);
		}

		public void HandleKeyStates(IEnumerable<KeyInformation> keysPressed, IEnumerable<KeyInformation> keysReleased)
		{
			foreach (var key in keysPressed)
				if (FocusedControl != null)
					FocusedControl.HandleKeyPressed(key);
		}

		private void ChangeState(UserInterfaceState state)
		{
			// TODO: maybe a global here isn't the best idea?
			timeOfStatusChange = SimulationManager.SimulationTime;
			CurrentState = state;
			labelState.Text = String.Format("UI State: {0}", state.ToString());
		}

		#endregion User Input Events

		#region Dispose

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			labelMousePositionAbsolute.Dispose();
			labelMousePositionIsometric.Dispose();
			labelSimulationTime.Dispose();
			labelState.Dispose();

			menuCompany.Dispose();
			menuInspectEmployee.Dispose();
			menuMailbox.Dispose();
			menuPurchaseEquipment.Dispose();
			menuPurchaseRooms.Dispose();
		}

		#endregion Dispose
	}
}