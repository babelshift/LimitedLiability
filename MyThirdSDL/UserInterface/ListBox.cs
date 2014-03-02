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
	public class ListBox<T> : Control
	{
		private Vector iconScrollerMinPosition;
		private Vector iconScrollerMaxPosition;

		private RenderTarget renderTarget;
		private Texture textureFrame;
		private Icon iconScrollbar;
		private Icon iconScroller;

		private List<ListItem<T>> items = new List<ListItem<T>>();

		public IReadOnlyList<ListItem<T>> Items { get { return items; } }

		/// <summary>
		/// Vertical spacing between each item
		/// </summary>
		public int ItemSpacing { get; set; }

		public bool IsAnyItemHovered
		{
			get
			{
				foreach (var item in items)
					if (item.IsHovered)
						return true;
				return false;
			}
		}

		public override Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				iconScrollbar.Position = base.Position + new Vector(Width + 5, 0);
				iconScroller.Position = base.Position + new Vector(Width + 6, 2);

				// find the max column count of our list items
				int maxColumnCount = GetMaxColumnCount();

				// set the column widths for all items in the listbox to be equal to the greatest control width in each column
				// this normalizes the columns so that they are aligned properly
				SetColumnWidths(maxColumnCount);

				// set the position of each item with a vertical spacing between each item
				for (int i = 0; i < items.Count; i++)
					items[i].Position = new Vector(0, i * ItemSpacing);

				iconScrollerMinPosition = base.Position + new Vector(Width + 6, 1);
				iconScrollerMaxPosition = new Vector(iconScroller.Position.X, iconScrollerMinPosition.Y + iconScrollbar.Height - iconScroller.Height - 2);
			}
		}

		public event EventHandler<ListItemHoveredEventArgs<T>> ItemHovered;
		public event EventHandler<ListItemSelectedEventArgs<T>> ItemSelected;

		/// <summary>
		/// Default constructor to create a scrollable listbox that uses the passed textures to form a background and scrollbar visual.
		/// </summary>
		/// <param name="contentManager"></param>
		/// <param name="textureFrame"></param>
		/// <param name="iconScrollbar"></param>
		public ListBox(ContentManager contentManager, Texture textureFrame, Icon iconScrollbar, Icon iconScroller)
		{
			this.textureFrame = textureFrame;
			Width = textureFrame.Width;
			Height = textureFrame.Height;

			// the render target is used to render the contents of the listbox to be contained within the frame of the listbox
			// this allows scrolling objects up and down while containing their rendering to the frame
			this.renderTarget = contentManager.CreateRenderTarget(this.textureFrame.Width, this.textureFrame.Height);
			this.renderTarget.SetBlendMode(BlendMode.Blend);

			this.iconScroller = iconScroller;
			this.iconScrollbar = iconScrollbar;

			ItemSpacing = 30;
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

		public override void Update(SharpDL.GameTime gameTime)
		{
			if (!Visible)
				return;

			foreach (var item in items)
				item.Update(gameTime);

			iconScrollbar.Update(gameTime);
			iconScroller.Update(gameTime);

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

			// adjust our positioning to handle render target controls
			e.RelativeToWindowX -= (int)Position.X;
			e.RelativeToWindowY -= (int)Position.Y;

			foreach (var item in items)
				item.HandleMouseButtonPressedEvent(sender, e);

			e.RelativeToWindowX += (int)Position.X;
			e.RelativeToWindowY += (int)Position.Y;
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

			// adjust our positioning to handle render target controls
			e.RelativeToWindowX -= (int)Position.X;
			e.RelativeToWindowY -= (int)Position.Y;

			foreach (var item in items)
				item.HandleMouseMovingEvent(sender, e);

			e.RelativeToWindowX += (int)Position.X;
			e.RelativeToWindowY += (int)Position.Y;
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

		/// <summary>
		/// Loops through all columns in the listbox, calculates the maximum width of all items the column and then sets the width of all controls in the column to be equal to that maximum width.
		/// This normalizes the width of each column so that everything is aligned properly. Without adjusting to the maximum width of items in the columns, it is possible to get a jagged, misaligned
		/// grid effect.
		/// </summary>
		/// <param name="maxColumnCount"></param>
		private void SetColumnWidths(int maxColumnCount)
		{
			for (int i = 0; i < maxColumnCount; i++)
			{
				int maxItemWidthInColumn = GetMaximumColumnWidth(i);

				SetMaxColumnWidth(i, maxItemWidthInColumn);
			}
		}

		/// <summary>
		/// Loops through all items in the listbox and sets the width of the column at index i to the passed value
		/// </summary>
		/// <param name="i"></param>
		/// <param name="maxItemWidthInColumn"></param>
		private void SetMaxColumnWidth(int i, int maxItemWidthInColumn)
		{
			foreach (var item in items)
				item.SetColumnWidth(i, maxItemWidthInColumn);
		}

		/// <summary>
		/// Calculates and returns the maximum width of items in the column i.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		private int GetMaximumColumnWidth(int i)
		{
			int maxItemWidthInColumn = 0;
			foreach (var item in items)
			{
				int itemWidthInColumn = item.Columns[i].Width;
				if (itemWidthInColumn > maxItemWidthInColumn)
					maxItemWidthInColumn = itemWidthInColumn;
			}
			return maxItemWidthInColumn;
		}

		/// <summary>
		/// Returns the maximum number of columns found in all listitems.
		/// </summary>
		/// <returns></returns>
		private int GetMaxColumnCount()
		{
			int maxColumnCount = 0;
			foreach (var item in items)
			{
				int columnCount = item.Columns.Count;
				if (columnCount > maxColumnCount)
					maxColumnCount = columnCount;
			}
			return maxColumnCount;
		}

		/// <summary>
		/// Adds a new list item to the list box. Note that this will not adjust the positions and widths until the position of the entire control is reset.
		/// </summary>
		/// <param name="item"></param>
		public void AddItem(ListItem<T> item)
		{
			item.Hovered += item_Hovered;
			item.Clicked += item_Selected;
			item.SetWidth(renderTarget.Width);
			item.SetHeight(ItemSpacing);
			items.Add(item);
		}

		private void item_Selected(object sender, ListItemSelectedEventArgs<T> e)
		{
			if (ItemSelected != null)
				ItemSelected(sender, e);
		}

		private void item_Hovered(object sender, ListItemHoveredEventArgs<T> e)
		{
			if (ItemHovered != null)
				ItemHovered(sender, e);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
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
