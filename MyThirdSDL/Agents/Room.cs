using System.Collections.Generic;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;
using System;
using System.Linq;

namespace MyThirdSDL.Agents
{
	public class Room : IPurchasable
	{
		private readonly MapCell[,] mapCells;

		public NecessityEffect NecessityEffect { get; protected set; }

		public SkillEffect SkillEffect { get; protected set; }

		public IReadOnlyList<Texture> ActiveTextures
		{
			get
			{
				List<Texture> textures = new List<Texture>();
				foreach (var mapCell in mapCells)
					textures.Add(mapCell.BaseTexture);
				return textures;
			}
		}

		public string Name { get; private set; }

		public int Price { get; private set; }

		public string IconTextureKey { get; private set; }

		public int Width { get { return mapCells.GetLength(0); } }

		public int Height { get { return mapCells.GetLength(1); } }

		public Room(string name, int price, int width, int height, string iconTextureKey)
		{
			Name = name;
			Price = price;
			IconTextureKey = iconTextureKey;
			mapCells = new MapCell[width, height];
		}

		protected void AddMapCell(MapCell mapCell, int rowIndex, int columnIndex)
		{
			if (rowIndex < 0)
				throw new ArgumentOutOfRangeException("rowIndex", "Row index must be greater than 0.");

			if (columnIndex < 0)
				throw new ArgumentOutOfRangeException("columnIndex", "Column index must be greater than 0.");

			if (rowIndex >= Width)
				throw new ArgumentOutOfRangeException("rowIndex", "Row index cannot exceed the width of the array.");

			if (columnIndex >= Height)
				throw new ArgumentOutOfRangeException("columnIndex", "Column index cannot exceed the height of the array.");

			mapCells[rowIndex, columnIndex] = mapCell;
		}
	}
}