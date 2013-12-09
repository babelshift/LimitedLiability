using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Simulation;

namespace MyThirdSDL.Agents
{
	public abstract class Agent : IDrawable, ICollidable
	{
		#region Members

		public Texture Texture { get; private set; }

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, Texture.Width / 2, Texture.Height / 2);
			}
		}

		private TimeSpan SimulationBirthTime { get; set; }

		#endregion

		#region Properties

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
					CoordinateHelper.ScreenProjectionType.Isometric
				);

				return projectedPosition;
			}
		}

		#endregion

		#region Constructors

		public Agent(TimeSpan birthTime, string name, Texture texture, Vector startingPosition)
		{
			ID = Guid.NewGuid();
			SimulationBirthTime = birthTime;
			Name = name;
			Texture = texture;
			State = AgentState.Unknown;
			WorldPosition = startingPosition;
		}

		#endregion

		#region Utilities

		private void SetSimulationAge(TimeSpan simulationTime)
		{
			SimulationAge = simulationTime.Subtract(SimulationBirthTime);
		}

		#endregion

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

		public virtual void Update(GameTime gameTime)
		{
			SetSimulationAge(gameTime.TotalGameTime);
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
			// adjust the positions so we draw at the center of the Texture and at the correct camera position
			Vector drawPosition = CoordinateHelper.ProjectedPositionToDrawPosition(ProjectedPosition);
			renderer.RenderTexture(Texture, drawPosition.X, drawPosition.Y);
		}

		#endregion

	}
}
