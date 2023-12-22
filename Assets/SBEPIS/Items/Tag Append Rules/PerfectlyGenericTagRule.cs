using System.Collections.Generic;
using SBEPIS.Bits.Tags;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules
{
	[CreateAssetMenu(fileName = nameof(PerfectlyGenericTagRule), menuName = "TagAppendRules/" + nameof(PerfectlyGenericTagRule))]
	public class PerfectlyGenericTagRule : TagAppendRule
	{
		[SerializeField]
		private Material perfectlyGenericMaterial;
		
		public override void Apply(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags)
		{
			if (resultBits == BitSet.Empty)
				resultTags.Add(new MaterialTag(perfectlyGenericMaterial));
		}
	}
}