using MyThirdSDL.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Content
{
	public class CompanyMetadata
	{
		public string Name { get; private set; }
		public MailAddressType MailAddressType { get; set; }
		public string Domain { get; set; }

		public CompanyMetadata(string name, string mailAddressType, string domain)
		{
			Name = name;
			Domain = domain;

			if (mailAddressType == MailAddressType.Competitor.ToString())
				MailAddressType = MailAddressType.Competitor;
			else if (mailAddressType == MailAddressType.Employee.ToString())
				MailAddressType = MailAddressType.Employee;
			else if (mailAddressType == MailAddressType.Headhunter.ToString())
				MailAddressType = MailAddressType.Headhunter;
			else if (mailAddressType == MailAddressType.Spammer.ToString())
				MailAddressType = MailAddressType.Spammer;
		}
	}
}
