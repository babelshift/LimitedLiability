using System;
using SharpDL.Graphics;
using SharpDL;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace MyThirdSDL.Descriptors
{
	public abstract class MobileAgent : Agent, ITriggerSubscriber
	{
		public enum AgentActivity
		{
			Unknown,
			Walking,
			Idle
		}

		private ConcurrentQueue<Intent> intents = new ConcurrentQueue<Intent>();
		private Queue<MapObject> pathNodes;
		private Vector currentDestination;
		private Agent walkingTowardsAgent;

		public AgentActivity Activity { get; private set; }

		public Vector Speed { get; private set; }

		public bool IsWalkingTowardsAgent { get { return walkingTowardsAgent != null; } }

		private bool IsAtFinalWalkToDestination
		{
			get
			{
				if (CollisionBox.Intersects(walkingTowardsAgent.CollisionBox))
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

			// if we are actively walking towards something and we are at the final destination (the thing we are walking towards)
			// then we should take an action and get the next intent
			if (IsWalkingTowardsAgent && IsAtFinalWalkToDestination)
			{
				// if we are walking towards something triggerable, then trigger it now that we are at it
				if (walkingTowardsAgent is ITriggerable)
				{
					var triggerable = walkingTowardsAgent as ITriggerable;
					triggerable.ExecuteTrigger();
				}

				// we are done walking towards the intended agent
				walkingTowardsAgent = null;

				// get the next intent in our queue now that we've completed this intent
				Intent nextIntent = GetNextIntent();

				// if we have another intent, perform it, otherwise clear our walking towards destination because have no other intents
				if (nextIntent != null)
					WalkOnPathTowardsAgent(nextIntent.PathNodesToAgent, nextIntent.WalkToAgent);
			}

			base.Update(gameTime);
		}

		#endregion

		#region Movement

		private Intent GetNextIntent()
		{
			Intent intent = null;

			if (intents.Count > 0)
				intents.TryDequeue(out intent);

			return intent;
		}

		public void AddIntent(Intent intent)
		{
			// only add this intent if we don't already have an intent to walk to the agent
			if (!intents.Any(i => i.WalkToAgent.ID == intent.WalkToAgent.ID))
			{
				// if we aren't currently walking anywhere then this is most likely the first intent, perform that intent immediately
				if (walkingTowardsAgent == null)
					WalkOnPathTowardsAgent(intent.PathNodesToAgent, intent.WalkToAgent);
				// otherwise, if we aren't already performing this intent, add it to the queue
				else if (walkingTowardsAgent.ID != intent.WalkToAgent.ID)
					intents.Enqueue(intent);
			}
		}

		private void WalkOnPathTowardsAgent(Queue<MapObject> pathNodes, Agent agent)
		{
			// if we are not yet walking towards an agent, walk towards it!
			if (walkingTowardsAgent == null)
			{
				walkingTowardsAgent = agent;
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

