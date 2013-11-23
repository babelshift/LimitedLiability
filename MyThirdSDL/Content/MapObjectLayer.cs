using SharpDL;
using SharpDL.Graphics;
using SharpTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Descriptors;
using MyThirdSDL.Simulation;

namespace MyThirdSDL.Content
{
	public class MapObjectLayer
	{
		private List<MapObject> mapObjects = new List<MapObject>();

		public string Name { get; private set; }

		public MapObjectLayerType Type { get; private set; }

		public IEnumerable<MapObject> MapObjects { get { return mapObjects; } }

		public MapObjectLayer(string name, MapObjectLayerType type)
		{
			Name = name;
			Type = type;
		}

		public IEnumerable<PathNode> GetNeighboringPathNodes(PathNode pathNode)
		{
			List<PathNode> neighboringPathNodes = new List<PathNode>();

			PathNode leftNeighboringPathNode = GetNeighboringPathNode(mo => mo.Bounds.Right == pathNode.Bounds.Left && mo.Bounds.Bottom == pathNode.Bounds.Bottom);
			if (leftNeighboringPathNode != null)
				neighboringPathNodes.Add(leftNeighboringPathNode);

			PathNode rightNeighboringPathNode =  GetNeighboringPathNode(mo => mo.Bounds.Left == pathNode.Bounds.Right && mo.Bounds.Bottom == pathNode.Bounds.Bottom);
			if (rightNeighboringPathNode != null)
				neighboringPathNodes.Add(rightNeighboringPathNode);

			PathNode topNeighboringPathNode =  GetNeighboringPathNode(mo => mo.Bounds.Bottom == pathNode.Bounds.Top && mo.Bounds.Left == pathNode.Bounds.Left);
			if (topNeighboringPathNode != null)
				neighboringPathNodes.Add(topNeighboringPathNode);

			PathNode bottomNeighboringPathNode =  GetNeighboringPathNode(mo => mo.Bounds.Top == pathNode.Bounds.Bottom && mo.Bounds.Left == pathNode.Bounds.Left);
			if (bottomNeighboringPathNode != null)
				neighboringPathNodes.Add(bottomNeighboringPathNode);

			return neighboringPathNodes;
		}

		private PathNode GetNeighboringPathNode(Func<MapObject, bool> predicate)
		{
			MapObject neighboringMapObject = MapObjects.FirstOrDefault(predicate);
			if (neighboringMapObject != null)
				return (PathNode)neighboringMapObject;
			else
				return null;
		}

		public MapObject GetMapObjectContainingPoint(Point point)
		{
			MapObject mapObject = MapObjects.FirstOrDefault(mo => mo.Bounds.Contains(point));
			return mapObject;
		}

		public void AddMapObject(MapObject mapObject)
		{
			mapObjects.Add(mapObject);
		}
	}
}
