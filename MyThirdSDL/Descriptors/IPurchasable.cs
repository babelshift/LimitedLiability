using MyThirdSDL.Content;
using SharpDL;
using SharpDL.Graphics;
using System.Collections.Generic;

namespace MyThirdSDL.Descriptors
{
	public interface IPurchasable : IAffectsNecessities, ISkillsAffector
	{
		void Draw(GameTime gameTime, Renderer renderer, int x, int y, bool isOverlappingDeadZone);

		string Name { get; }

		int Price { get; }

		string IconTextureKey { get; }

		int HorizontalMapCellCount { get; }

		int VerticalMapCellCount { get; }

		bool IsOverlappingDeadZone(IReadOnlyList<MapCell> mapCells);
	}
}