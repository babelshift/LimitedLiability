﻿using SharpDL.Graphics;

namespace MyThirdSDL.Content
{
	public class TilePosition
	{
		public Vector WorldPosition { get; private set; }

		public Vector ProjectedPosition { get; private set; }

		public TilePosition(Vector worldPosition, Vector projectedPosition)
		{
			WorldPosition = worldPosition;
			ProjectedPosition = projectedPosition;
		}
	}
}