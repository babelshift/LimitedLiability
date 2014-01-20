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
		private Button buttonSelect;
		private Label labelActiveOverview;
		private Icon iconActiveOverview;

		private int scenarioItemsPerPage = 5;
		private int currentPageNumber = 1;
		private Dictionary<int, List<ScenarioItem>> scenarioItemPages = new Dictionary<int, List<ScenarioItem>>();

		private ScenarioItem selectedScenarioItem;

		public event EventHandler ReturnToMainMenu;
		public event EventHandler<ScenarioSelectedEventArgs> ScenarioSelected;

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

			buttonSelect = ControlFactory.CreateButton(ContentManager, "ButtonSquare", "ButtonSquareHover");
			buttonSelect.Icon = ControlFactory.CreateIcon(ContentManager, "IconWindowConfirm");
			buttonSelect.IconHovered = ControlFactory.CreateIcon(ContentManager, "IconWindowConfirm");
			buttonSelect.ButtonType = ButtonType.IconOnly;
			buttonSelect.Visible = false;
			buttonSelect.Clicked += buttonSelect_Clicked;

			AddScenarioItem("ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "A Fresh Start (Plain)", "Fresh out of college, you're on top of the world. You're an aspiring manager who has been given a once in a lifetime opportunity to create a successful business. If you can manage to stay in business for 6 months, you might just prove to your parents that you aren't a loser after all.");
			AddScenarioItem("ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "Broke as a Joke (Mild)", "After investing the company's money into a pyramid scheme, you find yourself at the bottom of the barrel. Your credit cards are maxed out, and your spouse is thinking of leaving you. How will you manage to bring the company back to its former glory?");
			AddScenarioItem("ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "Mutiny! (Spicy)", "Your employees are just one empty snack machine away from finally quitting. Unless you can convince them to stay by proving that the company is worth their time, you will surely find yourself on the streets (again). Maybe this time you'll decide that restocking the toilet paper makes employees happy.");
			AddScenarioItem("ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "Monopoly City (Spicy)", "You're a small fish in a huge ocean dominated by a single, massive shark. While that shark is around, there's no way you'll be able to grow large enough to knock him out of the water. Do what it takes to destroy the monopoly's stranglehold on the market you desire.");
			AddScenarioItem("ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "Sandbox Mode", "So you don't want to follow the rules, huh? You're probably a hotshot CEO of a Fortune 500 company, aren't you? Well look no further, you finally found somewhere to express your talents.");
		}

		public override void Update(SharpDL.GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			iconFrame.Update(gameTime);
			labelTitle.Update(gameTime);

			if (iconActiveOverview != null)
				iconActiveOverview.Update(gameTime);
			if (labelActiveOverview != null)
				labelActiveOverview.Update(gameTime);

			buttonSelect.Update(gameTime);

			foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
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

			if (iconActiveOverview != null)
				iconActiveOverview.Draw(gameTime, renderer);
			if (labelActiveOverview != null)
				labelActiveOverview.Draw(gameTime, renderer);

			buttonSelect.Draw(gameTime, renderer);

			foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
				scenarioItem.Draw(gameTime, renderer);
		}

		public override void HandleKeyStates(System.Collections.Generic.IEnumerable<SharpDL.Input.KeyInformation> keysPressed, System.Collections.Generic.IEnumerable<SharpDL.Input.KeyInformation> keysReleased)
		{
			base.HandleKeyStates(keysPressed, keysReleased);

			foreach (var key in keysPressed)
				if (key.VirtualKey == SharpDL.Input.VirtualKeyCode.Escape)
					if (ReturnToMainMenu != null)
						ReturnToMainMenu(this, EventArgs.Empty);
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			labelTitle.HandleMouseButtonPressedEvent(sender, e);
			buttonSelect.HandleMouseButtonPressedEvent(sender, e);

			foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
				scenarioItem.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			labelTitle.HandleMouseMovingEvent(sender, e);
			buttonSelect.HandleMouseMovingEvent(sender, e);

			foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
				scenarioItem.HandleMouseMovingEvent(sender, e);
		}

		private void AddScenarioItem(string iconThumbnailKey, string iconThumbnailSelectedKey, string iconOverviewKey, string textItemName, string textOverview)
		{
			ScenarioItem scenarioItem = new ScenarioItem(ContentManager, iconThumbnailKey, iconThumbnailSelectedKey,  iconOverviewKey, textItemName, textOverview);
			scenarioItem.Clicked += (sender, e) => OnScenarioItemClicked(sender);

			int lastPageNumber = scenarioItemPages.Keys.Count();

			List<ScenarioItem> scenarioItemsOnLastPage = new List<ScenarioItem>();
			if (scenarioItemPages.Keys.Count > 0)
			{
				scenarioItemsOnLastPage = GetScenarioItemsOnPage(lastPageNumber);

				if (scenarioItemsOnLastPage.Count < scenarioItemsPerPage)
					scenarioItemsOnLastPage.Add(scenarioItem);
				else
				{
					scenarioItemsOnLastPage = new List<ScenarioItem>();
					scenarioItemsOnLastPage.Add(scenarioItem);
					scenarioItemPages.Add(lastPageNumber + 1, scenarioItemsOnLastPage);
				}
			}
			else
			{
				scenarioItemsOnLastPage = new List<ScenarioItem>();
				scenarioItemsOnLastPage.Add(scenarioItem);
				scenarioItemPages.Add(1, scenarioItemsOnLastPage);
			}

			int itemsOnLastPageCount = scenarioItemsOnLastPage.Count;

			switch(itemsOnLastPageCount)
			{
				case 1:
					scenarioItem.Position = iconFrame.Position + new Vector(4, 45); break;
				case 2:
					scenarioItem.Position = iconFrame.Position + new Vector(4, 115); break;
				case 3:
					scenarioItem.Position = iconFrame.Position + new Vector(4, 185); break;
				case 4:
					scenarioItem.Position = iconFrame.Position + new Vector(4, 255); break;
				case 5:
					scenarioItem.Position = iconFrame.Position + new Vector(4, 325); break;
			}
		}

		private List<ScenarioItem> GetScenarioItemsOnPage(int pageNumber)
		{
			List<ScenarioItem> scenarioItemsOnCurrentPage = new List<ScenarioItem>();
			bool success = scenarioItemPages.TryGetValue(pageNumber, out scenarioItemsOnCurrentPage);
			if (success)
				return scenarioItemsOnCurrentPage;
			else
				return new List<ScenarioItem>();
		}

		private void buttonSelect_Clicked(object sender, EventArgs e)
		{
			if (ScenarioSelected != null)
				ScenarioSelected(sender, new ScenarioSelectedEventArgs(selectedScenarioItem.MapPathToLoad));
		}

		private void OnScenarioItemClicked(object sender)
		{
			foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
			{
				if (sender != scenarioItem)
					scenarioItem.ResetPosition();
				else
				{
					labelActiveOverview = scenarioItem.LabelOverview;
					iconActiveOverview = scenarioItem.IconOverview;

					iconActiveOverview.Position = iconFrame.Position + new Vector(354, 50);
					labelActiveOverview.Position = iconFrame.Position + new Vector(353, 125);

					selectedScenarioItem = scenarioItem;
				}
			}

			buttonSelect.Position = iconFrame.Position + new Vector(iconFrame.Width - buttonSelect.Width, iconFrame.Height + 3);
			buttonSelect.Visible = true;
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
			if (labelTitle != null)
				labelTitle.Dispose();
			if (buttonSelect != null)
				buttonSelect.Dispose();
			if (iconFrame != null)
				iconFrame.Dispose();
			if (textureBackgroundTile != null)
				textureBackgroundTile.Dispose();
			foreach (var key in scenarioItemPages.Keys)
			{
				foreach (var scenarioItem in scenarioItemPages[key])
					if (scenarioItem != null)
						scenarioItem.Dispose();
				scenarioItemPages[key].Clear();
			}
			scenarioItemPages.Clear();
		}
	}

	public class ScenarioSelectedEventArgs : EventArgs
	{
		public string MapPathToLoad { get; private set;}

		public ScenarioSelectedEventArgs(string mapPathToLoad)
		{
			MapPathToLoad = mapPathToLoad;
		}
	}
}
