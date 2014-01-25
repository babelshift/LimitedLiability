using System;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.UserInterface
{
	public class PurchasableItemSelectedEventArgs : EventArgs
	{
		public IPurchasable PurchasableItem { get; private set; }

		public PurchasableItemSelectedEventArgs(IPurchasable purchasableItem)
		{
			PurchasableItem = purchasableItem;
		}
	}
}

