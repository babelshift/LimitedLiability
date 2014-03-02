using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.Descriptors
{
	public interface IAffectsNecessities
	{
		NecessityEffect NecessityEffect { get; }
	}
}
