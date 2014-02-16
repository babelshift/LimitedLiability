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

namespace MyThirdSDL.UserInterface
{
	public class UserInterfaceManager : IDisposable
	{
		#region Members

		private readonly ContentManager contentManager;
		private Point bottomRightPointOfWindow;
		private TimeSpan timeOfStatusChange = TimeSpan.Zero;
		private Control focusedControl;
		private IReadOnlyList<MapCell> hoveredMapCells;

		private bool isSelectedPurchasableOverlappingDeadZone;

		#region Controls

		private MessageBox messageBox;
		private readonly ToolboxTray toolboxTray;
		private readonly TopToolboxTray topToolboxTray;
		private readonly IEnumerable<IPurchasable> purchasableEquipment;
		private readonly IEnumerable<IPurchasable> purchasableRooms;
		private MenuMailbox menuMailbox;
		private MenuCompany menuCompany;
		private MenuPurchase menuPurchaseEquipment;
		private MenuPurchase menuPurchaseRooms;
		private MenuInspectEmployee menuInspectEmployee;
		private MenuResume menuResume;

		#endregion Controls

		#endregion Members

		#region Properties

		/// <summary>
		/// Gets the mouse mode.
		/// </summary>
		/// <value>The mouse mode.</value>
		public UserInterfaceState CurrentState { get; private set; }

		/// <summary>
		/// The time spent in the current state after a state change.
		/// </summary>
		public TimeSpan TimeSpentInCurrentState { get; private set; }

		public IPurchasable SelectedPurchasableItem { get; private set; }

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

		public bool IsToolboxTrayHovered
		{
			get
			{
				if (toolboxTray != null)
					return toolboxTray.IsHovered;
				return false;
			}
		}

		private bool AnyMenusAreOpen
		{
			get
			{
				return menuCompany.Visible || menuPurchaseEquipment.Visible || menuInspectEmployee.Visible || menuMailbox.Visible || menuPurchaseRooms.Visible || menuInspectEmployee.Visible;
			}
		}

		#endregion Properties

		#region Diagnostic Items

		private Label labelMousePositionAbsolute;
		private Label labelMousePositionIsometric;
		private Label labelSimulationTime;
		private Label labelState;

		#endregion Diagnostic Items

		#region Message List

		private readonly Dictionary<Guid, Dictionary<SimulationMessageType, SimulationLabel>> labelMessagesForMultipleAgents
			= new Dictionary<Guid, Dictionary<SimulationMessageType, SimulationLabel>>();

		private List<Label> labels = new List<Label>();

		#endregion Message List

		#region Public Events

		public event EventHandler MainMenuButtonClicked;

		public event EventHandler<SelectedMailItemActionEventArgs> ArchiveMailButtonClicked;

		public event EventHandler<PurchasableItemPlacedEventArgs> PurchasableItemPlaced;

		public event EventHandler<PurchasableItemSelectedEventArgs> PurchasableItemSelected;

		public event EventHandler<ResumeAcceptedEventArgs> ResumeAccepted;

		#endregion Public Events

		#region General Methods

		private void ClearMenusOpen()
		{
			HideMenuInspectEmployee();
			HideMenuEquipment();
			HideMenuMailbox();
			HideMenuRooms();
			HideMenuCompany();
		}

