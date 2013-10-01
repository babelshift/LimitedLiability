using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public interface ICollidable
	{
		Rectangle CollisionBox { get; }
		Vector WorldPosition { get; }
		void ResolveCollision(ICollidable i);
	}
}
