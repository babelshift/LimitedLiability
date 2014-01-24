using System;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;

namespace MyThirdSDL.Agents
{
	public class OfficeDesk : Equipment
	{
		public Employee AssignedEmployee { get; private set; }

		public bool IsAssignedToAnEmployee { get { return AssignedEmployee != null; } }

		public OfficeDesk(TimeSpan birthTime, Texture texture, Vector startingPosition, string name, int price, string iconTextureKey,
			NecessityEffect necessityEffect, SkillEffect skillEffect)
			: base(birthTime, name, texture, startingPosition, price, iconTextureKey)
		{
			NecessityEffect = necessityEffect;
			SkillEffect = skillEffect;
		}

		public void AssignEmployee(Employee employee)
		{
			AssignedEmployee = employee;
		}
	}
}

