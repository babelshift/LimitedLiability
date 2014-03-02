using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.UserInterface
{
	public class ListItemHoveredEventArgs<T> : EventArgs
	{
		public T Value { get; private set; }

		public ListItemHoveredEventArgs(T value)
		{
			Value = value;
		}
	}
}
