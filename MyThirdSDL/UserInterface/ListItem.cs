using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class ListItem : Control
	{
		private List<Control> columns = new List<Control>();

		public IReadOnlyList<Control> Columns { get { return columns; } }

		public ListItem()
		{
		}

		public override void Draw(SharpDL.GameTime gameTime, SharpDL.Graphics.Renderer renderer)
		{
			foreach (var control in columns)
				control.Draw(gameTime, renderer);
		}

		public void AddColumn(Control column)
		{
			columns.Add(column);
		}

		public void RemoveColumn(Control column)
		{
			columns.Remove(column);
		}

		public override void Dispose()
		{
			foreach (var control in columns)
				control.Dispose();
		}
	}
}
