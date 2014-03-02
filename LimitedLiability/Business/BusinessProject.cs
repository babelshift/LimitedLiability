using System;

namespace LimitedLiability
{
	public class BusinessProject
	{
		private const int totalRequiredProgress = 100;
		private double currentProgress = 0;

		public int PercentComplete { get { return Convert.ToInt32(Math.Round(currentProgress / totalRequiredProgress)); } }

		public Guid ID { get; private set; }

		public ResearchLevel RequiredResearchLevel { get; private set; }

		public BusinessProjectType BusinessProjectType { get; private set; }

		public BusinessProjectState State { get; private set; }

		public event EventHandler<EventArgs> Started;
		public event EventHandler<EventArgs> Paused;
		public event EventHandler<EventArgs> Cancelled;
		public event EventHandler<EventArgs> Completed;

		public BusinessProject(ResearchLevel requiredResearchLevel, BusinessProjectType businessProjectType)
		{
			RequiredResearchLevel = requiredResearchLevel;
			BusinessProjectType = businessProjectType;
			ID = Guid.NewGuid();
		}

		private void ChangeState(BusinessProjectState state)
		{
			State = state;
		}

		public void Start()
		{
			ChangeState(BusinessProjectState.InProgress);
			EventHelper.FireEvent<EventArgs>(Started, this, EventArgs.Empty);
		}

		public void Pause()
		{
			ChangeState(BusinessProjectState.Paused);
			EventHelper.FireEvent<EventArgs>(Paused, this, EventArgs.Empty);
		}

		public void Cancel()
		{
			ChangeState(BusinessProjectState.Cancelled);
			EventHelper.FireEvent<EventArgs>(Cancelled, this, EventArgs.Empty);
		}

		/// <summary>
		/// Projects should only be completed internally when the progress indicator reaches maximum.
		/// </summary>
		private void Complete()
		{
			ChangeState(BusinessProjectState.Completed);
			EventHelper.FireEvent<EventArgs>(Completed, this, EventArgs.Empty);
		}

		public void AddProgress(double progressAddition)
		{
			if (currentProgress + progressAddition > totalRequiredProgress)
			{
				currentProgress = totalRequiredProgress;
				Complete();
			}
			else
				currentProgress += progressAddition;
		}
	}
}

