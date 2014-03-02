using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.UserInterface
{
	public class TabContainer : Control
	{
		private List<TabPanel> tabs = new List<TabPanel>();

		public override SharpDL.Graphics.Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				for (int i = 0; i < tabs.Count; i++)
				{
					if (i == 0)
						tabs[i].Position = base.Position;
					else
						tabs[i].Position = base.Position + new Vector(tabs[i].TabButtonWidth + 5, 0);
				}
			}
		}

		public TabContainer()
		{

		}

		public override void Update(GameTime gameTime)
		{
			if (!Visible)
				return;
			base.Update(gameTime);

			foreach (var tab in tabs)
				tab.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible)
				return;
			base.Update(gameTime);

			foreach (var tab in tabs)
				tab.Draw(gameTime, renderer);
		}

		public override void HandleMouseButtonReleasedEvent(object sender, MouseButtonEventArgs e)
		{
			if (!Visible)
				return;
			base.HandleMouseButtonReleasedEvent(sender, e);

			foreach (var tab in tabs)
				tab.HandleMouseButtonReleasedEvent(sender, e);
		}

		public override void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			if (!Visible)
				return;
			base.HandleMouseButtonPressedEvent(sender, e);

			foreach (var tab in tabs)
				tab.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			if (!Visible)
				return;
			base.HandleMouseMovingEvent(sender, e);

			foreach (var tab in tabs)
				tab.HandleMouseMovingEvent(sender, e);
		}

		public void AddTab(TabPanel tab)
		{
			// make our first tab active
			if (tabs.Count == 0)
				tab.IsActive = true;
			else
				tab.IsActive = false;
			tab.SetParentTabContainer(this);
			tab.HeaderClicked += tab_HeaderClicked;
			tabs.Add(tab);
		}

		private void tab_HeaderClicked(object sender, EventArgs e)
		{
			TabPanel clickedTab = sender as TabPanel;
			if (clickedTab != null)
			{
				foreach (var tab in tabs)
					tab.IsActive = false;
				clickedTab.IsActive = true;
			}
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach (var tab in tabs)
				tab.Dispose();
		}
	}
}
