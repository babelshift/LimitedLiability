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
using MyThirdSDL.Content;
using MyThirdSDL.Simulation;
using MyThirdSDL.Agents;
using MyThirdSDL.Mail;

namespace MyThirdSDL.UserInterface
{
	public class UserInterfaceManager
	{
		#region Members

		private Point bottomRightPointOfWindow;
		private ControlFactory controlFactory;
		private ContentManager contentManager;
		private TimeSpan timeOfStatusChange = TimeSpan.Zero;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the mouse mode.
		/// </summary>
		/// <value>The mouse mode.</value>
		public UserInterfaceState CurrentState { get; private set; }

		public TimeSpan TimeSpentInCurrentState { get; private set; }

		#endregion

		#region Diagnostic Items

		private Label labelMousePositionAbsolute;
		private Label labelMousePositionIsometric;
		private Label labelSimulationTime;
		private Label labelState;

		#endregion

		#region Message List

		private Dictionary<Guid, Dictionary<SimulationMessageType, SimulationLabel>> labelMessagesForMultipleAgents
			= new Dictionary<Guid, Dictionary<SimulationMessageType, SimulationLabel>>();
		private List<Label> labels = new List<Label>();

		#endregion

		#region Controls

		private ToolboxTray toolboxTray;
		private IEnumerable<IPurchasable> purchasableItems;
		private MenuEquipment menuEquipment;
		private bool isMenuEquipmentOpen = false;
		private MenuInspectEmployee menuInspectEmployee;
		private bool isMenuInspectEmployeeOpen = false;
		private MenuMailbox menuMailbox;
		private bool isMenuMailboxOpen = false;

		#endregion

		public bool IsToolboxTrayHovered 
		{ 
			get 
			{ 
				if(toolboxTray != null) 
					return toolboxTray.IsHovered;
				return false;
			} 
		}

		private void ClearMenusOpen()
		{
			HideMenuInspectEmployee();
			HideMenuEquipment();
			HideMenuMailbox();
		}

		#region Constructors

		public event EventHandler<PurchasableItemSelectedEventArgs> PurchasableItemSelected;

		/// <summary>
		/// Initializes a new instance of the <see cref="MyThirdSDL.UserInterface.UserInterfaceManager"/> class.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		/// <param name="contentManager">Content manager.</param>
		/// <param name="bottomRightPointOfWindow">Bottom right point of window.</param>
		/// <param name="purchasableItems">Purchasable items.</param>
		public UserInterfaceManager(Renderer renderer, ContentManager contentManager, Point bottomRightPointOfWindow, 
			IEnumerable<IPurchasable> purchasableItems,
			IEnumerable<MailItem> inbox, 
			IEnumerable<MailItem> outbox,
			IEnumerable<MailItem> archive,
			int unreadMailCount,
			int money)
		{
			this.bottomRightPointOfWindow = bottomRightPointOfWindow;
			this.purchasableItems = purchasableItems;
			this.contentManager = contentManager;

			controlFactory = new ControlFactory(renderer, contentManager);

			Vector toolboxTrayPosition = new Vector(bottomRightPointOfWindow.X / 2 - 300, bottomRightPointOfWindow.Y - 50);
			toolboxTray = controlFactory.CreateToolboxTray(toolboxTrayPosition, unreadMailCount, money);
			toolboxTray.ButtonSelectGeneralClicked += ToolboxTray_ButtonSelectGeneralClicked;
			toolboxTray.ButtonSelectEquipmentClicked += ToolboxTray_ButtonSelectEquipmentClicked;
			toolboxTray.ButtonSelectRoomClicked += ToolboxTray_ButtonSelectRoomClicked;
			toolboxTray.ButtonFinancesClicked += ToolboxTray_ButtonFinancesClicked;
			toolboxTray.ButtonCompanyClicked += ToolboxTray_ButtonCompanyClicked;
			toolboxTray.ButtonEmployeesClicked += ToolboxTray_ButtonEmployeesClicked;
			toolboxTray.ButtonProductsClicked += ToolboxTray_ButtonProductsClicked;
			toolboxTray.ButtonMainMenuClicked += ToolboxTray_ButtonMainMenuClicked;
			toolboxTray.ButtonMailMenuClicked += toolboxTray_ButtonMailMenuClicked;

			Color fontColor;
			int fontSizeContent;
			string fontPath = GetLabelFontDetails(contentManager, out fontColor, out fontSizeContent);

			labelMousePositionAbsolute = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelMousePositionIsometric = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelSimulationTime = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelState = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");

			labels.Add(labelMousePositionAbsolute);
			labels.Add(labelMousePositionIsometric);
			labels.Add(labelSimulationTime);
			labels.Add(labelState);

			CreateMenuEquipment();
			CreateMenuInspectEmployee();
			CreateMenuMailbox(inbox, outbox, archive);

			ChangeState(UserInterfaceState.Default);
		}

		/// <summary>
		/// Gets the label font details.
		/// </summary>
		/// <returns>The label font details.</returns>
		/// <param name="contentManager">Content manager.</param>
		/// <param name="fontColor">Font color.</param>
		/// <param name="fontSizeContent">Font size content.</param>
		private string GetLabelFontDetails(ContentManager contentManager, out Color fontColor, out int fontSizeContent)
		{
			string fontPath = contentManager.GetContentPath("Arcade");
			fontColor = new Color(255, 165, 0);
			fontSizeContent = 16;
			return fontPath;
		}

		#endregion

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
				string fontPath = GetLabelFontDetails(contentManager, out fontColor, out fontSizeContent);
				SimulationLabel labelMessage = controlFactory.CreateSimulationLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, message);
				labelMessagesForSingleAgent.Add(message.Type, labelMessage);
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

		#endregion

		#region ToolboxTray Events

