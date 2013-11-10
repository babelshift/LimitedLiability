using System;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL
{
    public interface ITriggerable
    {
		Trigger Trigger { get; }
		void ExecuteTrigger();
    }
}

