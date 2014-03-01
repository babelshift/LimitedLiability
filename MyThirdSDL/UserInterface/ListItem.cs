using SharpDL;
using SharpDL.Events;
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
	public class ListItem<T> : Control
	{
		private Texture textureHighlight;

		private List<ListItemColumn> columns = new List<ListItemColumn>();

		/// <summary>
		/// Collection of all columns (containing controls) that are in this ListItem.
		/// </summary>
		public IReadOnlyList<ListItemColumn> Columns { get { return columns; } }

		/// <summary>
		/// The underlying value of this ListItem (such as a GUID, a Purchasable Item, or another asset that has been bound to this ListItem)
		/// </summary>
		public T Value { get; private set; }

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
		
		/// <summary>
		/// Fires when the ListItem is hovered by the Mouse, EventArgs will contain the Value of this ListItem
		/// </summary>
		public new event EventHandler<ListItemHoveredEventArgs<T>> Hovered;

		/// <summary>
		/// Fires when the ListItem is clicked by the Mouse, EventArgs will contain the Value of this ListItem
		/// </summary>
		public new event EventHandler<ListItemSelectedEventArgs<T>> Clicked;

		/// <summary>
		/// Default constructor will establish the value of the ListItem and wire up necessary events.
		/// </summary>
		/// <param name="value"></param>
		public ListItem(T value, Texture textureHighlight)
		{
			Value = value;

			base.Clicked += ListItem_Clicked;
			base.Hovered += ListItem_Hovered;

			this.textureHighlight = textureHighlight;
		}

		/// <summary>
		/// Updates this ListItem and all columns contained within.
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			if (!Visible)
				return;
			base.Update(gameTime);

			foreach (var column in columns)
				column.Update(gameTime);
		}

		/// <summary>
		/// Draws all columns contained within.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="renderer"></param>
		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			if (!Visible)
				return;

			// if we are hovered, draw our highlight to fill our contents horizontally
			if (IsHovered)
				for(int i = 0; i < Width; i++)
					textureHighlight.Draw(Position.X + i, Position.Y);

			foreach (var column in columns)
				column.Draw(gameTime, renderer);
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
				columns[columnIndex].SetWidth(width);
		}

		/// <summary>
		/// Adds a new column (containing a control) to the ListItem collection.
		/// </summary>
		/// <param name="control"></param>
		public void AddColumn(Control control)
		{
			var column = new ListItemColumn(control);
			columns.Add(column);
		}

		/// <summary>
		/// Excplicitly sets the width of this Control. This is needed because only our parent ListBox knows how wide we need to be.
		/// </summary>
		/// <param name="width"></param>
		public void SetWidth(int width)
		{
			Width = width;
		}

		/// <summary>
		/// Explicitly sets the height of this Control. This is needed because only our parent ListBox knows how tall we need to be.
		/// </summary>
		/// <param name="height"></param>
		public void SetHeight(int height)
		{
			Height = height;
		}

		/// <summary>
		/// Passes the Value of the ListItem up to any subscribers when hovered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListItem_Hovered(object sender, EventArgs e)
		{
			if (Hovered != null)
				Hovered(sender, new ListItemHoveredEventArgs<T>(Value));
		}

		/// <summary>
		/// Passes the Value of the ListItem up to any subscribers when clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListItem_Clicked(object sender, EventArgs e)
		{
			if (Clicked != null)
				Clicked(sender, new ListItemSelectedEventArgs<T>(Value));
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
			textureHighlight.Dispose();
		}
	}
}
