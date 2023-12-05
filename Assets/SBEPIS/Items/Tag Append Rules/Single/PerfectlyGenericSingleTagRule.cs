using System.Collections.Generic;
using System.Linq;
using SBEPIS.Bits.Tags;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules.Single
{
	[CreateAssetMenu(fileName = nameof(PerfectlyGenericSingleTagRule), menuName = "SingleTagAppendRules/" + nameof(PerfectlyGenericSingleTagRule))]
	public class PerfectlyGenericSingleTagRule : SingleTagAppendRule
	{
		[SerializeField]
		private Material perfectlyGenericMaterial;
		
		public override void Apply(BitSet aBits, List<Tag> aTags, BitSet resultBits, List<Tag> resultTags)
		{
			if (resultBits == BitSet.Empty)
				resultTags.Add(new MaterialTag(perfectlyGenericMaterial));
		}
	}
}