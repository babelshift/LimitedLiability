using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.Mail
{
	public class MailAddress
	{
		public MailAddressType AddressType { get; private set; }
		public string Address { get; private set; }

		public MailAddress(string address, MailAddressType addressType)
		{
			Address = address;
			AddressType = addressType;
		}
	}
}
