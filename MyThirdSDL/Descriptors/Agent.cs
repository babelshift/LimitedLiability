using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public enum AgentStatus
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
		public Guid ID { get; private set; }
		public string AgentName { get; private set; }
		public double SimulationAge { get; private set; }
		public AgentStatus Status { get; private set; }
		public AgentActivity Activity { get; private set; }

		public Vector WorldGridIndex { get; private set; }
		public Vector WorldPosition { get; protected set; }
		public float Depth { get { return WorldPosition.X + WorldPosition.Y; } }
		public Vector Speed { get; private set; }

		private Texture Texture { get; set; }
		public Vector ProjectedPosition { get; private set; }
		private Vector Destination { get; set; }

		private Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, Texture.Width / 2, Texture.Height / 2);
			}
		}

		private Queue<MapObject> pathNodes;

		public Agent(string name, Texture texture, Vector startingPosition, Vector startingSpeed)
		{
			ID = Guid.NewGuid();
			AgentName = name;
			SimulationAge = 0.0;
			Status = AgentStatus.Unknown;
			Activity = AgentActivity.Unknown;

			WorldPosition = startingPosition;
			Speed = startingSpeed;
			Texture = texture;

			ProjectedPosition = GetProjectedPosition();
			WorldGridIndex = GetWorldGridIndex();
			Destination = CoordinateHelper.DefaultVector;
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
			if (Status == AgentStatus.Active)
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
			ChangeStatus(AgentStatus.Active);
		}

		public void Deactivate()
		{
			ChangeStatus(AgentStatus.Inactive);
		}

		protected void ChangeStatus(AgentStatus status)
		{
			if (Status != status)
				Status = status;
		}

		#endregion

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

		private void Move(GameTime gameTime)
		{
			if (Status == AgentStatus.Active)
			{
				if (IsAtDestination())
					GetNextDestinationNode();

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

		private void ResetMovement()
		{
			pathNodes = null;
			Destination = CoordinateHelper.DefaultVector;
			Activity = AgentActivity.Idle;
		}

		private void Move(double dt)
		{
			Activity = AgentActivity.Walking;
			Vector direction = GetMovementDirection();
			WorldPosition += new Vector((float)(direction.X * Speed.X * dt), (float)(direction.Y * Speed.Y * dt));
		}

		private bool IsAtDestination()
		{
			if (Destination == CoordinateHelper.DefaultVector || CollisionBox.Contains(new Point((int)Destination.X, (int)Destination.Y)))
				return true;
			else
				return false;
		}

		private void GetNextDestinationNode()
		{
			if (pathNodes != null)
				if (pathNodes.Count() > 0)
					Destination = pathNodes.Dequeue().WorldPosition;
		}
	}
}
