using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;

namespace MyThirdSDL.Agents
{
	public class Library : Room, ISkillsAffector
	{
		private const int WIDTH_IN_TILES = 5;
		private const int HEIGHT_IN_TILES = 5;

		public int IntelligenceEffectiveness { get; private set; }
		public int CreativityEffectiveness { get; private set; }
		public int CommunicationEffectiveness { get; private set; }
		public int LeadershipEffectiveness { get; private set; }

		public Library(TimeSpan birthTime, string agentName, Texture texture, Vector position)
			: base(birthTime, agentName, texture, position, WIDTH_IN_TILES, HEIGHT_IN_TILES)
		{
			IntelligenceEffectiveness = 1;
			CreativityEffectiveness = 1;
			CommunicationEffectiveness = 0;
			LeadershipEffectiveness = 0;
		}
	}
}
