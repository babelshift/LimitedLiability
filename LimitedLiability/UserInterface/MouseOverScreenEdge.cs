using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using LimitedLiability.Descriptors;
using SharpDL.Events;
using SharpDL.Input;
using LimitedLiability.Content;
using LimitedLiability.Simulation;

namespace LimitedLiability.UserInterface
{
	public enum MouseOverScreenEdge
	{
		Unknown,
		None,
		Top,
		Bottom,
		Left,
		Right,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}
}
