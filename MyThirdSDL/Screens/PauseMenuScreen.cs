using MyThirdSDL.Content;
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
		private Icon iconFrame;
		private Label labelTitle;
		private Label labelTitleShadow;
		private Button buttonNewGame;
		private Button buttonLoadGame;
		private Button buttonOptions;
		private Button buttonQuit;

		public PauseMenuScreen(ContentManager contentManager) 
			: base(contentManager)
		{
			IsPopup = true;
		}

		public event EventHandler NewGameButtonClicked;
		public event EventHandler LoadGameButtonClicked;
		public event EventHandler OptionsButtonClicked;
		public event EventHandler CreditsButtonClicked;
		public event EventHandler QuitButtonClicked;

		public override void Activate(SharpDL.Graphics.Renderer renderer)
		{
			base.Activate(renderer);

			string fontPath = ContentManager.GetContentPath(Styles.FontPaths.Arcade);
			Color fontColorTitle = Styles.Colors.MainMenuTitleText;
			Color fontColorTitleShadow = Styles.Colors.MainMenuTitleTextShadow;
			Color fontColorLabelValue = Styles.Colors.ButtonMainMenuItemText;
			int fontSizeTitle = Styles.FontSizes.MainMenuTitle;
			int fontSizeContent = Styles.FontSizes.Content;

			iconFrame = new Icon(ContentManager.GetTexture("MenuPauseFrame"));
			iconFrame.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - iconFrame.Height / 2, MainGame.SCREEN_HEIGHT_LOGICAL / 2 - iconFrame.Height / 2);

			labelTitle = new Label();
			labelTitle.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeTitle, fontColorTitle, "Paused");
			labelTitle.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - labelTitle.Width / 2, 15);

			labelTitleShadow = new Label();
			labelTitleShadow.TrueTypeText = TrueTypeTextFactory.CreateTrueTypeText(renderer, fontPath, fontSizeTitle, fontColorTitleShadow, "Paused");
			labelTitleShadow.Position = iconFrame.Position + new Vector((iconFrame.Width / 2 - labelTitle.Width / 2) + 4, 19);

			buttonNewGame = new Button();
			buttonNewGame.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonNewGame.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonNewGame.Label = new Label();
			buttonNewGame.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "New Game");
			buttonNewGame.ButtonType = ButtonType.TextOnly;
			buttonNewGame.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonNewGame.Width / 2, 75);
			buttonNewGame.Clicked += buttonNewGame_Clicked;

			buttonLoadGame = new Button();
			buttonLoadGame.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonLoadGame.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonLoadGame.Label = new Label();
			buttonLoadGame.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Load Game");
			buttonLoadGame.ButtonType = ButtonType.TextOnly;
			buttonLoadGame.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonLoadGame.Width / 2, 110);
			buttonLoadGame.Clicked += buttonLoadGame_Clicked;

			buttonOptions = new Button();
			buttonOptions.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonOptions.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonOptions.Label = new Label();
			buttonOptions.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Options");
			buttonOptions.ButtonType = ButtonType.TextOnly;
			buttonOptions.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonOptions.Width / 2, 145);
			buttonOptions.Clicked += buttonOptions_Clicked;

			buttonQuit = new Button();
			buttonQuit.TextureFrame = ContentManager.GetTexture("ButtonMainMenuItem");
			buttonQuit.TextureFrameHovered = ContentManager.GetTexture("ButtonMainMenuItemHover");
			buttonQuit.Label = new Label();
			buttonQuit.Label.TrueTypeText = ContentManager.GetTrueTypeText(fontPath, fontSizeContent, fontColorLabelValue, "Quit");
			buttonQuit.ButtonType = ButtonType.TextOnly;
			buttonQuit.Position = iconFrame.Position + new Vector(iconFrame.Width / 2 - buttonQuit.Width / 2, 180);
			buttonQuit.Clicked += buttonQuit_Clicked;
		}

		public override void Draw(SharpDL.GameTime gameTime, SharpDL.Graphics.Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			iconFrame.Draw(gameTime, renderer);
			labelTitleShadow.Draw(gameTime, renderer);
			labelTitle.Draw(gameTime, renderer);
			buttonNewGame.Draw(gameTime, renderer);
			buttonLoadGame.Draw(gameTime, renderer);
			buttonOptions.Draw(gameTime, renderer);
			buttonQuit.Draw(gameTime, renderer);
		}

		public override void Update(SharpDL.GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			iconFrame.Update(gameTime);
			labelTitleShadow.Update(gameTime);
			labelTitle.Update(gameTime);
			buttonNewGame.Update(gameTime);
			buttonLoadGame.Update(gameTime);
			buttonOptions.Update(gameTime);
			buttonQuit.Update(gameTime);
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
			labelTitleShadow.Dispose();
			labelTitle.Dispose();
			buttonNewGame.Dispose();
			buttonLoadGame.Dispose();
			buttonQuit.Dispose();
			buttonOptions.Dispose();
		}
	}
}
