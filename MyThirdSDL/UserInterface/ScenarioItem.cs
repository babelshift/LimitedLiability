using MyThirdSDL.Content;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	public class ScenarioItem : Control
	{
		private Label labelName;
		private Label labelNameSelected;
		private Label labelActive;
		private Label labelOverview;
		private Icon iconThumbnail;
		private Icon iconThumbnailSelected;
		private Icon iconOverview;
		private Icon iconActive;

		private Vector originalPosition = Vector.Zero;
		private bool isSelected = false;

		public Label LabelOverview { get { return labelOverview; } }

		public Icon IconOverview { get { return iconOverview; } }

		public string MapPathToLoad { get; private set; }

		public new event EventHandler Clicked;

		public override Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				if (originalPosition == Vector.Zero)
					originalPosition = base.Position;

				iconThumbnail.Position = base.Position + new Vector(8, 8);
				iconThumbnailSelected.Position = base.Position + new Vector(8, 8);
				labelName.Position = new Vector(iconThumbnail.Position.X + iconThumbnail.Width + 15, iconThumbnail.Position.Y + (iconThumbnail.Height / 2 - labelName.Height / 2));
				labelNameSelected.Position = new Vector(iconThumbnail.Position.X + iconThumbnail.Width + 15, iconThumbnail.Position.Y + (iconThumbnail.Height / 2 - labelName.Height / 2));
			}
		}

		public ScenarioItem(ContentManager content, string iconThumbnailKey, string iconThumbnailSelectedKey, string iconOverviewKey, string textItemName, string textOverview, string mapPathToLoad)
		{
			MapPathToLoad = mapPathToLoad;

			string fontPath = content.GetContentPath("Arcade");
			Color fontColorWhite = Styles.Colors.White;
			Color fontColorPaleYellow = Styles.Colors.PaleYellow;
			int fontSizeName = 14;

			labelName = ControlFactory.CreateLabel(content, fontPath, fontSizeName, fontColorWhite, textItemName, 225);
			labelName.EnableShadow(content, 2, 2);
			labelNameSelected = ControlFactory.CreateLabel(content, fontPath, fontSizeName, fontColorPaleYellow, textItemName, 225);
			labelNameSelected.EnableShadow(content, 2, 2);

			labelOverview = ControlFactory.CreateLabel(content, fontPath, fontSizeName, fontColorWhite, textOverview, 445);
			labelOverview.EnableShadow(content, 2, 2);

			iconThumbnail = new Icon(content.GetTexture(iconThumbnailKey));
			iconThumbnailSelected = new Icon(content.GetTexture(iconThumbnailSelectedKey));
			iconActive = iconThumbnail;
			iconOverview = new Icon(content.GetTexture(iconOverviewKey));

			Width = iconThumbnail.Width + labelName.Width;
			Height = iconThumbnail.Height;

			base.Clicked += OnClicked;
		}

		private void OnClicked(object sender, EventArgs eventArgs)
		{
			ToggleSelectedOn();
			if (Clicked != null)
				Clicked(sender, eventArgs);
		}

		public override void Update(SharpDL.GameTime gameTime)
		{
			if (!Visible) return;

			base.Update(gameTime);

			if (IsHovered || isSelected)
			{
				iconActive = iconThumbnailSelected;
				labelActive = labelNameSelected;
			}
			else
			{
				iconActive = iconThumbnail;
				labelActive = labelName;
			}

			if (labelActive != null)
				labelActive.Update(gameTime);

			if (iconActive != null)
				iconActive.Update(gameTime);

			if (isSelected)
			{
				labelOverview.Update(gameTime);
				iconOverview.Update(gameTime);
			}
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			if (!Visible) return;

			if (labelActive != null)
				labelActive.Draw(gameTime, renderer);

			if (iconActive != null)
				iconActive.Draw(gameTime, renderer);

			if (isSelected)
			{
				labelOverview.Draw(gameTime, renderer);
				iconOverview.Draw(gameTime, renderer);
			}
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			if (!Visible) return;

			base.HandleMouseMovingEvent(sender, e);

			if (labelActive != null)
				labelActive.HandleMouseMovingEvent(sender, e);

			if (iconActive != null)
				iconActive.HandleMouseMovingEvent(sender, e);

			if (isSelected)
			{
				labelOverview.HandleMouseMovingEvent(sender, e);
				iconOverview.HandleMouseMovingEvent(sender, e);
			}
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			if (!Visible) return;

			base.HandleMouseButtonPressedEvent(sender, e);

			if (labelActive != null)
				labelActive.HandleMouseButtonPressedEvent(sender, e);

			if (iconActive != null)
				iconActive.HandleMouseButtonPressedEvent(sender, e);

			if (isSelected)
			{
				labelOverview.HandleMouseButtonPressedEvent(sender, e);
				iconOverview.HandleMouseButtonPressedEvent(sender, e);
			}
		}

		private void ToggleSelectedOn()
		{
			if (isSelected) return;

			isSelected = true;
			Position += new Vector(10, 0);
		}

		public void ResetPosition()
		{
			isSelected = false;
			Position = originalPosition;
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
			if (labelNameSelected != null)
				labelNameSelected.Dispose();
			if (labelOverview != null)
				labelOverview.Dispose();
			if (iconThumbnail != null)
				iconThumbnail.Dispose();
			if (iconThumbnailSelected != null)
				iconThumbnailSelected.Dispose();
			if (iconOverview != null)
				iconOverview.Dispose();
		}
	}
}