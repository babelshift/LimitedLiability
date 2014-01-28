using MyThirdSDL.Agents;
using SharpDL.Graphics;

namespace MyThirdSDL.Content
{
	public class TextureBook
	{
		private Texture textureTopLeft;
		private Texture textureBottomLeft;
		private Texture textureTopRight;
		private Texture textureBottomRight;

		public Texture ActiveTexture { get; private set; }

		public TextureBook(Texture textureTopLeft, Texture textureBottomLeft, Texture textureTopRight, Texture textureBottomRight)
		{
			this.textureTopLeft = textureTopLeft;
			this.textureBottomLeft = textureBottomLeft;
			this.textureTopRight = textureTopRight;
			this.textureBottomRight = textureBottomRight;

			SetOrientation(AgentOrientation.FacingLeft);
		}

		public void SetOrientation(AgentOrientation orientation)
		{
			switch (orientation)
			{
				case AgentOrientation.FacingLeft:
					if (textureTopLeft != null)
						ActiveTexture = textureTopLeft;
					break;
				case AgentOrientation.FacingDown:
					if (textureBottomLeft != null)
						ActiveTexture = textureBottomLeft;
					break;
				case AgentOrientation.FacingUp:
					if (textureBottomRight != null)
						ActiveTexture = textureBottomRight;
					break;
				case AgentOrientation.FacingRight:
					if (textureTopRight != null)
						ActiveTexture = textureTopRight;
					break;
			}
		}
	}
}