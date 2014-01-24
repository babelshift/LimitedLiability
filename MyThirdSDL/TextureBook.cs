using MyThirdSDL.Agents;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL
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
			if (orientation == AgentOrientation.FacingLeft)
			{
				if (textureTopLeft != null)
					ActiveTexture = textureTopLeft;
			}
			else if (orientation == AgentOrientation.FacingDown)
			{
				if (textureBottomLeft != null)
					ActiveTexture = textureBottomLeft;
			}
			else if (orientation == AgentOrientation.FacingUp)
			{
				if (textureBottomRight != null)
					ActiveTexture = textureBottomRight;
			}
			else if (orientation == AgentOrientation.FacingRight)
			{
				if (textureTopRight != null)
					ActiveTexture = textureTopRight;
			}
		}
	}
}
