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

namespace MyThirdSDL.UserInterface
{
	public class ControlFactory
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private ContentManager contentManager;

		public ControlFactory(ContentManager contentManager)
		{
			this.contentManager = contentManager;
		}

		public SimulationLabel CreateSimulationLabel(Vector position, string fontPath, int fontSize, Color color, SimulationMessage simulationMessage)
		{
			if (log.IsDebugEnabled)
				log.Debug(String.Format("Creating simulation label at ({0},{1}) with type: {2} and text: {3}", position.X, position.Y, simulationMessage.Type, simulationMessage.Text));
			TrueTypeText trueTypeText = contentManager.GetTrueTypeText(fontPath, fontSize, color, simulationMessage.Text);
			return new SimulationLabel(position, trueTypeText, simulationMessage);
		}
	}
}
