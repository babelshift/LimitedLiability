using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MyThirdSDL.Descriptors;
using SharpDL.Events;
using SharpDL.Input;
using MyThirdSDL.Content;
using MyThirdSDL.Simulation;

namespace MyThirdSDL.UserInterface
{
	public enum MouseMode
	{
		SelectGeneral,
		SelectEquipment,
		SelectRoom
	}
}
