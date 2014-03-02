using LimitedLiability.Content;
using LimitedLiability.Descriptors;
using LimitedLiability.Simulation;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LimitedLiability.Agents
{
	public class Employee : MobileAgent
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private double necessityDecayRate = -0.005;
		private static Vector speed = new Vector(25, 25);
		private List<Thought> thoughtLog = new List<Thought>();
		private Dictionary<ThoughtType, Thought> unsatisfiedThoughts = new Dictionary<ThoughtType, Thought>();

		public IEnumerable<Thought> UnsatisfiedThoughts { get { return unsatisfiedThoughts.Values; } }

		public IEnumerable<Thought> ThoughtLog { get { return thoughtLog; } }

		public string FullName { get { return FirstName + " " + LastName; } }

		public string FirstName { get; private set; }

		public string LastName { get; private set; }

		public TimeSpan Age { get; private set; }

		public DateTime Birthday { get; private set; }

		public Job Job { get; private set; }

		public Necessities Necessities { get; private set; }

		public Skills Skills { get; private set; }

		public OfficeDesk AssignedOfficeDesk { get; private set; }

		public bool IsAssignedAnOfficeDesk { get { return AssignedOfficeDesk != null; } }

		public bool IsAtOfficeDesk { get; private set; }

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

		public event EventHandler<ThoughtEventArgs> HadThought;

		public event EventHandler<ThoughtEventArgs> ThoughtSatisfied;

		public Employee(TimeSpan birthTime, string agentName, TextureBook textureBook, Vector position, AgentOrientation orientation,
			string firstName, string lastName, DateTime birthday, Skills skills, Job job)
			: base(birthTime, agentName, textureBook, position, speed, orientation)
		{
			FirstName = firstName;
			LastName = lastName;
			Birthday = birthday;
			Job = job;
			Skills = skills;

			Necessities = new Necessities(Necessities.Rating.Full);
		}

		public void AssignOfficeDesk(OfficeDesk officeDesk)
		{
			AssignedOfficeDesk = officeDesk;
			officeDesk.AssignEmployee(this);
			OnThoughtSatisfied(ThoughtType.NeedsDeskAssignment);
		}

		private void AdjustNecessities(double necessityEffect)
		{
			Necessities.AdjustSleep(necessityEffect);
			Necessities.AdjustHunger(necessityEffect);
			Necessities.AdjustThirst(necessityEffect);
			Necessities.AdjustHygiene(necessityEffect);
			Necessities.AdjustHealth(necessityEffect);
		}

		private void AdjustNecessities(NecessityEffect necessityEffect)
		{
			Necessities.AdjustSleep(necessityEffect.SleepEffectiveness);
			Eat(necessityEffect.HungerEffectiveness);
			Drink(necessityEffect.ThirstEffectiveness);
			Necessities.AdjustHygiene(necessityEffect.HygieneEffectiveness);
			Necessities.AdjustHealth(necessityEffect.HealthEffectiveness);
		}

		public void Drink(int thirstEffectiveness)
		{
			Necessities.Rating previousThirstRating = Necessities.Thirst;
			Necessities.AdjustThirst(thirstEffectiveness);

			log.Debug(String.Format("Employee named {0} just drank to increase thirst need from {1} to {2}.", FullName, previousThirstRating, Necessities.Thirst));

			// if, after drinking, our thirst is above the threshold AND our previous thirst was below the threshold, our thirst has been satisfied, notify subscribers
			if (Necessities.Thirst >= Necessities.Rating.Neutral && previousThirstRating < Necessities.Rating.Neutral)
				OnThoughtSatisfied(ThoughtType.Thirsty);
		}

		public void Eat(int hungerEffectiveness)
		{
			Necessities.Rating previousHungerRating = Necessities.Hunger;
			Necessities.AdjustHunger(hungerEffectiveness);

			log.Debug(String.Format("Employee named {0} just ate to increase hunger need from {1} to {2}.", FullName, previousHungerRating, Necessities.Hunger));

			// if, after eating, our hunger is above the threshold AND our previous hunger was below the threshold, our hunger has been satisfied, notify subscribers
			if (Necessities.Hunger >= Necessities.Rating.Neutral && previousHungerRating < Necessities.Rating.Neutral)
				OnThoughtSatisfied(ThoughtType.Hungry);
		}

		public void Promote()
		{
			Job.Promote();
		}

		public void Demote()
		{
			Job.Demote();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// if we are actively walking towards something and we are at the final destination (the thing we are walking towards)
			// then we should take an action and get the next intent
			if (HasCurrentIntention && IsAtFinalWalkToDestination)
			{
				// if we are walking towards something triggerable, then trigger it now that we are at it
				var iTriggerable = CurrentIntention.WalkToAgent as ITriggerable;
				if (iTriggerable != null)
				{
					var triggerable = iTriggerable;
					triggerable.ExecuteTrigger();
				}

				// if we are walking towards something that affects our necessities (such as a soda machine or water fountain, get its effectiveness
				// and adjust our necessities accordingly
				if (!(CurrentIntention.WalkToAgent is SodaMachine))
				{
					var sodaMachine = CurrentIntention.WalkToAgent as SodaMachine;

					if (CurrentIntention.Type == IntentionType.BuyDrink)
					{
						var necessityEffect = sodaMachine.DispenseDrink();
						AdjustNecessities(necessityEffect);
					}
				}
				else if (CurrentIntention.WalkToAgent is SnackMachine)
				{
					var snackMachine = CurrentIntention.WalkToAgent as SnackMachine;

					if (CurrentIntention.Type == IntentionType.BuySnack)
					{
						var necessityEffect = snackMachine.DispenseFood();
						AdjustNecessities(necessityEffect);
					}
				}
				else if (CurrentIntention.WalkToAgent is OfficeDesk)
				{
					var officeDesk = CurrentIntention.WalkToAgent as OfficeDesk;

					// if we are going to a desk and we are not assigned to a desk and our target desk is not already assigned, assign this desk to ourself
					if (CurrentIntention.Type == IntentionType.GoToDesk)
					if (!IsAssignedAnOfficeDesk)
					if (!officeDesk.IsAssignedToAnEmployee)
						AssignOfficeDesk(officeDesk);

					// if we are assigned an office desk, we are now at our office desk
					if (IsAssignedAnOfficeDesk)
					if (officeDesk.ID == AssignedOfficeDesk.ID)
						IsAtOfficeDesk = true;
				}

				// we are done walking towards the intended agent
				//ResetWalkingTowardsAgent();
				ResetIntention();
				StartNextIntention();
			}

			// if we have no current intention, try to work at our desk
			if (!HasCurrentIntention)
			{
				// if we are assigned an office desk and we are at our office desk, indicate that we are working at our office desk
				if (IsAssignedAnOfficeDesk)
				{
					if (IsAtOfficeDesk)
						ChangeActivity(AgentActivity.WorkingAtDesk);
					else
						OnThought(ThoughtType.IsIdle);
				}
			}
			// if we have an intention, then we need something, so we are not at our office desk anymore
			else
				IsAtOfficeDesk = false;

			AdjustNecessities(necessityDecayRate);
			CheckIfEmployeeNeedsAnything(gameTime.TotalGameTime);
			CheckIfEmployeeIsUnhappy();
		}

		private void CheckIfEmployeeNeedsAnything(TimeSpan simulationTime)
		{
			// if hungry, find vending machine / lunch room, eat
			if (Necessities.Hunger < Necessities.Rating.Neutral)
				OnThought(ThoughtType.Hungry);
			// if thirsty, find vending machine / lunch room, drink
			if (Necessities.Thirst < Necessities.Rating.Neutral)
				OnThought(ThoughtType.Thirsty);
			// if dirty, find bathroom, wash, relieve self
			if (Necessities.Hygiene < Necessities.Rating.Neutral)
				OnThought(ThoughtType.Dirty);
			// if unhealthy, find gym/go exercise
			if (Necessities.Health < Necessities.Rating.Neutral)
				OnThought(ThoughtType.Unhealthy);
			// if sleepy
			if (Necessities.Sleep < Necessities.Rating.Neutral)
			{
				OnThought(ThoughtType.Sleepy);
				// if during work hours, sleep at desk, perform poorly, complain
				// if after work hours, leave work
			}

			// else reduce happiness somehow (skills slowly go down?)
			if (!IsAssignedAnOfficeDesk)
				OnThought(ThoughtType.NeedsDeskAssignment);
		}

		private void CheckIfEmployeeIsUnhappy()
		{
			// if unhappy, complain, threaten to quit, quit, lash out at office / others
			if (HappinessRating < (int)Necessities.Rating.Neutral)
				OnThought(ThoughtType.Unhappy);
		}

		/// <summary>
		/// Reacts to an action fired as the result of a trigger.
		/// </summary>
		/// <param name="actionType">Action type.</param>
		/// <param name="affector">Affector.</param>
		public override void ReactToAction(ActionType actionType, NecessityEffect affector)
		{
			if (actionType == ActionType.DispenseDrink)
				Drink(affector.ThirstEffectiveness);
			else if (actionType == ActionType.DispenseFood)
				Eat(affector.HungerEffectiveness);
		}

		public void UpdateAge(DateTime worldDateTime)
		{
			Age = worldDateTime.Subtract(Birthday);
		}

		private void OnThought(ThoughtType type)
		{
			// only think about this if we are previously satisfied
			if (!unsatisfiedThoughts.Any(t => t.Key == type))
				EventHelper.FireEvent(HadThought, this, new ThoughtEventArgs(type));
		}

		public void AddUnsatisfiedThought(Thought thought)
		{
			unsatisfiedThoughts.Add(thought.Type, thought);
		}

		private void OnThoughtSatisfied(ThoughtType type)
		{
			SatisfyThought(type);
			EventHelper.FireEvent(ThoughtSatisfied, this, new ThoughtEventArgs(type));
		}

		private void SatisfyThought(ThoughtType type)
		{
			Thought satisfiedThought;
			bool success = unsatisfiedThoughts.TryGetValue(type, out satisfiedThought);

			if (success)
			{
				thoughtLog.Add(satisfiedThought);
				unsatisfiedThoughts.Remove(type);
			}
		}

		//private TimeSpan GetTimeOfMostRecentThoughtByType(ThoughtType type)
		//{
		//	IEnumerable<Thought> filteredThoughts = thoughtLog.Where(t => t.Type == type);
		//	if (filteredThoughts.Count() > 0)
		//		return filteredThoughts.Max(t => t.SimulationTime);
		//	else
		//		return TimeSpan.Zero;
		//}
	}
}