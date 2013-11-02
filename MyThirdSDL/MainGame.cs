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
		#region Constants

		private const int SCREEN_WIDTH = 1440;
		private const int SCREEN_HEIGHT = 900;

		#endregion

		#region Input Data

		private Vector mousePositionWorldGridIndex = Vector.Zero;
		private Vector mouseClickPositionWorldGridIndex = CoordinateHelper.DefaultVector;
		private Vector mouseClickWorldPosition = Vector.Zero;
		private MouseOverScreenEdge mouseOverScreenEdge;
		private List<KeyInformation.VirtualKeyCode> keysPressed = new List<KeyInformation.VirtualKeyCode>();

		#endregion

		#region Factories and Managers

		private JobFactory jobFactory;
		private AgentFactory agentFactory;
		private ContentManager contentManager;
		private SimulationManager simulationManager;
		private UserInterfaceManager userInterfaceManager;

		#endregion

		private TiledMap tiledMap;

		#region Constructors

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

		#endregion

		#region Event Handlers

		private void MainGame_MouseButtonPressed(object sender, MouseButtonEventArgs e)
		{
			userInterfaceManager.HandleMouseButtonPressedEvent(sender, e);

			if (e.MouseButton == MouseButtonCode.Left)
			{
				mouseClickPositionWorldGridIndex = mousePositionWorldGridIndex;
				mouseClickWorldPosition = new Vector(e.RelativeToWindowX, e.RelativeToWindowY);
				hasPathPossiblyChanged = true;
			}
		}

		private void MainGame_MouseMoving(object sender, MouseMotionEventArgs e)
		{
			userInterfaceManager.HandleMouseMovingEvent(sender, e);

			Vector mousePositionIsometric = CoordinateHelper.ScreenSpaceToWorldSpace(e.RelativeToWindowX, e.RelativeToWindowY, 
				CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
			mousePositionWorldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(mousePositionIsometric.X, mousePositionIsometric.Y, tiledMap.TileWidth / 2, tiledMap.TileHeight);

			if (e.RelativeToWindowX < 50 && e.RelativeToWindowX > 0)
				mouseOverScreenEdge = MouseOverScreenEdge.Left;
			else if (e.RelativeToWindowX > SCREEN_WIDTH - 50 && e.RelativeToWindowX < SCREEN_WIDTH - 1)
				mouseOverScreenEdge = MouseOverScreenEdge.Right;
			else if (e.RelativeToWindowY < 50 && e.RelativeToWindowY > 0)
				mouseOverScreenEdge = MouseOverScreenEdge.Top;
			else if (e.RelativeToWindowY > SCREEN_HEIGHT - 50 && e.RelativeToWindowY < SCREEN_HEIGHT - 1)
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

		#endregion

		/// <summary>
		/// Initialize the SDL Window and SDL Renderer with any required flags. 
		/// Also initialize anything else of interest (SDL_ttf, SDL_image, etc).
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			CreateWindow("My Third SDL", 100, 100, SCREEN_WIDTH, SCREEN_HEIGHT, WindowFlags.Shown);
			CreateRenderer(RendererFlags.RendererAccelerated);

			contentManager = new ContentManager();
			simulationManager = new SimulationManager();
			jobFactory = new JobFactory();
			agentFactory = new AgentFactory(Renderer, contentManager, jobFactory);

			Camera.Position = Vector.Zero;
		}

		/// <summary>
		/// Load any content that you will need to use in the update/draw game loop.
		/// </summary>
		protected override void LoadContent()
		{
			base.LoadContent();

			string mapPath = contentManager.GetContentPath("Map2");
			string tileHighlightTexturePath = contentManager.GetContentPath("TileHighlight2");
			string tileHighlightSelectedTexturePath = contentManager.GetContentPath("TileHighlightSelected");
			//string fontPath = contentManager.GetContentPath("Arcade");

			tiledMap = new TiledMap(mapPath, Renderer);

			employee = agentFactory.CreateEmployee(TimeSpan.Zero, new Vector(100, 100));
			simulationManager.AddAgent(employee);

			Surface tileHighlightSurface = new Surface(tileHighlightTexturePath, Surface.SurfaceType.PNG);
			tileHighlightImage = new Image(Renderer, tileHighlightSurface, Image.ImageFormat.PNG);

			Surface tileHightlightSelectedSurface = new Surface(tileHighlightSelectedTexturePath, Surface.SurfaceType.PNG);
			tileHighlightSelectedImage = new Image(Renderer, tileHightlightSelectedSurface, Image.ImageFormat.PNG);

//			isoWorldGridIndexText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
//			orthoWorldGridIndexText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
//			thingStatusText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
//			simulationTimeText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
//			simulationAgeText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);
//			worldAgeText = TrueTypeTextFactory.CreateTrueTypeText(Renderer, fontPath, 16, color);

			List<IPurchasable> purchasableItems = new List<IPurchasable>();
			purchasableItems.Add(agentFactory.CreateSnackMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateSodaMachine(TimeSpan.Zero));
			purchasableItems.Add(agentFactory.CreateWaterFountain(TimeSpan.Zero));
			userInterfaceManager = new UserInterfaceManager(Renderer, contentManager, new Point(SCREEN_WIDTH, SCREEN_HEIGHT), purchasableItems);
		}

//		private TrueTypeText isoWorldGridIndexText;
//		private TrueTypeText orthoWorldGridIndexText;
//		private TrueTypeText thingStatusText;
//		private TrueTypeText simulationTimeText;
//		private TrueTypeText simulationAgeText;
//		private TrueTypeText worldAgeText;
		private Image tileHighlightImage;
		private Image tileHighlightSelectedImage;
		private Employee employee;
		//private SharpDL.Graphics.Color color = new SharpDL.Graphics.Color(255, 165, 0);

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
			Camera.Update(mouseOverScreenEdge);
			simulationManager.Update(gameTime);

			if (mouseClickPositionWorldGridIndex != CoordinateHelper.DefaultVector && hasPathPossiblyChanged)
			{
				hasPathPossiblyChanged = false;

				Vector roundedMouseClickIndex = new Vector(
					(int)(Math.Round(mouseClickPositionWorldGridIndex.X)),
					(int)(Math.Round(mouseClickPositionWorldGridIndex.Y))
				);

				try
				{
					Queue<MapObject> bestPath = tiledMap.FindBestPath(employee.WorldGridIndex, roundedMouseClickIndex);
					employee.SetPath(bestPath);
				}
				catch { /* show error somewhere, we have chosen an invalid location */ }
			}

			//isoWorldGridIndexText.UpdateText(String.Format("(Iso) WorldX: {0}, WorldY: {1}", isoMouseWorldGridIndex.X, isoMouseWorldGridIndex.Y));
			//orthoWorldGridIndexText.UpdateText(String.Format("(X,Y): ({0},{1})", orthoMouseWorldPosition.X, orthoMouseWorldPosition.Y));
			//thingStatusText.UpdateText(String.Format("{0} Activity: {1}", employee.Name, employee.Activity));
			//simulationAgeText.UpdateText(String.Format("{0} Simulation Age: {1}", employee.Name, employee.SimulationAge));
			//worldAgeText.UpdateText(String.Format("{0} World Age: {1} years, {2} months, {3} days", employee.Name, 
			//	employee.WorldAge.Days / 365, employee.WorldAge.Days / 12, employee.WorldAge.Days));

			string simulationTimeText = String.Format("Simulation Time: {0}", simulationManager.SimulationTimeDisplay);
			userInterfaceManager.Update(gameTime, simulationTimeText);
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

				if (CoordinateHelper.AreIndicesEqual(baseTile.WorldGridIndex, mouseClickPositionWorldGridIndex))
					DrawTileHighlight(tileHighlightSelectedImage, baseTile.ProjectedPosition - Camera.Position);

				if (CoordinateHelper.AreIndicesEqual(baseTile.WorldGridIndex, mousePositionWorldGridIndex))
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
					if (CoordinateHelper.AreIndicesEqual(drawable.WorldGridIndex, mouseClickPositionWorldGridIndex))
						DrawTileHighlight(tileHighlightSelectedImage, drawable.ProjectedPosition - Camera.Position);

					if (CoordinateHelper.AreIndicesEqual(drawable.WorldGridIndex, mousePositionWorldGridIndex))
						DrawTileHighlight(tileHighlightImage, drawable.ProjectedPosition - Camera.Position);
				}
			}

			employee.Draw(gameTime, Renderer);

			//Renderer.RenderTexture(isoWorldGridIndexText.Texture, 0, 0);
			//Renderer.RenderTexture(orthoWorldGridIndexText.Texture, 0, 18);
			//Renderer.RenderTexture(thingStatusText.Texture, 0, 36);
			//Renderer.RenderTexture(simulationTimeText.Texture, 0, 54);
			//Renderer.RenderTexture(simulationAgeText.Texture, 0, 72);
			//Renderer.RenderTexture(worldAgeText.Texture, 0, 90);

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
