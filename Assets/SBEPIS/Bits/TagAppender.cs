using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Bits.TagAppendRules;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Bits
{
	[CreateAssetMenu(fileName = nameof(TagAppender))]
	public class TagAppender : ScriptableSingleton<TagAppender>
	{
		[FormerlySerializedAs("doubleRules")]
		[SerializeField]
		private List<TagAppendRule> rules = new();

		public TaggedBitSet Append(BitSet resultBits) => Append(TaggedBitSet.Empty, TaggedBitSet.Empty, resultBits);

		public TaggedBitSet Append(TaggedBitSet a, BitSet resultBits) => Append(a, TaggedBitSet.Empty, resultBits);

		public TaggedBitSet Append(TaggedBitSet a, TaggedBitSet b, BitSet resultBits)
		{
			List<Tag> aTags = a.Tags.ToList(), bTags = b.Tags.ToList(), resultTags = new();

			rules.ForEach(rule => rule.Apply(a.Bits, aTags, b.Bits, bTags, resultBits, resultTags));

			if (aTags.Count > 0 || bTags.Count > 0)
				throw new InvalidOperationException($"Tags left over {aTags.ToDelimString()}, {bTags.ToDelimString()}");

			return resultBits + resultTags;
		}
	}
}

namespace SBEPIS.Bits.TagAppendRules
{
	public abstract class TagAppendRule : ScriptableObject
	{
		public abstract void Apply(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags);
	}
}
