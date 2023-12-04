using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules.Double
{
	[CreateAssetMenu(fileName = nameof(AppendDoubleTagRule), menuName = "DoubleTagAppendRules/" + nameof(AppendDoubleTagRule))]
	public class AppendDoubleTagRule : DoubleTagAppendRule
	{
		public override void Apply(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags)
		{
			List<Tag> res = resultTags.Union(aTags).Union(bTags).ToList();
			resultTags.Clear();
			aTags.Clear();
			bTags.Clear();
			resultTags.AddRange(res);
		}
	}
}