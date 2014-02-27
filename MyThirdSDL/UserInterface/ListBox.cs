using MyThirdSDL.Content;
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
				iconScrollerOriginPosition = base.Position + new Vector(Width + 6, 2);

				for(int i = 0; i < items.Count; i++)
					items[i].Position = new Vector(5, i * 30);
			}
		}

		public ListBox(ContentManager contentManager, Texture textureFrame)
		{
			this.textureFrame = textureFrame;
			Width = textureFrame.Width;
			Height = textureFrame.Height;

			this.renderTarget = contentManager.CreateRenderTarget(this.textureFrame.Width, this.textureFrame.Height);
			this.renderTarget.SetBlendMode(BlendMode.Blend);

			iconScroller = ControlFactory.CreateIcon(contentManager, "IconScroller");
			iconScrollbar = ControlFactory.CreateIcon(contentManager, "IconScrollbarMenuPurchase");

			iconScroller.Clicked += iconScroller_Clicked;
		}

		private void iconScroller_Clicked(object sender, EventArgs e)
		{
			isScrollerClicked = true;
		}

		bool isScrollerClicked = false;

		public override void Update(SharpDL.GameTime gameTime)
		{
			if (!Visible)
				return;

			base.Update(gameTime);
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			if (!Visible)
				return;
			base.HandleMouseButtonPressedEvent(sender, e);

			iconScroller.HandleMouseButtonPressedEvent(sender, e);
		}

		private Vector iconScrollerOriginPosition = Vector.Zero;

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			if (!Visible)
				return;
			base.HandleMouseMovingEvent(sender, e);

			if (isScrollerClicked)
			{
				int yDistance = e.RelativeToLastMotionEventY;
				foreach (var item in items)
					item.Position = item.Position - new Vector(0, yDistance);

				Vector iconScrollerPosition = iconScroller.Position + new Vector(0, yDistance);
				if (iconScrollerPosition.Y < iconScrollerOriginPosition.Y)
					iconScroller.Position = iconScrollerOriginPosition;
				else if (iconScrollerPosition.Y > iconScrollerOriginPosition.Y + iconScrollbar.Height - iconScroller.Height - 5)
					iconScroller.Position = iconScrollerOriginPosition + new Vector(0, iconScrollbar.Height - iconScroller.Height - 5);
				else
					iconScroller.Position = iconScrollerPosition;
			}
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
			item.Position = new Vector(5, items.Count * 30);
			items.Add(item);
		}

		public void RemoveItem(ListItem item)
		{
			items.Remove(item);
		}

		public override void Dispose()
		{
			textureFrame.Dispose();
			iconScroller.Dispose();
			iconScrollbar.Dispose();

			foreach (var item in items)
				item.Dispose();
		}
	}
}
