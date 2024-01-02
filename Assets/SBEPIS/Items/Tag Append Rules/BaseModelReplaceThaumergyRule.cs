using System.Collections.Generic;
using System.Linq;
using SBEPIS.Bits.Tags;
using SBEPIS.Items;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules
{
	[CreateAssetMenu(fileName = nameof(BaseModelFromBitsTagRule), menuName = "TagAppendRules/" + nameof(BaseModelFromBitsTagRule))]
	public class BaseModelFromBitsTagRule : TagAppendRule
	{
		public override void Apply(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags)
		{
			ItemModule module = ItemModuleManager.Instance.Modules
				.Where(module => resultBits.Has(module.Bits.Bits))
				.MaxBy(module => BitSet.GetUniquenessScore(BitSet.Empty, module.Bits.Bits));
			bTags.Add(new BaseModelTag(module));
		}
	}
}