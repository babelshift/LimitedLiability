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

		private void ButtonOK_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
