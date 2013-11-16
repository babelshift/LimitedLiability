using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Agents
{
	public class Room : Agent
	{
		private List<Texture> tileTextures = new List<Texture>();

		public int Price { get; private set; }
		public int WidthInTiles { get; private set; }
		public int HeightInTiles { get; private set; }

		public Room(TimeSpan birthTime, string agentName, Texture texture, Vector startingPosition, int widthInTiles, int heightInTiles, int price)
			: base(birthTime, agentName, texture, startingPosition)
		{
			WidthInTiles = widthInTiles;
			HeightInTiles = heightInTiles;
			Price = price;
		}
	}
}
