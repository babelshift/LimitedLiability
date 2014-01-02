using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Agents
{
	public enum ThoughtType
	{
		Thirsty,
		Hungry,
		Sleepy,
		Miscellaneous,
		NotEnoughEquipment,
		NotChallenged
	}

	public class Thought
	{
		public string Idea { get; private set; }
		public ThoughtType Type { get; private set; }
		public DateTime WorldDateTime { get; private set; }
		public TimeSpan SimulationTime { get; private set; }

		public Thought(string idea, ThoughtType type, DateTime worldDateTime, TimeSpan simulationTime)
		{
			Idea = idea;
			Type = type;
			WorldDateTime = worldDateTime;
			SimulationTime = simulationTime;
		}
	}
}
