using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public interface IDrawable : IDisposable
	{
		Guid ID { get; }
		Vector WorldPosition { get; }
		Vector ProjectedPosition { get; }
		float Depth { get; }
		void Draw(GameTime gameTime, Renderer renderer);
		void Draw(GameTime gameTime, Renderer renderer, int x, int y, bool isOverlappingDeadZone);
	}
}
