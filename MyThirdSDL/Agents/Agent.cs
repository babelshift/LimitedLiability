using SharpDL;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.Agents
{
	public abstract class Agent : ICollidable, IDrawable
	{
		#region Members

		private TimeSpan SimulationBirthTime { get; set; }

		#endregion Members

		#region Properties

		protected Texture ActiveTexture { get; set; }

		public Guid ID { get; private set; }

		public string Name { get; private set; }

		public TimeSpan SimulationAge { get; private set; }

		public AgentState State { get; private set; }

		public Vector WorldPosition { get; protected set; }

		public float Depth { get { return WorldPosition.X + WorldPosition.Y; } }

		public Vector ProjectedPosition
		{
			get
			{
				Vector projectedPosition = CoordinateHelper.WorldSpaceToScreenSpace(
					WorldPosition.X,
					WorldPosition.Y,
					CoordinateHelper.ScreenOffset,
					CoordinateHelper.ScreenProjectionType.Orthogonal
				);

				return projectedPosition;
			}
		}

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, ActiveTexture.Width, ActiveTexture.Height);
			}
		}

		#endregion Properties

		#region Constructors

		protected Agent(TimeSpan birthTime, string name, Vector startingPosition)
		{
			ID = Guid.NewGuid();
			SimulationBirthTime = birthTime;
			Name = name;
			State = AgentState.Unknown;
			WorldPosition = startingPosition;
		}

		#endregion Constructors

		#region Utilities

		private void SetSimulationAge(TimeSpan simulationTime)
		{
			SimulationAge = simulationTime.Subtract(SimulationBirthTime);
		}

		#endregion Utilities

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

		#endregion Status Changes

		#region Game Loop

		public virtual void Update(GameTime gameTime)
		{
			SetSimulationAge(gameTime.TotalGameTime);
		}

		public virtual void Draw(GameTime gameTime, Renderer renderer)
		{
			renderer.RenderTexture(
				ActiveTexture,
				ProjectedPosition.X - Camera.Position.X,
				ProjectedPosition.Y - Camera.Position.Y
			);
		}

		#endregion Game Loop
	}
}