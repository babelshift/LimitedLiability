using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using System;

namespace MyThirdSDL.UserInterface
{
	/// <summary>
	/// Represents a single column to be added to a ListItem. Consists of a rendered Control and a Width value.
	/// </summary>
	public class ListItemColumn : Control
	{
		private Control control;

		public override Vector Position
		{
			get { return control.Position; }
			set 
			{
				base.Position = value;
				control.Position = base.Position;
			}
		}

		public ListItemColumn(Control control)
		{
			this.control = control;
			Height = control.Height;
			Width = control.Width;
		}

		public override void Update(GameTime gameTime)
		{
			if (!Visible)
				return;
			base.Update(gameTime);

			control.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible)
				return;

			control.Draw(gameTime, renderer);
		}

		/// <summary>
		/// Used by the ListBox control to render the proper width between columns for proper spacing. Default is the Control width.
		/// </summary>
		/// <param name="width"></param>
		public void SetWidth(int width)
		{
			Width = width;
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			control.Dispose();
		}
	}
}