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
		private Button ButtonOK { get; set; }
		private Texture TextureIcon { get; set; }
		private string Text { get; set; }
		private bool IsVisible { get; set; }

		public MessageBox(Texture texture, Vector position, Texture textureIcon, string text)
			: base(texture, position)
		{
			//ButtonOK.Clicked += ButtonOK_Clicked;
			Text = text;
			TextureIcon = textureIcon;
		}

		public void Show()
		{
			IsVisible = true;
		}

		public void Hide()
		{
			IsVisible = false;
		}

		public override void Draw(SharpDL.GameTime gameTime, Renderer renderer)
		{
			if (IsVisible)
			{
				base.Draw(gameTime, renderer);

				//ButtonOK.Draw(gameTime, renderer);
			}
		}

		private void ButtonOK_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
