using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules.Single
{
	[CreateAssetMenu(fileName = nameof(AppendSingleTagRule), menuName = "SingleTagAppendRules/" + nameof(AppendSingleTagRule))]
	public class AppendSingleTagRule : SingleTagAppendRule
	{
		public override void Apply(BitSet aBits, List<Tag> aTags, BitSet resultBits, List<Tag> resultTags)
		{
			List<Tag> res = resultTags.Union(aTags).ToList();
			resultTags.Clear();
			aTags.Clear();
			resultTags.AddRange(res);
		}
	}
}