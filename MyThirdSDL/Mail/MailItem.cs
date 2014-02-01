using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Mail
{
	public class MailItem
	{
		public Guid ID { get; private set; }

		public string From { get; private set; }

		public string To { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public MailState MailState { get; private set; }

		public AttachmentType AttachmentType { get; private set; }

		public MailItem(string from, string to, string subject, string body, MailState mailState)
		{
			ID = Guid.NewGuid();
			From = from;
			To = to;
			Subject = subject;
			Body = body;
			MailState = mailState;
		}

		public void ChangeState(MailState mailState)
		{
			if(MailState != mailState)
				MailState = mailState;
		}
	}
}
