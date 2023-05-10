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
		
		public override string ToString() => $"Tagged{bits}{Tags.ToDelimString()}";
		public override int GetHashCode() => (bits, Tags).GetHashCode();
		public override bool Equals(object obj) => obj is TaggedBitSet other && this == other;
		public bool Equals(TaggedBitSet other) => this == other;
		
		public static TaggedBitSet operator |(TaggedBitSet a, TaggedBitSet b) => (a.bits | b.bits).With(a.Tags, b.Tags);
		public static TaggedBitSet operator &(TaggedBitSet a, TaggedBitSet b) => (a.bits & b.bits).With(a.Tags, b.Tags);
		public static TaggedBitSet operator ^(TaggedBitSet a, TaggedBitSet b) => (a.bits ^ b.bits).With(a.Tags, b.Tags);
		public static TaggedBitSet operator -(TaggedBitSet a, TaggedBitSet b) => (a.bits - b.bits).With(a.Tags, b.Tags);
		public static bool operator ==(TaggedBitSet a, TaggedBitSet b) => a.bits == b.bits && TagAppender.MembersEqual(a.Tags, b.Tags);
		public static bool operator !=(TaggedBitSet a, TaggedBitSet b) => a.bits != b.bits || !TagAppender.MembersEqual(a.Tags, b.Tags);
		
		public static TaggedBitSet operator |(TaggedBitSet a, BitSet b) => (a.bits | b).With(a.Tags);
		public static TaggedBitSet operator |(BitSet a, TaggedBitSet b) => (a | b.bits).With(b.Tags);
		public static TaggedBitSet operator &(TaggedBitSet a, BitSet b) => (a.bits & b).With(a.Tags);
		public static TaggedBitSet operator &(BitSet a, TaggedBitSet b) => (a & b.bits).With(b.Tags);
		public static TaggedBitSet operator ^(TaggedBitSet a, BitSet b) => (a.bits ^ b).With(a.Tags);
		public static TaggedBitSet operator ^(BitSet a, TaggedBitSet b) => (a ^ b.bits).With(b.Tags);
		
		public static TaggedBitSet operator +(TaggedBitSet a, Tag b) => a.With(new[]{ b });
		public static TaggedBitSet operator +(TaggedBitSet a, Tag[] b) => a.With(b);
		public static TaggedBitSet operator -(TaggedBitSet a, Tag b) => a.Without(new[]{ b });
		public static TaggedBitSet operator -(TaggedBitSet a, Tag[] b) => a.Without(b);
		
		public bool Has(BitSet other) => bits.Has(other);
		
		public TaggedBitSet With(IEnumerable<Tag> other) => bits.With(TagAppender.Append(Tags, other));
		public TaggedBitSet Without(IEnumerable<Tag> other) => bits.With(Tags.Where(member => !other.Contains(member)).ToArray());
	}
}
