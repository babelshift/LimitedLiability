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
	public class ScenarioScreen : Screen
	{
		private Label labelTitle;
		private Texture textureBackgroundTile;
		private Icon iconFrame;

		private List<ScenarioItem> scenarioItems = new List<ScenarioItem>();

		public event EventHandler ReturnToMainMenu;

		public ScenarioScreen(ContentManager contentManager)
			: base(contentManager)
		{
		}

		public override void Activate(Renderer renderer)
		{
			base.Activate(renderer);
			
			string fontPath = ContentManager.GetContentPath("Arcade");
			Color fontColorWhite = Styles.Colors.White;
			int fontSizeTitle = 18;

			labelTitle = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeTitle, fontColorWhite, "Select a Scenario");
			labelTitle.EnableShadow(ContentManager, 2, 2);

			textureBackgroundTile = ContentManager.GetTexture("BackgroundScenarioTile");

			iconFrame = ControlFactory.CreateIcon(ContentManager, "MenuScenarioFrame");
			iconFrame.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - iconFrame.Width / 2, MainGame.SCREEN_HEIGHT_LOGICAL / 2 - iconFrame.Height / 2);

			labelTitle.Position = iconFrame.Position + new Vector(9, 13);

			AddScenarioItem("IconPizza", "IconPizza", "A Fresh Start...", "In this scenario...");
		}

		public override void Update(SharpDL.GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			iconFrame.Update(gameTime);
			labelTitle.Update(gameTime);

			foreach (var scenarioItem in scenarioItems)
				scenarioItem.Update(gameTime);
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			for (int x = 0; x <= MainGame.SCREEN_WIDTH_LOGICAL / textureBackgroundTile.Width; x++)
				for (int y = 0; y <= MainGame.SCREEN_HEIGHT_LOGICAL / textureBackgroundTile.Height; y++)
					renderer.RenderTexture(textureBackgroundTile, x * textureBackgroundTile.Width, y * textureBackgroundTile.Height);

			iconFrame.Draw(gameTime, renderer);
			labelTitle.Draw(gameTime, renderer);

			foreach (var scenarioItem in scenarioItems)
				scenarioItem.Draw(gameTime, renderer);
		}

		public override void HandleKeyStates(System.Collections.Generic.IEnumerable<SharpDL.Input.KeyInformation> keysPressed, System.Collections.Generic.IEnumerable<SharpDL.Input.KeyInformation> keysReleased)
		{
			base.HandleKeyStates(keysPressed, keysReleased);

			foreach (var key in keysPressed)
				if (key.VirtualKey == SharpDL.Input.VirtualKeyCode.Escape)
					if(ReturnToMainMenu != null)
						ReturnToMainMenu(this, EventArgs.Empty);
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			labelTitle.HandleMouseButtonPressedEvent(sender, e);

			foreach (var scenarioItem in scenarioItems)
				scenarioItem.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			labelTitle.HandleMouseMovingEvent(sender, e);

			foreach (var scenarioItem in scenarioItems)
				scenarioItem.HandleMouseMovingEvent(sender, e);
		}

		private void AddScenarioItem(string iconThumbnailKey, string iconOverviewKey, string textItemName, string textOverview)
		{
			ScenarioItem scenarioItem = new ScenarioItem(ContentManager, iconThumbnailKey, iconOverviewKey, textItemName, textOverview);
			scenarioItem.Position = iconFrame.Position + new Vector(9, 45);
			scenarioItems.Add(scenarioItem);
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
			if(labelTitle != null)
				labelTitle.Dispose();
			foreach (var scenarioItem in scenarioItems)
				if(scenarioItem != null)
					scenarioItem.Dispose();
			scenarioItems.Clear();
		}
	}
}
