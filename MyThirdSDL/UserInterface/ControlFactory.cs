﻿using MyThirdSDL.Content;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	public static class ControlFactory
	{
		//private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		//private contentManager contentManager;

		//public ControlFactory(contentManager contentManager)
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

		public static MessageBox CreateMessageBox(ContentManager content, MessageBoxType type)
		{
			if (content == null) throw new ArgumentNullException("content");
			MessageBox messageBox = new MessageBox(content, type);
			return messageBox;
		}

		public static Button CreateButton(ContentManager content, string textureFrameKey, string textureFrameHoveredKey = "", string textureFrameSelectedKey = "")
		{
			if (content == null) throw new ArgumentNullException("content");

			if (String.IsNullOrEmpty(textureFrameKey))
				throw new ArgumentNullException("textureFrameKey");

			Button button = new Button();
			button.TextureFrame = content.GetTexture(textureFrameKey);

			if(!String.IsNullOrEmpty(textureFrameHoveredKey))
				button.TextureFrameHovered = content.GetTexture(textureFrameHoveredKey);

			if(!String.IsNullOrEmpty(textureFrameSelectedKey))
				button.TextureFrameSelected = content.GetTexture(textureFrameSelectedKey);

			return button;
		}

		public static Icon CreateIcon(ContentManager content, string iconKey)
		{
			if (content == null) throw new ArgumentNullException("content");

			if (String.IsNullOrEmpty(iconKey))
				throw new ArgumentNullException("iconKey");

			Icon icon = new Icon(content.GetTexture(iconKey));
			return icon;
		}

		public static Label CreateLabel(ContentManager content, string fontPath, int fontSize, Color fontColor, string text, int wrapLength = 0)
		{
			if (content == null) throw new ArgumentNullException("content");

			if (String.IsNullOrEmpty(fontPath))
				throw new ArgumentNullException("fontPath");

			if (String.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");

			if (fontSize <= 0)
				throw new ArgumentOutOfRangeException("fontSize");

			Label label = new Label();
			label.TrueTypeText = content.GetTrueTypeText(fontPath, fontSize, fontColor, text, wrapLength);

			return label;
		}

		public static Tooltip CreateTooltip(ContentManager content, string textureFrameKey, string fontPath, int fontSize, Color fontColor, string text)
		{
			if (content == null) throw new ArgumentNullException("content");

			if (String.IsNullOrEmpty(textureFrameKey))
				throw new ArgumentNullException("textureFrameKey");

			if (String.IsNullOrEmpty(fontPath))
				throw new ArgumentNullException("fontPath");

			if (String.IsNullOrEmpty(text))
				throw new ArgumentNullException("text");

			if (fontSize <= 0)
				throw new ArgumentOutOfRangeException("fontSize");

			Tooltip tooltip = new Tooltip(content.GetTexture(textureFrameKey));
			tooltip.Label = CreateLabel(content, fontPath, fontSize, fontColor, text);

			return tooltip;
		}
	}
}