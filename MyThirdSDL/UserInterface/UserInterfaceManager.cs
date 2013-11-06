﻿using SharpDL;
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
		private Label labelEmployeeHealthRaw;
		private Label labelEmployeeHealthRating;

		#endregion

		#region Message List

		private Dictionary<Guid, List<SimulationLabel>> labelMessagesForAgents = new Dictionary<Guid, List<SimulationLabel>>();

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
			var fontPath = GetLabelFontDetails(contentManager, out fontColor, out fontSizeContent);

			labelMousePositionAbsolute = controlFactory.CreateLabel(Vector.Zero, fontPath, fontSizeContent, fontColor, ".");
			labelMousePositionIsometric = controlFactory.CreateLabel(Vector.Zero + new Vector(0, 18), fontPath, fontSizeContent, fontColor, ".");
			labelSimulationTime = controlFactory.CreateLabel(Vector.Zero + new Vector(0, 36), fontPath, fontSizeContent, fontColor, ".");

			labelEmployeeHealthRaw = controlFactory.CreateLabel(Vector.Zero + new Vector(0, 54), fontPath, fontSizeContent, fontColor, ".");
			labelEmployeeHealthRating = controlFactory.CreateLabel(Vector.Zero + new Vector(0, 72), fontPath, fontSizeContent, fontColor, ".");
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
		public void AddMessage(Guid agentId, SimulationMessage message)
		{
			Color fontColor;
			int fontSizeContent;
			var fontPath = GetLabelFontDetails(contentManager, out fontColor, out fontSizeContent);

			var labelMessagesForAgent = GetMessagesByAgentId(agentId);

			// if there isn't already a message of this type in this agent's collection, then add the message
			if (!AnyMessagesWithTypeForAgent(labelMessagesForAgent, message.Type))
			{
				int messageOffsetY = ((labelMessagesForAgent.Count() + 1) * 18) + 72;
				var labelMessage = controlFactory.CreateSimulationLabel(Vector.Zero + new Vector(0, messageOffsetY), fontPath, fontSizeContent, fontColor, message);
				labelMessagesForAgent.Add(labelMessage);
			}
		}

		/// <summary>
		/// Determines whether the passed message collection contains any messages with the passed message type.
		/// </summary>
		/// <returns><c>true</c> if the passed message collection contains any messages with the passed message type; otherwise, <c>false</c>.</returns>
		/// <param name="messages">Messages.</param>
		/// <param name="type">Type.</param>
		private bool AnyMessagesWithTypeForAgent(IEnumerable<SimulationLabel> messages, SimulationMessage.MessageType type)
		{
			if (messages.Any(m => m.SimulationMessage.Type == type))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Gets the messages by agent identifier.
		/// </summary>
		/// <returns>The messages by agent identifier.</returns>
		/// <param name="agentId">Agent identifier.</param>
		private IList<SimulationLabel> GetMessagesByAgentId(Guid agentId)
		{
			List<SimulationLabel> labelMessagesForAgent = new List<SimulationLabel>();

			// try to get existing message list for the passed agent id
			bool success = labelMessagesForAgents.TryGetValue(agentId, out labelMessagesForAgent);

			// if we don't yet have any messages for this agent, create a spot in the dictionary for this agent
			if (!success)
			{
				labelMessagesForAgent = new List<SimulationLabel>();
				labelMessagesForAgents.Add(agentId, labelMessagesForAgent);
			}

			return labelMessagesForAgent;
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
			labelMousePositionAbsolute.Draw(gameTime, renderer);
			labelMousePositionIsometric.Draw(gameTime, renderer);
			labelSimulationTime.Draw(gameTime, renderer);
			labelEmployeeHealthRating.Draw(gameTime, renderer);
			labelEmployeeHealthRaw.Draw(gameTime, renderer);

			DrawAgentMessages(gameTime, renderer);

			toolboxTray.Draw(gameTime, renderer);

			if (isEquipmentMenuOpen)
				menuEquipment.Draw(gameTime, renderer);
		}

		/// <summary>
		/// Draws the agent messages.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="renderer">Renderer.</param>
		private void DrawAgentMessages(GameTime gameTime, Renderer renderer)
		{
			foreach (var agentId in labelMessagesForAgents.Keys)
			{
				List<SimulationLabel> labelMessagesForAgent = new List<SimulationLabel>();
				bool success = labelMessagesForAgents.TryGetValue(agentId, out labelMessagesForAgent);
				if (success)
					foreach (var labelMessage in labelMessagesForAgent)
						labelMessage.Draw(gameTime, renderer);
			}
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
