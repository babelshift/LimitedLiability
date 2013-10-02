using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class SnackMachine : Equipment, INecessityAffector
	{
		public int HealthEffectiveness { get; private set; }
		public int HygieneEffectiveness { get; private set; }
		public int SleepEffectiveness { get; private set; }
		public int ThirstEffectiveness { get; private set; }
		public int HungerEffectiveness { get; private set; }

		public SnackMachine(string agentName, Texture texture, Vector startingPosition)
			: base(agentName, texture, startingPosition)
		{
			HealthEffectiveness = -1;
			HygieneEffectiveness = 0;
			SleepEffectiveness = 0;
			ThirstEffectiveness = 0;
			HungerEffectiveness = 2;
		}
	}
}
