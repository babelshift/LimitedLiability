using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Agents
{
	public class ThoughtEventArgs : EventArgs
	{
		public ThoughtType Type { get; private set; }

		public ThoughtEventArgs(ThoughtType type)
		{
			Type = type;
		}
	}
}
