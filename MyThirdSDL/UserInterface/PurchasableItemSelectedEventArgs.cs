using System;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL
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

