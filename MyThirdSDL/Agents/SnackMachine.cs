using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.Agents
{
	public class SnackMachine : Equipment, IPurchasable
	{
		public string IconTextureKey { get; private set; }

		public NecessityEffect NecessityEffect { get; private set; }

		public SkillEffect SkillEffect { get; private set; }

		public Employee AssignedEmployee { get; private set; }

		public bool IsAssignedToAnEmployee { get { return AssignedEmployee != null; } }

		public SnackMachine(TimeSpan birthTime, Texture texture, Vector startingPosition,
			string name, int price, string iconTextureKey, NecessityEffect necessityEffect, SkillEffect skillEffect)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
			IconTextureKey = iconTextureKey;
		}

		public NecessityEffect DispenseFood()
		{
			return NecessityEffect;
		}
	}
}
