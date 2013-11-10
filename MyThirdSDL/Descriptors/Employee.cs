using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL;
using System.Collections.Concurrent;

namespace MyThirdSDL.Descriptors
{
	public class Employee : MobileAgent, ITriggerSubscriber
	{
		private double necessityDecayRate = -0.01;
		private static Vector speed = new Vector(25, 25);

		public string FullName { get { return FirstName + " " + LastName; } }

		public string FirstName { get; private set; }

		public string LastName { get; private set; }

		public int Age { get; private set; }

		public DateTime Birthday { get; private set; }

		public Job Job { get; private set; }

		public Necessities Necessities { get; private set; }

		public Skills Skills { get; private set; }

		public OfficeDesk AssignedOfficeDesk { get; private set; }

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

		public event EventHandler<EventArgs> IsSleepy;
		public event EventHandler<EventArgs> IsUnhealthy;
		public event EventHandler<EventArgs> IsDirty;
		public event EventHandler<EventArgs> IsHungry;
		public event EventHandler<EventArgs> IsThirsty;
		public event EventHandler<EventArgs> IsUnhappy;
		public event EventHandler<EventArgs> NeedsOfficeDesk;

		public event EventHandler<EventArgs> ThirstSatisfied;

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

		public void AssignOfficeDesk(OfficeDesk officeDesk)
		{
			AssignedOfficeDesk = officeDesk;
		}

		public void Drink(int thirstEffectiveness)
		{
			Necessities.Rating previousThirstRating = Necessities.Thirst;
			Necessities.AdjustThirst(thirstEffectiveness);

			// if, after drinking, our thirst is above the threshold AND our previous thirst was below the threshold, our thirst has been satisfied, notify subscribers
			if (Necessities.Thirst >= Necessities.Rating.Neutral && previousThirstRating < Necessities.Rating.Neutral)
				EventHelper.FireEvent(ThirstSatisfied, this, EventArgs.Empty);
		}

		public void Eat(int hungerEffectiveness)
		{
			Necessities.AdjustHunger(hungerEffectiveness);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			AdjustNecessitiesBasedOnDecayRate();
			CheckIfEmployeeNeedsAnything();
			CheckIfEmployeeIsUnhappy();

			if (IsWalkingTowardsAgent && IsAtFinalDestination)
			{
				// if soda machine, drink
				if (WalkingTowardsAgent is ITriggerable)
				{
					var triggerable = WalkingTowardsAgent as ITriggerable;
					triggerable.ExecuteTrigger();
				}

				ResetWalkingTowardsAgent();
			}
		}

		private void AdjustNecessitiesBasedOnDecayRate()
		{
			// as time goes on
			// we need to slowly increase sleepiness
			// we need to slowly increase hunger
			// we need to slowly increase thirst
			// we need to slowly reduce hygiene
			// we need to slowly reduce health / fitness
			Necessities.AdjustSleep(necessityDecayRate);
			Necessities.AdjustHunger(necessityDecayRate);
			Necessities.AdjustThirst(necessityDecayRate);
			Necessities.AdjustHygiene(necessityDecayRate);
			Necessities.AdjustHealth(necessityDecayRate);
		}

		private void CheckIfEmployeeNeedsAnything()
		{
			// if hungry, find vending machine / lunch room, eat
			if (Necessities.Hunger < Necessities.Rating.Neutral)
				EventHelper.FireEvent(IsHungry, this, EventArgs.Empty);
			// if thirsty, find vending machine / lunch room, drink
			if (Necessities.Thirst < Necessities.Rating.Neutral)
				EventHelper.FireEvent(IsThirsty, this, EventArgs.Empty);
			// if dirty, find bathroom, wash, relieve self
			if (Necessities.Hygiene < Necessities.Rating.Neutral)
				EventHelper.FireEvent(IsDirty, this, EventArgs.Empty);
			// if unhealthy, find gym/go exercise
			if (Necessities.Health < Necessities.Rating.Neutral)
				EventHelper.FireEvent(IsUnhealthy, this, EventArgs.Empty);
			// if sleepy
			if (Necessities.Sleep < Necessities.Rating.Neutral)
			{
				EventHelper.FireEvent(IsSleepy, this, EventArgs.Empty);
				// if during work hours, sleep at desk, perform poorly, complain
				// if after work hours, leave work
			}

			// if after all the needs have been met and if desk/office is available, go to desk/office and work 
			if (AssignedOfficeDesk != null)
			{
			}
			// else reduce happiness somehow (skills slowly go down?)
			else
				EventHelper.FireEvent(NeedsOfficeDesk, this, EventArgs.Empty);
		}

		private void CheckIfEmployeeIsUnhappy()
		{
			// if unhappy, complain, threaten to quit, quit, lash out at office / others
			if (HappinessRating < (int)Necessities.Rating.Neutral)
				EventHelper.FireEvent(IsUnhappy, this, EventArgs.Empty);
		}

		/// <summary>
		/// Reacts to an action fired as the result of a trigger.
		/// </summary>
		/// <param name="actionType">Action type.</param>
		/// <param name="affector">Affector.</param>
		public override void ReactToAction(ActionType actionType, NecessityAffector affector) 
		{
			if (actionType == ActionType.DispenseDrink)
				Drink(affector.ThirstEffectiveness);
			else if (actionType == ActionType.DispenseFood)
				Eat(affector.HungerEffectiveness);
		}
	}
}
