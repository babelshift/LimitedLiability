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
	/// <summary>A TileLayer is a representation of a tile layer in a .tmx file. These layers contain Tile objects which can be accessed
	/// in order to render the textures associated with the tiles.
	/// </summary>
	public class TileLayer : IDisposable
	{
		private List<Tile> tiles = new List<Tile>();

		public string Name { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public IList<Tile> Tiles { get { return tiles; } }

		public TileLayer(string name, int width, int height)
		{
			Name = name;
			Width = width;
			Height = height;
		}

		public void AddTile(Tile tile)
		{
			tiles.Add(tile);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~TileLayer()
		{
			Dispose(false);
		}

		private void Dispose(bool isDisposing)
		{
			foreach (Tile tile in tiles)
				tile.Dispose();
		}
	}
	
}
