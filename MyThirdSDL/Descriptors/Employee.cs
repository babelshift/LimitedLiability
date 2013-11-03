using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class Employee : MobileAgent
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

		public Employee(TimeSpan birthTime, string agentName, Texture texture, Vector position, string firstName, string lastName, int age, DateTime birthday, Skills skills, Job job)
			: base(birthTime, agentName, texture, position, speed)
		{
			FirstName = firstName;
			LastName = lastName;
			Age = age;
			Birthday = birthday;
			Job = job;
			Skills = skills;

			Necessities = new Necessities(Necessities.Rating.Full);
		}

		public override void Update(SharpDL.GameTime gameTime)
		{
			// as time goes on
			// we need to slowly increase sleepiness
			// we need to slowly increase hunger
			// we need to slowly increase thirst
			// we need to slowly reduce hygiene
			// we need to slowly reduce health / fitness
			Necessities.AdjustSleep(-0.01);
			Necessities.AdjustHunger(-0.01);
			Necessities.AdjustThirst(-0.01);
			Necessities.AdjustHygiene(-0.01);
			Necessities.AdjustHealth(-0.01);

			// if desk/office is available, go to desk/office and work 
			// else reduce happiness somehow (skills slowly go down?)

			// if hungry, find vending machine / lunch room, eat
			// else increase hunger (reduce hunger rating)

			// if thirsty, find vending machine / lunch room, drink
			// else increase thirst (reduce thirst rating)

			// if dirty, find bathroom, wash, relieve self
			// else decrease hygiene (reduce hygiene rating)

			// if sleepy
			// if during work hours, sleep at desk, perform poorly, complain
			// if after work hours, leave work

			// if unhappy, complain, threaten to quit, quit, lash out at office / others

			base.Update(gameTime);
		}
	}
}
