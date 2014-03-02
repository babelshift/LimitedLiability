namespace LimitedLiability
{
	public class BusinessProjectMetadata
	{
		public ResearchLevel RequiredResearchLevel { get; private set; }
		public BusinessProjectType BusinessProjectType { get; private set; }

		public BusinessProjectMetadata(ResearchLevel requiredResearchLevel, BusinessProjectType businessProjectType)
		{
			RequiredResearchLevel = requiredResearchLevel;
			BusinessProjectType = businessProjectType;
		}
	}
}

