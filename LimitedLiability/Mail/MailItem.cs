using System;

namespace LimitedLiability.Mail
{
	public class MailItem
	{
		public Guid ID { get; private set; }

		public string From { get; private set; }

		public string To { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public MailState MailState { get; private set; }

		public IAttachment Attachment { get; private set; }

		public MailItem(string from, string to, string subject, string body, IAttachment attachment, MailState mailState)
		{
			ID = Guid.NewGuid();
			From = from;
			To = to;
			Subject = subject;
			Body = body;
			MailState = mailState;
			Attachment = attachment;
		}

		public void ChangeState(MailState mailState)
		{
			if (MailState != mailState)
				MailState = mailState;
		}
	}
}