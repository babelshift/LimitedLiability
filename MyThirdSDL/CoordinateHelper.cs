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
		/// Using the world space coordinates and tile width/height, converts the values to the world space grid indices
		/// </summary>
		/// <param name="worldX"></param>
		/// <param name="worldY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Vector WorldSpaceToWorldGridIndex(float worldX, float worldY, int width, int height)
		{
			float worldGridX = (worldX) / width;
			float worldGridY = (worldY)/ height;

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
			Vector worldGridIndex = CoordinateHelper.WorldSpaceToWorldGridIndex(worldX, worldY, width, height);
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

		public static bool AreIndicesEqual(Vector index1, Vector index2)
		{
			if (Math.Round(index1.X) == Math.Round(index2.X) && Math.Round(index1.Y) == Math.Round(index2.Y))
				return true;
			else
				return false;
		}
	}
}
