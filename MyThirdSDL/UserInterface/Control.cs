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
		private Guid ID { get; set; }
		protected Texture Texture { get; set; }
		public virtual Vector Position { get; set; }

		protected Rectangle Bounds
		{
			get
			{
				return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
			}
		}

		public Control(Texture texture, Vector position)
		{
			ID = Guid.NewGuid();
			Texture = texture;
			Position = position;
		}

		public virtual void Update(GameTime gameTime)
		{

		}

		public virtual void Draw(GameTime gameTime, Renderer renderer)
		{
			if(Texture != null)
				renderer.RenderTexture(Texture, Position.X, Position.Y);
		}
	}
}
