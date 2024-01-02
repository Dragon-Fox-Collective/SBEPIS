using System.Collections.Generic;
using SBEPIS.Bits.Tags;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules
{
	[CreateAssetMenu(fileName = nameof(BaseModelSingletonTagRule), menuName = "TagAppendRules/" + nameof(BaseModelSingletonTagRule))]
	public class BaseModelSingletonTagRule : SingletonTagRule<BaseModelTag>
	{
		protected override bool Predicate(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags, BaseModelTag tag) =>
			tag.ItemModule.Bits.Bits != BitSet.Empty && resultBits.Has(tag.ItemModule.Bits.Bits);
	}
}