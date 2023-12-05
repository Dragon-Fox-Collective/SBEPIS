﻿using System.Collections.Generic;
using System.Linq;
using SBEPIS.Bits.Tags;
using UnityEngine;

namespace SBEPIS.Bits.TagAppendRules.Double
{
	[CreateAssetMenu(fileName = nameof(PerfectlyGenericDoubleTagRule), menuName = "DoubleTagAppendRules/" + nameof(PerfectlyGenericDoubleTagRule))]
	public class PerfectlyGenericDoubleTagRule : DoubleTagAppendRule
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