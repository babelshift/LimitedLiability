using MyThirdSDL.Descriptors;
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
using MyThirdSDL.Agents;
using MyThirdSDL.Content;
using MyThirdSDL.Simulation;
using MyThirdSDL.Screens;

namespace MyThirdSDL
{
	public class MainGame : Game
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Constants

		public static readonly int SCREEN_WIDTH = 1440;
		public static readonly int SCREEN_HEIGHT = 900;

		#endregion

		#region Input Data

		private MouseOverScreenEdge mouseOverScreenEdge;

		#endregion

		#region Factories and Managers

		private ContentManager contentManager;
		private ScreenManager screenManager;
		private ScreenFactory screenFactory;

		#endregion

		private bool isMouseInsideWindowBounds = true;
		private bool isWindowFocused = true;
		private GameState currentGameState;

		#region Constructors

		/// <summary>
		/// By default, the constructor does nothing. Something to do is subscribe to various game events.
		/// </summary>
		public MainGame()
		{
//			KeyPressed += HandleKeyPressed;
//			KeyReleased += HandleKeyReleased;
			//MouseMoving += HandleMouseMoving;
			//MouseButtonPressed += HandleMouseButtonClicked;
			WindowEntered += (object sender, WindowEventArgs e) => isMouseInsideWindowBounds = true;
			WindowLeave += (object sender, WindowEventArgs e) => isMouseInsideWindowBounds = false;
			WindowFocusLost += (object sender, WindowEventArgs e) => isWindowFocused = false;
			WindowFocusGained += (object sender, WindowEventArgs e) => isWindowFocused = true;

			if (log.IsDebugEnabled)
				log.Debug("Game class has been constructed.");
		}

		#endregion

		#region Event Handlers

		//		private void HandleMouseButtonClicked(object sender, MouseButtonEventArgs e)
		//		{
		//			userInterfaceManager.HandleMouseButtonPressedEvent(sender, e);
		//
		//			if (e.MouseButton == MouseButtonCode.Left)
		//			{
		//				mouseClickPositionWorldGridIndex = mousePositionWorldGridIndex;
		//			}
		//		}
		//		private void HandleMouseMoving(object sender, MouseMotionEventArgs e)
		//		{
		//			userInterfaceManager.HandleMouseMovingEvent(sender, e);
		//
		//			Vector mousePositionIsometric = CoordinateHelper.ScreenSpaceToWorldSpace(e.RelativeToWindowX, e.RelativeToWindowY,
		//				                                CoordinateHelper.ScreenOffset, CoordinateHelper.ScreenProjectionType.Isometric);
		//			mousePositionWorldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(mousePositionIsometric.X, mousePositionIsometric.Y, tiledMap.TileWidth / 2, tiledMap.TileHeight);
		//
		//			if (e.RelativeToWindowX < 50 && e.RelativeToWindowX > 0)
		//				mouseOverScreenEdge = MouseOverScreenEdge.Left;
		//			else if (e.RelativeToWindowX > SCREEN_WIDTH - 50 && e.RelativeToWindowX < SCREEN_WIDTH - 1)
		//				mouseOverScreenEdge = MouseOverScreenEdge.Right;
		//			else if (e.RelativeToWindowY < 50 && e.RelativeToWindowY > 0)
		//				mouseOverScreenEdge = MouseOverScreenEdge.Top;
		//			else if (e.RelativeToWindowY > SCREEN_HEIGHT - 50 && e.RelativeToWindowY < SCREEN_HEIGHT - 1)
		//				mouseOverScreenEdge = MouseOverScreenEdge.Bottom;
		//			else
		//				mouseOverScreenEdge = MouseOverScreenEdge.None;
		//		}
		//		private void HandleKeyPressed(object sender, KeyboardEventArgs e)
		//		{
		//			if (!keysPressed.Contains(e.KeyInformation.VirtualKey))
		//				keysPressed.Add(e.KeyInformation.VirtualKey);
		//		}
		//
		//		private void HandleKeyReleased(object sender, KeyboardEventArgs e)
		//		{
		//			if (keysPressed.Contains(e.KeyInformation.VirtualKey))
		//				keysPressed.Remove(e.KeyInformation.VirtualKey);
		//		}

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
			screenManager = new ScreenManager(Renderer);
			screenManager.Initialize();
			screenFactory = new ScreenFactory();

			Camera.Position = Vector.Zero;

			currentGameState = GameState.Title;
			MainGameScreen mainGameScreen = CreateMainGameScreen();
			screenManager.AddScreen(mainGameScreen);

			if (log.IsDebugEnabled)
				log.Debug("Game loop Initialize has been completed.");
		}

		#region Create Screens

		public MainMenuScreen CreateMainMenuScreen()
		{
			return new MainMenuScreen(Renderer, contentManager);
		}

		public MainGameScreen CreateMainGameScreen()
		{
			return new MainGameScreen(Renderer, contentManager);
		}

		#endregion

		/// <summary>
		/// Load any content that you will need to use in the update/draw game loop.
		/// </summary>
		protected override void LoadContent()
		{
			base.LoadContent();

			if (log.IsDebugEnabled)
				log.Debug("Game loop LoadContent has been completed.");
		}

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
			if (isWindowFocused)
			{
				KeyboardHelper.Update();
				MouseHelper.Update();
				screenManager.Update(gameTime, !isWindowFocused);

				if (isMouseInsideWindowBounds)
				{
					if (MouseHelper.CurrentMouseState.X < 50 && MouseHelper.CurrentMouseState.X > 0)
						mouseOverScreenEdge = MouseOverScreenEdge.Left;
					else if (MouseHelper.CurrentMouseState.X > SCREEN_WIDTH - 50 && MouseHelper.CurrentMouseState.X < SCREEN_WIDTH - 1)
						mouseOverScreenEdge = MouseOverScreenEdge.Right;
					else if (MouseHelper.CurrentMouseState.Y < 50 && MouseHelper.CurrentMouseState.Y > 0)
						mouseOverScreenEdge = MouseOverScreenEdge.Top;
					else if (MouseHelper.CurrentMouseState.Y > SCREEN_HEIGHT - 50 && MouseHelper.CurrentMouseState.Y < SCREEN_HEIGHT - 1)
						mouseOverScreenEdge = MouseOverScreenEdge.Bottom;
					else
						mouseOverScreenEdge = MouseOverScreenEdge.None;

					Camera.Update(mouseOverScreenEdge);
				}
			}
		}

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
			if (isWindowFocused)
				screenManager.Draw(gameTime, Renderer);
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
