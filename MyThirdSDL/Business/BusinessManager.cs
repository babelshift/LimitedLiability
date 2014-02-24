using System;
using System.Linq;
using System.Collections.Generic;

namespace MyThirdSDL
{
	public class BusinessManager
	{
		private readonly List<BusinessProjectMetadata> potentialProjects = new List<BusinessProjectMetadata>();
		private readonly List<BusinessProject> projects = new List<BusinessProject>();

		public ResearchLevel CurrentResearchLevel { get; private set; }

		public IEnumerable<BusinessProject> InProgressProjects { get { return projects.Where(p => p.State == BusinessProjectState.InProgress); } }

		public IEnumerable<BusinessProject> PausedProjects { get { return projects.Where(p => p.State == BusinessProjectState.Paused); } }

		public IEnumerable<BusinessProject> CancelledProjects { get { return projects.Where(p => p.State == BusinessProjectState.Cancelled); } }

		public IEnumerable<BusinessProject> CompletedProjects { get { return projects.Where(p => p.State == BusinessProjectState.Completed); } }

		public event EventHandler<EventArgs> ProjectStarted;
		public event EventHandler<EventArgs> ProjectPaused;
		public event EventHandler<EventArgs> ProjectCancelled;
		public event EventHandler<EventArgs> ProjectCompleted;

		public BusinessManager()
		{
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.None, BusinessProjectType.Peripheral));
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.None, BusinessProjectType.MobileGame));
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.None, BusinessProjectType.InternetAdvertising));
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.Basic, BusinessProjectType.ConsoleGame));
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.Intermediate, BusinessProjectType.PCGame));
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.Intermediate, BusinessProjectType.CellPhone));
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.Intermediate, BusinessProjectType.Tablets));
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.Advanced, BusinessProjectType.PreBuiltPC));
			potentialProjects.Add(new BusinessProjectMetadata(ResearchLevel.Advanced, BusinessProjectType.GameConsole));
		}

		public void AddProgressToProject(Guid projectId, double progressAddition)
		{
			var project = GetProject(projectId);
			project.AddProgress(progressAddition);
		}

		public void StartProject(BusinessProjectType projectType)
		{
			BusinessProjectMetadata projectData = potentialProjects.FirstOrDefault(pp => pp.BusinessProjectType == projectType);
			if (projectData == null)
				throw new ArgumentOutOfRangeException(String.Format("No project registered with type {0}.", projectType));

			BusinessProject project = new BusinessProject(projectData.RequiredResearchLevel, projectData.BusinessProjectType);
			project.Started += (sender, e) => EventHelper.FireEvent(ProjectStarted, sender, e);
			project.Paused += (sender, e) => EventHelper.FireEvent(ProjectPaused, sender, e);
			project.Cancelled += (sender, e) => EventHelper.FireEvent(ProjectCancelled, sender, e);
			project.Completed += (sender, e) => EventHelper.FireEvent(ProjectCompleted, sender, e);

			project.Start();
			projects.Add(project);
		}

		public void CancelProject(Guid projectId)
		{
			var project = GetProject(projectId);
			project.Cancel();
		}

		private BusinessProject GetProject(Guid projectId)
		{
			BusinessProject project = projects.FirstOrDefault(ap => ap.ID == projectId);
			if (project == null)
				throw new InvalidOperationException(String.Format("Project with ID {0} doesn't exist.", projectId));
			return project;
		}
	}
}

