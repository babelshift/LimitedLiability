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
		private Guid ID { get; private set; }
		private Texture Texture { get; private set; }
		private Vector Position { get; private set; }
		
		public void Update(GameTime gameTime)
		{
		}

		public void Draw(GameTime gameTime, Renderer renderer)
		{
		}
	}
}
