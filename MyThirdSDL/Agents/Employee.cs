using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using MyThirdSDL.Simulation;
using MyThirdSDL.Descriptors;
using SharpDL;

namespace MyThirdSDL.Agents
{
	public class Employee : MobileAgent, ITriggerSubscriber
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
		public event EventHandler<EventArgs> HungerSatisfied;

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

			log.Debug(String.Format("Employee named {0} just drank to increase thirst need from {1} to {2}.", FullName, previousThirstRating, Necessities.Thirst));

			// if, after drinking, our thirst is above the threshold AND our previous thirst was below the threshold, our thirst has been satisfied, notify subscribers
			if (Necessities.Thirst >= Necessities.Rating.Neutral && previousThirstRating < Necessities.Rating.Neutral)
				EventHelper.FireEvent(ThirstSatisfied, this, EventArgs.Empty);
		}

		public void Eat(int hungerEffectiveness)
		{
			Necessities.Rating previousHungerRating = Necessities.Hunger;
			Necessities.AdjustHunger(hungerEffectiveness);

			log.Debug(String.Format("Employee named {0} just ate to increase hunger need from {1} to {2}.", FullName, previousHungerRating, Necessities.Hunger));

			// if, after eating, our hunger is above the threshold AND our previous hunger was below the threshold, our hunger has been satisfied, notify subscribers
			if (Necessities.Hunger >= Necessities.Rating.Neutral && previousHungerRating < Necessities.Rating.Neutral)
				EventHelper.FireEvent(HungerSatisfied, this, EventArgs.Empty);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// if we are actively walking towards something and we are at the final destination (the thing we are walking towards)
			// then we should take an action and get the next intent
			if (IsWalkingTowardsAgent && IsAtFinalWalkToDestination)
			{
				// if we are walking towards something triggerable, then trigger it now that we are at it
				if (WalkingTowardsAgent is ITriggerable)
				{
					var triggerable = WalkingTowardsAgent as ITriggerable;
					triggerable.ExecuteTrigger();
				}

				// if we are walking towards something that affects our necessities (such as a soda machine or water fountain, get its effectiveness
				// and adjust our necessities accordingly
				if (WalkingTowardsAgent is SodaMachine)
				{
					var sodaMachine = WalkingTowardsAgent as SodaMachine;

					if (CurrentIntention.Type == IntentionType.BuyDrink)
					{
						var necessityAffector = sodaMachine.DispenseDrink();
						AdjustNecessities(necessityAffector);
					}
				}
				else if (WalkingTowardsAgent is SnackMachine)
				{
					var snackMachine = WalkingTowardsAgent as SnackMachine;

					if (CurrentIntention.Type == IntentionType.BuySnack)
					{
						var necessityAffector = snackMachine.DispenseFood();
						AdjustNecessities(necessityAffector);
					}
				}

				// we are done walking towards the intended agent
				ResetWalkingTowardsAgent();
				ResetIntention();

				SetNextIntention();
			}

			AdjustNecessities(necessityDecayRate);
			CheckIfEmployeeNeedsAnything();
			CheckIfEmployeeIsUnhappy();
		}

		private void AdjustNecessities(double necessityEffect)
		{
			Necessities.AdjustSleep(necessityEffect);
			Necessities.AdjustHunger(necessityEffect);
			Necessities.AdjustThirst(necessityEffect);
			Necessities.AdjustHygiene(necessityEffect);
			Necessities.AdjustHealth(necessityEffect);
		}

		private void AdjustNecessities(NecessityAffector necessityEffect)
		{
			Necessities.AdjustSleep(necessityEffect.SleepEffectiveness);
			Eat(necessityEffect.HungerEffectiveness);
			Drink(necessityEffect.ThirstEffectiveness);
			Necessities.AdjustHygiene(necessityEffect.HygieneEffectiveness);
			Necessities.AdjustHealth(necessityEffect.HealthEffectiveness);
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
