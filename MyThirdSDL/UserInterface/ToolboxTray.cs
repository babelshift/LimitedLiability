using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ToolboxTray : Control
	{
		private Label LabelMoney { get; private set; }
		private Label LabelSimulationDateTime { get; private set; }
		private Button ButtonGeneralSelect { get; private set; }
		private Button ButtonPlaceEquipment { get; private set; }
		private Button ButtonPlaceRoom { get; private set; }
		private Button ButtonCompanyData { get; private set; }
	}
}
