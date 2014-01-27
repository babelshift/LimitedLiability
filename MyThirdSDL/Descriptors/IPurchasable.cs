using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SharpDL;
using SharpDL.Graphics;
using MyThirdSDL.Content;

namespace MyThirdSDL.Descriptors
{
	public interface IPurchasable : IAffectsNecessities, ISkillsAffector
	{
		void Draw(GameTime gameTime, Renderer renderer, int x, int y);

		string Name { get; }

		int Price { get; }

		string IconTextureKey { get; }

		int HorizontalMapCellCount { get; }

		int VerticalMapCellCount { get; }

		void CheckOverlap(IReadOnlyList<MapCell> mapCells);
	}
}