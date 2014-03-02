using System.Collections.Generic;

namespace LimitedLiability.Content
{
	public interface IHasNeighbors<TNode>
	{
		IEnumerable<TNode> Neighbors { get; }
	}
}
