using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public abstract class Control
	{
		public Guid ID { get; private set; }

		protected Rectangle Bounds
		{
			get
			{
				return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
			}
		}

		public virtual Vector Position { get; set; }

		public int Width { get; protected set; }

		public int Height { get; protected set; }

		public Control()
		{
			ID = Guid.NewGuid();
		}

		public abstract void Update(GameTime gameTime);

		public abstract void Draw(GameTime gameTime, Renderer renderer);
	}
}
