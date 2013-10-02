using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public class Job
	{
		public string Title { get; private set; }
		public double Salary { get; private set; }
		public Skills.Rating RequiredIntelligence { get; private set; }
		public Skills.Rating RequiredCreativity { get; private set; }
		public Skills.Rating RequiredCommunication { get; private set; }
		public Skills.Rating RequiredLeadership { get; private set; }

		public Job(string title, double salary, 
			Skills.Rating requiredIntelligence, 
			Skills.Rating requiredCreativity, 
			Skills.Rating requiredCommunication, 
			Skills.Rating requiredLeadership)
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
