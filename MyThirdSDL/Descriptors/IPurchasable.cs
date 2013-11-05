﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public interface IPurchasable : INecessityAffector
	{
		string Name { get; }
		int Price { get; }
		string IconTextureKey { get; }
	}
}