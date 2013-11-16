using System;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;

namespace MyThirdSDL.Agents
{
	public class OfficeDesk : Equipment, IPurchasable
	{
		public string IconTextureKey { get; private set; }

		public NecessityEffects NecessityEffects { get; private set; }

		public SkillEffects SkillEffects { get; private set; }

		public Employee AssignedEmployee { get; private set; }

		public bool IsAssignedToAnEmployee { get { return AssignedEmployee != null; } }

		public OfficeDesk(TimeSpan birthTime, Texture texture, Vector startingPosition, 
			string name, int price, string iconKey, NecessityEffects necessityEffect, SkillEffects skillEffect)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityEffects = necessityEffect;
			SkillEffects = skillEffect;
			IconTextureKey = iconKey;
        }

		public void AssignEmployee(Employee employee)
		{
			AssignedEmployee = employee;
		}
    }
}

