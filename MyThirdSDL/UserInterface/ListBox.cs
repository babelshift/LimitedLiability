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
		private Texture textureTarget;
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

				iconScrollbar.Position = base.Position;
				iconScroller.Position = base.Position;
			}
		}

		public ListBox(ContentManager contentManager, Texture textureFrame)
		{
			this.textureTarget = contentManager.GetTextureTarget(textureFrame.Width, textureFrame.Height);
			this.textureTarget.SetBlendMode(BlendMode.Blend);
			this.textureFrame = textureFrame;

			iconScroller = ControlFactory.CreateIcon(contentManager, "IconScroller");
			iconScrollbar = ControlFactory.CreateIcon(contentManager, "IconScrollbarMenuPurchase");
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			renderer.SetRenderTarget(textureTarget);

			foreach (var item in items)
				item.Draw(gameTime, renderer);

			renderer.RenderTexture(textureFrame, 0, 0);

			renderer.ResetRenderTarget();

			renderer.RenderTexture(textureTarget, Position.X, Position.Y);

			iconScrollbar.Draw(gameTime, renderer);
			iconScroller.Draw(gameTime, renderer);
		}

		public void AddItem(ListItem item)
		{
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
