using System;

namespace LimitedLiability.Simulation
{
	public interface ITriggerable
	{
		Trigger Trigger { get; }
		void ExecuteTrigger();
	}
}

