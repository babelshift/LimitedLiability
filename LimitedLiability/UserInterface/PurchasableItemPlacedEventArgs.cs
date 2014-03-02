using System;
using LimitedLiability.Content;
using LimitedLiability.Descriptors;
using System.Collections.Generic;

namespace LimitedLiability.UserInterface
{
	public class PurchasableItemPlacedEventArgs : EventArgs
	{
		public IPurchasable PurchasableItem { get; private set; }
		public IReadOnlyList<MapCell> HoveredMapCells { get; private set; }

		public PurchasableItemPlacedEventArgs(IPurchasable purchasableItem, IReadOnlyList<MapCell> hoveredMapCells)
		{
			PurchasableItem = purchasableItem;
			HoveredMapCells = hoveredMapCells;
		}
	}
}

