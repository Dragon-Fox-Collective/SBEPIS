using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

namespace SBEPIS.Bits
{
	[Serializable]
	public class TaggedBitSet : IEquatable<TaggedBitSet>
	{
		[SerializeField]
		private BitSet _bits;
		public BitSet bits => _bits;

		[SerializeField]
		[FormerlySerializedAs("_members")]
		private List<Tag> _tags;
		public IEnumerable<Tag> tags => _tags;
		
		public TaggedBitSet(BitSet bits, IEnumerable<Tag> tags)
		{
			_bits = bits;
			_tags = tags is null ? new List<Tag>() : tags.ToList();
		}

		public override string ToString() => $"Tagged{_bits}{_tags.ToDelimString()}";
		public override int GetHashCode() => (bits, tags).GetHashCode();
		public override bool Equals(object obj) => obj is TaggedBitSet other && this == other;
		public bool Equals(TaggedBitSet other) => this == other;

		public static TaggedBitSet operator |(TaggedBitSet a, TaggedBitSet b) => (a.bits | b.bits).With(a._tags, b._tags);
		public static TaggedBitSet operator &(TaggedBitSet a, TaggedBitSet b) => (a.bits & b.bits).With(a._tags, b._tags);
		public static TaggedBitSet operator ^(TaggedBitSet a, TaggedBitSet b) => (a.bits ^ b.bits).With(a._tags, b._tags);
		public static TaggedBitSet operator -(TaggedBitSet a, TaggedBitSet b) => (a.bits - b._bits).With(a._tags, b._tags);
		public static bool operator ==(TaggedBitSet a, TaggedBitSet b) => a?.bits == b?.bits && TagAppender.MembersEqual(a?._tags, b?._tags);
		public static bool operator !=(TaggedBitSet a, TaggedBitSet b) => a?.bits != b?.bits || !TagAppender.MembersEqual(a?._tags, b?._tags);

		public static TaggedBitSet operator |(TaggedBitSet a, BitSet b) => (a.bits | b).With(a._tags);
		public static TaggedBitSet operator |(BitSet a, TaggedBitSet b) => (a | b.bits).With(b._tags);
		public static TaggedBitSet operator &(TaggedBitSet a, BitSet b) => (a.bits & b).With(a._tags);
		public static TaggedBitSet operator &(BitSet a, TaggedBitSet b) => (a & b.bits).With(b._tags);
		public static TaggedBitSet operator ^(TaggedBitSet a, BitSet b) => (a.bits ^ b).With(a._tags);
		public static TaggedBitSet operator ^(BitSet a, TaggedBitSet b) => (a ^ b.bits).With(b._tags);

		public static TaggedBitSet operator +(TaggedBitSet a, Tag b) => a.With(new[]{ b });
		public static TaggedBitSet operator +(TaggedBitSet a, Tag[] b) => a.With(b);
		public static TaggedBitSet operator -(TaggedBitSet a, Tag b) => a.Without(new[]{ b });
		public static TaggedBitSet operator -(TaggedBitSet a, Tag[] b) => a.Without(b);

		public bool Has(BitSet other) => _bits.Has(other);

		public TaggedBitSet With(IEnumerable<Tag> other) => _bits.With(TagAppender.Append(_tags, other));
		public TaggedBitSet Without(IEnumerable<Tag> other) => _bits.With(_tags.Where(member => !other.Contains(member)).ToArray());
	}
}
