﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.Agents
{
	public class SodaMachine : Equipment, IPurchasable
	{
		public string IconTextureKey { get; private set; }

		public NecessityEffect NecessityEffect { get; private set; }

		public SkillEffect SkillEffect { get; private set; }

		public Employee AssignedEmployee { get; private set; }

		public bool IsAssignedToAnEmployee { get { return AssignedEmployee != null; } }

		public SodaMachine(TimeSpan birthTime, TextureBook textureBook, Vector startingPosition, AgentOrientation orientation,
			string name, int price, string iconTextureKey, NecessityEffect necessityEffect, SkillEffect skillEffect)
			: base(birthTime, name, textureBook, startingPosition, orientation, price)
		{
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
			IconTextureKey = iconTextureKey;
		}

		public NecessityEffect DispenseDrink()
		{
			return NecessityEffect;
		}
	}
}
