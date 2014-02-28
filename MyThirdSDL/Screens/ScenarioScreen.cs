using MyThirdSDL.Content;
using MyThirdSDL.UserInterface;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyThirdSDL.Screens
{
	public class ScenarioScreen : Screen
	{
		private enum ScenarioSelectState
		{
			FirstScreen,
			SecondScreen
		}

		private ScenarioSelectState currentSelectState;
		private Button buttonSelect;
		private Button buttonBack;

		#region Members (Scenario Screen 1)

		private Label labelTitle;
		private Texture textureBackgroundTile;
		private Icon iconFrame;
		private Label labelActiveOverview;
		private Icon iconActiveOverview;

		private const int scenarioItemsPerPage = 5;
		private const int currentPageNumber = 1;
		private readonly Dictionary<int, List<ScenarioItem>> scenarioItemPages = new Dictionary<int, List<ScenarioItem>>();

		private ScenarioItem selectedScenarioItem;

		#endregion Members (Scenario Screen 1)

		#region Members (Scenario Screen 2)

		private Icon iconFrameInputNames;
		private Label labelInputPlayerName;
		private Label labelInputCompanyName;
		private Textbox textboxInputPlayerName;
		private Textbox textboxInputCompanyName;

		#endregion Members (Scenario Screen 2)

		#region Properties

		private ScenarioSelectState CurrentSelectState
		{
			get { return currentSelectState; }
			set
			{
				currentSelectState = value;

				if (value == ScenarioSelectState.FirstScreen)
				{
					iconFrame.Visible = true;
					buttonSelect.Visible = true;
					buttonSelect.Position = iconFrame.Position + new Vector(iconFrame.Width - buttonSelect.Width, iconFrame.Height + 3);
					buttonBack.Position = iconFrame.Position + new Vector(0, iconFrame.Height + 3);
					labelActiveOverview.Visible = true;
					iconActiveOverview.Visible = true;
					foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
						scenarioItem.Visible = true;
					labelTitle.Text = "Select a Scenario";
					labelTitle.EnableShadow(ContentManager, 2, 2);
					labelTitle.Position = iconFrame.Position + new Vector(9, 13);

					iconFrameInputNames.Visible = false;
					textboxInputCompanyName.Visible = false;
					textboxInputPlayerName.Visible = false;
					labelInputCompanyName.Visible = false;
					labelInputPlayerName.Visible = false;

					ResetScenarioItemSelections();
				}
				else if (value == ScenarioSelectState.SecondScreen)
				{
					iconFrame.Visible = false;
					buttonSelect.Visible = true;
					buttonSelect.Position = iconFrameInputNames.Position + new Vector(iconFrameInputNames.Width - buttonSelect.Width, iconFrameInputNames.Height + 3);
					buttonBack.Visible = true;
					buttonBack.Position = iconFrameInputNames.Position + new Vector(0, iconFrameInputNames.Height + 3);
					labelActiveOverview.Visible = false;
					iconActiveOverview.Visible = false;
					foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
						scenarioItem.Visible = false;

					iconFrameInputNames.Visible = true;
					textboxInputCompanyName.Visible = true;
					textboxInputPlayerName.Visible = true;
					labelInputCompanyName.Visible = true;
					labelInputPlayerName.Visible = true;
					labelTitle.Text = "Enter your name and your company's name";
					labelTitle.EnableShadow(ContentManager, 2, 2);
					labelTitle.Position = iconFrameInputNames.Position + new Vector(9, 13);

					ResetAllTextboxes();
				}
			}
		}

		#endregion Properties

		#region Public Event Handlers

		public event EventHandler ReturnToMainMenu;

		public event EventHandler<ScenarioSelectedEventArgs> ScenarioSelected;

		#endregion Public Event Handlers

		#region Constructors

		public ScenarioScreen(ContentManager contentManager)
			: base(contentManager)
		{
		}

		#endregion Constructors

		#region Game Loop

		public override void Activate(Renderer renderer)
		{
			base.Activate(renderer);

			string fontPath = ContentManager.GetContentPath("Arcade");
			Color fontColorWhite = Styles.Colors.White;
			const int fontSizeTitle = 18;
			const int fontSizeContent = 14;

			labelTitle = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeTitle, fontColorWhite, "Select a Scenario");
			labelTitle.EnableShadow(ContentManager, 2, 2);

			textureBackgroundTile = ContentManager.GetTexture("BackgroundScenarioTile");

			iconFrame = ControlFactory.CreateIcon(ContentManager, "MenuScenarioFrame");
			iconFrame.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - iconFrame.Width / 2, MainGame.SCREEN_HEIGHT_LOGICAL / 2 - iconFrame.Height / 2);
			
			iconFrameInputNames = ControlFactory.CreateIcon(ContentManager, "MenuScenarioFrame2");
			iconFrameInputNames.Position = new Vector(MainGame.SCREEN_WIDTH_LOGICAL / 2 - iconFrameInputNames.Width / 2, MainGame.SCREEN_HEIGHT_LOGICAL / 2 - iconFrameInputNames.Height / 2);
			iconFrameInputNames.Visible = false;

			labelTitle.Position = iconFrame.Position + new Vector(9, 13);

			buttonSelect = ControlFactory.CreateButton(ContentManager, "ButtonSquare", "ButtonSquareHover");
			buttonSelect.Icon = ControlFactory.CreateIcon(ContentManager, "IconWindowConfirm");
			buttonSelect.IconHovered = ControlFactory.CreateIcon(ContentManager, "IconWindowConfirm");
			buttonSelect.ButtonType = ButtonType.IconOnly;
			buttonSelect.Visible = false;
			buttonSelect.Clicked += buttonSelect_Clicked;

			buttonBack = ControlFactory.CreateButton(ContentManager, "ButtonSquare", "ButtonSquare");
			buttonBack.Icon = ControlFactory.CreateIcon(ContentManager, "IconArrowCircleLeft");
			buttonBack.IconHovered = ControlFactory.CreateIcon(ContentManager, "IconArrowCircleLeft");
			buttonBack.ButtonType = ButtonType.IconOnly;
			buttonBack.Clicked += ButtonBackOnClicked;
			buttonBack.Position = iconFrame.Position + new Vector(0, iconFrame.Height + 3);

			AddScenarioItem("OfficeOrthogonal1", "ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "A Fresh Start (Plain)", "Fresh out of college, you're on top of the world. You're an aspiring manager who has been given a once in a lifetime opportunity to create a successful business. If you can manage to stay in business for 6 months, you might just prove to your parents that you aren't a loser after all.");
			AddScenarioItem("OfficeOrthogonal1", "ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "Broke as a Joke (Mild)", "After investing the company's money into a pyramid scheme, you find yourself at the bottom of the barrel. Your credit cards are maxed out, and your spouse is thinking of leaving you. How will you manage to bring the company back to its former glory?");
			AddScenarioItem("OfficeOrthogonal1", "ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "Mutiny! (Spicy)", "Your employees are just one empty snack machine away from finally quitting. Unless you can convince them to stay by proving that the company is worth their time, you will surely find yourself on the streets (again). Maybe this time you'll decide that restocking the toilet paper makes employees happy.");
			AddScenarioItem("OfficeOrthogonal1", "ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "Monopoly City (Spicy)", "You're a small fish in a huge ocean dominated by a single, massive shark. While that shark is around, there's no way you'll be able to grow large enough to knock him out of the water. Do what it takes to destroy the monopoly's stranglehold on the market you desire.");
			AddScenarioItem("OfficeOrthogonal2", "ScenarioThumbnail1", "ScenarioThumbnail1Selected", "ScenarioOverview1", "Sandbox Mode", "So you don't want to follow the rules, huh? You're probably a hotshot CEO of a Fortune 500 company, aren't you? Well look no further, you finally found somewhere to express your talents.");

			textboxInputPlayerName = new Textbox(ContentManager);
			textboxInputPlayerName.Visible = false;
			textboxInputPlayerName.Position = iconFrameInputNames.Position + new Vector(200, 60);
			textboxInputPlayerName.Clicked += (sender, e) => textboxInputCompanyName.Blur();
			textboxInputPlayerName.Blurred += textboxInputPlayerName_Blurred;

			textboxInputCompanyName = new Textbox(ContentManager);
			textboxInputCompanyName.Visible = false;
			textboxInputCompanyName.Position = iconFrameInputNames.Position + new Vector(200, 95);
			textboxInputCompanyName.Clicked += (sender, e) => textboxInputPlayerName.Blur();
			textboxInputCompanyName.Blurred += textboxInputCompanyName_Blurred;

			labelInputPlayerName = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeContent, fontColorWhite, "Your Name:");
			labelInputPlayerName.EnableShadow(ContentManager, 2, 2);
			labelInputPlayerName.Position = new Vector(textboxInputPlayerName.Bounds.Left - labelInputPlayerName.Width - 5, textboxInputPlayerName.Position.Y + textboxInputPlayerName.Height / 2 - labelInputPlayerName.Height / 2);
			labelInputPlayerName.Visible = false;
			labelInputCompanyName = ControlFactory.CreateLabel(ContentManager, fontPath, fontSizeContent, fontColorWhite, "Company Name:");
			labelInputCompanyName.EnableShadow(ContentManager, 2, 2);
			labelInputCompanyName.Position = new Vector(textboxInputCompanyName.Bounds.Left - labelInputCompanyName.Width - 5, textboxInputCompanyName.Position.Y + textboxInputCompanyName.Height / 2 - labelInputCompanyName.Height / 2);
			labelInputCompanyName.Visible = false;
		}

		public override void Update(SharpDL.GameTime gameTime, bool otherWindowHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherWindowHasFocus, coveredByOtherScreen);

			if(CurrentSelectState == ScenarioSelectState.FirstScreen)
			{
				iconFrame.Update(gameTime);

				if (iconActiveOverview != null)
					iconActiveOverview.Update(gameTime);
				if (labelActiveOverview != null)
					labelActiveOverview.Update(gameTime);

				foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
					scenarioItem.Update(gameTime);
			}
			else if(CurrentSelectState == ScenarioSelectState.SecondScreen)
			{
				iconFrameInputNames.Update(gameTime);
				textboxInputCompanyName.Update(gameTime);
				textboxInputPlayerName.Update(gameTime);
				labelInputCompanyName.Update(gameTime);
				labelInputPlayerName.Update(gameTime);	
			}

			labelTitle.Update(gameTime);
			buttonSelect.Update(gameTime);
			buttonBack.Update(gameTime);
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			for (int x = 0; x <= MainGame.SCREEN_WIDTH_LOGICAL / textureBackgroundTile.Width; x++)
				for (int y = 0; y <= MainGame.SCREEN_HEIGHT_LOGICAL / textureBackgroundTile.Height; y++)
					textureBackgroundTile.Draw(x * textureBackgroundTile.Width, y * textureBackgroundTile.Height);

			if (CurrentSelectState == ScenarioSelectState.FirstScreen)
			{
				iconFrame.Draw(gameTime, renderer);

				if (iconActiveOverview != null)
					iconActiveOverview.Draw(gameTime, renderer);
				if (labelActiveOverview != null)
					labelActiveOverview.Draw(gameTime, renderer);

				foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
					scenarioItem.Draw(gameTime, renderer);
			}
			else if (CurrentSelectState == ScenarioSelectState.SecondScreen)
			{
				iconFrameInputNames.Draw(gameTime, renderer);
				textboxInputCompanyName.Draw(gameTime, renderer);
				textboxInputPlayerName.Draw(gameTime, renderer);
				labelInputCompanyName.Draw(gameTime, renderer);
				labelInputPlayerName.Draw(gameTime, renderer);
			}

			labelTitle.Draw(gameTime, renderer);
			buttonSelect.Draw(gameTime, renderer);
			buttonBack.Draw(gameTime, renderer);
		}

		#endregion Game Loop

		#region Handle Input

		public override void HandleMouseButtonReleasedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
		}

		public override void HandleKeyStates(System.Collections.Generic.IEnumerable<SharpDL.Input.KeyInformation> keysPressed, System.Collections.Generic.IEnumerable<SharpDL.Input.KeyInformation> keysReleased)
		{
			base.HandleKeyStates(keysPressed, keysReleased);

			foreach (var key in keysPressed)
			{
				if (key.VirtualKey == SharpDL.Input.VirtualKeyCode.Escape)
					OnReturnToMainMenu();

				iconFrameInputNames.HandleKeyPressed(key);
				textboxInputCompanyName.HandleKeyPressed(key);
				textboxInputPlayerName.HandleKeyPressed(key);
			}
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			labelTitle.HandleMouseButtonPressedEvent(sender, e);
			buttonSelect.HandleMouseButtonPressedEvent(sender, e);
			buttonBack.HandleMouseButtonPressedEvent(sender, e);

			foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
				scenarioItem.HandleMouseButtonPressedEvent(sender, e);

			iconFrameInputNames.HandleMouseButtonPressedEvent(sender, e);
			textboxInputCompanyName.HandleMouseButtonPressedEvent(sender, e);
			textboxInputPlayerName.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			labelTitle.HandleMouseMovingEvent(sender, e);
			buttonSelect.HandleMouseMovingEvent(sender, e);
			buttonBack.HandleMouseMovingEvent(sender, e);

			foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
				scenarioItem.HandleMouseMovingEvent(sender, e);

			iconFrameInputNames.HandleMouseMovingEvent(sender, e);
			textboxInputCompanyName.HandleMouseMovingEvent(sender, e);
			textboxInputPlayerName.HandleMouseMovingEvent(sender, e);
		}

		public override void HandleTextInputtingEvent(object sender, SharpDL.Events.TextInputEventArgs e)
		{
			textboxInputCompanyName.HandleTextInput(e.Text);
			textboxInputPlayerName.HandleTextInput(e.Text);
		}

		#endregion Handle Input

		#region Control Events

		private void ButtonBackOnClicked(object sender, EventArgs eventArgs)
		{
			if (CurrentSelectState == ScenarioSelectState.SecondScreen)
				CurrentSelectState = ScenarioSelectState.FirstScreen;
			else
				OnReturnToMainMenu();
		}

		private void OnReturnToMainMenu()
		{
			if (ReturnToMainMenu != null)
				ReturnToMainMenu(this, EventArgs.Empty);
		}

		/// <summary>
		/// When a textbox is blurred, we need to check if any other textboxes got focus after our blur. If another textbox has focus, we need to enable text input again.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textboxInputCompanyName_Blurred(object sender, EventArgs e)
		{
			if (textboxInputPlayerName.IsFocused)
				Keyboard.StartTextInput();
		}

		/// <summary>
		/// When a textbox is blurred, we need to check if any other textboxes got focus after our blur. If another textbox has focus, we need to enable text input again.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textboxInputPlayerName_Blurred(object sender, EventArgs e)
		{
			if (textboxInputCompanyName.IsFocused)
				Keyboard.StartTextInput();
		}

		private void buttonSelect_Clicked(object sender, EventArgs e)
		{
			if (CurrentSelectState == ScenarioSelectState.FirstScreen)
				CurrentSelectState = ScenarioSelectState.SecondScreen;
			else if (CurrentSelectState == ScenarioSelectState.SecondScreen)
				if (ScenarioSelected != null)
					if(!String.IsNullOrEmpty(textboxInputPlayerName.Text) && !String.IsNullOrEmpty(textboxInputCompanyName.Text))
						ScenarioSelected(sender, new ScenarioSelectedEventArgs(selectedScenarioItem.MapPathToLoad, textboxInputPlayerName.LabelText.Text, textboxInputCompanyName.LabelText.Text));
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

		#endregion Control Events

		#region General Methods

		private void ResetAllTextboxes()
		{
			textboxInputCompanyName.Clear();
			textboxInputPlayerName.Clear();
		}

		private void ResetScenarioItemSelections()
		{
			foreach (var scenarioItem in GetScenarioItemsOnPage(currentPageNumber))
				scenarioItem.ResetPosition();
		}

		private void AddScenarioItem(string mapPathToLoad, string iconThumbnailKey, string iconThumbnailSelectedKey, string iconOverviewKey, string textItemName, string textOverview)
		{
			ScenarioItem scenarioItem = new ScenarioItem(ContentManager, iconThumbnailKey, iconThumbnailSelectedKey, iconOverviewKey, textItemName, textOverview, mapPathToLoad);
			scenarioItem.Clicked += (sender, e) => OnScenarioItemClicked(sender);

			int lastPageNumber = scenarioItemPages.Keys.Count();

			List<ScenarioItem> scenarioItemsOnLastPage;
			if (scenarioItemPages.Keys.Count > 0)
			{
				scenarioItemsOnLastPage = GetScenarioItemsOnPage(lastPageNumber);

				if (scenarioItemsOnLastPage.Count < scenarioItemsPerPage)
					scenarioItemsOnLastPage.Add(scenarioItem);
				else
				{
					scenarioItemsOnLastPage = new List<ScenarioItem> { scenarioItem };
					scenarioItemPages.Add(lastPageNumber + 1, scenarioItemsOnLastPage);
				}
			}
			else
			{
				scenarioItemsOnLastPage = new List<ScenarioItem> { scenarioItem };
				scenarioItemPages.Add(1, scenarioItemsOnLastPage);
			}

			int itemsOnLastPageCount = scenarioItemsOnLastPage.Count;

			switch (itemsOnLastPageCount)
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

		#endregion General Methods

		#region Dispose

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

			if (textboxInputCompanyName != null)
				textboxInputCompanyName.Dispose();
			if (textboxInputPlayerName != null)
				textboxInputPlayerName.Dispose();
		}

		#endregion Dispose
	}

	public class ScenarioSelectedEventArgs : EventArgs
	{
		public string MapPathToLoad { get; private set; }

		public string PlayerName { get; private set; }

		public string CompanyName { get; private set; }

		public ScenarioSelectedEventArgs(string mapPathToLoad, string playerName, string companyName)
		{
			MapPathToLoad = mapPathToLoad;
			PlayerName = playerName;
			CompanyName = companyName;
		}
	}
}