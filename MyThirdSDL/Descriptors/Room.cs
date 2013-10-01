using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class Room : Agent
	{
		private List<Texture> tileTextures = new List<Texture>();

		public int WidthInTiles { get; private set; }
		public int HeightInTiles { get; private set; }

		public Room(string agentName, int widthInTiles, int heightInTiles)
			: base(agentName)
		{
			WidthInTiles = widthInTiles;
			HeightInTiles = heightInTiles;
		}
	}
}
