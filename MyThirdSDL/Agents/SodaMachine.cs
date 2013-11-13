﻿using System;
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

		public NecessityEffects NecessityEffects { get; private set; }
		public string IconTextureKey { get { return "IconSoda"; } }

		//public Trigger Trigger { get; private set; }

		public NecessityEffects DispenseDrink()
		{
			return NecessityEffects;
		}

		public SodaMachine(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityEffects = new NecessityEffects(-1, 0, 1, 10, 0);

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
