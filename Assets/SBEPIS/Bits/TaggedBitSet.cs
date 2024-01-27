using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

namespace SBEPIS.Bits
{
	[Serializable]
	public struct TaggedBitSet : IEquatable<TaggedBitSet>
	{
		public static readonly TaggedBitSet Empty = new(BitSet.Empty, Enumerable.Empty<Tag>());
		
		[FormerlySerializedAs("_bits")]
		[SerializeField] private BitSet bits;
		public BitSet Bits => bits;
		
		[FormerlySerializedAs("_tags")]
		[FormerlySerializedAs("_members")]
		[SerializeField] private Tag[] tags;
		public IEnumerable<Tag> Tags => tags ??= Array.Empty<Tag>();
		
		public TaggedBitSet(BitSet bits, IEnumerable<Tag> tags)
		{
			this.bits = bits;
			this.tags = tags.ToArray();
		}
		
		public override string ToString() => bits + Tags.ToDelimString();
		public override int GetHashCode() => (bits, Tags).GetHashCode();
		public override bool Equals(object obj) => obj is TaggedBitSet other && this == other;
		public bool Equals(TaggedBitSet other) => this == other;
		
		public static TaggedBitSet operator |(TaggedBitSet a, TaggedBitSet b) => TagAppender.Instance.Append(a, b, a.bits | b.bits);
		public static TaggedBitSet operator &(TaggedBitSet a, TaggedBitSet b) => TagAppender.Instance.Append(a, b, a.bits & b.bits);
		public static TaggedBitSet operator ^(TaggedBitSet a, TaggedBitSet b) => TagAppender.Instance.Append(a, b, a.bits ^ b.bits);
		public static TaggedBitSet operator -(TaggedBitSet a, TaggedBitSet b) => TagAppender.Instance.Append(a, b, a.bits - b.bits);
		public static bool operator ==(TaggedBitSet a, TaggedBitSet b) => a.bits == b.bits && TagsEqual(a.Tags, b.Tags);
		public static bool operator !=(TaggedBitSet a, TaggedBitSet b) => a.bits != b.bits || !TagsEqual(a.Tags, b.Tags);
		
		public static TaggedBitSet operator |(TaggedBitSet a, BitSet b) => TagAppender.Instance.Append(a, a.bits | b);
		public static TaggedBitSet operator |(BitSet a, TaggedBitSet b) => TagAppender.Instance.Append(b, a | b.bits);
		public static TaggedBitSet operator &(TaggedBitSet a, BitSet b) => TagAppender.Instance.Append(a, a.bits & b);
		public static TaggedBitSet operator &(BitSet a, TaggedBitSet b) => TagAppender.Instance.Append(b, a & b.bits);
		public static TaggedBitSet operator ^(TaggedBitSet a, BitSet b) => TagAppender.Instance.Append(a, a.bits ^ b);
		public static TaggedBitSet operator ^(BitSet a, TaggedBitSet b) => TagAppender.Instance.Append(b, a ^ b.bits);
		public static TaggedBitSet operator -(TaggedBitSet a, BitSet b) => TagAppender.Instance.Append(a, a.bits - b);
		public static TaggedBitSet operator -(BitSet a, TaggedBitSet b) => TagAppender.Instance.Append(b, a - b.bits);
		
		public static TaggedBitSet operator +(TaggedBitSet a, IEnumerable<Tag> b) => a.bits + a.Tags.Union(b);
		public static TaggedBitSet operator +(TaggedBitSet a, Tag b) => a + EnumerableOf.Of(b);
		public static TaggedBitSet operator -(TaggedBitSet a, IEnumerable<Tag> b) => a.bits + a.Tags.Except(b);
		public static TaggedBitSet operator -(TaggedBitSet a, Tag b) => a - EnumerableOf.Of(b);
		
		public static implicit operator TaggedBitSet(BitSet bits) => new(bits, Enumerable.Empty<Tag>());
		
		public bool Has(BitSet other) => bits.Has(other);
		
		public static bool TagsEqual(IEnumerable<Tag> a, IEnumerable<Tag> b)
		{
			if (a is null && b is null)
				return true;
			if (a is null || b is null)
				return false;
			return new HashSet<Tag>(a).SetEquals(b);
		}
	}
}
