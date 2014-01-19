using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public enum MessageBoxType
	{
		Information,
		Warning,
		Error
	}

	public class MessageBox : Control
	{
		private MessageBoxType Type { get; set; }
		private Button ButtonOK { get; set; }
		private Texture TextureFrame { get; set; }
		private Texture TextureIcon { get; set; }
		private string Text { get; set; }
		private bool IsVisible { get; set; }

		public MessageBox(string text, MessageBoxType type)
		{
			//ButtonOK.Clicked += ButtonOK_Clicked;
			Text = text;
			Type = type;
		}

		public void Show()
		{
			IsVisible = true;
		}

		public void Hide()
		{
			IsVisible = false;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if(ButtonOK != null)
				ButtonOK.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (IsVisible)
			{
				if (TextureFrame != null)
					renderer.RenderTexture(TextureFrame, Position.X, Position.Y);
				if (TextureIcon != null)
					renderer.RenderTexture(TextureIcon, Position.X, Position.Y);
				if (ButtonOK != null)
					ButtonOK.Draw(gameTime, renderer);
			}
		}

		public override void HandleMouseButtonPressedEvent(object sender, SharpDL.Events.MouseButtonEventArgs e)
		{
			ButtonOK.HandleMouseButtonPressedEvent(sender, e);
		}

		public override void HandleMouseMovingEvent(object sender, SharpDL.Events.MouseMotionEventArgs e)
		{
			ButtonOK.HandleMouseMovingEvent(sender, e);
		}

		private void ButtonOK_Clicked(object sender, EventArgs e)
		{
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (ButtonOK != null)
				ButtonOK.Dispose();
			if (TextureFrame != null)
				TextureFrame.Dispose();
			if (TextureIcon != null)
				TextureIcon.Dispose();
		}
	}
}
