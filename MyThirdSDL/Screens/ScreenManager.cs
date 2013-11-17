using System;
using System.Collections.Generic;
using SharpDL.Graphics;
using SharpDL;
using MyThirdSDL.Content;

namespace MyThirdSDL.Screens
{
	/// <summary>
	/// The screen manager is a component which manages one or more Screen
	/// instances. It maintains a stack of screens, calls their Update and Draw
	/// methods at the appropriate times, and automatically routes input to the
	/// topmost active screen.
	/// </summary>
	public class ScreenManager
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#region Fields

		List<Screen> screens = new List<Screen>();
		List<Screen> tempScreensList = new List<Screen>();

		public Texture BlankTexture { get; private set; }

		public bool IsInitialized { get; private set; }

		/// <summary>
		/// If true, the manager prints out a list of all the screens
		/// each time it is updated. This can be useful for making sure
		/// everything is being added and removed at the right times.
		/// </summary>
		public bool IsTraceEnabled { get; private set; }

		#endregion

		#region Initialization

		private ContentManager contentManager;

		/// <summary>
		/// Constructs a new screen manager component.
		/// </summary>
		public ScreenManager(ContentManager contentManager)
		{
			this.contentManager = contentManager;
		}

		/// <summary>
		/// Initializes the screen manager component.
		/// </summary>
		public void Initialize()
		{
			IsInitialized = true;
		}

		/// <summary>
		/// Load your graphics content.
		/// </summary>
		protected void LoadContent()
		{
			// Tell each of the screens to load their content.
			foreach (Screen screen in screens)
			{
				screen.Activate(false);
			}
		}

		/// <summary>
		/// Unload your graphics content.
		/// </summary>
		protected void UnloadContent()
		{
			// Tell each of the screens to unload their content.
			foreach (Screen screen in screens)
			{
				screen.Unload();
			}
		}

		#endregion

		#region Update and Draw

		/// <summary>
		/// Allows each screen to run logic.
		/// </summary>
		public void Update(GameTime gameTime, bool otherWindowHasFocus)
		{
			// Make a copy of the master screen list, to avoid confusion if
			// the process of updating one screen adds or removes others.
			tempScreensList.Clear();

			foreach (Screen screen in screens)
				tempScreensList.Add(screen);

			bool coveredByOtherScreen = false;

			// Loop as long as there are screens waiting to be updated.
			while (tempScreensList.Count > 0)
			{
				// Pop the topmost screen off the waiting list.
				Screen screen = tempScreensList[tempScreensList.Count - 1];

				tempScreensList.RemoveAt(tempScreensList.Count - 1);

				// Update the screen.
				screen.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen); //, otherWindowHasFocus, coveredByOtherScreen);

				if (screen.ScreenState == ScreenState.TransitionOn ||
				    screen.ScreenState == ScreenState.Active)
				{
					// If this is the first active screen we came across,
					// give it a chance to handle input.
					// The first active screen should inform subsequent screens
					// that something else has the focus of input.
					// Subsequent screens will thus be marked as "InActive"
					if (!otherWindowHasFocus)
					{
						screen.HandleInput(gameTime);

						otherWindowHasFocus = true;
					}

					// If this is an active non-popup, inform any subsequent
					// screens that they are covered by it.
					// If it is a popup, the screens under it should not transition off.
					// In this case, the under screens will still have a screen state of "Active".
					if (!screen.IsPopup)
						coveredByOtherScreen = true;
				}
			}

			// Print debug trace?
			if (IsTraceEnabled)
				TraceScreens();
		}

		/// <summary>
		/// Prints a list of all the screens, for debugging.
		/// </summary>
		void TraceScreens()
		{
			List<string> screenNames = new List<string>();

			foreach (Screen screen in screens)
				screenNames.Add(screen.GetType().Name);

			if (log.IsDebugEnabled)
				log.Debug((String.Join(", ", screenNames.ToArray())));
		}

		/// <summary>
		/// Tells each screen to draw itself.
		/// </summary>
		public void Draw(GameTime gameTime)
		{
			//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
			//MenuBackground.Draw(spriteBatch, Color.White);
			//spriteBatch.End();

			foreach (Screen screen in screens)
			{
				if (screen.ScreenState == ScreenState.Hidden)
					continue;

				screen.Draw(gameTime);
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds a new screen to the screen manager.
		/// </summary>
		public void AddScreen(Screen screen)
		{
			screen.ScreenManager = this;
			screen.IsExiting = false;

			// If we have a graphics device, tell the screen to load content.
			if (IsInitialized)
			{
				screen.Activate(false);
			}

			screens.Add(screen);
		}

		/// <summary>
		/// Removes a screen from the screen manager. You should normally
		/// use Screen.ExitScreen instead of calling this directly, so
		/// the screen can gradually transition off rather than just being
		/// instantly removed.
		/// </summary>
		public void RemoveScreen(Screen screen)
		{
			// If we have a graphics device, tell the screen to unload content.
			if (IsInitialized)
			{
				screen.Unload();
			}

			screens.Remove(screen);
			tempScreensList.Remove(screen);
		}

		/// <summary>
		/// Expose an array holding all the screens. We return a copy rather
		/// than the real master list, because screens should only ever be added
		/// or removed using the AddScreen and RemoveScreen methods.
		/// </summary>
		public Screen[] GetScreens()
		{
			return screens.ToArray();
		}

		/// <summary>
		/// Helper draws a translucent black fullscreen sprite, used for fading
		/// screens in and out, and for darkening the background behind popups.
		/// </summary>
		public void FadeBackBufferToBlack(float alpha)
		{
			//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
			//spriteBatch.Draw(blankTexture, Resolution.VirtualViewport.Bounds, Color.Black * alpha);
			//spriteBatch.End();
		}

		#endregion

	}
}