		public void SetHoveredMapCells(IReadOnlyList<MapCell> hoveredMapCells)
		{
			if (hoveredMapCells == null) throw new ArgumentNullException("hoveredMapCells");

			this.hoveredMapCells = hoveredMapCells;

			// if any of these hovered map cells overlaps with selected purchasable map cells, shade the selected purchasable in red
			isSelectedPurchasableOverlappingDeadZone = SelectedPurchasableItem.IsOverlappingDeadZone(hoveredMapCells);
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

		public void UpdateDisplayedBankAccountBalance(int balance)
		{
			if (topToolboxTray != null)
				topToolboxTray.UpdateDisplayedBankAccountBalance(balance);
		}

		public void SetEmployeeBeingInspected(Employee employee)
		{
			if (employee == null) throw new ArgumentNullException("employee");

			ClearMenusOpen();
			ShowMenuInspectEmployee();
			menuInspectEmployee.SetInfoValues(employee);
			menuInspectEmployee.SetNeedsValues(employee.Necessities);
			menuInspectEmployee.SetSkillsValues(employee.Skills);
		}

		public void UpdateTrackedEmployeeCount(int trackedEmployeeCount)
		{
			menuCompany.UpdateEmployeeCount(trackedEmployeeCount);
		}

		private void UpdateDisplayedDateAndTime(DateTime dateTime)
		{
			if (topToolboxTray != null)
				topToolboxTray.UpdateDisplayedDateAndTime(dateTime);
		}

		private void ChangeState(UserInterfaceState state)
		{
			timeOfStatusChange = SimulationManager.SimulationTime;
			CurrentState = state;
			labelState.Text = String.Format("UI State: {0}", state);
		}

		#endregion General Methods

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MyThirdSDL.UserInterface.UserInterfaceManager"/> class.
		/// </summary>
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
		public UserInterfaceManager(ContentManager contentManager,
			Point bottomRightPointOfWindow,
			IEnumerable<IPurchasable> purchasableEquipment,
			IEnumerable<IPurchasable> purchasableRooms,
			IEnumerable<MailItem> inbox,
			IEnumerable<MailItem> outbox,
			IEnumerable<MailItem> archive,
			int unreadMailCount,
			int money,
			int employeeCount)
		{
			if (purchasableEquipment == null) throw new ArgumentNullException("purchasableEquipment");
			if (purchasableRooms == null) throw new ArgumentNullException("purchasableRooms");
			if (inbox == null) throw new ArgumentNullException("inbox");
			if (outbox == null) throw new ArgumentNullException("outbox");
			if (archive == null) throw new ArgumentNullException("archive");

			this.bottomRightPointOfWindow = bottomRightPointOfWindow;
			this.contentManager = contentManager;
			this.purchasableEquipment = purchasableEquipment;
			this.purchasableRooms = purchasableRooms;

			toolboxTray = new ToolboxTray(contentManager);
			toolboxTray.Position = new Vector(bottomRightPointOfWindow.X / 2 - toolboxTray.Width / 2, bottomRightPointOfWindow.Y - toolboxTray.Height);
			toolboxTray.ButtonSelectGeneralClicked += ToolboxTray_ButtonSelectGeneralClicked;
			toolboxTray.ButtonSelectEquipmentClicked += ToolboxTray_ButtonSelectEquipmentClicked;
			toolboxTray.ButtonSelectRoomClicked += ToolboxTray_ButtonSelectRoomClicked;
			toolboxTray.ButtonFinancesClicked += ToolboxTray_ButtonFinancesClicked;
			toolboxTray.ButtonCompanyClicked += ToolboxTray_ButtonCompanyClicked;
			toolboxTray.ButtonEmployeesClicked += ToolboxTray_ButtonEmployeesClicked;
			toolboxTray.ButtonProductsClicked += ToolboxTray_ButtonProductsClicked;

			topToolboxTray = new TopToolboxTray(contentManager, money);
			topToolboxTray.Position = Vector.Zero;
			topToolboxTray.ButtonMainMenuClicked += ButtonMainMenuOnClicked;
			topToolboxTray.ButtonMailMenuClicked += ButtonMailMenuOnClicked;

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
			//if (!labelMessagesForSingleAgent.ContainsKey(message.Type))
			//{
			//	Color fontColor;
			//	int fontSizeContent;
			//	string fontPath = GetLabelFontDetails(out fontColor, out fontSizeContent);
			//	//SimulationLabel labelMessage = controlFactory.CreateSimulationLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, message);
			//	//labelMessagesForSingleAgent.Add(message.Type, labelMessage);
			//}
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

			if (!success) return;

			labelToRemove.Dispose();
			labelMessagesForSingleAgent.Remove(messageType);
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

			return new Dictionary<SimulationMessageType, SimulationLabel>();
		}

		#endregion Message Events

		#region ToolboxTray Events

		private void ButtonMainMenuOnClicked(object sender, EventArgs e)
		{
			if (MainMenuButtonClicked != null)
				MainMenuButtonClicked(sender, e);
		}

		private void ButtonMailMenuOnClicked(object sender, EventArgs e)
		{
			if (menuMailbox.Visible)
				ClearMenusOpen();
			else if (AnyMenusAreOpen)
			{
				ClearMenusOpen();
				ShowMenuMailbox();
			}
			else
				ShowMenuMailbox();
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
			if (menuCompany.Visible)
				ClearMenusOpen();
			else if (AnyMenusAreOpen)
			{
				ClearMenusOpen();
				ShowMenuCompany();
			}
			else
				ShowMenuCompany();
		}

		private void ToolboxTray_ButtonFinancesClicked(object sender, EventArgs e)
		{
			// show finances menu
		}

		private void ToolboxTray_ButtonSelectRoomClicked(object sender, EventArgs e)
		{
			if (menuPurchaseRooms.Visible)
				ClearMenusOpen();
			else if (AnyMenusAreOpen)
			{
				ClearMenusOpen();
				ShowMenuRooms();
			}
			else
				ShowMenuRooms();
		}

		private void ToolboxTray_ButtonSelectEquipmentClicked(object sender, EventArgs e)
		{
			if (menuPurchaseEquipment.Visible)
				ClearMenusOpen();
			else if (AnyMenusAreOpen)
			{
				ClearMenusOpen();
				ShowMenuEquipment();
			}
			else
				ShowMenuEquipment();
		}

		private void ToolboxTray_ButtonSelectGeneralClicked(object sender, EventArgs e)
		{
			ChangeState(UserInterfaceState.Default);
		}

		#endregion ToolboxTray Events

		#region Menu Mailbox Events

		public void UpdateUnreadMailCount(int unreadMailCount)
		{
			if (topToolboxTray != null)
				topToolboxTray.UpdateDisplayedUnreadMailCount(unreadMailCount);
		}

		public void UpdateMenuMailBox(IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			menuMailbox.ClearButtonsAndSeparators();
			menuMailbox.AddButtonMailItems(contentManager, inbox, outbox, archive);
		}

		private void ShowMenuMailbox()
		{
			menuMailbox.Visible = true;
			ChangeState(UserInterfaceState.MailboxMenuActive);
		}

		private void HideMenuMailbox()
		{
			menuMailbox.Visible = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void CreateMenuMailbox(IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			menuMailbox = new MenuMailbox(contentManager, inbox, outbox, archive); //controlFactory.CreateMenuMailbox(menuPosition, inbox, outbox, archive);
			menuMailbox.Position = new Vector(bottomRightPointOfWindow.X / 2 - menuMailbox.Width / 2, bottomRightPointOfWindow.Y / 2 - menuMailbox.Height / 2);
			menuMailbox.ArchiveMailButtonClicked += MenuMailboxArchiveMailButtonClicked;
			menuMailbox.CloseButtonClicked += MenuMailboxOnCloseButtonClicked;
		}

		private void MenuMailboxOnCloseButtonClicked(object sender, EventArgs e)
		{
			HideMenuMailbox();
		}

		private void MenuMailboxArchiveMailButtonClicked(object sender, SelectedMailItemActionEventArgs e)
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

		private void ShowMenuInspectEmployee()
		{
			menuInspectEmployee.Visible = true;
			ChangeState(UserInterfaceState.InspectEmployeeMenuActive);
		}

		private void HideMenuInspectEmployee()
		{
			menuInspectEmployee.Visible = false;
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
			menuPurchaseRooms.ButtonConfirmWindowClicked += menuPurchaseRooms_PurchasableItemSelected;
		}

		private void ShowMenuRooms()
		{
			menuPurchaseRooms.Visible = true;
			ChangeState(UserInterfaceState.SelectRoomMenuActive);
		}

		private void HideMenuRooms()
		{
			menuPurchaseRooms.Visible = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void menuPurchaseRooms_PurchasableItemSelected(object sender, ButtonConfirmWindowClickedEventArgs e)
		{
			SelectedPurchasableItem = e.PurchasableItem;

			HideMenuRooms();

			ChangeState(UserInterfaceState.PlaceRoomActive);

			if (PurchasableItemSelected != null)
				PurchasableItemSelected(this, new PurchasableItemSelectedEventArgs(SelectedPurchasableItem));
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
			menuPurchaseEquipment.Visible = true;
			ChangeState(UserInterfaceState.SelectEquipmentMenuActive);
		}

		private void HideMenuEquipment()
		{
			menuPurchaseEquipment.Visible = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void menuEquipment_PurchasableItemSelected(object sender, ButtonConfirmWindowClickedEventArgs e)
		{
			SelectedPurchasableItem = e.PurchasableItem;

			HideMenuEquipment();

			ChangeState(UserInterfaceState.PlaceEquipmentActive);

			if (PurchasableItemSelected != null)
				PurchasableItemSelected(this, new PurchasableItemSelectedEventArgs(SelectedPurchasableItem));
		}

		private void menuEquipment_ButtonCloseWindowClicked(object sender, EventArgs e)
		{
			HideMenuEquipment();
		}

		#endregion Menu Equipment Events

		#region Menu Company

		private void CreateMenuCompany(int employeeCount)
		{
			menuCompany = new MenuCompany(contentManager, "Skiles Inc.", employeeCount, 10, 15, "Energy", 500000);
			menuCompany.Position = new Vector(bottomRightPointOfWindow.X / 2 - menuCompany.Width / 2, bottomRightPointOfWindow.Y / 2 - menuCompany.Height / 2);
			menuCompany.CloseButtonClicked += menuCompany_CloseButtonClicked;
		}

		private void ShowMenuCompany()
		{
			menuCompany.Visible = true;
			ChangeState(UserInterfaceState.CompanyMenuActive);
		}

		private void HideMenuCompany()
		{
			menuCompany.Visible = false;
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
				SimulationManager.SimulationTime.Minutes, SimulationManager.SimulationTime.Seconds, SimulationManager.SimulationTime.Milliseconds);
			labelSimulationTime.Text = String.Format("Simulation Time: {0}", simulationTimeDisplay);

			toolboxTray.Update(gameTime);
			topToolboxTray.Update(gameTime);

			menuPurchaseEquipment.Update(gameTime);
			menuInspectEmployee.Update(gameTime);
			menuMailbox.Update(gameTime);
			menuPurchaseRooms.Update(gameTime);
			menuCompany.Update(gameTime);

			if (messageBox != null)
				messageBox.Update(gameTime);

			if (menuResume != null)
				menuResume.Update(gameTime);

			TimeSpentInCurrentState = SimulationManager.SimulationTime.Subtract(timeOfStatusChange);

			UpdateDisplayedDateAndTime(worldDateTime);
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			DrawSelectedPurchasableItem(gameTime, renderer);

			//DrawDiagnosticLabels(gameTime, renderer);

			toolboxTray.Draw(gameTime, renderer);
			topToolboxTray.Draw(gameTime, renderer);

			DrawMenus(gameTime, renderer);

			if (menuResume != null)
				menuResume.Draw(gameTime, renderer);

			if (messageBox != null)
				messageBox.Draw(gameTime, renderer);
		}

		private void DrawMenus(GameTime gameTime, Renderer renderer)
		{
			menuPurchaseEquipment.Draw(gameTime, renderer);
			menuInspectEmployee.Draw(gameTime, renderer);
			menuMailbox.Draw(gameTime, renderer);
			menuPurchaseRooms.Draw(gameTime, renderer);
			menuCompany.Draw(gameTime, renderer);
		}

		private void DrawDiagnosticLabels(GameTime gameTime, Renderer renderer)
		{
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
		}

		private void DrawSelectedPurchasableItem(GameTime gameTime, Renderer renderer)
		{
			if (CurrentState == UserInterfaceState.PlaceEquipmentActive || CurrentState == UserInterfaceState.PlaceRoomActive)
			{
				if (hoveredMapCells != null)
				{
					if (hoveredMapCells[0] != null)
					{
						Vector drawPosition = CoordinateHelper.ProjectedPositionToDrawPosition(hoveredMapCells[0].ProjectedPosition);

						SelectedPurchasableItem.Draw(gameTime, renderer, (int)drawPosition.X, (int)drawPosition.Y, isSelectedPurchasableOverlappingDeadZone);
					}
				}
			}
		}

		#endregion Game Loop

		#region User Input Events

		public void HandleTextInputtingEvent(object sender, TextInputEventArgs e)
		{
			focusedControl.HandleTextInput(e.Text);
		}

		public void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			if (CurrentState == UserInterfaceState.PlaceEquipmentActive || CurrentState == UserInterfaceState.PlaceRoomActive)
			{
				if (e.MouseButton == MouseButtonCode.Right)
				{
					ChangeState(UserInterfaceState.Default);
				}
			}

			TryToPlacePurchasableItem(e);

			toolboxTray.HandleMouseButtonPressedEvent(sender, e);
			topToolboxTray.HandleMouseButtonPressedEvent(sender, e);

			menuPurchaseEquipment.HandleMouseButtonPressedEvent(sender, e);
			menuInspectEmployee.HandleMouseButtonPressedEvent(sender, e);
			menuMailbox.HandleMouseButtonPressedEvent(sender, e);
			menuPurchaseRooms.HandleMouseButtonPressedEvent(sender, e);
			menuCompany.HandleMouseButtonPressedEvent(sender, e);

			if (messageBox != null)
				messageBox.HandleMouseButtonPressedEvent(sender, e);

			if (menuResume != null)
				menuResume.HandleMouseButtonPressedEvent(sender, e);
		}

		public void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			UpdateMousePositionDiagnostics(e);

			toolboxTray.HandleMouseMovingEvent(sender, e);
			topToolboxTray.HandleMouseMovingEvent(sender, e);

			menuPurchaseEquipment.HandleMouseMovingEvent(sender, e);
			menuInspectEmployee.HandleMouseMovingEvent(sender, e);
			menuMailbox.HandleMouseMovingEvent(sender, e);
			menuPurchaseRooms.HandleMouseMovingEvent(sender, e);
			menuCompany.HandleMouseMovingEvent(sender, e);

			if (messageBox != null)
				messageBox.HandleMouseMovingEvent(sender, e);

			if (menuResume != null)
				menuResume.HandleMouseMovingEvent(sender, e);
		}

		private void TryToPlacePurchasableItem(MouseButtonEventArgs e)
		{
			if (CurrentState != UserInterfaceState.PlaceEquipmentActive && CurrentState != UserInterfaceState.PlaceRoomActive) return;
			if (e.MouseButton != MouseButtonCode.Left) return;
			if (isSelectedPurchasableOverlappingDeadZone) return;
			if (PurchasableItemPlaced != null)
			{
				PurchasableItemPlaced(this, new PurchasableItemPlacedEventArgs(SelectedPurchasableItem, hoveredMapCells));
				ChangeState(UserInterfaceState.Default);
			}
		}

		private void UpdateMousePositionDiagnostics(MouseMotionEventArgs e)
		{
			var mousePositionAbsolute = new Vector(e.RelativeToWindowX, e.RelativeToWindowY);
			var mousePositionIsometric = CoordinateHelper.ScreenSpaceToWorldSpace(e.RelativeToWindowX, e.RelativeToWindowY,
				CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Orthogonal);

			labelMousePositionAbsolute.Text = String.Format("Mouse Position (Absolute): ({0}, {1})", mousePositionAbsolute.X,
				mousePositionAbsolute.Y);
			labelMousePositionIsometric.Text = String.Format("Mouse Position (Isometric): ({0}, {1})", mousePositionIsometric.X,
				mousePositionIsometric.Y);
		}

		public void HandleKeyStates(IEnumerable<KeyInformation> keysPressed, IEnumerable<KeyInformation> keysReleased)
		{
			foreach (var key in keysPressed)
				if (FocusedControl != null)
					FocusedControl.HandleKeyPressed(key);
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

		/// <summary>
		/// Shows the passed resume in a window and hides the mailbox menu. This method should only be called in an Attachment action where the attachment is a resume.
		/// The resume will invoke its Open action when the user opens the attachment, causing this method to execute.
		/// </summary>
		/// <param name="resume"></param>
		public void ShowMenuResume(Resume resume)
		{
			menuResume = new MenuResume(contentManager, resume);
			menuResume.Accepted += MenuResumeOnAccepted;
			menuResume.Rejected += MenuResumeOnRejected;
			menuResume.Position = menuMailbox.Position;
			menuResume.Visible = true;
		}

		/// <summary>
		/// When the user rejects a resume, make the mailbox menu visible again and archive the mail away.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void MenuResumeOnRejected(object sender, EventArgs eventArgs)
		{
			menuMailbox.Visible = true;
			menuMailbox.ArchiveSelectedMailItem(sender);
		}

		/// <summary>
		/// When the user accepts a resume, make the mailbox menu visible again, archive the mail, and inform the game that a new employee needs to be added to the simulation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void MenuResumeOnAccepted(object sender, ResumeAcceptedEventArgs eventArgs)
		{
			menuMailbox.Visible = true;

			messageBox = ControlFactory.CreateMessageBox(contentManager, MessageBoxType.Information);
			messageBox.UpdateLabels(contentManager, "Resume Accepted!", "Your new employee will begin working in the next 7 to 10 business days.");
			messageBox.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL - messageBox.Width - 5, topToolboxTray.Height + 5);
			messageBox.Show();

			menuMailbox.ArchiveSelectedMailItem(sender);

			if (ResumeAccepted != null)
				ResumeAccepted(sender, eventArgs);
		}
	}
}