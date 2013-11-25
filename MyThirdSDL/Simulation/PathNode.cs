using System;
using MyThirdSDL.Content;
using System.Collections.Generic;
using SharpTiles;
using SharpDL.Graphics;

namespace MyThirdSDL.Simulation
{
	public class PathNode : MapObject, INode, IHasNeighbors<PathNode>
	{
		private List<PathNode> neighbors = new List<PathNode>();

		public IEnumerable<PathNode> Neighbors { get { return neighbors; } }

		public bool IsEnabled { get; private set; }

		public PathNode(string name, Rectangle bounds, Orientation orientation)
			: base(name, bounds, orientation, MyThirdSDL.Content.MapObjectType.PathNode)
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

