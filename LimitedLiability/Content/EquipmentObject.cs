using SharpDL.Graphics;
using SharpTiles;

namespace LimitedLiability.Content
{
	public class EquipmentObject : MapObject
	{
		public EquipmentObjectType Subtype { get; private set; }

		public EquipmentObject(string name, Rectangle bounds, Orientation orientation,
			PropertyCollection properties, EquipmentObjectType subtype)
			: base(name, bounds, orientation, MapObjectType.Equipment, properties)
		{
			Subtype = subtype;
		}
	}
}