using System;
using MyThirdSDL.Content;
using SharpDL.Graphics;
using MyThirdSDL.UserInterface;

namespace MyThirdSDL.Screens
{
	public class MainMenuScreen : Screen
	{
		private Label labelTitle;
		private Button buttonNewGame;
		private Button buttonLoadGame;
		private Button buttonQuit;
		private Button buttonCredits;
		private Button buttonOptions;
		private Texture textureBackgroundStripeTile;

		public MainMenuScreen(Renderer renderer, ContentManager contentManager)
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

			string fontPath = ContentManager.GetContentPath(Styles.FontPaths.Arcade);
			Color fontColorTitle = Styles.Colors.MainMenuTitleText;
			Color fontColorLabelValue = Styles.Colors.ButtonMainMenuItemText;
			int fontSizeTitle = Styles.FontSizes.MainMenuTitle;
			int fontSizeContent = Styles.FontSizes.Content;

			labelTitle = new Label();
			labelTitle.TrueTypeText = TrueTypeTextFactory.CreateTrueTypeText(renderer, fontPath, fontSizeTitle, fontColorTitle, "Limited Liability");
			labelTitle.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - labelTitle.Width / 2, 300);
			labelTitle.EnableShadow(ContentManager, 3, 3);

			buttonNewGame = new Button();
			buttonNewGame.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonNewGame.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonNewGame.Label = new Label();
			buttonNewGame.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "New Game");
			buttonNewGame.ButtonType = ButtonType.TextOnly;
			buttonNewGame.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - buttonNewGame.Width / 2, 400);
			buttonNewGame.Clicked += buttonNewGame_Clicked;

			buttonLoadGame = new Button();
			buttonLoadGame.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonLoadGame.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonLoadGame.Label = new Label();
			buttonLoadGame.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Load Game");
			buttonLoadGame.ButtonType = ButtonType.TextOnly;
			buttonLoadGame.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - buttonNewGame.Width / 2, 435);
			buttonLoadGame.Clicked += buttonLoadGame_Clicked;

			buttonOptions = new Button();
			buttonOptions.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonOptions.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonOptions.Label = new Label();
			buttonOptions.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Options");
			buttonOptions.ButtonType = ButtonType.TextOnly;
			buttonOptions.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - buttonNewGame.Width / 2, 470);
			buttonOptions.Clicked += buttonOptions_Clicked;

			buttonCredits = new Button();
			buttonCredits.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonCredits.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonCredits.Label = new Label();
			buttonCredits.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Credits");
			buttonCredits.ButtonType = ButtonType.TextOnly;
			buttonCredits.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - buttonNewGame.Width / 2, 505);
			buttonCredits.Clicked += buttonCredits_Clicked;

			buttonQuit = new Button();
			buttonQuit.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonQuit.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonQuit.Label = new Label();
			buttonQuit.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Quit");
			buttonQuit.ButtonType = ButtonType.TextOnly;
			buttonQuit.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - buttonNewGame.Width / 2, 540);
			buttonQuit.Clicked += buttonQuit_Clicked;

			textureBackgroundStripeTile = ContentManager.GetTexture("BackgroundStripeTile");
		}

		private void buttonCredits_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void buttonOptions_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void buttonLoadGame_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void buttonNewGame_Clicked(object sender, EventArgs e)
		{
			if (NewGameButtonClicked != null)
				NewGameButtonClicked(this, EventArgs.Empty);
		}

		private void buttonQuit_Clicked(object sender, EventArgs e)
		{
			if(QuitButtonClicked != null) 
				QuitButtonClicked(this, EventArgs.Empty);
		}

		public override void Update(SharpDL.GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			labelTitle.Update(gameTime);
			buttonNewGame.Update(gameTime);
			buttonLoadGame.Update(gameTime);
			buttonQuit.Update(gameTime);
			buttonCredits.Update(gameTime);
			buttonOptions.Update(gameTime);
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			for (int x = 0; x <= MainGame.SCREEN_WIDTH_LOGICAL / textureBackgroundStripeTile.Width; x++)
				for (int y = 0; y <= MainGame.SCREEN_HEIGHT_LOGICAL / textureBackgroundStripeTile.Height; y++)
					renderer.RenderTexture(textureBackgroundStripeTile, x * textureBackgroundStripeTile.Width, y * textureBackgroundStripeTile.Height);

			labelTitle.Draw(gameTime, renderer);
			buttonNewGame.Draw(gameTime, renderer);
			buttonLoadGame.Draw(gameTime, renderer);
			buttonQuit.Draw(gameTime, renderer);
			buttonCredits.Draw(gameTime, renderer);
			buttonOptions.Draw(gameTime, renderer);

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

		public override void Unload()
		{
			base.Unload();

			Dispose();
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
	}
}

