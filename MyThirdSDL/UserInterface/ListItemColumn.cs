using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	/// <summary>
	/// Represents a single column to be added to a ListItem. Consists of a rendered Control and a Width value.
	/// </summary>
	public class ListItemColumn : Control
	{
		public Control Control { get; private set; }

		/// <summary>
		/// Used by the ListBox control to render the proper width between columns for proper spacing. Default is the Control width.
		/// </summary>
		public int Width { get; set; }

		public Vector Position
		{
			get { return Control.Position; }
			set { Control.Position = value; }
		}

		public ListItemColumn(Control control)
		{
			Control = control;
			Width = control.Width;
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible)
				return;
			Control.Draw(gameTime, renderer);
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			Control.Dispose();
		}
	}
}
