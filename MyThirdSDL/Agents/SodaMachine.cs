using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.Agents
{
	public class SodaMachine : Equipment, IPurchasable//, ITriggerable
	{
		private const int price = 50;
		private const string name = "Soda Machine";

		public NecessityAffector NecessityAffector { get; private set; }
		public string IconTextureKey { get { return "IconSoda"; } }

		//public Trigger Trigger { get; private set; }

		public NecessityAffector DispenseDrink()
		{
			return NecessityAffector;
		}

		public SodaMachine(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityAffector = new NecessityAffector(-1, 0, 1, 10, 0);

			//Trigger = new Trigger();
			//Action dispenseDrink = new Action(ActionType.DispenseDrink);
			//Trigger.AddAction(dispenseDrink);
		}

//		public void ExecuteTrigger()
//		{
//			Trigger.Execute(NecessityAffector);
//		}
	}
}
