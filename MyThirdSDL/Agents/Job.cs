using System;
using System.Collections.Generic;
using log4net.Appender;

namespace MyThirdSDL.Agents
{
	public class Job
	{
		private int currentLevel = 0;
		private readonly IReadOnlyList<JobLevel> levels;

		public string FullTitle { get { return String.Format("{0} {1}", CurrentLevel.Prefix, Title); } }

		public string Title { get; private set; }

		public JobLevel CurrentLevel { get { return levels[currentLevel]; } }

		public Job(string title, IReadOnlyList<JobLevel> levels)
		{
			Title = title;
			this.levels = levels;
		}

		/// <summary>
		/// Private copy constructor
		/// </summary>
		/// <param name="title"></param>
		/// <param name="levels"></param>
		/// <param name="currentLevel"></param>
		private Job(string title, IReadOnlyList<JobLevel> levels, int currentLevel)
			: this(title, levels)
		{
			this.currentLevel = currentLevel;
		}

		public double GetMonthlyPaymentAmount()
		{
			return CurrentLevel.Salary / 12.0f;
		}

		public void Promote()
		{
			if (currentLevel + 1 > 4)
				throw new InvalidOperationException("Cannot promote beyond Level 5.");

			currentLevel++;
		}

		public void Demote()
		{
			if (currentLevel - 1 < 0)
				throw new InvalidOperationException("Cannot demote beyond Level 1.");

			currentLevel--;
		}

		public Job Copy()
		{
			return new Job(Title, levels, currentLevel);
		}
	}
}