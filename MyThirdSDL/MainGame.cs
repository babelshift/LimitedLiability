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

		public static readonly int SCREEN_WIDTH_LOGICAL = 1152;
		public static readonly int SCREEN_HEIGHT_LOGICAL = 720;

		#endregion

		#region Inputs

		private List<KeyInformation> keysPressed = new List<KeyInformation>();
		private List<KeyInformation> keysReleased = new List<KeyInformation>();

		#endregion

		#region Factories and Managers

		private ContentManager contentManager;
		private ScreenManager screenManager;
		private ScreenFactory screenFactory;

		#endregion

		private bool isMouseInsideWindowBounds = true;
		private bool isWindowFocused = true;
		private GameState currentGameState;
		
		private GameState CurrentGameState
		{
			get { return currentGameState; }
			set { currentGameState = value; }
		}
		
		#region Constructors

		/// <summary>
		/// By default, the constructor does nothing. Something to do is subscribe to various game events.
		/// </summary>
		public MainGame()
		{
			MouseMoving += HandleMouseMoving;
			MouseButtonPressed += HandleMouseButtonClicked;
			MouseButtonReleased += HandleMouseButtonReleased;
			WindowEntered += (object sender, WindowEventArgs e) => isMouseInsideWindowBounds = true;
			WindowLeave += (object sender, WindowEventArgs e) => isMouseInsideWindowBounds = false;
			WindowFocusLost += (object sender, WindowEventArgs e) => isWindowFocused = false;
			WindowFocusGained += (object sender, WindowEventArgs e) => isWindowFocused = true;
			TextInputting += HandleTextInputting;
			KeyPressed += HandleKeyPressed;
			KeyReleased += HandleKeyReleased;

			if (log.IsDebugEnabled)
				log.Debug("Game class has been constructed.");
		}


		#endregion

		#region Event Handlers

		private void HandleMouseButtonReleased(object sender, MouseButtonEventArgs e)
		{
		}

		private void HandleTextInputting(object sender, TextInputEventArgs e)
		{
			screenManager.PassTextInputEventToActiveScreen(sender, e);
		}
		
		private void HandleMouseButtonClicked(object sender, MouseButtonEventArgs e)
		{
			screenManager.PassMouseButtonPressedEventToActiveScreen(sender, e);
		}

		private void HandleMouseMoving(object sender, MouseMotionEventArgs e)
		{
			screenManager.PassMouseMovingEventToActiveScreen(sender, e);
		}

		private void HandleKeyPressed(object sender, KeyboardEventArgs e)
		{
			if (!keysPressed.Contains(e.KeyInformation))
				keysPressed.Add(e.KeyInformation);

			screenManager.PassKeyStatesToActiveScreen(keysPressed, keysReleased);

			keysPressed.Clear();
		}

		private void HandleKeyReleased(object sender, KeyboardEventArgs e)
		{
			if (keysPressed.Contains(e.KeyInformation))
				keysReleased.Add(e.KeyInformation);

			screenManager.PassKeyStatesToActiveScreen(keysPressed, keysReleased);

			keysReleased.Clear();
		}

		#endregion

		/// <summary>
		/// Initialize the SDL Window and SDL Renderer with any required flags. 
		/// Also initialize anything else of interest (SDL_ttf, SDL_image, etc).
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			CreateWindow("My Third SDL", 100, 100, 1152, 720, WindowFlags.Shown | WindowFlags.GrabbedInputFocus);
			CreateRenderer(RendererFlags.RendererAccelerated | RendererFlags.RendererPresentVSync);
			Renderer.SetRenderLogicalSize(SCREEN_WIDTH_LOGICAL, SCREEN_HEIGHT_LOGICAL);

			contentManager = new ContentManager(Renderer);
			screenManager = new ScreenManager(Renderer);
			screenManager.Initialize();
			screenFactory = new ScreenFactory();

			Camera.Position = Vector.Zero;

			LoadMainMenuScreen();

			if (log.IsDebugEnabled)
				log.Debug("Game loop Initialize has been completed.");
		}

		#region Create Screens

		public MainMenuScreen CreateMainMenuScreen()
		{
			MainMenuScreen mainMenuScreen = new MainMenuScreen(contentManager);
			mainMenuScreen.QuitButtonClicked += (sender, e) => Quit();
			mainMenuScreen.NewGameButtonClicked += (sender, e) => LoadScenariosScreen();
			return mainMenuScreen;
		}

		public MainGameScreen CreateMainGameScreen(string mapPathToLoad)
		{
			MainGameScreen mainGameScreen = new MainGameScreen(Renderer, contentManager, mapPathToLoad);
			mainGameScreen.ReturnToMainMenu += (sender, e) => LoadMainMenuScreen();
			return mainGameScreen;
		}

		public ScenarioScreen CreateScenarioScreen()
		{
			ScenarioScreen scenarioScreen = new ScenarioScreen(contentManager);
			scenarioScreen.ReturnToMainMenu += (sender, e) => LoadMainMenuScreen();
			scenarioScreen.ScenarioSelected += (sender, e) => LoadMainGameScreen(e.MapPathToLoad);
			return scenarioScreen;
		}

		private void LoadScreen(Screen screen)
		{
			if (screen != null)
				LoadingScreen.Load(contentManager, screenManager, false, screen);
		}

		private void LoadMainMenuScreen()
		{
			CurrentGameState = GameState.MainMenu;
			Screen screen = CreateMainMenuScreen();
			LoadScreen(screen);
		}

		private void LoadScenariosScreen()
		{
			CurrentGameState = GameState.Scenarios;
			Screen screen = CreateScenarioScreen();
			LoadScreen(screen);
		}

		private void LoadMainGameScreen(string mapPathToLoad)
		{
			CurrentGameState = GameState.Scenarios;
			Screen screen = CreateMainGameScreen(mapPathToLoad);
			LoadScreen(screen);
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
			// TODO: move the focus logic to sharpdl game class?
			if (isWindowFocused)
			{
				base.Update(gameTime);
				screenManager.Update(gameTime, !isWindowFocused, isMouseInsideWindowBounds);
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
			// TODO: move the focus logic to sharpdl game class?
			if (isWindowFocused)
			{
				base.Draw(gameTime);
				screenManager.Draw(gameTime, Renderer);
				Renderer.RenderPresent();
			}
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
