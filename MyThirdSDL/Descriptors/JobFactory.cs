using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public static class JobFactory
	{
		private static List<Job> jobs = new List<Job>();
		private static Random random = new Random();
		private static bool isInitialized = false;

		/// <summary>
		/// Initializes all jobs in the system. Must be called prior to calling any other methods.
		/// </summary>
		public static void Initialize()
		{
			Job janitor = new Job("Janitor", 25000, Skills.Rating.Atrocious, Skills.Rating.Atrocious, Skills.Rating.Atrocious, Skills.Rating.Atrocious);
			Job maintenance = new Job("Maintenance", 30000, Skills.Rating.Satisfactory, Skills.Rating.Atrocious, Skills.Rating.Atrocious, Skills.Rating.Atrocious);
			Job entertainer = new Job("Entertainer", 40000, Skills.Rating.Neutral, Skills.Rating.Great, Skills.Rating.Great, Skills.Rating.Neutral);
			Job accountant = new Job("Accountant", 50000, Skills.Rating.Great, Skills.Rating.Atrocious, Skills.Rating.Atrocious, Skills.Rating.Atrocious);
			Job engineer = new Job("Engineer", 70000, Skills.Rating.Excellent, Skills.Rating.Bad, Skills.Rating.Bad, Skills.Rating.Bad);
			Job marketer = new Job("Marketer", 40000, Skills.Rating.Neutral, Skills.Rating.Good, Skills.Rating.Excellent, Skills.Rating.Neutral);
			Job sales = new Job("Sales", 40000, Skills.Rating.Neutral, Skills.Rating.Great, Skills.Rating.Great, Skills.Rating.Neutral);
			Job humanResources = new Job("Human Resources", 50000, Skills.Rating.Neutral, Skills.Rating.Atrocious, Skills.Rating.Great, Skills.Rating.Good);
			Job manager = new Job("Manager", 90000, Skills.Rating.Good, Skills.Rating.Atrocious, Skills.Rating.Great, Skills.Rating.Great);
			Job executive = new Job("Executive", 110000, Skills.Rating.Great, Skills.Rating.Atrocious, Skills.Rating.Excellent, Skills.Rating.Excellent);

			jobs.Add(janitor);
			jobs.Add(maintenance);
			jobs.Add(entertainer);
			jobs.Add(accountant);
			jobs.Add(engineer);
			jobs.Add(marketer);
			jobs.Add(sales);
			jobs.Add(humanResources);
			jobs.Add(manager);
			jobs.Add(executive);

			isInitialized = true;
		}

		/// <summary>
		/// Return a random job that fits the passed skill criteria.
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		public static Job CreateJob(Skills skill)
		{
			if (!isInitialized)
				throw new Exception("JobFactory has not been initialized yet. Call Initialize() first.");

			 List<Job> validJobs = jobs.Where(j =>
				skill.Intelligence >= j.RequiredIntelligence
				&& skill.Creativity >= j.RequiredCreativity
				&& skill.Communication >= j.RequiredCommunication
				&& skill.Leadership >= j.RequiredLeadership).ToList();

			int i = random.Next(0, validJobs.Count - 1);

			return validJobs[i];
		}
	}
}
