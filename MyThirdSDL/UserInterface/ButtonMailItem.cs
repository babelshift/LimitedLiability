using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyThirdSDL.Mail;

namespace MyThirdSDL.UserInterface
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
				// calculate the change in position for the parent and move the children by that amount
				float changeX = value.X - base.Position.X;
				float changeY = value.Y - base.Position.Y;

				base.Position = value;

				if (IconMailUnread != null)
					IconMailUnread.Position = new Vector(IconMailUnread.Position.X + changeX, IconMailUnread.Position.Y + changeY);

				if (IconMailRead != null)
					IconMailRead.Position = new Vector(IconMailRead.Position.X + changeX, IconMailRead.Position.Y + changeY);

				if (LabelFrom != null)
					LabelFrom.Position = new Vector(LabelFrom.Position.X + changeX, LabelFrom.Position.Y + changeY);

				if (LabelSubject != null)
					LabelSubject.Position = new Vector(LabelSubject.Position.X + changeX, LabelSubject.Position.Y + changeY);
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

		~ButtonMailItem()
		{
			Dispose(false);
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
