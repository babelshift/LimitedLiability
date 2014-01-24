using System;
using SharpDL.Graphics;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.Agents
{
	public class TrashBin : Equipment
	{
		public Employee AssignedEmployee { get; private set; }

		public bool IsAssignedToAnEmployee { get { return AssignedEmployee != null; } }

		public TrashBin(TimeSpan birthTime, Texture texture, Vector startingPosition, string name, int price, string iconTextureKey, 
			NecessityEffect necessityEffect, SkillEffect skillEffect)
			: base(birthTime, name, texture, startingPosition, price, iconTextureKey)
		{
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
		}
	}
}

