using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class Employee : Agent
	{
		private static Vector speed = new Vector(25, 25);

		public string FullName { get { return FirstName + " " + LastName;}}
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public int Age { get; private set; }
		public DateTime Birthday { get; private set; }
		public Job Job { get; private set; }
		public Necessities Necessities { get; private set; }
		public Skills Skills { get; private set; }

		public int HappinessRating
		{
			get
			{
				int necessityRatingAverage = 
					((int)Necessities.Sleep
					+ (int)Necessities.Health
					+ (int)Necessities.Hygiene
					+ (int)Necessities.Hunger
					+ (int)Necessities.Thirst) / 5;
				return necessityRatingAverage;
			}
		}

		public Employee(string agentName, Texture texture, Vector position, string firstName, string lastName, int age, DateTime birthday, Job job)
			: base(agentName, texture, position, speed)
		{
			FirstName = firstName;
			LastName = lastName;
			Age = age;
			Birthday = birthday;
			Job = job;

			Necessities = new Necessities(Necessities.Rating.Full);
			Skills = new Skills(Skills.Rating.Genius);
		}
	}
}
