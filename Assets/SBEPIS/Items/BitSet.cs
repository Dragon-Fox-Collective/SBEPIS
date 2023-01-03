using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public class BitSet : IEnumerable<Bit>, IEquatable<BitSet>
	{
		[SerializeField]
		protected List<Bit> bits;

		public BitSet()
		{
			bits = new List<Bit>();
		}
		
		public BitSet(IEnumerable<Bit> bits)
		{
			this.bits = bits.ToList();
		}

		public override string ToString() => $"BitSet{bits.ToDelimString()}";
		public override int GetHashCode() => bits.GetHashCode();
		public override bool Equals(object obj) => obj is BitSet other && this == other;
		public bool Equals(BitSet other) => this == other;

		public static BitSet operator |(BitSet a, BitSet b) => new(a.bits.Union(b.bits));
		public static BitSet operator &(BitSet a, BitSet b) => new(a.bits.Intersect(b.bits));
		public static BitSet operator ^(BitSet a, BitSet b) => new(a.bits.Except(b.bits).Union(b.bits.Except(a.bits)));
		public static BitSet operator -(BitSet a, BitSet b) => new(a.bits.Except(b.bits));
		public static bool operator ==(BitSet a, BitSet b) => a is not null && b is not null ? new HashSet<Bit>(a.bits).SetEquals(b.bits) : a is null && b is null;
		public static bool operator !=(BitSet a, BitSet b) => !(a == b);

		public static implicit operator BitSet(Bit bit) => new(new []{bit});

		public TaggedBitSet With(IEnumerable<Tag> tags) => new(this, tags);
		public TaggedBitSet With(IEnumerable<Tag> a, IEnumerable<Tag> b) => With(TagAppender.Append(a, b));

		public bool Has(Bit other) => bits.Contains(other);
		public bool Has(BitSet other) => (this & other) == other;

		public int Count => bits.Count;
		public bool isPerfectlyGeneric => bits.Count == 0;

		public static int GetUniquenessScore(BitSet baseBits, BitSet appliedBits)
		{
			// For base     01110011
			// and applied  11000110
			//              --------
			// uniqueBits = 10000100
			// commonBits = 01000010
			// so  uniqueBits | commonBits == applied
			// and uniqueBits & commonBits == 0

			BitSet uniqueBits = (appliedBits ^ baseBits) & appliedBits;
			BitSet commonBits = appliedBits & baseBits;
			return uniqueBits.bits.Count - commonBits.bits.Count;
		}

		public IEnumerator GetEnumerator() => bits.GetEnumerator();
		IEnumerator<Bit> IEnumerable<Bit>.GetEnumerator() => bits.GetEnumerator();
	}
}
