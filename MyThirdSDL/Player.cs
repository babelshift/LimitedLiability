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
	public class Player : Entity, ICollidable, IDrawable
	{
		private Texture texture;
		private bool isMovingLeft;
		private bool isMovingRight;
		private bool isMovingDown;
		private bool isMovingUp;
		private Rectangle previousCollisionBox;

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y, texture.Width / 2, texture.Height / 2);
			}
		}

		public Player(Vector Position, Vector speed, Texture texture)
			: base(Position, speed)
		{
			this.texture = texture;

			ProjectedPosition = CoordinateHelper.WorldSpaceToScreenSpace(
				WorldPosition.X, WorldPosition.Y, texture.Width / 2, texture.Height,
				CoordinateHelper.ScreenOffset,
				CoordinateHelper.ScreenProjectionType.Isometric
			);
		}

		public override void Update(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed = null)
		{
			Move(gameTime, keysPressed);
			
			// TODO: THIS SHOULD BE BASED ON TILE WIDTH / HEIGHT
			WorldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(WorldPosition.X, WorldPosition.Y, texture.Width / 2, texture.Height);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			ProjectedPosition = CoordinateHelper.WorldSpaceToScreenSpace(
				WorldPosition.X, WorldPosition.Y, texture.Width / 2, texture.Height,
				CoordinateHelper.ScreenOffset, 
				CoordinateHelper.ScreenProjectionType.Isometric
			);

			float drawPositionX = ProjectedPosition.X - texture.Width * 0.5f;
			float drawPositionY = ProjectedPosition.Y - texture.Height * 0.25f;

			renderer.RenderTexture(texture, drawPositionX, drawPositionY);
		}

		protected override void Move(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed = null)
		{
			if (Status == EntityStatus.Active)
			{
				Vector movementDirection = GetMovementDirection(keysPressed);
				double dt = gameTime.ElapsedGameTime.TotalSeconds;
				WorldPosition += new Vector((float)(movementDirection.X * Speed.X * dt), (float)(movementDirection.Y * Speed.Y * dt));
			}
		}

		protected override Vector GetMovementDirection(IEnumerable<KeyInformation.VirtualKeyCode> keysPressed)
		{
			if (Status == EntityStatus.Active)
			{
				float movementDirectionX = 0f;
				float movementDirectionY = 0f;

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.W))
				{
					isMovingUp = true;
					movementDirectionY -= 1f;
				}

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.A))
				{
					isMovingLeft = true;
					movementDirectionX -= 1f;
				}

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.S))
				{
					isMovingDown = true;
					movementDirectionY += 1f;
				}

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.D))
				{
					isMovingRight = true;
					movementDirectionX += 1f;
				}

				return new Vector(movementDirectionX, movementDirectionY);
			}
			else
				return Vector.Zero;
		}

		public void SaveCollisionBox()
		{
			previousCollisionBox = CollisionBox;
		}

		public void ResolveCollision(ICollidable collidableEntity)
		{
			if (collidableEntity is MapObject)
			{
				MapObject collidableMapObject = collidableEntity as MapObject;

				//if (collidableMapObject.Type == MapObjectType.Collidable)
				//{
					// calculate how deep the intersection is
					Vector collisionDepth = CollisionBox.GetIntersectionDepth(collidableMapObject.CollisionBox);

					// no intersection, so no collision!
					if (collisionDepth != Vector.Zero)
					{
						float absDepthX = Math.Abs(collisionDepth.X);
						float absDepthY = Math.Abs(collisionDepth.Y);

						// this offset will push the entity 1px beyond the depth correction
						float offsetY = collisionDepth.Y < 0 ? -1f : 1f;
						float offsetX = collisionDepth.X < 0 ? -1f : 1f;

						// this vector resolves collision in the Y direction
						// entity keeps same X coordinate but moves from current Y to Y + the intersection depth correction factor + the offset
						Vector resolutionPositionY = new Vector(WorldPosition.X, WorldPosition.Y + collisionDepth.Y + offsetY);

						// this vector resolves collision in the X direction
						// entity keeps same Y coordinate but moves from current X to X + the intersection depth correction factor + the offset
						Vector resolutionPositionX = new Vector(WorldPosition.X + collisionDepth.X + offsetX, WorldPosition.Y);

						// collision is less severe in the Y direction, so correct in favor of Y
						if (absDepthY < absDepthX)
							WorldPosition = resolutionPositionY;
						// collision is less sever in the X direction, so correct in favor of X
						else if (absDepthX < absDepthY)
							WorldPosition = resolutionPositionX;
						// collision is equally severe in both the X and the Y directions, so we need to determine which direction the player is moving and
						// on which side of the boxes the collision is occurring
						else
						{
							if (isMovingDown && isMovingLeft)
							{
								// our bottom passed the top of a tile, we hit the floor, correct us above the floor
								if (previousCollisionBox.Bottom <= collidableMapObject.CollisionBox.Top)
									WorldPosition = resolutionPositionY;
								// our left passed the right of a tile, we hit the left wall, correct us to the right of the wall
								else if (previousCollisionBox.Left <= collidableMapObject.CollisionBox.Right)
									WorldPosition = resolutionPositionX;
							}
							else if (isMovingUp && isMovingLeft)
							{
								// our top passed the bottom of a tile, we hit the ceiling, correct us below the ceiling
								if (previousCollisionBox.Top <= collidableMapObject.CollisionBox.Bottom)
									WorldPosition = resolutionPositionY;
								// our left passed the right of a tile, we hit the left wall, correct us to the right of the wall
								else if (previousCollisionBox.Left <= collidableMapObject.CollisionBox.Right)
									WorldPosition = resolutionPositionX;
							}
							else if (isMovingUp && isMovingRight)
							{
								// our bottom passed the top of a tile, we hit the floor, correct us above the floor
								if (previousCollisionBox.Top <= collidableMapObject.CollisionBox.Bottom)
									WorldPosition = resolutionPositionY;
								// our right passed the left of a tile, we hit the right wall, correct us to the left of the wall
								else if (previousCollisionBox.Right >= collidableMapObject.CollisionBox.Left)
									WorldPosition = resolutionPositionX;
							}
							else if (isMovingDown && isMovingRight)
							{
								// our top passed the bottom of a tile, we hit the ceiling, correct us below the ceiling
								if (previousCollisionBox.Bottom <= collidableMapObject.CollisionBox.Top)
									WorldPosition = resolutionPositionY;
								// our right passed the left of a tile, we hit the right wall, correct us to the left of the wall
								else if (previousCollisionBox.Right >= collidableMapObject.CollisionBox.Left)
									WorldPosition = resolutionPositionX;
							}
						}
					}
				//}
			}
		}
	}
}
