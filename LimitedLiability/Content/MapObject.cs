using SharpDL.Graphics;
using SharpTiles;

namespace LimitedLiability.Content
{
	public class MapObject
	{
		public string Name { get; private set; }

		public MapObjectType Type { get; private set; }

		public Rectangle Bounds { get; private set; }

		/// <summary>
		/// World position is the position within world space that the object exists. This position is defined within the .tmx tiled map file. When in isometric,
		/// it seems that the first rows of the collection are actually shifted by -32 in both X and Y directions.
		/// For example, in an isometric map with tile dimensions 64x32:
		///		The tile at [0,0] is positioned at [-32,-32].
		///		The tile at [0,1] is positioned at [-32,0].
		///		The tile at [1,0] is positioned at [0,-32].
		///	Because of that craziness, I am forced to shift the X and Y coordinates in the positive direction by the bounds and height, respectively.
		///	For example:
		///		The tile at [0,0] will be positioned (when shifted) at [0, 0].
		///		The tile at [0,1] will be positioned (when shifted) at [0, 32].
		///		The tile at [1,0] will be positioned (when shifted) at [32, 0].
		/// </summary>
		public Vector WorldPosition { get { return new Vector(Bounds.X, Bounds.Y); } }

		public Orientation Orientation { get; private set; }

		public PropertyCollection Properties { get; private set; }

		public MapObject(string name, Rectangle bounds, Orientation orientation, MapObjectType type, PropertyCollection properties)
		{
			Name = name;
			Type = type;
			Bounds = bounds;
			Orientation = orientation;
			Properties = properties;
		}
	}
}