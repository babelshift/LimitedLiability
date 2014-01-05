using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
{
	public static class CoordinateHelper
	{
		// TODO: set these values on init based on .tmx file?
		public static readonly int TileMapTileWidth = 32;
		public static readonly int TileMapTileHeight = 32;
		public static readonly int WorldGridCellWidth = 32;
		public static readonly int WorldGridCellHeight = 32;
		public static readonly int PathNodeGridCellWidth = 32;
		public static readonly int PathNodeGridCellHeight = 32;
		public static Point DefaultPoint = new Point(-9999, -9999);
		public static Vector DefaultVector = new Vector(-9999, -9999);

		public enum ScreenProjectionType
		{
			Isometric,
			Orthogonal
		}

		/// <summary>
		/// The offset used to reposition the screen space at rendering (useful when projecting from orthogonal to isometric where the position will be off centered)
		/// </summary>
		public static Vector ScreenOffset = Vector.Zero;

		/// <summary>
		/// Converts the passed angle in degrees to angle in radians
		/// </summary>
		/// <param name="degrees"></param>
		/// <returns></returns>
		private static double DegreesToRadians(double degrees)
		{
			return (Math.PI / 180) * degrees;
		}

		/// <summary>
		/// Using the screen space coordinates and offset, converts the values to the world space equivalent based on the projection type
		/// </summary>
		/// <param name="screenX"></param>
		/// <param name="screenY"></param>
		/// <param name="offset"></param>
		/// <param name="projectionType"></param>
		/// <returns></returns>
		public static Vector ScreenSpaceToWorldSpace(float screenX, float screenY, Vector offset, ScreenProjectionType projectionType)
		{
			// undo the offset when going from screen to world space
			screenX -= offset.X;
			screenY -= offset.Y;

			// undo the camera transformation when going from screen to world space
			screenX += Camera.Position.X;
			screenY += Camera.Position.Y;

			if (projectionType == ScreenProjectionType.Isometric)
			{
				float worldX = (2 * screenY + screenX) / 2;
				float worldY = (2 * screenY - screenX) / 2;
				Vector worldSpace = new Vector(worldX, worldY);
				return worldSpace;
			}
			else if (projectionType == ScreenProjectionType.Orthogonal)
				return new Vector(screenX, screenY);
			else
				throw new NotImplementedException("Projections other than Isometric are not supported");
		}

		/// <summary>
		/// Using the passed world space coordinates, object width/height, and offset, converts the values to the projected screen space equivalent based on the projection type
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="offset"></param>
		/// <param name="projectionType"></param>
		/// <returns></returns>
		public static Vector WorldSpaceToScreenSpace(float worldX, float worldY, Vector offset, ScreenProjectionType projectionType)
		{
			if (projectionType == ScreenProjectionType.Isometric)
			{
				float screenSpaceX = worldX - worldY;
				float screenSpaceY = (worldX + worldY) / 2;

				Vector screenSpace = new Vector(screenSpaceX, screenSpaceY);
				Vector offsetScreenSpace = screenSpace + offset;
				return offsetScreenSpace;
			}
			else
				return new Vector(worldX, worldY);
		}

		public static Vector ProjectedPositionToDrawPosition(Vector projectedPosition)
		{
			float drawPositionX = projectedPosition.X - Camera.Position.X;
			float drawPositionY = projectedPosition.Y - Camera.Position.Y - CoordinateHelper.TileMapTileHeight;

			return new Vector(drawPositionX, drawPositionY);
		}
	}
}
