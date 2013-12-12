﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Mail
{
	public class Mailbox
	{
		private List<MailItem> inbox = new List<MailItem>();
		private List<MailItem> outbox = new List<MailItem>();
		private List<MailItem> archive = new List<MailItem>();

		public MailAddressType FromAddressType { get; private set; }

		public MailAddress MailAddress { get; private set; }

		public IEnumerable<MailItem> InboxMailItems { get { return inbox; } }
		public IEnumerable<MailItem> OutboxMailItems { get { return outbox; } }
		public IEnumerable<MailItem> ArchiveMailItems { get { return archive; } }

		public Mailbox(MailAddress mailAddress)
		{
			MailAddress = mailAddress;
		}

		public void AddMailToInbox(MailItem mail)
		{
			if (!inbox.Any(m => m.ID == mail.ID))
				inbox.Add(mail);
		}

		public void AddMailToOutbox(MailItem mail)
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
			MailItem mail = inbox.FirstOrDefault(m => m.ID == mailId);
			if (mail != null)
			{
				inbox.Remove(mail);
				archive.Add(mail);
			}
		}
	}
}
