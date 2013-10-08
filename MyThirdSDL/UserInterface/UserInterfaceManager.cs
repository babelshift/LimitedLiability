using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public enum MouseModeType
	{
		GeneralSelect,
		PlaceAgent,
		PlaceRoom
	}

	public class UserInterfaceManager
	{
		public MouseModeType MouseMode { get; private set; }
		public ToolboxTray ToolboxTray { get; private set; }

		public UserInterfaceManager()
		{
			MouseMode = MouseModeType.GeneralSelect;
		}
	}
}
