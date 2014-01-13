using System;
using SharpDL.Graphics;
using MyThirdSDL.Content;
using MyThirdSDL.UserInterface;
using SharpDL.Input;

namespace MyThirdSDL
{
	public class Cursor : IDisposable
	{
		private Texture cursorHandRight;
		private Texture cursorHandLeft;
		private Texture cursorHandDown;
		private Texture cursorHandUp;
		private Texture cursorActive;

		public Cursor(ContentManager contentManager, Renderer renderer)
		{
			string texturePath = contentManager.GetContentPath("CursorHandRight");
			Surface surface = new Surface(texturePath, SurfaceType.PNG);
			cursorHandRight = new Texture(renderer, surface);

			texturePath = contentManager.GetContentPath("CursorHandLeft");
			Surface surface2 = new Surface(texturePath, SurfaceType.PNG);
			cursorHandLeft = new Texture(renderer, surface2);

			texturePath = contentManager.GetContentPath("CursorHandUp");
			Surface surface3 = new Surface(texturePath, SurfaceType.PNG);
			cursorHandUp = new Texture(renderer, surface3);

			texturePath = contentManager.GetContentPath("CursorHandDown");
			Surface surface4 = new Surface(texturePath, SurfaceType.PNG);
			cursorHandDown = new Texture(renderer, surface4);
		}

		public void Update(bool isMouseInsideWindowBounds, MouseOverScreenEdge mouseOverScreenEdge)
		{
			if (isMouseInsideWindowBounds)
				SetMouseOverScreenEdge(mouseOverScreenEdge);
		}

		public void Draw(Renderer renderer)
		{
			if (cursorActive != null)
				renderer.RenderTexture(cursorActive, MouseHelper.CurrentPosition.X - cursorHandRight.Width / 2, MouseHelper.CurrentPosition.Y - cursorHandRight.Height / 2);
		}

		private void SetMouseOverScreenEdge(MouseOverScreenEdge mouseOverScreenEdge)
		{

			if (mouseOverScreenEdge == MouseOverScreenEdge.Left)
				cursorActive = cursorHandLeft;
			else if (mouseOverScreenEdge == MouseOverScreenEdge.Right)
				cursorActive = cursorHandRight;
			else if (mouseOverScreenEdge == MouseOverScreenEdge.Bottom)
				cursorActive = cursorHandDown;
			else if (mouseOverScreenEdge == MouseOverScreenEdge.Top)
				cursorActive = cursorHandUp;
			else
				cursorActive = null;

			if (mouseOverScreenEdge == MouseOverScreenEdge.None)
				Mouse.ShowCursor();
			else
				Mouse.HideCursor();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			cursorActive.Dispose();
			cursorHandDown.Dispose();
			cursorHandLeft.Dispose();
			cursorHandRight.Dispose();
			cursorHandUp.Dispose();
		}
	}
}

