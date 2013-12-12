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
		private Icon iconMailUnread;
		private Icon iconMailRead;

		private Label labelFrom;
		private Label labelSubject;

		public MailItem MailItem { get; private set; }

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

				if (iconMailUnread != null)
					iconMailUnread.Position = new Vector(iconMailUnread.Position.X + changeX, iconMailUnread.Position.Y + changeY);

				if (iconMailRead != null)
					iconMailRead.Position = new Vector(iconMailRead.Position.X + changeX, iconMailRead.Position.Y + changeY);

				if (labelFrom != null)
					labelFrom.Position = new Vector(labelFrom.Position.X + changeX, labelFrom.Position.Y + changeY);

				if (labelSubject != null)
					labelSubject.Position = new Vector(labelSubject.Position.X + changeX, labelSubject.Position.Y + changeY);

				base.Position = value;
			}
		}

		public ButtonMailItem(Texture texture, Texture textureHover, Vector position, Icon iconMailUnread, Icon iconMailRead, Label labelFrom, Label labelSubject, MailItem mailItem)
			: base(texture, textureHover, position)
		{
			this.iconMailRead = iconMailRead;
			this.iconMailUnread = iconMailUnread;
			this.labelFrom = labelFrom;
			this.labelSubject = labelSubject;
			this.MailItem = mailItem;
		}

		public override void Draw(GameTime gameTime, Renderer renderer)
		{
			base.Draw(gameTime, renderer);

			if (MailItem.MailState == MailState.Unread)
				iconMailUnread.Draw(gameTime, renderer);
			else if (MailItem.MailState == MailState.Read)
				iconMailRead.Draw(gameTime, renderer);

			labelFrom.Draw(gameTime, renderer);
			labelSubject.Draw(gameTime, renderer);
		}
	}
}
