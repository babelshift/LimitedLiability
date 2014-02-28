using MyThirdSDL.Content;
using SharpDL.Events;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ListBox : Control
	{
		private Vector iconScrollerMinPosition;
		private Vector iconScrollerMaxPosition;

		private RenderTarget renderTarget;
		private Texture textureFrame;
		private Icon iconScrollbar;
		private Icon iconScroller;

		private List<ListItem> items = new List<ListItem>();

		public IReadOnlyList<ListItem> Items { get { return items; } }

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				iconScrollbar.Position = base.Position + new Vector(Width + 5, 0);
				iconScroller.Position = base.Position + new Vector(Width + 6, 2);

				for (int i = 0; i < items.Count; i++)
					items[i].Position = new Vector(5, i * ItemSpacing);

				iconScrollerMinPosition = base.Position + new Vector(Width + 6, 1);
				iconScrollerMaxPosition = new Vector(iconScroller.Position.X, iconScrollerMinPosition.Y + iconScrollbar.Height - iconScroller.Height - 2);
			}
		}

		public int ItemSpacing { get; set; }

		public int ColumnSpacing { get; set; }

		public ListBox(ContentManager contentManager, Texture textureFrame, Icon iconScrollbar)
		{
			this.textureFrame = textureFrame;
			Width = textureFrame.Width;
			Height = textureFrame.Height;

			this.renderTarget = contentManager.CreateRenderTarget(this.textureFrame.Width, this.textureFrame.Height);
			this.renderTarget.SetBlendMode(BlendMode.Blend);

			iconScroller = ControlFactory.CreateIcon(contentManager, "IconScroller");
			this.iconScrollbar = iconScrollbar;

			ItemSpacing = 30;
			ColumnSpacing = 75;
		}

		public override void Update(SharpDL.GameTime gameTime)
		{
			if (!Visible)
				return;

			base.Update(gameTime);
		}

		public override void HandleMouseButtonReleasedEvent(object sender, MouseButtonEventArgs e)
		{
			if (!Visible)
				return;
			base.HandleMouseButtonReleasedEvent(sender, e);

			iconScroller.HandleMouseButtonReleasedEvent(sender, e);
		}

		public override void HandleMouseButtonPressedEvent(object sender, MouseButtonEventArgs e)
		{
			if (!Visible)
				return;
			base.HandleMouseButtonPressedEvent(sender, e);

			iconScroller.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, MouseMotionEventArgs e)
		{
			if (!Visible)
				return;
			base.HandleMouseMovingEvent(sender, e);

			if (iconScroller.IsClicked)
			{
				// allow scrolling under three conditions
				// 1) mouse is between min/max scroller positions (don't scroll if mouse is outside the bounds)
				// 2) mouse is outside bounds, but scroller isn't at the minimum and should be
				// 3) mouse is outside bounds, but scroller isn't at the maximum and should be
				if (IsMouseBetweenMinAndMax(e) || IsScrollerNotAtMinimumButShouldBe(e) || IsScrollerNotAtMaximumButShouldBe(e))
				{
					int scrollDistance = GetScrollDistance(e);

					Vector iconScrollerPosition = GetNewScrollerPosition(scrollDistance);
					ScrollItemsAndScroller(iconScrollerPosition, scrollDistance);
				}
			}
		}

		/// <summary>
		/// Determines the distance that the listbox and the scroller image should scroll based on the mouse movement. The distance at which we scroll is limited
		/// to within the bounds of the minimum and maximum scroll positions.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private int GetScrollDistance(MouseMotionEventArgs e)
		{
			int scrollDistance = e.RelativeToLastMotionEventY;

			if (iconScroller.Position.Y + scrollDistance < iconScrollerMinPosition.Y)
				scrollDistance = (int)(iconScrollerMinPosition.Y - iconScroller.Position.Y);
			else if (iconScroller.Position.Y + scrollDistance > iconScrollerMaxPosition.Y)
				scrollDistance = (int)(iconScrollerMaxPosition.Y - iconScroller.Position.Y);

			return scrollDistance;
		}

		/// <summary>
		/// Determines if the mouse is beyond the minimum scroll position but the scroller is not. If this is the case, true is returned. Otherwise, false.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private bool IsScrollerNotAtMaximumButShouldBe(MouseMotionEventArgs e)
		{
			return e.RelativeToWindowY > iconScrollerMaxPosition.Y && iconScroller.Position.Y < iconScrollerMaxPosition.Y;
		}

		/// <summary>
		/// Determines if the mouse is beyond the maximum scroll position but the scroller is not. If this is the case, true is returned. Otherwise, false.
		/// <param name="e"></param>
		/// <returns></returns>
		private bool IsScrollerNotAtMinimumButShouldBe(MouseMotionEventArgs e)
		{
			return e.RelativeToWindowY < iconScrollerMinPosition.Y && iconScroller.Position.Y > iconScrollerMinPosition.Y;
		}

		/// <summary>
		/// Determines if the mouse is between the minimum and maximum scroll positions. If this is the case, true is returned. Otherwise, false.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private bool IsMouseBetweenMinAndMax(MouseMotionEventArgs e)
		{
			return e.RelativeToWindowY > iconScrollerMinPosition.Y && e.RelativeToWindowY < iconScrollerMaxPosition.Y;
		}

		/// <summary>
		/// Attempts to scroll all the items in the list and the scroller image. If the mouse is beyond the scroll minimum and maximum, no movement occurs.
		/// </summary>
		/// <param name="iconScrollerPosition"></param>
		/// <param name="scrollDistance"></param>
		private void ScrollItemsAndScroller(Vector iconScrollerPosition, int scrollDistance)
		{
			// if the position will be less than the origin, lock it to the origin
			if (iconScrollerPosition.Y < iconScrollerMinPosition.Y)
			{
				iconScroller.Position = iconScrollerMinPosition;
				scrollDistance = 0;
			}
			// if the position will be greater than the origin plus the height of the scroll bar, lock it to the origin plus the height of the scrollbar
			else if (iconScrollerPosition.Y > iconScrollerMaxPosition.Y)
			{
				iconScroller.Position = iconScrollerMaxPosition;
				scrollDistance = 0;
			}
			// otherwise, move it relative to the mouse movement
			else
				iconScroller.Position = iconScrollerPosition;

			foreach (var item in items)
				item.Position = item.Position - new Vector(0, scrollDistance);
		}

		/// <summary>
		/// Gets the new scroller image position based on how far we are going to scroll from the mouse movement.
		/// </summary>
		/// <param name="yDistance"></param>
		/// <returns></returns>
		private Vector GetNewScrollerPosition(int yDistance)
		{
			Vector iconScrollerPosition = iconScroller.Position + new Vector(0, yDistance);
			return iconScrollerPosition;
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			if (!Visible)
				return;

			renderer.SetRenderTarget(renderTarget);

			// draw our frame at 0,0 in the render target
			textureFrame.Draw(0, 0);

			// draw all the items of the listbox on top of the frame
			foreach (var item in items)
				item.Draw(gameTime, renderer);

			// reset our render target so that we render to the full screen now
			renderer.ResetRenderTarget();

			// render our final render target texture
			renderTarget.Draw(Position.X, Position.Y);

			iconScrollbar.Draw(gameTime, renderer);
			iconScroller.Draw(gameTime, renderer);
		}

		public void AddItem(ListItem item)
		{
			item.Position = new Vector(5, items.Count * ItemSpacing);
			item.ColumnSpacing = ColumnSpacing;
			items.Add(item);
		}

		public void RemoveItem(ListItem item)
		{
			items.Remove(item);
		}

		public override void Dispose()
		{
			renderTarget.Dispose();
			textureFrame.Dispose();
			iconScroller.Dispose();
			iconScrollbar.Dispose();

			foreach (var item in items)
				item.Dispose();
		}
	}
}
