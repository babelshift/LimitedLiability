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
	/// Represents a single ListItem to be added to a ListBox. Each ListItem is a single row of a ListBox containing many Columns of Controls.
	/// </summary>
	public class ListItem : Control
	{
		private List<ListItemColumn> columns = new List<ListItemColumn>();

		public IReadOnlyList<ListItemColumn> Columns { get { return columns; } }

		/// <summary>
		/// Adjusts the position of all Columns contained within this ListItem.
		/// </summary>
		public override Vector Position
		{
			get { return base.Position; }
			set
			{
				base.Position = value;

				SetColumnPositions();
			}
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible)
				return;
			foreach (var column in columns)
				column.Control.Draw(gameTime, renderer);
		}

		/// <summary>
		/// Adjusts the positions of all columns in this list item.
		/// </summary>
		private void SetColumnPositions()
		{
			for (int i = 0; i < columns.Count; i++)
				SetColumnPosition(i);
		}

		/// <summary>
		/// Adjusts the column position of a single column at index i. The position of a column is 0 when it is the first column or
		/// separated by its left neighbor in an amount equal to the left neighbors width +25 pixels.
		/// </summary>
		/// <param name="i"></param>
		private void SetColumnPosition(int i)
		{
			int widthOfLeftNeighbor = 0;
			Vector positionOfLeftNeighbor = base.Position;
			if (i > 0)
			{
				widthOfLeftNeighbor = columns[i - 1].Width + 25;
				positionOfLeftNeighbor = columns[i - 1].Position;
			}
			columns[i].Position = positionOfLeftNeighbor + new Vector(widthOfLeftNeighbor, 0);
		}

		/// <summary>
		/// Sets the column width at the passed columnIndex to the passed width value. Does nothing if the column doesn't exist.
		/// </summary>
		/// <param name="columnIndex"></param>
		/// <param name="width"></param>
		public void SetColumnWidth(int columnIndex, int width)
		{
			if (columnIndex < columns.Count)
				columns[columnIndex].Width = width;
		}

		public void AddColumn(Control control)
		{
			columns.Add(new ListItemColumn(control));
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach (var column in columns)
				column.Dispose();
		}
	}
}
