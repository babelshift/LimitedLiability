using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public abstract class Equipment : Agent
	{
		public double Cost { get; private set; }

		public Equipment(string agentName) : base(agentName) { }
	}
}
