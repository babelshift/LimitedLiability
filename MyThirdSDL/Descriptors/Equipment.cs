﻿using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public abstract class Equipment : Agent
	{
		public int Price { get; private set; }

		public Equipment(string agentName, Texture texture, Vector startingPosition, int price)
			: base(agentName, texture, startingPosition, Vector.Zero)
		{
			Price = price;
		}
	}
}
