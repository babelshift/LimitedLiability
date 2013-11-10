using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class SnackMachine : Equipment, IPurchasable, ITriggerable
	{
		private const int price = 50;
		private const string name = "Snack Machine";

		public NecessityAffector NecessityAffector { get; private set; }
		public string IconTextureKey { get { return "IconPizza"; } }

		public Trigger Trigger { get; private set; }

		public SnackMachine(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityAffector = new NecessityAffector(-1, 0, 0, 0, 2);

			Trigger = new Trigger();
			Action dispenseFood = new Action(ActionType.DispenseFood);
			Trigger.AddAction(dispenseFood);
		}

		public void ExecuteTrigger()
		{
			Trigger.Execute(NecessityAffector);
		}
	}
}
