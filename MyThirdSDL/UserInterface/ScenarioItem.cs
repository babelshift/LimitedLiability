using MyThirdSDL.Content;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ScenarioItem : Control
	{
		private Label labelName;
		private Label labelOverview;
		private Icon iconThumbnail;
		private Icon iconOverview;
		private Button buttonSelect;

		public override Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				iconThumbnail.Position = base.Position + new Vector(8, 8);
				labelName.Position = new Vector(iconThumbnail.Position.X + iconThumbnail.Width + 15, iconThumbnail.Position.Y + (iconThumbnail.Height / 2 - labelName.Height / 2));
			}
		}

		public ScenarioItem(ContentManager contentManager, string iconThumbnailKey, string iconOverviewKey, string textItemName, string textOverview)
		{
			string fontPath = contentManager.GetContentPath("Arcade");
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorPaleYellow = Styles.Colors.PaleYellow;
			int fontSizeName = 14;
			int fontSizeOverview = 10;

			labelName = ControlFactory.CreateLabel(contentManager, fontPath, fontSizeName, fontColorPaleYellow, textItemName, 225);
			labelName.EnableShadow(contentManager, 2, 2);

			iconThumbnail = new Icon(contentManager.GetTexture(iconThumbnailKey));
			iconOverview = new Icon(contentManager.GetTexture(iconOverviewKey));

			buttonSelect = ControlFactory.CreateButton(contentManager, "ButtonSquare", "ButtonSquareHover");
			buttonSelect.Icon = ControlFactory.CreateIcon(contentManager, "IconWindowConfirm");
			buttonSelect.IconHovered = ControlFactory.CreateIcon(contentManager, "IconWindowConfirm");
			buttonSelect.ButtonType = ButtonType.IconOnly;

			Width = labelName.Bounds.Right - iconThumbnail.Bounds.Left;
			Height = iconThumbnail.Height;
		}

		public override void Update(SharpDL.GameTime gameTime)
		{
			base.Update(gameTime);

			labelName.Update(gameTime);
			//labelOverview.Update(gameTime);
			iconThumbnail.Update(gameTime);
			iconOverview.Update(gameTime);
			buttonSelect.Update(gameTime);
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			labelName.Draw(gameTime, renderer);
			//labelOverview.Draw(gameTime, renderer);
			iconThumbnail.Draw(gameTime, renderer);
			//iconOverview.Draw(gameTime, renderer);
			//buttonSelect.Draw(gameTime, renderer);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			base.HandleMouseMovingEvent(sender, e);

			labelName.HandleMouseMovingEvent(sender, e);
			//labelOverview.Update(gameTime);
			iconThumbnail.HandleMouseMovingEvent(sender, e);
			//iconOverview.HandleMouseMovingEvent(sender, e);
			//buttonSelect.HandleMouseMovingEvent(sender, e);
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			base.HandleMouseButtonPressedEvent(sender, e);

			labelName.HandleMouseButtonPressedEvent(sender, e);
			//labelOverview.Update(gameTime);
			iconThumbnail.HandleMouseButtonPressedEvent(sender, e);
			iconOverview.HandleMouseButtonPressedEvent(sender, e);
			//buttonSelect.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (labelName != null)
				labelName.Dispose();
			if (labelOverview != null)
				labelOverview.Dispose();
			if (iconThumbnail != null)
				iconThumbnail.Dispose();
			if (iconOverview != null)
				iconOverview.Dispose();
			if (buttonSelect != null)
				buttonSelect.Dispose();
		}
	}
}
