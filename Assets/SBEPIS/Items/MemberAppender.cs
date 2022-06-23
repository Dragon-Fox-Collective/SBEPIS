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
				HashSet<Member> aList = new(a), bList = new(b), result = new();

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
			if (a is null && b is null)
				return true;
			else if (a is null || b is null)
				return false;
			else if (a.Length != b.Length)
				return false;
			else
				return new HashSet<Member>(a).SetEquals(b);
		}
	}
}

namespace SBEPIS.Bits.MemberAppendRules
{
	public abstract class MemberAppendRule
	{
		public abstract bool IsApplicable(HashSet<Member> a, HashSet<Member> b);
		public abstract void Apply(HashSet<Member> a, HashSet<Member> b, HashSet<Member> result);
	}

	public class AppendMemberAppendRule : MemberAppendRule
	{
		public override bool IsApplicable(HashSet<Member> a, HashSet<Member> b) => true;

		public override void Apply(HashSet<Member> a, HashSet<Member> b, HashSet<Member> result)
		{
			result.Union(a);
			result.Union(b);
			a.Clear();
			b.Clear();
		}
	}
}
