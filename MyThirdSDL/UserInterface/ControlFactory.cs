using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ControlFactory
	{
		private Renderer renderer;

		public ControlFactory(Renderer renderer)
		{
			this.renderer = renderer;
		}

		public MessageBox CreateMessageBox(Vector position, string text, MessageBoxType type)
		{
			string textureFramePath = "Images/User Interface/MessageBoxFrame.png";
			string textureWarningIconPath = "Images/User Interface/IconWarning.png";
			Surface surfaceFrame = new Surface(textureFramePath, Surface.SurfaceType.PNG);
			Texture textureFrame = new Texture(renderer, surfaceFrame);
			Surface surfaceIcon = new Surface(textureWarningIconPath, Surface.SurfaceType.PNG);
			Texture textureIcon = new Texture(renderer, surfaceIcon);
			MessageBox messageBox = new MessageBox(textureFrame, position, textureIcon, text);
			return messageBox;
		}
	}
}
