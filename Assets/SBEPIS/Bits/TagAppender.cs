using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Bits.TagAppendRules.Double;
using SBEPIS.Bits.TagAppendRules.Single;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Bits
{
	[CreateAssetMenu(fileName = nameof(TagAppender))]
	public class TagAppender : ScriptableSingleton<TagAppender>
	{
		[SerializeField]
		private List<SingleTagAppendRule> singleRules = new();

		[SerializeField]
		private List<DoubleTagAppendRule> doubleRules = new();

		public TaggedBitSet Append(TaggedBitSet a, BitSet resultBits)
		{
			List<Tag> aTags = a.Tags.ToList(), resultTags = new();

			singleRules.ForEach(rule => rule.Apply(a.Bits, aTags, resultBits, resultTags));

			if (aTags.Count > 0)
				throw new InvalidOperationException($"Tags left over {aTags.ToDelimString()}");

			return resultBits + resultTags;
		}

		public TaggedBitSet Append(TaggedBitSet a, TaggedBitSet b, BitSet resultBits)
		{
			List<Tag> aTags = a.Tags.ToList(), bTags = b.Tags.ToList(), resultTags = new();

			doubleRules.ForEach(rule => rule.Apply(a.Bits, aTags, b.Bits, bTags, resultBits, resultTags));

			if (aTags.Count > 0 || bTags.Count > 0)
				throw new InvalidOperationException($"Tags left over {aTags.ToDelimString()}, {bTags.ToDelimString()}");

			return resultBits + resultTags;
		}
	}
}

namespace SBEPIS.Bits.TagAppendRules.Single
{
	public abstract class SingleTagAppendRule : ScriptableObject
	{
		public abstract void Apply(BitSet aBits, List<Tag> aTags, BitSet resultBits, List<Tag> resultTags);
	}
}

namespace SBEPIS.Bits.TagAppendRules.Double
{
	public abstract class DoubleTagAppendRule : ScriptableObject
	{
		public abstract void Apply(BitSet aBits, List<Tag> aTags, BitSet bBits, List<Tag> bTags, BitSet resultBits, List<Tag> resultTags);
	}
}
