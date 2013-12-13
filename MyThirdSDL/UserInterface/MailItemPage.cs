using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.UserInterface
{
	public class MailItemPage
	{
		private List<ButtonMailItem> buttons = new List<ButtonMailItem>();
		private List<Icon> separators = new List<Icon>();

		public IEnumerable<ButtonMailItem> Buttons { get { return buttons; } }
		public IEnumerable<Icon> Separators { get { return separators; } }

		public void AddButton(ButtonMailItem button)
		{
			buttons.Add(button);
		}

		public void AddSeparator(Icon separator)
		{
			separators.Add(separator);
		}

		public void Clear()
		{
			foreach (var button in buttons)
				button.Dispose();
			foreach (var separator in separators)
				separator.Dispose();

			buttons.Clear();
			separators.Clear();
		}
	}
}
