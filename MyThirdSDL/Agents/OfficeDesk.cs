using System;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;

namespace MyThirdSDL.Agents
{
	public class OfficeDesk : Equipment, IPurchasable
	{
		private const int price = 50;
		private const string name = "Office Desk";

		public NecessityEffects NecessityEffects { get; private set; }
		public string IconTextureKey { get { return "IconOfficeDesk"; } }

		public Employee AssignedEmployee { get; private set; }

		public bool IsAssignedToAnEmployee { get { return AssignedEmployee != null; } }

		public OfficeDesk(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityEffects = new NecessityEffects(0, 0, -2, 0, 0);
        }

		public void AssignEmployee(Employee employee)
		{
			AssignedEmployee = employee;
		}
    }
}

