using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.Agents
{
	public class WaterFountain : Equipment, IPurchasable
	{
		public string IconTextureKey { get; private set; }

		public NecessityEffects NecessityEffects { get; private set; }

		public SkillEffects SkillEffects { get; private set; }

		public Employee AssignedEmployee { get; private set; }

		public bool IsAssignedToAnEmployee { get { return AssignedEmployee != null; } }

		public WaterFountain(TimeSpan birthTime, Texture texture, Vector startingPosition, 
			string name, int price, string iconKey, NecessityEffects necessityEffect, SkillEffects skillEffect)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityEffects = necessityEffect;
			SkillEffects = skillEffect;
			IconTextureKey = iconKey;
		}
	}
}
