using System;
using LimitedLiability.Content;
using SharpDL.Graphics;
using LimitedLiability.UserInterface;
using System.Reflection;
using System.Diagnostics;

namespace LimitedLiability.Screens
{
	public class MainMenuScreen : Screen
	{
		private Label labelTitle;
		private Label labelVersion;
		private Icon iconFrame;
		private Button buttonNewGame;
		private Button buttonLoadGame;
		private Button buttonQuit;
		private Button buttonCredits;
		private Button buttonOptions;
		private Texture textureBackgroundStripeTile;

		public MainMenuScreen(ContentManager contentManager)
			: base(contentManager)
		{

		}

		public event EventHandler NewGameButtonClicked;
		public event EventHandler LoadGameButtonClicked;
		public event EventHandler OptionsButtonClicked;
		public event EventHandler CreditsButtonClicked;
		public event EventHandler QuitButtonClicked;

		public override void Activate(Renderer renderer)
		{
			base.Activate(renderer);

			string fontPath = ContentManager.GetContentPath(Styles.Fonts.DroidSansBold);
			Color fontColorTitle = Styles.Colors.PaleGreen;
			Color fontColorLabelValue = Styles.Colors.White;
			int fontSizeTitle = Styles.FontSizes.MainMenuTitle;
			int fontSizeContent = Styles.FontSizes.Content;

			iconFrame = ControlFactory.CreateIcon(ContentManager, "MenuPauseFrame");
			iconFrame.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - iconFrame.Width / 2, MainGame.SCREEN_HEIGHT_LOGICAL / 2 - iconFrame.Height / 2);

