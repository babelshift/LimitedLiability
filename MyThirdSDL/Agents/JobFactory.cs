using MyThirdSDL.Content;
using MyThirdSDL.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyThirdSDL.Agents
{
	public class JobFactory
	{
		private readonly Random random = new Random();
		private readonly List<Job> jobs = new List<Job>();

		public JobFactory(ContentManager contentManager)
		{
			IReadOnlyList<JobMetadata> jobMetadata = contentManager.Jobs;
			foreach (var job in jobMetadata)
			{
				List<JobLevel> jobLevels = new List<JobLevel>();
				foreach (var jobLevel in job.JobLevelMetadata)
				{
					string prefix = jobLevel.Prefix;
					int salary;
					Int32.TryParse(jobLevel.Salary, out salary);

					int requiredIntelligence;
					Int32.TryParse(jobLevel.Intelligence, out requiredIntelligence);

					int requiredCreativity;
					Int32.TryParse(jobLevel.Creativitity, out requiredCreativity);

					int requiredCommunication;
					Int32.TryParse(jobLevel.Communication, out requiredCommunication);

					int requiredLeadership;
					Int32.TryParse(jobLevel.Leadership, out requiredLeadership);

					JobLevel newJobLevel = new JobLevel(prefix, salary, (Skills.Rating)requiredIntelligence, (Skills.Rating)requiredCreativity, (Skills.Rating)requiredCommunication, (Skills.Rating)requiredLeadership);

					jobLevels.Add(newJobLevel);
				}
				Job newJob = new Job(job.Title, jobLevels);
				jobs.Add(newJob);
			}
		}

		/// <summary>
		/// Return a random job at a random job level. For example: Janitor at Junior level or Sales at Senior level.
		/// </summary>
		/// <returns></returns>
		public Job CreateRandomJob()
		{
			int randomJobIndex = random.Next(0, jobs.Count - 1);
			int randomJobLevel = random.Next(0, 4);

			var job = jobs[randomJobIndex];

			// return a copy because there is the potential for the employee to be promoted/demoted which will affect the individual state of the job object
			// if we returned a reference to the stored job object, every employee who had that reference would simultaneously be promoted/demoted
			var jobCopy = job.Copy();

			for(int i = 0; i <= randomJobLevel; i++)
				jobCopy.Promote();

			return jobCopy;
		}
	}
}