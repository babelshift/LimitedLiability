using System;
using SharpDL.Graphics;
using SharpDL;
using System.Collections.Generic;
using System.Linq;

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

		private Queue<MapObject> pathNodes;
		private Vector currentDestination;

		public AgentActivity Activity { get; private set; }

		public Vector Speed { get; private set; }

		public Agent WalkingTowardsAgent { get; private set; }

		public bool IsWalkingTowardsAgent { get { return WalkingTowardsAgent != null; } }

		public bool IsAtFinalDestination
		{
			get
			{
 				if (CollisionBox.Intersects(WalkingTowardsAgent.CollisionBox))
					return true;
				else
					return false;
			}
		}

		private bool IsAtDestination
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

		public void WalkOnPathTowardsAgent(Queue<MapObject> pathNodes, Agent agent)
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
				if (IsAtDestination)
					SetNextDestinationNode();

				if (!IsAtDestination)
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

		public void ResetWalkingTowardsAgent()
		{
			WalkingTowardsAgent = null;
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

