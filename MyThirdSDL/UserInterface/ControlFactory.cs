using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using MyThirdSDL.Simulation;
using MyThirdSDL.Mail;
using System.Reflection;

namespace MyThirdSDL.UserInterface
{
	public static class ControlFactory
	{
		//private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		//private ContentManager contentManager;

		//public ControlFactory(ContentManager contentManager)
		//{
		//	this.contentManager = contentManager;
		//}

		//public SimulationLabel CreateSimulationLabel(Vector position, string fontPath, int fontSize, Color color, SimulationMessage simulationMessage)
		//{
		//	if (log.IsDebugEnabled)
		//		log.Debug(String.Format("Creating simulation label at ({0},{1}) with type: {2} and text: {3}", position.X, position.Y, simulationMessage.Type, simulationMessage.Text));
		//	TrueTypeText trueTypeText = contentManager.GetTrueTypeText(fontPath, fontSize, color, simulationMessage.Text);
		//	return new SimulationLabel(position, trueTypeText, simulationMessage);
		//}

		public static Button CreateButton(ContentManager contentManager, string textureFrameKey, string textureFrameHoveredKey = "", string textureFrameSelectedKey = "")
		{
			if (String.IsNullOrEmpty(textureFrameKey))
				throw new ArgumentNullException("textureFrameKey");

			Button button = new Button();
			button.TextureFrame = contentManager.GetTexture(textureFrameKey);

			if(!String.IsNullOrEmpty(textureFrameHoveredKey))
				button.TextureFrameHovered = contentManager.GetTexture(textureFrameHoveredKey);

			if(!String.IsNullOrEmpty(textureFrameSelectedKey))
				button.TextureFrameSelected = contentManager.GetTexture(textureFrameSelectedKey);
	
			return button;
		}

		public static Icon CreateIcon(ContentManager contentManager, string iconKey)
		{
			if (String.IsNullOrEmpty(iconKey))
				throw new ArgumentNullException("iconKey");

			Icon icon = new Icon(contentManager.GetTexture(iconKey));
			return icon;
		}

		public static Label CreateLabel(ContentManager contentManager, string fontPath, int fontSize, Color fontColor, string text, int wrapLength = 0)
		{
			if (String.IsNullOrEmpty(fontPath))
				throw new ArgumentNullException("fontPath");

			if (String.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");

			if (fontSize <= 0)
				throw new ArgumentOutOfRangeException("fontSize");

			Label label = new Label();
			label.TrueTypeText = contentManager.GetTrueTypeText(fontPath, fontSize, fontColor, text, wrapLength);

			return label;
		}

		public static Tooltip CreateTooltip(ContentManager contentManager, string textureFrameKey, string fontPath, int fontSize, Color fontColor, string text)
		{
			if (String.IsNullOrEmpty(textureFrameKey))
				throw new ArgumentNullException("textureFrameKey");

			if (String.IsNullOrEmpty(fontPath))
				throw new ArgumentNullException("fontPath");

			if (String.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");

			if (fontSize <= 0)
				throw new ArgumentOutOfRangeException("fontSize");

			Tooltip tooltip = new Tooltip();
			tooltip.TextureFrame = contentManager.GetTexture(textureFrameKey);
			tooltip.Label = ControlFactory.CreateLabel(contentManager, fontPath, fontSize, fontColor, text);

			return tooltip;
		}
	}
}
