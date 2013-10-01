using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public interface INecessityAffector
	{
		int HealthEffectiveness { get; }
		int HygieneEffectiveness { get; }
		int SleepEffectiveness { get; }
		int ThirstEffectiveness { get; }
		int HungerEffectiveness { get; }
	}
}
