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
	/// <summary>A Tile is a representation of a Tile in a .tmx file. These tiles contain textures and positions in order to render the map properly.
	/// </summary>
	public class Tile : IDisposable, IDrawable
	{
		public static int EmptyTileID = 0;

		public Guid ID { get; private set; }

		/// <summary>
		/// World grid is the world space tile grid. This is the index within the world grid that the tile is positioned.
		/// </summary>
		public Point WorldGridIndex { get; internal set; }

		/// <summary>
		/// World position is the position within the world space
		/// </summary>
		public Vector WorldPosition { get; internal set; }

		/// <summary>
		/// Projected position is the calculated position for rendering based on world space position
		/// </summary>
		public Vector ProjectedPosition { get; internal set; }

		/// <summary>
		/// Depth determines the rendering order of the tile as compared to other drawable objects with depth
		/// </summary>
		public float Depth { get { return WorldPosition.X + WorldPosition.Y; } }

		/// <summary>
		/// Width of the tile in pixels
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// Height of the tile in pixels
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Texture from which to select a rectangle source texture (similar to selecting from a sprite sheet)
		/// </summary>
		public Texture Texture { get; private set; }

		/// <summary>
		/// Rectangle determining where in the Texture to select the specific texture for our sprite or frame
		/// </summary>
		private Rectangle SourceTextureBounds { get; set; }

		/// <summary>
		/// Tiles are empty if they have no texture assigned within Tiled Map Editor
		/// </summary>
		public bool IsEmpty { get; private set; }

		public TileType Type { get; private set; }

		/// <summary>
		/// Default empty constructor creates an empty tile
		/// </summary>
		public Tile()
		{
			ID = Guid.NewGuid();
			IsEmpty = true;
		}

		/// <summary>
		/// Main constructor used to instantiate a tile when data is known at import
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="source"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public Tile(Texture texture, Rectangle source, int width, int height, TileType type)
		{
			Type = type;
			IsEmpty = false;
			Texture = texture;
			SourceTextureBounds = source;
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Draws the tile to the passed renderer if the tile is not empty. The draw will occur at the center of the tile's texture.
		/// TODO: Allow passing of origin?
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="renderer"></param>
		public void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!IsEmpty)
			{
				renderer.RenderTexture(
					Texture,
					ProjectedPosition.X - Camera.Position.X,
					ProjectedPosition.Y - Camera.Position.Y,
					SourceTextureBounds
				);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Tile()
		{
			Dispose(false);
		}

		private void Dispose(bool isDisposing)
		{
			if (Texture != null)
				Texture.Dispose();
		}
	}

}
