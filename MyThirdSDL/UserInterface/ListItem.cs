using SharpDL;
using SharpDL.Graphics;
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

		public override SharpDL.Graphics.Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				for (int i = 0; i < columns.Count; i++)
					columns[i].Position = base.Position + new Vector(i * 50, 0);
			}
		}

		public ListItem()
		{
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			foreach (var control in columns)
				control.Draw(gameTime, renderer);
		}

		public void AddColumn(Control column)
		{
			column.Position = new Vector(columns.Count * 50, 0);
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
