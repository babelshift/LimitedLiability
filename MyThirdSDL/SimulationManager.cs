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

		public SimulationManager()
		{
		}

		public void Update(GameTime gameTime)
		{
			SimulationTime = gameTime.TotalGameTime;

			foreach (var agent in agents)
			{
				agent.SetSimulationAge(SimulationTime);
				agent.Update(gameTime);
			}
		}

		public void AddAgent(Agent agent)
		{
			if (!agents.Any(a => a.ID == agent.ID))
			{
				agent.Activate();
				agents.Add(agent);
			}
		}

		public void RemoveAgent(Guid agentId)
		{
			var agent = agents.FirstOrDefault(a => a.ID == agentId);
			if (agent != null)
			{
				agent.Deactivate();
				agents.Remove(agent);
			}
		}
	}
}
