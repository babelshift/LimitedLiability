using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.Descriptors
{
	public interface ISkillsAffector
	{
		SkillEffect SkillEffect { get; }
	}
}