		private void toolboxTray_ButtonMailMenuClicked(object sender, EventArgs e)
		{
			if (!isMenuMailboxOpen)
			{
				ClearMenusOpen();
				ShowMenuMailbox();
			}
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

		#endregion

		#region Menu Mailbox Events

		public void UpdateUnreadMailCount(int unreadMailCount)
		{
			toolboxTray.UpdateDisplayedUnreadMailCount(unreadMailCount);
		}

		public void UpdateMenuMailBox(IEnumerable<MailItem> inbox, IEnumerable<MailItem> outbox, IEnumerable<MailItem> archive)
		{
			menuMailbox.ClearButtonsAndSeparators();
			controlFactory.AddButtonMailItemsToMenu(menuMailbox, inbox, outbox, archive);
		}

		public event EventHandler<ArchiveEventArgs> ArchiveMailButtonClicked;

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
			Vector menuPosition = new Vector(bottomRightPointOfWindow.X, bottomRightPointOfWindow.Y);
			menuMailbox = controlFactory.CreateMenuMailbox(menuPosition, inbox, outbox, archive);
			menuMailbox.ArchiveMailButtonClicked += menuMailbox_ArchiveMailButtonClicked;
			menuMailbox.CloseButtonClicked += menuMailbox_CloseButtonClicked;
		}

		private void menuMailbox_CloseButtonClicked(object sender, EventArgs e)
		{
			HideMenuMailbox();
		}

		private void menuMailbox_ArchiveMailButtonClicked(object sender, ArchiveEventArgs e)
		{
			if(ArchiveMailButtonClicked != null)
				ArchiveMailButtonClicked(sender, e);
		}

		#endregion

		#region Menu Inspect Employee Events

		private void CreateMenuInspectEmployee()
		{
			Vector menuPosition = new Vector(bottomRightPointOfWindow.X, bottomRightPointOfWindow.Y);
			menuInspectEmployee = controlFactory.CreateMenuInspectEmployee(menuPosition);
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
			isMenuInspectEmployeeOpen = true;
			ChangeState(UserInterfaceState.InspectEmployeeMenuActive);
		}

		private void HideMenuInspectEmployee()
		{
			isMenuInspectEmployeeOpen = false;
			ChangeState(UserInterfaceState.Default);
		}

		private void menuInspectEmployee_ButtonCloseWindowClicked(object sender, EventArgs e)
		{
			HideMenuInspectEmployee();
		}

		#endregion

		#region Menu Equipment Events

		private void CreateMenuEquipment()
		{
			Vector menuPosition = new Vector(bottomRightPointOfWindow.X, bottomRightPointOfWindow.Y);
			menuEquipment = controlFactory.CreateMenuEquipment(menuPosition, purchasableItems);
			menuEquipment.ButtonCloseWindowClicked += menuEquipment_ButtonCloseWindowClicked;
			menuEquipment.ButtonConfirmWindowClicked += menuEquipment_ButtonConfirmWindowClicked;
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

		private void menuEquipment_ButtonConfirmWindowClicked(object sender, ButtonConfirmWindowClickedEventArgs e)
		{
			IPurchasable selectedPurchasableItem = e.PurchasableItem;
			if (PurchasableItemSelected != null)
				PurchasableItemSelected(sender, new PurchasableItemSelectedEventArgs(e.PurchasableItem));

			HideMenuEquipment();

			ChangeState(UserInterfaceState.PlaceEquipmentActive);
		}

		private void menuEquipment_ButtonCloseWindowClicked(object sender, EventArgs e)
		{
			HideMenuEquipment();
		}

		#endregion

		#region Game Loop

		public void Update(GameTime gameTime, DateTime worldDateTime)
		{
			string simulationTimeDisplay = String.Format("{0} minutes, {1} seconds, {2} milliseconds",
				SimulationManager.SimulationTime.Minutes.ToString(), SimulationManager.SimulationTime.Seconds.ToString(), SimulationManager.SimulationTime.Milliseconds.ToString());
			labelSimulationTime.Text = String.Format("Simulation Time: {0}", simulationTimeDisplay);

			toolboxTray.Update(gameTime);

			if (isMenuEquipmentOpen)
				menuEquipment.Update(gameTime);

			if (isMenuInspectEmployeeOpen)
				menuInspectEmployee.Update(gameTime);

			if (isMenuMailboxOpen)
				menuMailbox.Update(gameTime);

			if (CurrentState == UserInterfaceState.PlaceEquipmentActive)
				if (MouseHelper.CurrentMouseState.ButtonsPressed.Contains(MouseButtonCode.Right))
					ChangeState(UserInterfaceState.Default);

			TimeSpentInCurrentState = SimulationManager.SimulationTime.Subtract(timeOfStatusChange);

			UpdateDisplayedDateAndTime(worldDateTime);
		}

		public void Draw(GameTime gameTime, Renderer renderer)
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

			toolboxTray.Draw(gameTime, renderer);

			if (isMenuEquipmentOpen)
				menuEquipment.Draw(gameTime, renderer);

			if (isMenuInspectEmployeeOpen)
				menuInspectEmployee.Draw(gameTime, renderer);

			if (isMenuMailboxOpen)
				menuMailbox.Draw(gameTime, renderer);
		}

		private void UpdateDisplayedDateAndTime(DateTime dateTime)
		{
			if (toolboxTray != null)
				toolboxTray.UpdateDisplayedDateAndTime(dateTime);
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

		private void ChangeState(UserInterfaceState state)
		{
			// TODO: maybe a global here isn't the best idea?
			timeOfStatusChange = SimulationManager.SimulationTime;
			CurrentState = state;
			labelState.Text = String.Format("UI State: {0}", state.ToString());
		}

		#endregion
	}
}
