﻿using MyThirdSDL.Content;
using MyThirdSDL.UserInterface;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Screens
{
	public class PauseMenuScreen : Screen
	{
		private Texture textureBackgroundStripeTile;
		private Icon iconFrame;
		private Label labelTitle;
		private Button buttonResumeGame;
		private Button buttonNewGame;
		private Button buttonLoadGame;
		private Button buttonOptions;
		private Button buttonQuit;

		public PauseMenuScreen(ContentManager contentManager) 
			: base(contentManager)
		{
			IsPopup = true;
		}

		public event EventHandler ResumeGameButtonClicked;
		public event EventHandler NewGameButtonClicked;
		public event EventHandler LoadGameButtonClicked;
		public event EventHandler OptionsButtonClicked;
		public event EventHandler CreditsButtonClicked;
		public event EventHandler QuitButtonClicked;

		public override void Activate(SharpDL.Graphics.Renderer renderer)
		{
			base.Activate(renderer);

			textureBackgroundStripeTile = ContentManager.GetTexture("BackgroundStripeTileFaded");

			string fontPath = ContentManager.GetContentPath(Styles.FontPaths.Arcade);
			Color fontColorTitle = Styles.Colors.MainMenuTitleText;
			Color fontColorLabelValue = Styles.Colors.ButtonMainMenuItemText;
			int fontSizeTitle = Styles.FontSizes.MainMenuTitle;
			int fontSizeContent = Styles.FontSizes.Content;

			iconFrame = new Icon(ContentManager.GetTexture("MenuPauseFrame"));
			iconFrame.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - iconFrame.Height / 2, MainGame.SCREEN_HEIGHT_LOGICAL / 2 - iconFrame.Height / 2);

			labelTitle = new Label();
			labelTitle.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColorTitle, "Paused");
			labelTitle.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - labelTitle.Width / 2, 15);
			labelTitle.EnableShadow(ContentManager, 3, 3);

			buttonResumeGame = new Button();
			buttonResumeGame.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonResumeGame.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonResumeGame.Label = new Label();
			buttonResumeGame.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Resume");
			buttonResumeGame.ButtonType = ButtonType.TextOnly;
			buttonResumeGame.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonResumeGame.Width / 2, 75);
			buttonResumeGame.Clicked += (sender, e) => ExitScreen();

			buttonNewGame = new Button();
			buttonNewGame.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonNewGame.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonNewGame.Label = new Label();
			buttonNewGame.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "New Game");
			buttonNewGame.ButtonType = ButtonType.TextOnly;
			buttonNewGame.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonNewGame.Width / 2, 110);
			buttonNewGame.Clicked += buttonNewGame_Clicked;

			buttonLoadGame = new Button();
			buttonLoadGame.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonLoadGame.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonLoadGame.Label = new Label();
			buttonLoadGame.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Load Game");
			buttonLoadGame.ButtonType = ButtonType.TextOnly;
			buttonLoadGame.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonLoadGame.Width / 2, 145);
			buttonLoadGame.Clicked += buttonLoadGame_Clicked;

			buttonOptions = new Button();
			buttonOptions.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonOptions.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonOptions.Label = new Label();
			buttonOptions.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Options");
			buttonOptions.ButtonType = ButtonType.TextOnly;
			buttonOptions.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonOptions.Width / 2, 180);
			buttonOptions.Clicked += buttonOptions_Clicked;

			buttonQuit = new Button();
			buttonQuit.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonQuit.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonQuit.Label = new Label();
			buttonQuit.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Quit");
			buttonQuit.ButtonType = ButtonType.TextOnly;
			buttonQuit.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonQuit.Width / 2, 215);
			buttonQuit.Clicked += buttonQuit_Clicked;
		}

		public override void Draw(SharpDL.GameTime gameTime, SharpDL.Graphics.Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			for (int x = 0; x <= MainGame.SCREEN_WIDTH_LOGICAL / textureBackgroundStripeTile.Width; x++)
				for (int y = 0; y <= MainGame.SCREEN_HEIGHT_LOGICAL / textureBackgroundStripeTile.Height; y++)
					renderer.RenderTexture(textureBackgroundStripeTile, x * textureBackgroundStripeTile.Width, y * textureBackgroundStripeTile.Height);

			iconFrame.Draw(gameTime, renderer);
			labelTitle.Draw(gameTime, renderer);
			buttonResumeGame.Draw(gameTime, renderer);
			buttonNewGame.Draw(gameTime, renderer);
			buttonLoadGame.Draw(gameTime, renderer);
			buttonOptions.Draw(gameTime, renderer);
			buttonQuit.Draw(gameTime, renderer);
		}

		public override void Update(SharpDL.GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			iconFrame.Update(gameTime);
			labelTitle.Update(gameTime);
			buttonResumeGame.Update(gameTime);
			buttonNewGame.Update(gameTime);
			buttonLoadGame.Update(gameTime);
			buttonOptions.Update(gameTime);
			buttonQuit.Update(gameTime);
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			buttonResumeGame.HandleMouseButtonPressedEvent(sender, e);
			buttonNewGame.HandleMouseButtonPressedEvent(sender, e);
			buttonLoadGame.HandleMouseButtonPressedEvent(sender, e);
			buttonOptions.HandleMouseButtonPressedEvent(sender, e);
			buttonQuit.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			buttonResumeGame.HandleMouseMovingEvent(sender, e);
			buttonNewGame.HandleMouseMovingEvent(sender, e);
			buttonLoadGame.HandleMouseMovingEvent(sender, e);
			buttonOptions.HandleMouseMovingEvent(sender, e);
			buttonQuit.HandleMouseMovingEvent(sender, e);
		}

		public override void HandleKeyStates(IEnumerable<SharpDL.Input.KeyInformation> keysPressed, IEnumerable<SharpDL.Input.KeyInformation> keysReleased)
		{
			base.HandleKeyStates(keysPressed, keysReleased);

			foreach (var key in keysPressed)
				if (key.VirtualKey == SharpDL.Input.VirtualKeyCode.Escape)
					ExitScreen();
		}

		public override void Unload()
		{
			base.Unload();
			Dispose();
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
			if (QuitButtonClicked != null)
				QuitButtonClicked(this, EventArgs.Empty);
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
			buttonOptions.Dispose();
		}
	}
}
