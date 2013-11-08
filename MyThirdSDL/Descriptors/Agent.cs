using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public abstract class Agent : IDrawable, ICollidable
	{
		public enum AgentState
		{
			Unknown,
			Active,
			Inactive
		}

		#region Members 

		private Texture Texture { get; set; }

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, Texture.Width / 2, Texture.Height / 2);
			}
		}

		private TimeSpan BirthTime { get; set; }

		#endregion

		#region Properties

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

		public Vector WorldGridIndex { get; private set; }

		public Vector WorldPosition { get; protected set; }

		public float Depth { get { return WorldPosition.X + WorldPosition.Y; } }

		public Vector ProjectedPosition { get; private set; }

		#endregion

		#region Constructors

		public Agent(TimeSpan birthTime, string name, Texture texture, Vector startingPosition)
		{
			ID = Guid.NewGuid();
			BirthTime = birthTime;
			Name = name;
			Texture = texture;
			State = AgentState.Unknown;

			WorldPosition = startingPosition;
			ProjectedPosition = GetProjectedPosition();
			WorldGridIndex = GetWorldGridIndex();
		}

		#endregion

		#region Utilities

		private void SetSimulationAge(TimeSpan simulationTime)
		{
			SimulationAge = simulationTime.Subtract(BirthTime);
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

	}
}
