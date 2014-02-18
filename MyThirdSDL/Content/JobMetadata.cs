using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Content
{
	public class JobMetadata
	{
		private readonly List<JobLevelMetadata> jobLevelMetadataCollection = new List<JobLevelMetadata>();

		public string Title { get; private set; }
		public IReadOnlyList<JobLevelMetadata> JobLevelMetadata { get { return jobLevelMetadataCollection; } }

		public JobMetadata(string title)
		{
			Title = title;
		}

		public void AddJobLevelMetadata(JobLevelMetadata jobLevelMetadata)
		{
			jobLevelMetadataCollection.Add(jobLevelMetadata);
		}
	}
}
