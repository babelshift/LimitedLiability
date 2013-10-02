using SharpDL;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public enum EntityStatus
	{
		Active,
		Inactive,
	}

	public abstract class Entity
	{
		public Vector WorldGridIndex { get; protected set; }
		public Vector WorldPosition { get; protected set; }
		public Vector ProjectedPosition { get; protected set; }
		public float Depth { get { return WorldPosition.X + WorldPosition.Y; } }
		public EntityStatus Status { get; private set; }
		public Vector Speed { get; protected set; }

		public Entity(Vector position, Vector speed)
		{
			Status = EntityStatus.Inactive;
			WorldPosition = position;
			Speed = speed;
		}

		public void Activate()
		{
			if (Status != EntityStatus.Active)
				Status = EntityStatus.Active;
		}

		public void ChangeStatus(EntityStatus status)
		{
			if(Status != status)
				Status = status;
		}

		protected abstract Vector GetMovementDirection(IEnumerable<KeyInformation.VirtualKeyCode> keysPressed = null);
		protected abstract void Move(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed = null);
		public abstract void Update(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed = null);
		public abstract void Draw(GameTime gameTime, Renderer renderer);
	}
}
