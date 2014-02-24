using System.Collections.Generic;

namespace MyThirdSDL.Content
{
	public interface IHasNeighbors<TNode>
	{
		IEnumerable<TNode> Neighbors { get; }
	}
}
