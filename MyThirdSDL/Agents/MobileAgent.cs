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
		public enum AgentActivity
		{
			Unknown,
			Walking,
			Idle
		}

		private ConcurrentQueue<Intention> intentions = new ConcurrentQueue<Intention>();
		private Queue<MapObject> pathNodes;
		private Vector currentDestination;

		public AgentActivity Activity { get; private set; }

		public Vector Speed { get; private set; }

		public bool IsWalkingTowardsAgent { get { return WalkingTowardsAgent != null; } }

		protected Agent WalkingTowardsAgent;

		protected Intention CurrentIntention { get; private set; }

		protected bool IsAtFinalWalkToDestination
		{
			get
			{
				if (CollisionBox.Intersects(WalkingTowardsAgent.CollisionBox))
					return true;
				else
					return false;
			}
		}

		private bool IsAtPathNodeDestination
		{
			get
			{
				if (currentDestination == CoordinateHelper.DefaultVector || CollisionBox.Contains(new Point((int)currentDestination.X, (int)currentDestination.Y)))
					return true;
				else
					return false;
			}
		}

		public MobileAgent(TimeSpan birthTime, string name, Texture texture, Vector startingPosition, Vector startingSpeed)
			: base(birthTime, name, texture, startingPosition)
		{
			Activity = AgentActivity.Unknown;
			currentDestination = CoordinateHelper.DefaultVector;
			Speed = startingSpeed;
		}

		private void ChangeActivity(AgentActivity activity)
		{
			if (Activity != activity)
				Activity = activity;
		}

		public abstract void ReactToAction(ActionType actionType, NecessityAffector affector);

		#region Game Loop

		public override void Update(GameTime gameTime)
		{
			Move(gameTime);

			base.Update(gameTime);
		}

		#endregion

		#region Movement

		protected void ResetWalkingTowardsAgent()
		{
			WalkingTowardsAgent = null;
		}

		protected void ResetIntention()
		{
			CurrentIntention = null;
		}

		/// <summary>
		/// Sets the next intention for the agent to perform.
		/// </summary>
		protected void SetNextIntention()
		{
			if (CurrentIntention == null)
			{
				Intention intention = null;

				if (intentions.Count > 0)
					intentions.TryDequeue(out intention);

				// if we have another intent, perform it, otherwise clear our walking towards destination because have no other intents
				if (intention != null)
				{
					CurrentIntention = intention;
					WalkOnPathTowardsAgent(CurrentIntention.PathNodesToAgent, CurrentIntention.WalkToAgent);
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
			// only add this intent if we don't already have an intent to walk to the agent
			if (!intentions.Any(i => i.WalkToAgent.ID == intention.WalkToAgent.ID))
			{
				// if we aren't currently walking anywhere then this is most likely the first intent, perform that intent immediately
				if (WalkingTowardsAgent == null)
				{
					intentions.Enqueue(intention);
					SetNextIntention();
				}
				// otherwise, if we aren't already walking towards the intention, queue it up
				else if (WalkingTowardsAgent.ID != intention.WalkToAgent.ID)
					intentions.Enqueue(intention);
			}
		}

		/// <summary>
		/// Attempts to walk on the passed path to the passed agent. If we are already walking towards an agent, this method does nothing.
		/// </summary>
		/// <param name="pathNodes">Path nodes.</param>
		/// <param name="agent">Agent.</param>
		protected void WalkOnPathTowardsAgent(Queue<MapObject> pathNodes, Agent agent)
		{
			// if we are not yet walking towards an agent, walk towards it!
			if (WalkingTowardsAgent == null)
			{
				WalkingTowardsAgent = agent;
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
							currentDestination = pathNodes.Dequeue().WorldPosition;
					}
				}
			}
		}

		private void Move(double dt)
		{
			ChangeActivity(AgentActivity.Walking);
			Vector direction = GetMovementDirection();
			WorldPosition += new Vector((float)(direction.X * Speed.X * dt), (float)(direction.Y * Speed.Y * dt));
		}

		private void ResetMovement()
		{
			pathNodes = null;
			currentDestination = CoordinateHelper.DefaultVector;
			ChangeActivity(AgentActivity.Idle);
		}

		private void SetNextDestinationNode()
		{
			if (pathNodes != null)
			if (pathNodes.Count() > 0)
				currentDestination = pathNodes.Dequeue().WorldPosition;
		}

		private Vector GetMovementDirection()
		{
			if (State == AgentState.Active)
			{
				float movementDirectionX = 0f;
				float movementDirectionY = 0f;

				if (WorldPosition.X > currentDestination.X)
					movementDirectionX -= 1f;

				if (WorldPosition.X < currentDestination.X)
					movementDirectionX += 1f;

				if (WorldPosition.Y > currentDestination.Y)
					movementDirectionY -= 1f;

				if (WorldPosition.Y < currentDestination.Y)
					movementDirectionY += 1f;

				return new Vector(movementDirectionX, movementDirectionY);
			}
			else
				return Vector.Zero;
		}

		#endregion

	}
}

