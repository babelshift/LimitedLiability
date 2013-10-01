using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public abstract class Agent
	{
		public Guid ID { get; private set; }
		public string AgentName { get; private set; }
		public double SimulationAge { get; private set; }

		public Agent(string name)
		{
			ID = Guid.NewGuid();
			AgentName = name;
			SimulationAge = 0.0;
		}
	}
}
