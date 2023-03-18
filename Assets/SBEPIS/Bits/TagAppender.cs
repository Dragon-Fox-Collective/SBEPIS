using SBEPIS.Bits.TagAppendRules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SBEPIS.Bits
{
	public static class TagAppender
	{
		private static readonly TagAppendRule[] Rules = {
			new AppendTagAppendRule()
		};

		public static IEnumerable<Tag> Append(IEnumerable<Tag> a, IEnumerable<Tag> b)
		{
			List<Tag> aList = a.ToList(), bList = b.ToList();
			
			if (aList.Count == 0)
				return bList;
			if (bList.Count == 0)
				return aList;
			
			IEnumerable<Tag> result = Rules.Where(rule => rule.IsApplicable(aList, bList)).Aggregate(new List<Tag>().AsEnumerable(), (result, rule) => result.Union(rule.Apply(aList, bList)));

			if (aList.Count > 0 || bList.Count > 0)
				throw new InvalidOperationException($"Members left over {aList.ToDelimString()}, {bList.ToDelimString()}");

			return result ?? new List<Tag>();
		}

		public static bool MembersEqual(IEnumerable<Tag> a, IEnumerable<Tag> b)
		{
			if (a is null && b is null)
				return true;
			if (a is null || b is null)
				return false;
			return new HashSet<Tag>(a).SetEquals(b);
		}
	}
}

namespace SBEPIS.Bits.TagAppendRules
{
	public abstract class TagAppendRule
	{
		public abstract bool IsApplicable(List<Tag> a, List<Tag> b);
		public abstract IEnumerable<Tag> Apply(List<Tag> a, List<Tag> b);
	}

	public class AppendTagAppendRule : TagAppendRule
	{
		public override bool IsApplicable(List<Tag> a, List<Tag> b) => true;

		public override IEnumerable<Tag> Apply(List<Tag> a, List<Tag> b)
		{
			IEnumerable<Tag> union = a.Union(b);
			a.Clear();
			b.Clear();
			return union;
		}
	}
}
