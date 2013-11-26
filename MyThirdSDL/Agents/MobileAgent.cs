using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MyThirdSDL.Simulation;
using MyThirdSDL.Content;
using SharpDL.Graphics;
using SharpDL;

namespace MyThirdSDL.Agents
{
	public abstract class MobileAgent : Agent, ITriggerSubscriber
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private Queue<Intention> intentions = new Queue<Intention>();
		private Queue<PathNode> pathNodes;
		private PathNode currentDestination;

		public AgentActivity Activity { get; private set; }

		public Vector Speed { get; private set; }

		public Intention CurrentIntention { get; private set; }

		public bool HasCurrentIntention { get { return CurrentIntention != null; } }

		public Intention FinalIntention
		{
			get
			{
				if (intentions.Count > 0)
					return intentions.Peek();
				else
					return CurrentIntention;
			}
		}

		protected bool IsAtFinalWalkToDestination { get { return CurrentIntention.WalkToAgent.CollisionBox.Intersects(CollisionBox); } }

		private bool IsAtPathNodeDestination
		{
			get
			{
				if (currentDestination == null)
					return true;
				else
				{
					if (currentDestination.Bounds.Contains(CollisionBox.Center))
						return true;
					else
						return false;
				}
			}
		}

		public bool IsAlreadyIntention(IntentionType type)
		{
			bool isAlreadyIntention = false;

			if (intentions.Any(i => i.Type == type))
				isAlreadyIntention = true;

			if (HasCurrentIntention)
				if (CurrentIntention.Type == type)
					isAlreadyIntention = true;

			return isAlreadyIntention;
		}

		public MobileAgent(TimeSpan birthTime, string name, Texture texture, Vector startingPosition, Vector startingSpeed)
			: base(birthTime, name, texture, startingPosition)
		{
			Activity = AgentActivity.Unknown;
			Speed = startingSpeed;
		}

		protected void ChangeActivity(AgentActivity activity)
		{
			if (Activity != activity)
			{
				if (log.IsDebugEnabled)
					log.Debug(String.Format("Activity changed to from {0} to {1}.", Activity, activity));
				Activity = activity;
			}
		}

		public abstract void ReactToAction(ActionType actionType, NecessityEffect affector);

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			Move(gameTime);

			base.Update(gameTime);
		}

		#endregion

		#region Movement

		protected void ResetIntention()
		{
			CurrentIntention = null;
		}

		/// <summary>
		/// Sets the next intention for the agent to perform.
		/// </summary>
		protected void StartNextIntention()
		{
			if (!HasCurrentIntention)
			{
				Intention intention = null;

				if (intentions.Count > 0)
					intention = intentions.Dequeue();

				// if we have another intent, perform it, otherwise clear our walking towards destination because have no other intents
				if (intention != null)
				{
					if (intention.Type == IntentionType.BuyDrink)
						ChangeActivity(AgentActivity.WalkingToDrink);
					else if (intention.Type == IntentionType.BuySnack)
						ChangeActivity(AgentActivity.WalkingToFood);
					else if (intention.Type == IntentionType.GoToDesk)
						ChangeActivity(AgentActivity.WalkingToDesk);

					WalkOnPathTowardsIntendedAgent(intention.PathNodesToAgent, intention);
				}
				else
					CurrentIntention = null;
			}
		}

		/// <summary>
		/// Adds an intention to the queue of intentions for this agent. For example, an agent can have an intention to go to a soda machine and buy a drink
		/// followed by an intention to go to a vending machine and buy a snack. These intentions are kept in a queue and completed in FIFO order.
		/// </summary>
		/// <param name="intention">Intention.</param>
		public void AddIntention(Intention intention)
		{
			// TODO: This only checks if we intend based on intention type
			// TODO: Sometimes, we might want to perform the same type intention more than once in a row such as talking to a coworker
			if (!IsAlreadyIntention(intention.Type))
				intentions.Enqueue(intention);

			if (!HasCurrentIntention)
				StartNextIntention();
		}

		/// <summary>
		/// Attempts to walk on the passed path to the passed agent. If we are already walking towards an agent, this method does nothing.
		/// </summary>
		/// <param name="pathNodes">Path nodes.</param>
		/// <param name="agent">Agent.</param>
		protected void WalkOnPathTowardsIntendedAgent(Queue<PathNode> pathNodes, Intention intention)
		{
			// if we are not yet walking towards an agent, walk towards it!
			if (!HasCurrentIntention)
			{
				CurrentIntention = intention;
				this.pathNodes = pathNodes;
			}
		}

		private void Move(GameTime gameTime)
		{
			if (State == AgentState.Active)
			{
				if (IsAtPathNodeDestination)
					SetNextDestinationNode();

				if (!IsAtPathNodeDestination)
					Move(gameTime.ElapsedGameTime.TotalSeconds);
				else
				{
					if (pathNodes != null)
					{
						if (pathNodes.Count == 0)
							ResetMovement();
						else
							currentDestination = pathNodes.Dequeue();
					}
				}
			}
		}

		private void Move(double dt)
		{
			//ChangeActivity(AgentActivity.Walking);
			if (currentDestination != null)
			{
				Vector direction = GetMovementDirection();
				WorldPosition += new Vector((float)(direction.X * Speed.X * dt), (float)(direction.Y * Speed.Y * dt));
			}
		}

		private void ResetMovement()
		{
			pathNodes = null;
			currentDestination = null;
			//ChangeActivity(AgentActivity.Idle);
		}

		private void SetNextDestinationNode()
		{
			if (pathNodes != null)
				if (pathNodes.Count() > 0)
					currentDestination = pathNodes.Dequeue();
		}

		private Vector GetMovementDirection()
		{
			if (State == AgentState.Active)
			{
				float movementDirectionX = 0f;
				float movementDirectionY = 0f;

				if (CollisionBox.Center.X > currentDestination.Bounds.Center.X)
					movementDirectionX -= 1f;
				else if (CollisionBox.Center.X < currentDestination.Bounds.Center.X)
					movementDirectionX += 1f;

				if (CollisionBox.Center.Y > currentDestination.Bounds.Center.Y)
					movementDirectionY -= 1f;
				else if (CollisionBox.Center.Y < currentDestination.Bounds.Center.Y)
					movementDirectionY += 1f;

				return new Vector(movementDirectionX, movementDirectionY);
			}
			else
				return Vector.Zero;
		}

		#endregion

	}
}

