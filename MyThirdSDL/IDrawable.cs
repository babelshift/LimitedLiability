using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public interface IDrawable
	{
		Point WorldGridIndex { get; }
		Vector WorldPosition { get; }
		Vector ProjectedPosition { get; }
		float Depth { get; }
		void Draw(GameTime gameTime, Renderer renderer);
	}
}
