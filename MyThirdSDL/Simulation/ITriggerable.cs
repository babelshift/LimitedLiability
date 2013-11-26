using System;

namespace MyThirdSDL.Simulation
{
	public interface ITriggerable
	{
		Trigger Trigger { get; }
		void ExecuteTrigger();
	}
}

