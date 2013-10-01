using SharpDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using SharpDL.Input;

namespace MyThirdSDL
{
	public class Thing : Entity, ICollidable
	{
		public enum ThingStatus
		{
			Idle,
			Walking
		}

		private Texture texture;
		private Queue<MapObject> pathNodes = new Queue<MapObject>();

		public Vector Destination { get; private set; }
		public ThingStatus MyStatus { get; private set; }

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, texture.Width / 2, texture.Height / 2);
			}
		}

		public Thing(Vector position, Vector speed, Texture texture)
			: base(position, speed)
		{
			this.texture = texture;

			ProjectedPosition = CoordinateHelper.WorldSpaceToScreenSpace(
				WorldPosition.X, WorldPosition.Y, texture.Width / 2, texture.Height,
				CoordinateHelper.ScreenOffset,
				CoordinateHelper.ScreenProjectionType.Isometric
			);

			Vector worldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(
				WorldPosition.X,
				WorldPosition.Y, 
				CoordinateHelper.WorldGridCellWidth, 
				CoordinateHelper.WorldGridCellHeight
			);
			
			int worldGridIndexX = (int)Math.Round(worldGridIndex.X);
			int worldGridIndexY = (int)Math.Round(worldGridIndex.Y);
			WorldGridIndex = new Vector(worldGridIndexX + 1, worldGridIndexY + 1);
			WorldGridIndex = new Vector(worldGridIndexX, worldGridIndexY);

			Destination = CoordinateHelper.DefaultVector;
		}

		protected override void Move(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed = null)
		{
			if (Status == EntityStatus.Active)
			{
				if (Destination == CoordinateHelper.DefaultVector || CollisionBox.Contains(new Point((int)Destination.X, (int)Destination.Y)))
				{
					if (pathNodes != null)
					{
						if (pathNodes.Count() > 0)
						{
							MapObject pathNode = pathNodes.Dequeue();
							Destination = pathNode.WorldPosition;
						}
					}
				}

				if (Destination != CoordinateHelper.DefaultVector
					&& !CollisionBox.Contains(new Point((int)Destination.X, (int)Destination.Y)))
				{
					MyStatus = ThingStatus.Walking;
					Vector direction = GetMovementDirection();
					double dt = gameTime.ElapsedGameTime.TotalSeconds;
					WorldPosition += new Vector((float)(direction.X * Speed.X * dt), (float)(direction.Y * Speed.Y * dt));
				}
				else
				{
					if (pathNodes != null)
					{
						if (pathNodes.Count == 0)
						{
							pathNodes = null;
							Destination = CoordinateHelper.DefaultVector;
							MyStatus = ThingStatus.Idle;
						}
						else
							Destination = pathNodes.Dequeue().WorldPosition;
					}
				}
			}
		}

		public override void Update(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed = null)
		{
			Move(gameTime);

			Vector worldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(
				WorldPosition.X, 
				WorldPosition.Y, 
				CoordinateHelper.WorldGridCellWidth, 
				CoordinateHelper.WorldGridCellHeight
			);
			
			int worldGridIndexX = (int)Math.Round(worldGridIndex.X);
			int worldGridIndexY = (int)Math.Round(worldGridIndex.Y);
			//WorldGridIndex = new Vector(worldGridIndexX + 1, worldGridIndexY + 1);
			WorldGridIndex = new Vector(worldGridIndexX, worldGridIndexY);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			ProjectedPosition = CoordinateHelper.WorldSpaceToScreenSpace(
				WorldPosition.X, WorldPosition.Y, texture.Width / 2, texture.Height,
				CoordinateHelper.ScreenOffset,
				CoordinateHelper.ScreenProjectionType.Isometric
			);

			// adjust the positions so we draw at the center of the texture and at the correct camera position
			float drawPositionX = ProjectedPosition.X - texture.Width * 0.5f - Camera.Position.X;
			float drawPositionY = ProjectedPosition.Y - texture.Height * 0.25f - Camera.Position.Y;

			renderer.RenderTexture(texture, drawPositionX, drawPositionY);
		}

		protected override Vector GetMovementDirection(IEnumerable<KeyInformation.VirtualKeyCode> keysPressed = null)
		{
			if (Status == EntityStatus.Active)
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
		
		public void SetPath(Queue<MapObject> nodes)
		{
			if (pathNodes == null)
				pathNodes = nodes;
		}

		public void ResolveCollision(ICollidable collidableEntity) { }
	}
}
