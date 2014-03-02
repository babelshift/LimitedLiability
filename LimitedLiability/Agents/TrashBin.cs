using System;
using SharpDL.Graphics;
using LimitedLiability.Descriptors;

namespace LimitedLiability.Agents
{
	public class TrashBin : Equipment
	{
		public Employee AssignedEmployee { get; private set; }

		public bool IsAssignedToAnEmployee { get { return AssignedEmployee != null; } }

		public TrashBin(TimeSpan birthTime, Texture texture, Vector startingPosition, string name, int price, string description, string iconTextureKey,
			NecessityEffect necessityEffect, SkillEffect skillEffect)
			: base(birthTime, name, texture, startingPosition, price, description, iconTextureKey)
		{
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
		}
	}
}

