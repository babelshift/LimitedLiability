using System;
using System.Collections.Generic;
using SharpDL;
using SharpDL.Graphics;
using LimitedLiability.Content;
using LimitedLiability.Content.Data;
using LimitedLiability.Simulation;

namespace LimitedLiability.UserInterface
{
	public enum MessageBoxType
	{
		Information,
		Warning,
		Error
	}

	public class MessageBox : Control
	{
		private readonly List<Control> controls = new List<Control>();
		private readonly Icon iconFrame;
		private readonly Icon iconMain;
		private readonly Label labelTextTitle;
		private readonly Label labelTextContent;
		private TimeSpan simulationTimeShown;

		public override Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				iconFrame.Position = base.Position;
				iconMain.Position = base.Position + new Vector(10, 10);
				labelTextTitle.Position = base.Position + new Vector(40, 15);
				labelTextContent.Position = base.Position + new Vector(10, 45);
			}
		}

		public MessageBox(ContentManager contentManager, MessageBoxType type, string title, string content)
		{
			if (String.IsNullOrEmpty(title))
				title = StringReferenceKeys.DEFAULT_TEXT;
			if (String.IsNullOrEmpty(content))
				content = StringReferenceKeys.DEFAULT_TEXT;

			iconFrame = ControlFactory.CreateIcon(contentManager, "MessageBoxFrame");
			Width = iconFrame.Width;
			Height = iconFrame.Height;

			string fontPath = contentManager.GetContentPath(Styles.Fonts.Arcade);
			Color fontColorTitle = Styles.Colors.White;
			Color fontColorLabelValue = Styles.Colors.PaleYellow;
			int fontSizeTitle = Styles.FontSizes.Title;
			int fontSizeContent = Styles.FontSizes.Content;

			labelTextTitle = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeTitle, fontColorTitle, title);
			labelTextTitle.EnableShadow(contentManager, 2, 2);
			labelTextContent = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeContent, fontColorLabelValue, content, 325);

			if (type == MessageBoxType.Information)
				iconMain = ControlFactory.CreateIcon(contentManager, "IconInfo");

			controls.Add(iconFrame);
			controls.Add(iconMain);
			controls.Add(labelTextTitle);
			controls.Add(labelTextContent);
			
			Hide();
		}

		public void Show(TimeSpan simulationTime)
		{
			Visible = true;
			simulationTimeShown = simulationTime;
		}

		public void Hide()
		{
			Visible = false;
		}

		public override void Update(GameTime gameTime)
		{
			// stop showing the message box after 5 seconds
			if (SimulationManager.SimulationTime.Subtract(simulationTimeShown) > TimeSpan.FromSeconds(5.0))
				Visible = false;

			if (!Visible)
				return;

			base.Update(gameTime);

			foreach (var control in controls)
				control.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible)
				return;

			foreach (var control in controls)
				control.Draw(gameTime, renderer);
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			if (!Visible)
				return;

			foreach (var control in controls)
				control.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			if (!Visible)
				return;

			foreach (var control in controls)
				control.HandleMouseMovingEvent(sender, e);
		}

		private void ButtonOK_Clicked(object sender, EventArgs e)
		{
			Hide();
			Dispose();
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach (var control in controls)
				control.Dispose();
		}
	}
}