﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDL.Graphics;

namespace MyThirdSDL.Descriptors
{
	public interface IPurchasable : IAffectsNecessities, ISkillsAffector
	{
		Texture ActiveTexture { get; }
		string Name { get; }
		int Price { get; }
		string IconTextureKey { get; }
	}
}
