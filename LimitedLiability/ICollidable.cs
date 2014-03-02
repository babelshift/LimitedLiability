using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability
{
	public interface ICollidable
	{
		Rectangle CollisionBox { get; }
	}
}
