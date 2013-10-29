﻿using MyThirdSDL.Descriptors;
using MyThirdSDL.UserInterface;
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
		private Vector isoMouseWorldPosition = Vector.Zero;
		private Vector isoMouseClickWorldGridIndex = CoordinateHelper.DefaultVector;
		private Vector isoMouseClickWorldPosition = Vector.Zero;

		private List<KeyInformation.VirtualKeyCode> keysPressed = new List<KeyInformation.VirtualKeyCode>();

		private AgentFactory agentFactory;
		private ContentManager contentManager = new ContentManager();
		private SimulationManager simulationManager = new SimulationManager();

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
			if (e.MouseButton == MouseButtonCode.Left)
			{
				isoMouseClickWorldGridIndex = isoMouseWorldGridIndex;
				hasPathPossiblyChanged = true;

				isoMouseClickWorldPosition = isoMouseWorldPosition;

				//SnackMachine snackMachine = agentFactory.CreateSnackMachine(isoMouseClickWorldPosition);
				//userAddedDrawables.Add(snackMachine);
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
			isoMouseWorldPosition = worldSpace;
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
			if (keysPressed.Contains(e.KeyInformation.VirtualKey))
				keysPressed.Remove(e.KeyInformation.VirtualKey);
		}

		private MyThirdSDL.UserInterface.MessageBox messageBox;

		/// <summary>
		/// Initialize the SDL Window and SDL Renderer with any required flags. 
		/// Also initialize anything else of interest (SDL_ttf, SDL_image, etc).
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			CreateWindow("My Third SDL", 100, 100, SCREEN_WIDTH, SCREEN_HEIGHT, WindowFlags.Shown);
			CreateRenderer(RendererFlags.RendererAccelerated);

			JobFactory.Initialize();
			contentManager.Initialize();
			agentFactory = new AgentFactory(Renderer, contentManager);

			Camera.Position = Vector.Zero;
		}

		private TiledMap tiledMap;
		private UserInterfaceManager userInterfaceManager;
		//private CollisionManager collisionManager;

		/// <summary>
		/// Load any content that you will need to use in the update/draw game loop.
		/// </summary>
		protected override void LoadContent()
		{
			base.LoadContent();

			string mapPath = contentManager.GetContentPath("Map2");
			string tileHighlightTexturePath = contentManager.GetContentPath("TileHighlight2");
			string tileHighlightSelectedTexturePath = contentManager.GetContentPath("TileHighlightSelected");
			string fontPath = contentManager.GetContentPath("Arcade");

			tiledMap = new TiledMap(mapPath, Renderer);

			employee = agentFactory.CreateEmployee(TimeSpan.Zero, new Vector(100, 100));
			simulationManager.AddAgent(employee);

			Surface tileHighlightSurface = new Surface(tileHighlightTexturePath, Surface.SurfaceType.PNG);
			tileHighlightImage = new Image(Renderer, tileHighlightSurface, Image.ImageFormat.PNG);

			Surface tileHightlightSelectedSurface = new Surface(tileHighlightSelectedTexturePath, Surface.SurfaceType.PNG);
			tileHighlightSelectedImage = new Image(Renderer, tileHightlightSelectedSurface, Image.ImageFormat.PNG);

			isoWorldGridIndexText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
			orthoWorldGridIndexText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
			thingStatusText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
			simulationTimeText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
			simulationAgeText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
			worldAgeText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);

			List<IPurchasable> purchasableItems = new List<IPurchasable>();
			purchasableItems.Add(agentFactory.CreateSnackMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateSodaMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateWaterFountain(TimeSpan.Zero));
			userInterfaceManager = new UserInterfaceManager(Renderer, contentManager, new Point(SCREEN_WIDTH, SCREEN_HEIGHT), purchasableItems);
		}

		private TimeSpan simulationTime = TimeSpan.Zero;
		private Image tileHighlightSelectedImage;
		private TrueTypeText isoWorldGridIndexText;
		private TrueTypeText orthoWorldGridIndexText;
		private TrueTypeText thingStatusText;
		private TrueTypeText simulationTimeText;
		private TrueTypeText simulationAgeText;
		private TrueTypeText worldAgeText;
		private Image tileHighlightImage;
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
			simulationTime = gameTime.TotalGameTime;

			if (mouseOverScreenEdge == MouseOverScreenEdge.Top)
				Camera.MoveUp();
			else if (mouseOverScreenEdge == MouseOverScreenEdge.Bottom)
				Camera.MoveDown();
			else if (mouseOverScreenEdge == MouseOverScreenEdge.Left)
				Camera.MoveLeft();
			else if (mouseOverScreenEdge == MouseOverScreenEdge.Right)
				Camera.MoveRight();

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
			simulationManager.Update(gameTime);

			isoWorldGridIndexText.UpdateText(String.Format("(Iso) WorldX: {0}, WorldY: {1}", isoMouseWorldGridIndex.X, isoMouseWorldGridIndex.Y));
			orthoWorldGridIndexText.UpdateText(String.Format("(X,Y): ({0},{1})", orthoMouseWorldPosition.X, orthoMouseWorldPosition.Y));
			thingStatusText.UpdateText(String.Format("{0} Activity: {1}", employee.Name, employee.Activity));
			simulationAgeText.UpdateText(String.Format("{0} Simulation Age: {1}", employee.Name, employee.SimulationAge));
			worldAgeText.UpdateText(String.Format("{0} World Age: {1} years, {2} months, {3} days", employee.Name, 
				employee.WorldAge.Days / 365, employee.WorldAge.Days / 12, employee.WorldAge.Days));
			simulationTimeText.UpdateText(String.Format("Simulation Time: {0}", simulationManager.SimulationTimeDisplay));

			userInterfaceManager.Update(gameTime);
		}

		private bool hasPathPossiblyChanged = false;

		private List<IDrawable> userAddedDrawables = new List<IDrawable>();
		private List<IDrawable> allDrawables = new List<IDrawable>();

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
			allDrawables.Clear();
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
			allDrawables.AddRange(userAddedDrawables);

			// select out the drawable tiles from our height layer
			TileLayer heightLayer = tiledMap.TileLayers.First(tl => tl.Type == TileLayerType.Height);
			IEnumerable<Tile> drawableTiles = heightLayer.Tiles.Where(t => !t.IsEmpty);
			allDrawables.AddRange(drawableTiles);

			// sort the drawables by their depth
			allDrawables.Sort((d1, d2) => d1.Depth.CompareTo(d2.Depth));

			// draw the drawables!
			foreach (IDrawable drawable in allDrawables)
			{
				drawable.Draw(gameTime, Renderer);

				if (drawable is Tile)
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
			Renderer.RenderTexture(simulationTimeText.Texture, 0, 54);
			Renderer.RenderTexture(simulationAgeText.Texture, 0, 72);
			Renderer.RenderTexture(worldAgeText.Texture, 0, 90);

			userInterfaceManager.Draw(gameTime, Renderer);

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
