using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;

namespace MyThirdSDL.UserInterface
{
	public class TabPanel : Control
	{
		private bool isActive = false;
		private Button buttonHeader;

		private List<Control> controls = new List<Control>();

		public event EventHandler<EventArgs> HeaderClicked;

		public override Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				buttonHeader.Position = base.Position;

				foreach (var control in controls)
					control.Position = parent.Position + new Vector(-3, TabButtonHeight + 13);
			}
		}

		public bool IsActive
		{
			get { return isActive; }
			set
			{
				isActive = value;
				if (isActive)
				{
					Visible = true;
					buttonHeader.ToggleOn();
				}
				else
				{
					Visible = false;
					buttonHeader.ToggleOff();
				}
			}
		}

		public int TabButtonHeight { get { return buttonHeader.Height; } }

		public int TabButtonWidth { get { return buttonHeader.Width; } }

		public TabPanel(Button buttonHeader)
		{
			this.buttonHeader = buttonHeader;
			this.buttonHeader.Released += buttonHeader_Released;
		}

		private void buttonHeader_Released(object sender, EventArgs e)
		{
			// only fire the click event to register a click if this tab panel button isn't active
			if (!IsActive)
				if (HeaderClicked != null)
					HeaderClicked(this, e);
		}

		public override void Update(SharpDL.GameTime gameTime)
		{
			buttonHeader.Update(gameTime);

			if (!IsActive)
				return;
			base.Update(gameTime);

			foreach (var control in controls)
				control.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			buttonHeader.Draw(gameTime, renderer);

			if (!IsActive)
				return;

			foreach (var control in controls)
				control.Draw(gameTime, renderer);
		}

		public override void HandleMouseButtonReleasedEvent(object sender, MouseButtonEventArgs e)
		{
			if (!IsActive)
				buttonHeader.HandleMouseButtonReleasedEvent(sender, e);

			if (!Visible)
				return;

			base.HandleMouseButtonReleasedEvent(sender, e);

			foreach (var control in controls)
				control.HandleMouseButtonReleasedEvent(sender, e);
		}

		public override void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			if (!IsActive)
				buttonHeader.HandleMouseButtonPressedEvent(sender, e);

			if (!Visible)
				return;

			base.HandleMouseButtonPressedEvent(sender, e);

			foreach (var control in controls)
				control.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			//if (!IsActive)
				buttonHeader.HandleMouseMovingEvent(sender, e);

			if (!Visible)
				return;

			base.HandleMouseMovingEvent(sender, e);

			foreach (var control in controls)
				control.HandleMouseMovingEvent(sender, e);
		}

		public void AddControl(Control control)
		{
			controls.Add(control);
		}

		private TabContainer parent;

		public void SetParentTabContainer(TabContainer tabContainer)
		{
			parent = tabContainer;
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