using System;
using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;

namespace MyThirdSDL.UserInterface
{
	public class PurchasableItemPlacedEventArgs : EventArgs
	{
		public IPurchasable PurchasableItem { get; private set; }
		public MapCell HoveredMapCell { get; private set; }

		public PurchasableItemPlacedEventArgs(IPurchasable purchasableItem, MapCell hoveredMapCell)
		{
			PurchasableItem = purchasableItem;
			HoveredMapCell = hoveredMapCell;
		}
	}
}

