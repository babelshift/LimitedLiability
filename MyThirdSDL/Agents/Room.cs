using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyThirdSDL.Agents
{
	public class Room : IPurchasable
	{
		private TiledMap tiledMap;

		public NecessityEffect NecessityEffect { get; protected set; }

		public SkillEffect SkillEffect { get; protected set; }

		public string Name { get; private set; }

		public string Description { get; private set; }

		public int Price { get; private set; }

		public string IconTextureKey { get; private set; }

		public int Width { get { return tiledMap.PixelWidth; } }

		public int Height { get { return tiledMap.PixelHeight; } }

		public int HorizontalMapCellCount
		{
			get { return tiledMap.HorizontalTileCount; }
		}

		public int VerticalMapCellCount
		{
			get { return tiledMap.VerticalTileCount; }
		}

		public IEnumerable<Equipment> EquipmentOccupants
		{
			get
			{
				return tiledMap.MapCells
					.Where(mc => mc.OccupantEquipment != null)
					.Select(mc => mc.OccupantEquipment);
			}
		}

		public IEnumerable<MapCell> MapCells { get { return tiledMap.MapCells; } }

		public MapCell OriginMapCell { get { return tiledMap.OriginMapCell; } }

		public Room(string name, int price, string description, string iconTextureKey, TiledMap tiledMap)
		{
			Name = name;
			Price = price;
			IconTextureKey = iconTextureKey;
			this.tiledMap = tiledMap;
			Description = description;
		}

		public void Draw(GameTime gameTime, Renderer renderer, int x, int y, bool isOverlappingDeadZone)
		{
			tiledMap.Draw(gameTime, renderer, x, y, isOverlappingDeadZone);
		}

		public bool IsOverlappingDeadZone(IReadOnlyList<MapCell> mapCells)
		{
			if (mapCells == null) throw new ArgumentNullException("mapCells");

			if (mapCells[0] == null) return true;

			// mapCells = the cells that we have hovered over
			// need to check if our map cells overlap with any deadzones in the hovered map cells
			// problem is: our map cells are origined at 0,0 and simply drawn at an offset
			// mapCell[0] is origin

			Vector origin = mapCells[0].WorldPosition;

			foreach (var mapCell in tiledMap.MapCells)
			{
				Vector offsetPosition = new Vector(mapCell.WorldPosition.X + origin.X, mapCell.WorldPosition.Y + origin.Y);

				bool isOverlappingDeadZone = mapCells
					.Where(mc => mc != null)
					.Where(mc => mc.Type == MapCellType.DeadZone)
					.Any(mc => mc.Bounds.Contains(offsetPosition));

				if (isOverlappingDeadZone)
					return true;
			}

			return false;
		}
	}
}