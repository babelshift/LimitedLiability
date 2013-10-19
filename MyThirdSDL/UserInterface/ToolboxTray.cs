using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ToolboxTray : Control
	{
		public ToolboxTray(Texture texture, Vector position)
			: base(texture, position) { }

		private Label LabelMoney { get; set; }
		private Label LabelSimulationDateTime { get; set; }
		private Button ButtonGeneralSelect { get; set; }
		private Button ButtonPlaceEquipment { get; set; }
		private Button ButtonPlaceRoom { get; set; }
		private Button ButtonCompanyData { get; set; }
	}
}
