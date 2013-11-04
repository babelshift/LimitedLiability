using MyThirdSDL.Descriptors;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public class SimulationManager
	{
		private List<Agent> agents = new List<Agent>();

		public string SimulationTimeDisplay
		{
			get
			{
				return String.Format("{0} minutes, {1} seconds, {2} milliseconds",
					SimulationTime.Minutes.ToString(), SimulationTime.Seconds.ToString(), SimulationTime.Milliseconds.ToString());
			}
		}

		public TimeSpan SimulationTime { get; private set; }

		#region Public Simulation Events

		public event EventHandler<EventArgs> EmployeeIsSleepy;
		public event EventHandler<EventArgs> EmployeeIsUnhealthy;
		public event EventHandler<EventArgs> EmployeeIsDirty;
		public event EventHandler<EventArgs> EmployeeIsHungry;
		public event EventHandler<EventArgs> EmployeeIsThirsty;
		public event EventHandler<EventArgs> EmployeeIsUnhappy;
		public event EventHandler<EventArgs> EmployeeNeedsOfficeDesk;

		#endregion

		#region Private Simulation Event Handlers

		private EventHandler<EventArgs> EmployeeIsSleepyHandler;
		private EventHandler<EventArgs> EmployeeIsDirtyHandler;
		private EventHandler<EventArgs> EmployeeIsHungryHandler;
		private EventHandler<EventArgs> EmployeeIsThirstyHandler;
		private EventHandler<EventArgs> EmployeeIsUnhealthyHandler;
		private EventHandler<EventArgs> EmployeeIsUnhappyHandler;
		private EventHandler<EventArgs> EmployeeNeedsOfficeDeskHandler;

		#endregion

		public SimulationManager()
		{
			EmployeeIsSleepyHandler = (object sender, EventArgs e) => EventHelper.FireEvent(EmployeeIsSleepy, sender, e);
			EmployeeIsDirtyHandler = (object sender, EventArgs e) => EventHelper.FireEvent(EmployeeIsDirty, sender, e);
			EmployeeIsHungryHandler = (object sender, EventArgs e) => EventHelper.FireEvent(EmployeeIsHungry, sender, e);
			EmployeeIsThirstyHandler = (object sender, EventArgs e) => EventHelper.FireEvent(EmployeeIsThirsty, sender, e);
			EmployeeIsUnhealthyHandler = (object sender, EventArgs e) => EventHelper.FireEvent(EmployeeIsUnhealthy, sender, e);
			EmployeeIsUnhappyHandler = (object sender, EventArgs e) => EventHelper.FireEvent(EmployeeIsUnhappy, sender, e);
			EmployeeNeedsOfficeDeskHandler = (object sender, EventArgs e) => EventHelper.FireEvent(EmployeeNeedsOfficeDesk, sender, e);
		}

		/// <summary>
		/// Updates the simulation by setting the simulation time and updating all tracked agents.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public void Update(GameTime gameTime)
		{
			SimulationTime = gameTime.TotalGameTime;

			foreach (var agent in agents)
			{
				agent.SetSimulationAge(SimulationTime);
				agent.Update(gameTime);
			}
		}

		/// <summary>
		/// Adds the passed agent to the simulation (agent will be updated in the game loop). Will also subscribe to all agent events and bubble them accordingly.
		/// </summary>
		/// <param name="agent">Agent.</param>
		public void AddAgent(Agent agent)
		{
			if (!agents.Any(a => a.ID == agent.ID))
			{
				agent.Activate();

				if (agent is Employee)
				{
					var employee = agent as Employee;
					employee.IsSleepy += EmployeeIsSleepyHandler;
					employee.IsDirty += EmployeeIsDirtyHandler;
					employee.IsHungry += EmployeeIsHungryHandler;
					employee.IsThirsty += EmployeeIsThirstyHandler;
					employee.IsUnhealthy += EmployeeIsUnhealthyHandler;
					employee.IsUnhappy += EmployeeIsUnhappyHandler;
					employee.NeedsOfficeDesk += EmployeeNeedsOfficeDeskHandler;
					agents.Add(employee);
				}
				else
					agents.Add(agent);
			}
		}

		/// <summary>
		/// Removes an agent identified by the passed Guid from the simulation. Will also unsubscribe the simulation from any events the agent fires.
		/// </summary>
		/// <param name="agentId">Agent identifier.</param>
		public void RemoveAgent(Guid agentId)
		{
			var agent = agents.FirstOrDefault(a => a.ID == agentId);
			if (agent != null)
			{
				agent.Deactivate();

				if (agent is Employee)
				{
					var employee = agent as Employee;
					employee.IsSleepy -= EmployeeIsSleepyHandler;
					employee.IsDirty -= EmployeeIsDirtyHandler;
					employee.IsHungry -= EmployeeIsHungryHandler;
					employee.IsThirsty -= EmployeeIsThirstyHandler;
					employee.IsUnhealthy -= EmployeeIsUnhealthyHandler;
					employee.IsUnhappy -= EmployeeIsUnhappyHandler;
					employee.NeedsOfficeDesk -= EmployeeNeedsOfficeDeskHandler;
				}
				else
					agents.Remove(agent);
			}
		}
	}
}
