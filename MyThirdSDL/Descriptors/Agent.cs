using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public enum AgentState
	{
		Unknown,
		Active,
		Inactive
	}

	public enum AgentActivity
	{
		Unknown,
		Walking,
		Idle
	}

	public abstract class Agent : IDrawable
	{
		private Queue<MapObject> pathNodes;

		private Texture Texture { get; set; }
		private Vector Destination { get; set; }
		private Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, Texture.Width / 2, Texture.Height / 2);
			}
		}
		private TimeSpan BirthTime { get; set; }

		public Guid ID { get; private set; }
		public string Name { get; private set; }
		public TimeSpan SimulationAge { get; private set; }
		public TimeSpan WorldAge
		{
			get
			{
				return TimeSpan.FromMilliseconds(8640 * SimulationAge.TotalMilliseconds);
			}
		}
		public AgentState State { get; private set; }
		public AgentActivity Activity { get; private set; }
		public Vector WorldGridIndex { get; private set; }
		public Vector WorldPosition { get; protected set; }
		public float Depth { get { return WorldPosition.X + WorldPosition.Y; } }
		public Vector Speed { get; private set; }
		public Vector ProjectedPosition { get; private set; }

		public Agent(TimeSpan birthTime, string name, Texture texture, Vector startingPosition, Vector startingSpeed)
		{
			ID = Guid.NewGuid();
			BirthTime = birthTime;
			Name = name;
			State = AgentState.Unknown;
			Activity = AgentActivity.Unknown;

			WorldPosition = startingPosition;
			Speed = startingSpeed;
			Texture = texture;

			ProjectedPosition = GetProjectedPosition();
			WorldGridIndex = GetWorldGridIndex();
			Destination = CoordinateHelper.DefaultVector;
		}

		public void SetSimulationAge(TimeSpan simulationTime)
		{
			SimulationAge = simulationTime.Subtract(BirthTime);
		}

		public void SetPath(Queue<MapObject> pathNodes)
		{
			if (this.pathNodes == null)
				this.pathNodes = pathNodes;
		}

		private Vector GetWorldGridIndex()
		{
			Vector worldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(
				WorldPosition.X,
				WorldPosition.Y,
				CoordinateHelper.WorldGridCellWidth,
				CoordinateHelper.WorldGridCellHeight
			);

			int worldGridIndexX = (int)Math.Round(worldGridIndex.X);
			int worldGridIndexY = (int)Math.Round(worldGridIndex.Y);
			worldGridIndex = new Vector(worldGridIndexX, worldGridIndexY);

			return worldGridIndex;
		}

		private Vector GetProjectedPosition()
		{
			Vector projectedPosition = CoordinateHelper.WorldSpaceToScreenSpace(
				WorldPosition.X,
				WorldPosition.Y,
				Texture.Width / 2,
				Texture.Height,
				CoordinateHelper.ScreenOffset,
				CoordinateHelper.ScreenProjectionType.Isometric
			);

			return projectedPosition;
		}

		private Vector GetMovementDirection()
		{
			if (State == AgentState.Active)
			{
				float movementDirectionX = 0f;
				float movementDirectionY = 0f;

				if (WorldPosition.X > Destination.X)
					movementDirectionX -= 1f;

				if (WorldPosition.X < Destination.X)
					movementDirectionX += 1f;

				if (WorldPosition.Y > Destination.Y)
					movementDirectionY -= 1f;

				if (WorldPosition.Y < Destination.Y)
					movementDirectionY += 1f;

				return new Vector(movementDirectionX, movementDirectionY);
			}
			else
				return Vector.Zero;
		}

		#region Status Changes

		public void Activate()
		{
			ChangeStatus(AgentState.Active);
		}

		public void Deactivate()
		{
			ChangeStatus(AgentState.Inactive);
		}

		protected void ChangeStatus(AgentState status)
		{
			if (State != status)
				State = status;
		}

		#endregion

		#region Game Loop

		public void Update(GameTime gameTime)
		{
			Move(gameTime);
			WorldGridIndex = GetWorldGridIndex();
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			ProjectedPosition = GetProjectedPosition();

			// adjust the positions so we draw at the center of the Texture and at the correct camera position
			float drawPositionX = ProjectedPosition.X - Texture.Width * 0.5f - Camera.Position.X;
			float drawPositionY = ProjectedPosition.Y - Texture.Height * 0.25f - Camera.Position.Y;

			renderer.RenderTexture(Texture, drawPositionX, drawPositionY);
		}

		#endregion

		private void Move(GameTime gameTime)
		{
			if (State == AgentState.Active)
			{
				if (IsAtDestination())
					SetNextDestinationNode();

				if (!IsAtDestination())
					Move(gameTime.ElapsedGameTime.TotalSeconds);
				else
				{
					if (pathNodes != null)
					{
						if (pathNodes.Count == 0)
							ResetMovement();
						else
							Destination = pathNodes.Dequeue().WorldPosition;
					}
				}
			}
		}

		private void Move(double dt)
		{
			Activity = AgentActivity.Walking;
			Vector direction = GetMovementDirection();
			WorldPosition += new Vector((float)(direction.X * Speed.X * dt), (float)(direction.Y * Speed.Y * dt));
		}

		private void ResetMovement()
		{
			pathNodes = null;
			Destination = CoordinateHelper.DefaultVector;
			Activity = AgentActivity.Idle;
		}

		private bool IsAtDestination()
		{
			if (Destination == CoordinateHelper.DefaultVector || CollisionBox.Contains(new Point((int)Destination.X, (int)Destination.Y)))
				return true;
			else
				return false;
		}

		private void SetNextDestinationNode()
		{
			if (pathNodes != null)
				if (pathNodes.Count() > 0)
					Destination = pathNodes.Dequeue().WorldPosition;
		}
	}
}
