using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class Job : Agent
	{
		public string Title { get; private set; }
		public double Salary { get; private set; }
		public int RequiredIntelligence { get; private set; }
		public int RequiredCreativity { get; private set; }
		public int RequiredCommunication { get; private set; }
		public int RequiredLeadership { get; private set; }

		public Job(string agentName, string title, double salary, int requiredIntelligence, int requiredCreativity, int requiredCommunication, int requiredLeadership)
			: base(agentName)
		{
			Title = title;
			Salary = salary;
			RequiredIntelligence = requiredIntelligence;
			RequiredCreativity = requiredCreativity;
			RequiredCommunication = requiredCommunication;
			RequiredLeadership = requiredIntelligence;
		}

		public double GetMonthlyPaymentAmount()
		{
			return Salary / 12.0f;
		}
	}
}
