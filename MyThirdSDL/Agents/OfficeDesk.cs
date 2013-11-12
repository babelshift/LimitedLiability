using System;
using MyThirdSDL.Descriptors;
using SharpDL.Graphics;

namespace MyThirdSDL.Agents
{
	public class OfficeDesk : Equipment, IPurchasable
	{
		private const int price = 50;
		private const string name = "Office Desk";

		public NecessityAffector NecessityAffector { get; private set; }
		public string IconTextureKey { get { return "IconOfficeDesk"; } }

		public OfficeDesk(TimeSpan birthTime, Texture texture, Vector startingPosition)
			: base(birthTime, name, texture, startingPosition, price)
		{
			NecessityAffector = new NecessityAffector(0, 0, -1, 0, 0);
        }
    }
}

