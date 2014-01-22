﻿using SharpDL;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.Agents
{
	public enum AgentOrientation
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	public abstract class Agent : IDrawable, ICollidable
	{
		#region Members

		private readonly TextureBook textureBook;
		private AgentOrientation orientation;

		private TimeSpan SimulationBirthTime { get; set; }

		#endregion Members

		#region Properties

		public Texture ActiveTexture { get { return textureBook.ActiveTexture; } }

		public AgentOrientation Orientation
		{
			get { return orientation; }
			set
			{
				orientation = value;

				textureBook.SetOrientation(orientation);
			}
		}

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, ActiveTexture.Width, ActiveTexture.Height);
			}
		}

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

		#endregion Properties

		#region Constructors

		protected Agent(TimeSpan birthTime, string name, TextureBook textureBook, Vector startingPosition, AgentOrientation orientation)
		{
			ID = Guid.NewGuid();
			SimulationBirthTime = birthTime;
			Name = name;
			this.textureBook = textureBook;
			State = AgentState.Unknown;
			WorldPosition = startingPosition;
			Orientation = orientation;
		}

		#endregion Constructors

		#region Utilities

		public void Rotate()
		{
			if (Orientation == AgentOrientation.TopLeft)
				Orientation = AgentOrientation.TopRight;
			else if (Orientation == AgentOrientation.TopRight)
				Orientation = AgentOrientation.BottomRight;
			else if (Orientation == AgentOrientation.BottomRight)
				Orientation = AgentOrientation.BottomLeft;
			else if (Orientation == AgentOrientation.BottomLeft)
				Orientation = AgentOrientation.TopLeft;
		}

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