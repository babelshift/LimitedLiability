using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFileExtension="log4net")]

namespace MyThirdSDL
{
	class Program
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		[STAThread]
		static void Main(string[] args)
		{
			if (log.IsDebugEnabled)
				log.Debug("Program started, entered main.");
			MainGame mainGame = new MainGame();
			mainGame.Run();
		}
	}
}
