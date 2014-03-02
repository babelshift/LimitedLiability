namespace LimitedLiability.Content
{
	public class JobLevelMetadata
	{
		public string Prefix { get; private set; }

		public string Salary { get; private set; }

		public string Intelligence { get; private set; }

		public string Creativitity { get; private set; }

		public string Communication { get; private set; }

		public string Leadership { get; private set; }

		public JobLevelMetadata(string prefix, string salary, string intelligence, string creativity, string communication,
			string leadership)
		{
			Prefix = prefix;
			Salary = salary;
			Intelligence = intelligence;
			Creativitity = creativity;
			Communication = communication;
			Leadership = leadership;
		}
	}
}