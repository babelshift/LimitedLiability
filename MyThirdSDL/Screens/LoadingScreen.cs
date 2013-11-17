using System;
using SharpDL;
using SharpDL.Graphics;
using MyThirdSDL.Content;

namespace MyThirdSDL.Screens
{
	/// <summary>
	/// The loading screen coordinates transitions between the menu system and the
	/// game itself. Normally one screen will transition off at the same time as
	/// the next screen is transitioning on, but for larger transitions that can
	/// take a longer time to load their data, we want the menu system to be entirely
	/// gone before we start loading the game. This is done as follows:
	/// 
	/// - Tell all the existing screens to transition off.
	/// - Activate a loading screen, which will transition on at the same time.
	/// - The loading screen watches the state of the previous screens.
	/// - When it sees they have finished transitioning off, it activates the real
	///   next screen, which may take a long time to load its data. The loading
	///   screen will be the only thing displayed while this load is taking place.
	/// </summary>
	class LoadingScreen : Screen
	{
		#region Fields

		bool loadingIsSlow;
		bool otherScreensAreGone;

		Screen[] screensToLoad;

		#endregion

		//protected MenuBackground MenuBackground { get; set; }

		#region Initialization


		/// <summary>
		/// The constructor is private: loading screens should
		/// be activated via the static Load method instead.
		/// </summary>
		private LoadingScreen(ContentManager contentManager, ScreenManager screenManager, bool loadingIsSlow, Screen[] screensToLoad)
			: base(contentManager)
		{
			this.loadingIsSlow = loadingIsSlow;
			this.screensToLoad = screensToLoad;

			TransitionOnTime = TimeSpan.FromSeconds(0.5);

			//MenuBackground = screenManager.MenuBackground;
		}


		/// <summary>
		/// Activates the loading screen.
		/// </summary>
		public static void Load(ContentManager contentManager, ScreenManager screenManager, bool loadingIsSlow, params Screen[] screensToLoad)
		{
			// Tell all the current screens to transition off.
			foreach (Screen screen in screenManager.GetScreens())
				screen.ExitScreen();

			// Create and activate the loading screen.
			LoadingScreen loadingScreen = new LoadingScreen(contentManager, screenManager, loadingIsSlow, screensToLoad);

			screenManager.AddScreen(loadingScreen);
		}


		#endregion

		#region Update and Draw


		/// <summary>
		/// Updates the loading screen.
		/// </summary>
		public override void Update(GameTime gameTime, bool otherWindowHasFocus,
			bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			// If all the previous screens have finished transitioning
			// off, it is time to actually perform the load.
			if (otherScreensAreGone)
			{
				ScreenManager.RemoveScreen(this);

				foreach (Screen screen in screensToLoad)
				{
					if (screen != null)
					{
						ScreenManager.AddScreen(screen);
					}
				}

				// Once the load has finished, we use ResetElapsedTime to tell
				// the  game timing mechanism that we have just finished a very
				// long frame, and that it should not try to catch up.
				//ScreenManager.Game.ResetElapsedTime();
			}
		}


		/// <summary>
		/// Draws the loading screen.
		/// </summary>
		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			// If we are the only active screen, that means all the previous screens
			// must have finished transitioning off. We check for this in the Draw
			// method, rather than in Update, because it isn't enough just for the
			// screens to be gone: in order for the transition to look good we must
			// have actually drawn a frame without them before we perform the load.
			if ((ScreenState == ScreenState.Active) &&
				(ScreenManager.GetScreens().Length == 1))
			{
				otherScreensAreGone = true;
			}

			//SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());

			//Color color = Color.White * TransitionAlpha;

			//MenuBackground.Draw(spriteBatch, color);

			// The gameplay screen takes a while to load, so we display a loading
			// message while that is going on, but the menus load very quickly, and
			// it would look silly if we flashed this up for just a fraction of a
			// second while returning from the game to the menus. This parameter
			// tells us how long the loading is going to take, so we know whether
			// to bother drawing the message.
			if (loadingIsSlow)
			{
				//SpriteFont font = ScreenManager.Font;

				//const string message = "Loading...";

				// Center the text in the viewport.
				//Vector2 viewportSize = new Vector2(Resolution.VirtualViewport.Width, Resolution.VirtualViewport.Height);
				//Vector2 textSize = font.MeasureString(message);
				//Vector2 textPosition = (viewportSize - textSize) / 2;

				//spriteBatch.DrawString(font, message, textPosition, color);
			}

			//spriteBatch.End();
		}


		#endregion
	}
}