			labelTitle = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeTitle, fontColorTitle, "Limited Liability");
			labelTitle.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - labelTitle.Width / 2, iconFrame.Position.Y - labelTitle.Height - 15);
			labelTitle.EnableShadow(ContentManager, 3, 3);

			labelVersion = ControlFactory.CreateLabel(ContentManager, fontPath, 12, Styles.Colors.PaleGreen, String.Format("Build {0}", GetAssemblyFileVersion()));
			labelVersion.Position = new Vector(7, MainGame.SCREEN_HEIGHT_LOGICAL - labelVersion.Height - 5);

			buttonNewGame = ControlFactory.CreateButton(ContentManager, "ButtonLongRectangle", "ButtonLongRectangleHover", "ButtonLongRectangleSelected");
			buttonNewGame.Label = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeContent, fontColorLabelValue, "New Game");
			buttonNewGame.ButtonType = ButtonType.TextOnly;
			buttonNewGame.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonNewGame.Width / 2, 16);
			buttonNewGame.Released += buttonNewGame_Clicked;
			buttonNewGame.EnableLabelShadow(ContentManager, 2, 2);

			buttonLoadGame = ControlFactory.CreateButton(ContentManager, "ButtonLongRectangle", "ButtonLongRectangleHover", "ButtonLongRectangleSelected");
			buttonLoadGame.Label = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeContent, fontColorLabelValue, "Load Game");
			buttonLoadGame.ButtonType = ButtonType.TextOnly;
			buttonLoadGame.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonLoadGame.Width / 2, 50);
			buttonLoadGame.Released += buttonLoadGame_Clicked;
			buttonLoadGame.EnableLabelShadow(ContentManager, 2, 2);

			buttonOptions = ControlFactory.CreateButton(ContentManager, "ButtonLongRectangle", "ButtonLongRectangleHover", "ButtonLongRectangleSelected");
			buttonOptions.Label = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeContent, fontColorLabelValue, "Options");
			buttonOptions.ButtonType = ButtonType.TextOnly;
			buttonOptions.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonOptions.Width / 2, 84);
			buttonOptions.Released += buttonOptions_Clicked;
			buttonOptions.EnableLabelShadow(ContentManager, 2, 2);

			buttonCredits = ControlFactory.CreateButton(ContentManager, "ButtonLongRectangle", "ButtonLongRectangleHover", "ButtonLongRectangleSelected");
			buttonCredits.Label = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeContent, fontColorLabelValue, "Credits");
			buttonCredits.ButtonType = ButtonType.TextOnly;
			buttonCredits.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonCredits.Width / 2, 118);
			buttonCredits.Released += buttonCredits_Clicked;
			buttonCredits.EnableLabelShadow(ContentManager, 2, 2);

			buttonQuit = ControlFactory.CreateButton(ContentManager, "ButtonLongRectangle", "ButtonLongRectangleHover", "ButtonLongRectangleSelected");
			buttonQuit.Label = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeContent, fontColorLabelValue, "Quit");
			buttonQuit.ButtonType = ButtonType.TextOnly;
			buttonQuit.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonQuit.Width / 2, 152);
			buttonQuit.Released += buttonQuit_Clicked;
			buttonQuit.EnableLabelShadow(ContentManager, 2, 2);

			textureBackgroundStripeTile = ContentManager.GetTexture("BackgroundStripeTile");
		}

		private void buttonCredits_Clicked(object sender, EventArgs e)
		{
		}

		private void buttonOptions_Clicked(object sender, EventArgs e)
		{
		}

		private void buttonLoadGame_Clicked(object sender, EventArgs e)
		{
		}

		private void buttonNewGame_Clicked(object sender, EventArgs e)
		{
			if (NewGameButtonClicked != null)
				NewGameButtonClicked(this, EventArgs.Empty);
		}

		private void buttonQuit_Clicked(object sender, EventArgs e)
		{
			OnQuitButtonClicked(sender, e);
		}

		public override void Update(SharpDL.GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			iconFrame.Update(gameTime);
			labelTitle.Update(gameTime);
			buttonNewGame.Update(gameTime);
			buttonLoadGame.Update(gameTime);
			buttonQuit.Update(gameTime);
			buttonCredits.Update(gameTime);
			buttonOptions.Update(gameTime);
			labelVersion.Update(gameTime);
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			for (int x = 0; x <= MainGame.SCREEN_WIDTH_LOGICAL / textureBackgroundStripeTile.Width; x++)
				for (int y = 0; y <= MainGame.SCREEN_HEIGHT_LOGICAL / textureBackgroundStripeTile.Height; y++)
					textureBackgroundStripeTile.Draw(x * textureBackgroundStripeTile.Width, y * textureBackgroundStripeTile.Height);

			iconFrame.Draw(gameTime, renderer);
			labelTitle.Draw(gameTime, renderer);
			buttonNewGame.Draw(gameTime, renderer);
			buttonLoadGame.Draw(gameTime, renderer);
			buttonQuit.Draw(gameTime, renderer);
			buttonCredits.Draw(gameTime, renderer);
			buttonOptions.Draw(gameTime, renderer);
			labelVersion.Draw(gameTime, renderer);
		}

		public override void HandleKeyStates(System.Collections.Generic.IEnumerable<SharpDL.Input.KeyInformation> keysPressed, System.Collections.Generic.IEnumerable<SharpDL.Input.KeyInformation> keysReleased)
		{
			base.HandleKeyStates(keysPressed, keysReleased);

			foreach (var key in keysPressed)
				if (key.VirtualKey == SharpDL.Input.VirtualKeyCode.Escape)
					OnQuitButtonClicked(this, EventArgs.Empty);
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			buttonNewGame.HandleMouseButtonPressedEvent(sender, e);
			buttonLoadGame.HandleMouseButtonPressedEvent(sender, e);
			buttonQuit.HandleMouseButtonPressedEvent(sender, e);
			buttonCredits.HandleMouseButtonPressedEvent(sender, e);
			buttonOptions.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			buttonNewGame.HandleMouseMovingEvent(sender, e);
			buttonLoadGame.HandleMouseMovingEvent(sender, e);
			buttonQuit.HandleMouseMovingEvent(sender, e);
			buttonCredits.HandleMouseMovingEvent(sender, e);
			buttonOptions.HandleMouseMovingEvent(sender, e);
		}

		public override void HandleMouseButtonReleasedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			buttonNewGame.HandleMouseButtonReleasedEvent(sender, e);
			buttonLoadGame.HandleMouseButtonReleasedEvent(sender, e);
			buttonQuit.HandleMouseButtonReleasedEvent(sender, e);
			buttonCredits.HandleMouseButtonReleasedEvent(sender, e);
			buttonOptions.HandleMouseButtonReleasedEvent(sender, e);
		}

		private void OnQuitButtonClicked(object sender, EventArgs e)
		{
			if (QuitButtonClicked != null)
				QuitButtonClicked(sender, e);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			labelTitle.Dispose();
			buttonNewGame.Dispose();
			buttonLoadGame.Dispose();
			buttonQuit.Dispose();
			buttonCredits.Dispose();
			buttonOptions.Dispose();
			textureBackgroundStripeTile.Dispose();
		}

		private string GetAssemblyFileVersion()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fvi.FileVersion;
			return version;
		}
	}
}

