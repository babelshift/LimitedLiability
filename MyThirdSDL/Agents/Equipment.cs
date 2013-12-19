using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Agents
{
	public abstract class Equipment : Agent
	{
		public int Price { get; private set; }

		public Equipment(TimeSpan birthTime, string agentName, TextureBook textureBook, Vector startingPosition, AgentOrientation orientation, int price)
			: base(birthTime, agentName, textureBook, startingPosition, orientation)
		{
			Price = price;
		}
	}
}
