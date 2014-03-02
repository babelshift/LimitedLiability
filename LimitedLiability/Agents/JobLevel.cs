using LimitedLiability.Descriptors;

namespace LimitedLiability.Agents
{
	public class JobLevel
	{
		public string Prefix { get; private set; }

		public int Salary { get; private set; }

		public Skills.Rating RequiredIntelligence { get; private set; }

		public Skills.Rating RequiredCreativity { get; private set; }

		public Skills.Rating RequiredCommunication { get; private set; }

		public Skills.Rating RequiredLeadership { get; private set; }

		public JobLevel(string prefix, int salary, Skills.Rating requiredIntelligence, Skills.Rating requiredCreativity,
			Skills.Rating requiredCommunication, Skills.Rating requiredLeadership)
		{
			Prefix = prefix;
			Salary = salary;
			RequiredIntelligence = requiredIntelligence;
			RequiredCreativity = requiredCreativity;
			RequiredCommunication = requiredCommunication;
			RequiredLeadership = requiredLeadership;
		}
	}
}