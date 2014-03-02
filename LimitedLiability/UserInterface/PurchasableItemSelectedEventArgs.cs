using System;
using LimitedLiability.Descriptors;

namespace LimitedLiability.UserInterface
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

