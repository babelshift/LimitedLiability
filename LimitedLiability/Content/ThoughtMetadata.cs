using LimitedLiability.Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitedLiability.Content
{
	public class ThoughtMetadata
	{
		public ThoughtType Type { get; private set; }
		public string Idea { get; private set; }

		public ThoughtMetadata(ThoughtType type, string idea)
		{
			Type = type;
			Idea = idea;
		}
	}
}
