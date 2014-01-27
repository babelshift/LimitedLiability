using System;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using System.Collections.Generic;

namespace MyThirdSDL.UserInterface
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

