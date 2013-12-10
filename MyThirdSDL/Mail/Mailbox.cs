using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Mail
{
	public class Mailbox
	{
		private List<Mail> inbox = new List<Mail>();
		private List<Mail> outbox = new List<Mail>();
		private List<Mail> archive = new List<Mail>();

		public MailAddressType FromAddressType { get; private set; }

		public MailAddress MailAddress { get; private set; }

		public Mailbox(MailAddress mailAddress)
		{
			MailAddress = mailAddress;
		}

		public void AddMailToInbox(Mail mail)
		{
			if (!inbox.Any(m => m.ID == mail.ID))
				inbox.Add(mail);
		}

		public void AddMailToOutbox(Mail mail)
		{
			if (!outbox.Any(m => m.ID == mail.ID))
				outbox.Add(mail);
		}

		public void DeleteMailFromInbox(Guid mailId)
		{
			inbox.RemoveAll(m => m.ID == mailId);
		}

		public void DeleteMailFromOutbox(Guid mailId)
		{
			outbox.RemoveAll(m => m.ID == mailId);
		}

		public void DeleteMailFromArchive(Guid mailId)
		{
			archive.RemoveAll(m => m.ID == mailId);
		}

		public void MoveMailToArchive(Guid mailId)
		{
			Mail mail = inbox.FirstOrDefault(m => m.ID == mailId);
			if (mail != null)
			{
				inbox.Remove(mail);
				archive.Add(mail);
			}
		}
	}
}
