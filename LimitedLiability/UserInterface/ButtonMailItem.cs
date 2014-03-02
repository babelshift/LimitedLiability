using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimitedLiability.Mail;

namespace LimitedLiability.UserInterface
{
	public class ButtonMailItem : Button
	{
		public Icon IconMailUnread { get; set; }
		public Icon IconMailRead { get; set; }

		public Label LabelFrom { get; set; }
		public Label LabelSubject { get; set; }

		public MailItem MailItem { get; private set; }

		public bool IsSelected { get; set; }

		public override Vector Position
		{
			get
			{
				return base.Position;
			}
			set
			{
				base.Position = value;

				if (IconMailUnread != null)
					IconMailUnread.Position = new Vector(base.Position.X + 5, base.Position.Y);

				if (IconMailRead != null)
					IconMailRead.Position = new Vector(base.Position.X + 5, base.Position.Y);

				if (LabelFrom != null)
					LabelFrom.Position = new Vector(base.Position.X + 50, base.Position.Y + 10);

				if (LabelSubject != null)
					LabelSubject.Position = new Vector(base.Position.X + 181, base.Position.Y + 10);
			}
		}

		public ButtonMailItem(MailItem mailItem)
		{
			MailItem = mailItem;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (MailItem.MailState == MailState.Unread)
				if (IconMailUnread != null)
					IconMailUnread.Update(gameTime);
				else if (MailItem.MailState == MailState.Read)
					if (IconMailRead != null)
						IconMailRead.Update(gameTime);

			if (LabelFrom != null)
				LabelFrom.Update(gameTime);

			if (LabelSubject != null)
				LabelSubject.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			if (MailItem.MailState == MailState.Unread)
				if(IconMailUnread != null)
					IconMailUnread.Draw(gameTime, renderer);
			else if (MailItem.MailState == MailState.Read)
				if(IconMailRead != null)
					IconMailRead.Draw(gameTime, renderer);

			if(LabelFrom != null)
				LabelFrom.Draw(gameTime, renderer);

			if(LabelSubject != null)
				LabelSubject.Draw(gameTime, renderer);
		}

		public override void Dispose()
		{
 			base.Dispose();
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (IconMailUnread != null)
				IconMailUnread.Dispose();
			if (IconMailRead != null)
				IconMailRead.Dispose();
			if (LabelFrom != null)
				LabelFrom.Dispose();
			if (LabelSubject != null)
				LabelSubject.Dispose();
		}
	}
}
