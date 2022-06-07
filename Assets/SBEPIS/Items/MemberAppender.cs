using SBEPIS.Bits.MemberAppendRules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SBEPIS.Bits
{
	public static class MemberAppender
	{
		private static MemberAppendRule[] rules = {
			new AppendMemberAppendRule()
		};

		public static Member[] Append(Member[] a, Member[] b)
		{
			if (a.Length == 0)
				return b;
			else if (b.Length == 0)
				return a;
			else
			{
				SortedSet<Member> aList = new(a), bList = new(b), result = new();

				foreach (MemberAppendRule rule in rules)
					if (rule.IsApplicable(aList, bList))
						rule.Apply(aList, bList, result);

				if (aList.Count > 0 || bList.Count > 0)
					throw new InvalidOperationException($"Members left over {aList.ToDelimString()}, {bList.ToDelimString()}");

				return result.ToArray();
			}
		}

		public static bool MembersEqual(Member[] a, Member[] b)
		{
			if (a == null && b == null)
				return true;
			else if (a == null || b == null)
				return false;
			else if (a.Length != b.Length)
				return false;
			else
				return a.SequenceEqual(b);
		}
	}
}

namespace SBEPIS.Bits.MemberAppendRules
{
	public interface MemberAppendRule
	{
		public abstract bool IsApplicable(SortedSet<Member> a, SortedSet<Member> b);
		public abstract void Apply(SortedSet<Member> a, SortedSet<Member> b, SortedSet<Member> result);
	}

	public class AppendMemberAppendRule : MemberAppendRule
	{
		public bool IsApplicable(SortedSet<Member> a, SortedSet<Member> b) => true;

		public void Apply(SortedSet<Member> a, SortedSet<Member> b, SortedSet<Member> result)
		{
			result.Union(a);
			result.Union(b);
			a.Clear();
			b.Clear();
		}
	}
}
