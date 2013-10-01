using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThirdSDL.Descriptors
{
	public interface ISkillsAffector
	{
		int IntelligenceEffectiveness { get; }
		int CreativityEffectiveness { get; }
		int CommunicationEffectiveness { get; }
		int LeadershipEffectiveness { get; }
	}
}
