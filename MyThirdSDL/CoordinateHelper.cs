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
		public static int WorldGridCellWidth = 32;
		public static int WorldGridCellHeight = 32;
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
		public static Vector ScreenOffset = new Vector(700, WorldGridCellHeight * 2);

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
		/// Returns the world grid index in whole units (ints). Use this method when you need only the exact grid index value locked to the grid. For example, grid index
		/// at [6.5, 7.5] would return in this method as [7, 8] through rounding of the index values. Useful when you need to lock positions to the grid.
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Point WorldSpaceToWorldGridIndexPoint(float worldX, float worldY, int width, int height)
		{
			float worldGridX = worldX / width;
			float worldGridY = worldY / height;

			return new Point((int)Math.Round(worldGridX), (int)Math.Round(worldGridY));
		}

		/// <summary>
		/// Returns the world grid index (partial indices are possible since this is a vector). Use this method when you need exactly within a grid index. For example,
		/// grid index [6.5, 7.5] indicates half indices in between the grids. This is useful when needing to render between indices.
		/// </summary>
		/// <returns>The space to world grid index vector.</returns>
		/// <param name="worldX">World x.</param>
		/// <param name="worldY">World y.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public static Vector WorldSpaceToWorldGridIndexVector(float worldX, float worldY, int width, int height)
		{
			float worldGridX = worldX / width;
			float worldGridY = worldY / height;

			return new Vector(worldGridX, worldGridY);
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
		public static Vector WorldSpaceToScreenSpace(float worldX, float worldY, int width, int height, Vector offset, ScreenProjectionType projectionType)
		{
			Vector worldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndexVector(worldX, worldY, width, height);
			return CoordinateHelper.WorldGridIndexToScreenSpace(worldGridIndex.X, worldGridIndex.Y, width, height, offset, projectionType);
		}

		/// <summary>
		/// Using the passed world grid index, tile width/height, and offset, converts the values to the projected screen space equivalent based on the projection type
		/// </summary>
		/// <param name="worldGridX"></param>
		/// <param name="worldGridY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="offset"></param>
		/// <param name="projectionType"></param>
		/// <returns></returns>
		public static Vector WorldGridIndexToScreenSpace(float worldGridX, float worldGridY, int width, int height, Vector offset, ScreenProjectionType projectionType)
		{
			if (projectionType == ScreenProjectionType.Isometric)
			{
				float rawPositionX = worldGridX * width;
				float rawPositionY = worldGridY * height;

				float screenSpaceX = rawPositionX - rawPositionY;
				float screenSpaceY = (rawPositionX + rawPositionY) / 2;

				Vector screenSpace = new Vector(screenSpaceX, screenSpaceY);
				Vector offsetScreenSpace = screenSpace + offset;
				return offsetScreenSpace;
			}
			else
				throw new NotImplementedException("Projections other than Isometric are not supported");
		}
	}
}
