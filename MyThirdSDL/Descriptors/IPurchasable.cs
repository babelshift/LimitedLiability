using SharpDL;
using SharpDL.Graphics;

namespace MyThirdSDL.Descriptors
{
	public interface IPurchasable : IAffectsNecessities, ISkillsAffector
	{
		void Draw(GameTime gameTime, Renderer renderer, int x, int y);

		string Name { get; }

		int Price { get; }

		string IconTextureKey { get; }
	}
}