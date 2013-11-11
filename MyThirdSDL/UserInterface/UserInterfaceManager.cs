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
		private ContentManager contentManager;

		#region Diagnostic Items

		private Label labelMousePositionAbsolute;
		private Label labelMousePositionIsometric;
		private Label labelSimulationTime;
		private Label labelEmployeeThirstRaw;
		private Label labelEmployeeThirstRating;
		private Label labelEmployeeHungerRaw;
		private Label labelEmployeeHungerRating;

		#endregion

		#region Message List

		private Dictionary<Guid, Dictionary<SimulationMessageType, SimulationLabel>> labelMessagesForMultipleAgents 
			= new Dictionary<Guid, Dictionary<SimulationMessageType, SimulationLabel>>();

		private List<Label> labels = new List<Label>();

		#endregion

		public void SetEmployeeThirstDisplay(double raw, MyThirdSDL.Descriptors.Necessities.Rating rating)
		{
			labelEmployeeThirstRaw.Text = raw.ToString();
			labelEmployeeThirstRating.Text = rating.ToString();
		}

		public void SetEmployeeHungerDisplay(double raw, MyThirdSDL.Descriptors.Necessities.Rating rating)
		{
			labelEmployeeHungerRaw.Text = raw.ToString();
			labelEmployeeHungerRating.Text = rating.ToString();
		}

		#region Controls

		private ToolboxTray toolboxTray;
		private MenuEquipment menuEquipment;
		private IEnumerable<IPurchasable> purchasableItems;
		private bool isEquipmentMenuOpen = false;

		#endregion

		/// <summary>
		/// Gets the mouse mode.
		/// </summary>
		/// <value>The mouse mode.</value>
		public MouseModeType MouseMode { get; private set; }

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MyThirdSDL.UserInterface.UserInterfaceManager"/> class.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		/// <param name="contentManager">Content manager.</param>
		/// <param name="bottomRightPointOfWindow">Bottom right point of window.</param>
		/// <param name="purchasableItems">Purchasable items.</param>
		public UserInterfaceManager(Renderer renderer, ContentManager contentManager, Point bottomRightPointOfWindow, IEnumerable<IPurchasable> purchasableItems)
		{
			this.bottomRightPointOfWindow = bottomRightPointOfWindow;
			this.purchasableItems = purchasableItems;
			this.contentManager = contentManager;

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

			Color fontColor;
			int fontSizeContent;
			string fontPath = GetLabelFontDetails(contentManager, out fontColor, out fontSizeContent);

			labelMousePositionAbsolute = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelMousePositionIsometric = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelSimulationTime = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelEmployeeThirstRaw = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelEmployeeThirstRating = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelEmployeeHungerRaw = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelEmployeeHungerRating = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");

			labels.Add(labelMousePositionAbsolute);
			labels.Add(labelMousePositionIsometric);
			labels.Add(labelSimulationTime);
			labels.Add(labelEmployeeThirstRaw);
			labels.Add(labelEmployeeThirstRating);
			labels.Add(labelEmployeeHungerRaw);
			labels.Add(labelEmployeeHungerRating);
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
			if(!labelMessagesForSingleAgent.ContainsKey(message.Type))
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
			else
				return new Dictionary<SimulationMessageType, SimulationLabel>();
		}

		#endregion

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
