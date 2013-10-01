using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			MainGame mainGame = new MainGame();
			mainGame.Run();
		}
	}
}
