﻿using MyThirdSDL.Descriptors;
using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public class MainGame : Game
	{
		private const int SCREEN_WIDTH = 1440;
		private const int SCREEN_HEIGHT = 900;

		private Vector orthoMouseWorldPosition = Vector.Zero;
		private Vector isoMouseWorldGridIndex = Vector.Zero;
		private Vector isoMouseClickWorldGridIndex = CoordinateHelper.DefaultVector;

		private List<KeyInformation.VirtualKeyCode> keysPressed = new List<KeyInformation.VirtualKeyCode>();

		/// <summary>
		/// By default, the constructor does nothing. Something to do is subscribe to various game events.
		/// </summary>
		public MainGame()
		{
			KeyPressed += MainGame_KeyPressed;
			KeyReleased += MainGame_KeyReleased;
			MouseMoving += MainGame_MouseMoving;
			MouseButtonPressed += MainGame_MouseButtonPressed;
		}

		private void MainGame_MouseButtonPressed(object sender, MouseButtonEventArgs e)
		{
			if (e.MouseButton == MouseButtonEventArgs.MouseButtonCode.Left)
			{
				isoMouseClickWorldGridIndex = isoMouseWorldGridIndex;
				hasPathPossiblyChanged = true;
			}
		}

		public enum MouseOverScreenEdge
		{
			None,
			Top,
			Bottom,
			Left,
			Right
		}

		private MouseOverScreenEdge mouseOverScreenEdge;

		private void MainGame_MouseMoving(object sender, MouseMotionEventArgs e)
		{
			int mouseX = e.RelativeToWindowX;
			int mouseY = e.RelativeToWindowY;

			Vector worldSpace = CoordinateHelper.ScreenSpaceToWorldSpace(mouseX, mouseY, CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
			isoMouseWorldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(worldSpace.X, worldSpace.Y, tiledMap.TileWidth / 2, tiledMap.TileHeight);
			orthoMouseWorldPosition = new Vector(mouseX, mouseY);

			if (mouseX < 50)
				mouseOverScreenEdge = MouseOverScreenEdge.Left;
			else if (mouseX > SCREEN_WIDTH - 50)
				mouseOverScreenEdge = MouseOverScreenEdge.Right;
			else if (mouseY < 50)
				mouseOverScreenEdge = MouseOverScreenEdge.Top;
			else if (mouseY > SCREEN_HEIGHT - 50)
				mouseOverScreenEdge = MouseOverScreenEdge.Bottom;
			else
				mouseOverScreenEdge = MouseOverScreenEdge.None;
		}

		private void MainGame_KeyPressed(object sender, KeyboardEventArgs e)
		{
			if (!keysPressed.Contains(e.KeyInformation.VirtualKey))
				keysPressed.Add(e.KeyInformation.VirtualKey);
		}

		private void MainGame_KeyReleased(object sender, KeyboardEventArgs e)
		{
			//keysPressed.Add(e.KeyInformation.VirtualKey);
			//player.Update(null, keysPressed);
			//keysPressed.Remove(e.KeyInformation.VirtualKey);
			if (keysPressed.Contains(e.KeyInformation.VirtualKey))
				keysPressed.Remove(e.KeyInformation.VirtualKey);
		}

		/// <summary>
		/// Initialize the SDL Window and SDL Renderer with any required flags. 
		/// Also initialize anything else of interest (SDL_ttf, SDL_image, etc).
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			CreateWindow("My Third SDL", 100, 100, SCREEN_WIDTH, SCREEN_HEIGHT, WindowFlags.Shown);
			CreateRenderer(RendererFlags.RendererAccelerated);

			Camera.Position = Vector.Zero;
		}

		private TiledMap tiledMap;
		private CollisionManager collisionManager;

		/// <summary>
		/// Load any content that you will need to use in the update/draw game loop.
		/// </summary>
		protected override void LoadContent()
		{
			base.LoadContent();

			tiledMap = new TiledMap("Maps/Map2.tmx", Renderer);

			collisionManager = new CollisionManager(tiledMap.Width, tiledMap.Height);

			//Surface playerSurface = new Surface("Images/playerIsoBox.png", Surface.SurfaceType.PNG);
			//player = new Player(new Vector(100, 100), new Vector(200, 200), new Texture(Renderer, playerSurface));
			//player.Activate();
			
			Surface thingSurface = new Surface("Images/thing.png", Surface.SurfaceType.PNG);
			Texture thingTexture = new Texture(Renderer, thingSurface);
			Job job = new Job("Systems Engineer", 75000, Skills.Rating.Great, Skills.Rating.Bad, Skills.Rating.Neutral, Skills.Rating.Neutral);
			employee = new Employee("Employee 1", thingTexture, new Vector(100, 100), "Justin", "Skiles", 29, new DateTime(1984, 9, 9), job);
			employee.Activate();

			Surface tileSurface = new Surface("Images/tileCollisionBox.png", Surface.SurfaceType.PNG);
			tileCollisionBoxImage = new Image(Renderer, tileSurface, Image.ImageFormat.PNG);

			Surface tileHighlightSurface = new Surface("Images/tileHighlight2.png", Surface.SurfaceType.PNG);
			tileHighlightImage = new Image(Renderer, tileHighlightSurface, Image.ImageFormat.PNG);

			Surface tileHightlightSelectedSurface = new Surface("Images/tileHighlightSelected.png", Surface.SurfaceType.PNG);
			tileHighlightSelectedImage = new Image(Renderer, tileHightlightSelectedSurface, Image.ImageFormat.PNG);

			string isoWorldText = String.Format("(Iso) WorldX: {0}, WorldY: {1}", isoMouseWorldGridIndex.X, isoMouseWorldGridIndex.Y);
			string orthoWorldText = String.Format("(Ortho) WorldX: {0}, WorldY: {1}", isoMouseWorldGridIndex.X, isoMouseWorldGridIndex.Y);
			string thingStatusRawText = String.Format("Thing Activity: {0}", employee.Activity);
			Font font = new Font("Fonts/Arcade N.ttf", 18);
			Surface isoWorldFontSurface = new Surface(font, isoWorldText, color);
			Surface orthoWorldFontSurface = new Surface(font, orthoWorldText, color);
			Surface thingStatusFontSurface = new Surface(font, thingStatusRawText, color);
			isoWorldGridIndexText = new TrueTypeText(Renderer, isoWorldFontSurface, isoWorldText, font, color);
			orthoWorldGridIndexText = new TrueTypeText(Renderer, orthoWorldFontSurface, orthoWorldText, font, color);
			thingStatusText = new TrueTypeText(Renderer, thingStatusFontSurface, thingStatusRawText, font, color);
		}

		private Image thingImage;
		private Image tileHighlightSelectedImage;
		private TrueTypeText isoWorldGridIndexText;
		private TrueTypeText orthoWorldGridIndexText;
		private TrueTypeText thingStatusText;
		private Image tileHighlightImage;
		private Image tileCollisionBoxImage;
		//private Player player;
		private Employee employee;
		private SharpDL.Graphics.Color color = new SharpDL.Graphics.Color(255, 165, 0);

		/// <summary>
		/// Update the game state such as positions, health, power ups, ammo, and anything else that is used
		/// in the simulation parameters.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <remarks>This method is ideally called 60 times per second. However, based on the nature of the game, it is possible 
		/// for this method to run faster or slower. If faster, the game will attempt to fix the timestep to 60 FPS. If slower,
		/// you will experience update/draw lag.</remarks>
		protected override void Update(GameTime gameTime)
		{
			if (mouseOverScreenEdge == MouseOverScreenEdge.Top)
				Camera.MoveUp();
			else if(mouseOverScreenEdge == MouseOverScreenEdge.Bottom)
				Camera.MoveDown();
			else if(mouseOverScreenEdge == MouseOverScreenEdge.Left)
				Camera.MoveLeft();
			else if(mouseOverScreenEdge == MouseOverScreenEdge.Right)
				Camera.MoveRight();

			//player.Update(gameTime, keysPressed);

			List<ICollidable> collidables = new List<ICollidable>();
			//collidables.Add(player);

			IEnumerable<MapObjectLayer> collidableLayers = tiledMap.MapObjectLayers.Where(mol => mol.Type == MapObjectLayerType.Collidable);
			foreach (var collidableLayer in collidableLayers)
				collisionManager.HandleCollisions(collidableLayer.MapObjects, collidables);

			//player.SaveCollisionBox();

			if (isoMouseClickWorldGridIndex != CoordinateHelper.DefaultVector && hasPathPossiblyChanged)
			{
				hasPathPossiblyChanged = false;

				Vector roundedMouseClickIndex = new Vector(
					(int)(Math.Round(isoMouseClickWorldGridIndex.X)), 
					(int)(Math.Round(isoMouseClickWorldGridIndex.Y))
				);

				try
				{
					Queue<MapObject> bestPath = tiledMap.FindBestPath(employee.WorldGridIndex, roundedMouseClickIndex);
					employee.SetPath(bestPath);
				}
				catch { /* show error somewhere, we have chosen an invalid location */ }
			}
			employee.Update(gameTime);

			isoWorldGridIndexText.UpdateText(String.Format("(Iso) WorldX: {0}, WorldY: {1}", isoMouseWorldGridIndex.X, isoMouseWorldGridIndex.Y));
			orthoWorldGridIndexText.UpdateText(String.Format("(X,Y): ({0},{1})", orthoMouseWorldPosition.X, orthoMouseWorldPosition.Y));
			thingStatusText.UpdateText(String.Format("{0} Activity: {1}", employee.AgentName, employee.Activity));
		}

		private bool hasPathPossiblyChanged = false;

		/// <summary>
		/// Draw the game state such as player textures and positions, enemy textures and positions, map textures, and
		/// anything else that is used in the simulation state.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <remarks>This method is ideally called 60 times per second. However, based on the nature of the game, it is possible 
		/// for this method to run faster or slower. If faster, the game will attempt to fix the timestep to 60 FPS. If slower,
		/// you will experience update/draw lag.</remarks>
		protected override void Draw(GameTime gameTime)
		{
			Renderer.ClearScreen();

			TileLayer baseLayer = tiledMap.TileLayers.First(tl => tl.Type == TileLayerType.Base);
			IEnumerable<Tile> baseTiles = baseLayer.Tiles.Where(t => !t.IsEmpty);
			foreach (Tile baseTile in baseTiles)
			{
				baseTile.Draw(gameTime, Renderer);

				if (CoordinateHelper.AreIndicesEqual(baseTile.WorldGridIndex, isoMouseClickWorldGridIndex))
					DrawTileHighlight(tileHighlightSelectedImage, baseTile.ProjectedPosition - Camera.Position);

				if (CoordinateHelper.AreIndicesEqual(baseTile.WorldGridIndex, isoMouseWorldGridIndex))
					DrawTileHighlight(tileHighlightImage, baseTile.ProjectedPosition - Camera.Position);
			}

			// collect a list of the drawable objects that need to be depth sorted
			List<IDrawable> drawables = new List<IDrawable>();
			//drawables.Add(player);

			// select out the drawable tiles from our height layer
			TileLayer heightLayer = tiledMap.TileLayers.First(tl => tl.Type == TileLayerType.Height);
			IEnumerable<Tile> drawableTiles = heightLayer.Tiles.Where(t => !t.IsEmpty);
			drawables.AddRange(drawableTiles);

			// sort the drawables by their depth
			drawables.Sort((d1, d2) => d1.Depth.CompareTo(d2.Depth));

			// draw the drawables!
			foreach (IDrawable drawable in drawables)
			{
				drawable.Draw(gameTime, Renderer);

				if(drawable is Tile)
				{
					if (CoordinateHelper.AreIndicesEqual(drawable.WorldGridIndex, isoMouseClickWorldGridIndex))
						DrawTileHighlight(tileHighlightSelectedImage, drawable.ProjectedPosition - Camera.Position);

					if (CoordinateHelper.AreIndicesEqual(drawable.WorldGridIndex, isoMouseWorldGridIndex))
						DrawTileHighlight(tileHighlightImage, drawable.ProjectedPosition - Camera.Position);
				}
			}

			employee.Draw(gameTime, Renderer);

			Renderer.RenderTexture(isoWorldGridIndexText.Texture, 0, 0);
			Renderer.RenderTexture(orthoWorldGridIndexText.Texture, 0, 18);
			Renderer.RenderTexture(thingStatusText.Texture, 0, 36);

			Renderer.RenderPresent();
		}

		private void DrawTileHighlight(Image image, Vector position)
		{
			Renderer.RenderTexture(
				image.Texture,
				position.X - (image.Texture.Width * 0.5f),
				position.Y - (image.Texture.Height * 0.75f)
			);
		}

		/// <summary>
		/// Unload any content that was used during the update/draw game loop. If you load anything that uses native SDL structures such
		/// as textures, surfaces, fonts, and audio, you must dispose of them in this method to avoid memory leaks from native code.
		/// </summary>
		protected override void UnloadContent()
		{
			base.UnloadContent();
		}
	}
}
