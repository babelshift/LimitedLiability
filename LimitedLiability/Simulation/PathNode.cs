using System;
using LimitedLiability.Content;
using System.Collections.Generic;
using SharpTiles;
using SharpDL.Graphics;

namespace LimitedLiability.Simulation
{
	public class PathNode : MapObject, INode, IHasNeighbors<PathNode>
	{
		private List<PathNode> neighbors = new List<PathNode>();

		public IEnumerable<PathNode> Neighbors { get { return neighbors; } }

		public bool IsEnabled { get; private set; }

		public PathNode(string name, Rectangle bounds, Orientation orientation, PropertyCollection properties)
			: base(name, bounds, orientation, LimitedLiability.Content.MapObjectType.PathNode, properties)
		{
			Enable();
		}

		public void AddNeighbor(PathNode pathNode)
		{
			if (pathNode != null)
				neighbors.Add(pathNode);
		}

		public void Enable()
		{
			IsEnabled = true;
		}

		public void Disable()
		{
			IsEnabled = false;
		}
	}
}

