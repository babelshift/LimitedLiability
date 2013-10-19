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
		private Texture Texture { get; set; }
		private Vector Position { get; set; }

		public Control(Texture texture, Vector position)
		{
			ID = Guid.NewGuid();
			Texture = texture;
			Position = position;
		}

		public void Update(GameTime gameTime)
		{

		}

		public virtual void Draw(GameTime gameTime, Renderer renderer)
		{
			if(Texture != null)
				renderer.RenderTexture(Texture, Position.X, Position.Y);
		}
	}
}
