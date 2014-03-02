using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.Mail
{
	public class MailManager
	{
		private Mailbox playerMailbox;
		private Mailbox headhunterMailbox;
		private List<Mailbox> competitorMailboxes = new List<Mailbox>();
		private List<Mailbox> employeeMailboxes = new List<Mailbox>();
		private List<Mailbox> spammerMailboxes = new List<Mailbox>();

		public IEnumerable<MailItem> PlayerInbox { get { return playerMailbox.InboxMailItems; } }
		public IEnumerable<MailItem> PlayerOutbox { get { return playerMailbox.OutboxMailItems; } }
		public IEnumerable<MailItem> PlayerArchive { get { return playerMailbox.ArchiveMailItems; } }
		public int PlayerUnreadMailCount { get { return playerMailbox.UnreadMailCount; } }

		public event EventHandler UnreadMailCountChanged;

		private IEnumerable<Mailbox> AllMailboxes
		{
			get
			{
				List<Mailbox> allMailboxes = new List<Mailbox>();
				allMailboxes.Add(playerMailbox);
				allMailboxes.Add(headhunterMailbox);
				allMailboxes.AddRange(competitorMailboxes);
				allMailboxes.AddRange(employeeMailboxes);
				allMailboxes.AddRange(spammerMailboxes);
				return allMailboxes;
			}
		}

		public MailManager()
		{
			playerMailbox = new Mailbox(new MailAddress("first.last@company.com", MailAddressType.Player));
			headhunterMailbox = new Mailbox(new MailAddress("first.last@recruiters.com", MailAddressType.Headhunter));
			competitorMailboxes.Add(new Mailbox(new MailAddress("first.last@competitor1.com", MailAddressType.Competitor)));
			competitorMailboxes.Add(new Mailbox(new MailAddress("first.last@competitor2.com", MailAddressType.Competitor)));
			competitorMailboxes.Add(new Mailbox(new MailAddress("first.last@competitor3.com", MailAddressType.Competitor)));
			competitorMailboxes.Add(new Mailbox(new MailAddress("first.last@competitor4.com", MailAddressType.Competitor)));
			competitorMailboxes.Add(new Mailbox(new MailAddress("first.last@competitor5.com", MailAddressType.Competitor)));
			employeeMailboxes.Add(new Mailbox(new MailAddress("first.last@geemail.com", MailAddressType.Employee)));
			employeeMailboxes.Add(new Mailbox(new MailAddress("first.last@inlook.com", MailAddressType.Employee)));
			employeeMailboxes.Add(new Mailbox(new MailAddress("first.last@wahoo.com", MailAddressType.Employee)));
			employeeMailboxes.Add(new Mailbox(new MailAddress("first.last@tinymail.com", MailAddressType.Employee)));
			employeeMailboxes.Add(new Mailbox(new MailAddress("first.last@freemail.com", MailAddressType.Employee)));
			spammerMailboxes.Add(new Mailbox(new MailAddress("no_reply@pillz.com", MailAddressType.Spammer)));
			spammerMailboxes.Add(new Mailbox(new MailAddress("no_reply@bigpharma.com", MailAddressType.Spammer)));
			spammerMailboxes.Add(new Mailbox(new MailAddress("no_reply@worksafeporn.com", MailAddressType.Spammer)));
			spammerMailboxes.Add(new Mailbox(new MailAddress("no_reply@bizdev.com", MailAddressType.Spammer)));
			spammerMailboxes.Add(new Mailbox(new MailAddress("no_reply@mylittlepony.com", MailAddressType.Spammer)));

			playerMailbox.UnreadMailCountChanged += playerMailbox_UnreadMailCountChanged;
		}

		private void playerMailbox_UnreadMailCountChanged(object sender, EventArgs e)
		{
			if(UnreadMailCountChanged != null)
				UnreadMailCountChanged(sender, e);
		}

		public void SendMail(MailItem mail)
		{
			Mailbox fromMailbox = AllMailboxes.FirstOrDefault(m => m.MailAddress.Address == mail.From);
			if (fromMailbox == null)
				throw new Exception(String.Format("Mailbox with address {0} not found.", mail.From));

			Mailbox toMailbox = AllMailboxes.FirstOrDefault(m => m.MailAddress.Address == mail.To);
			if (toMailbox == null)
				throw new Exception(String.Format("Mailbox with address {0} not found.", mail.To));

			fromMailbox.AddMailToOutbox(mail);
			toMailbox.AddMailToInbox(mail);
		}

		public void ArchiveMail(MailItem mailItem)
		{
			playerMailbox.MoveMailToArchive(mailItem.ID);
		}
	}
}
