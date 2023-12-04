using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public struct BitSet : IEnumerable<Bit>, IEquatable<BitSet>
	{
		public static readonly BitSet Empty = new(Array.Empty<Bit>());
		
		[SerializeField] private Bit[] bits;
		private Bit[] Bits => bits ??= Array.Empty<Bit>();
		
		public BitSet(IEnumerable<Bit> bits)
		{
			this.bits = bits.ToArray();
		}
		
		public string Code => BitManager.Instance.Bits.BitSetToCode(this);
		
		public override string ToString() => Bits?.ToDelimString();
		public override int GetHashCode() => Bits != null ? Bits.GetHashCode() : 0;
		public override bool Equals(object obj) => obj is BitSet other && this == other;
		public bool Equals(BitSet other) => this == other;
		
		public static BitSet operator |(BitSet a, BitSet b) => new(a.Union(b));
		public static BitSet operator &(BitSet a, BitSet b) => new(a.Intersect(b));
		public static BitSet operator ^(BitSet a, BitSet b) => new(a.Except(b).Union(b.Except(a)));
		public static BitSet operator -(BitSet a, BitSet b) => new(a.Except(b));
		public static bool operator ==(BitSet a, BitSet b) => new HashSet<Bit>(a).SetEquals(b);
		public static bool operator !=(BitSet a, BitSet b) => !(a == b);
		
		public static implicit operator BitSet(Bit bit) => new(new []{bit});
		
		public static TaggedBitSet operator +(BitSet a, IEnumerable<Tag> b) => new(a, b);
		public static TaggedBitSet operator +(BitSet a, Tag b) => a + ExtensionMethods.EnumerableOf(b);
		
		public bool Has(Bit other) => Bits.Contains(other);
		public bool Has(BitSet other) => (this & other) == other;
		
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
			return uniqueBits.Bits.Length - commonBits.Bits.Length;
		}
		
		public IEnumerator GetEnumerator() => Bits.GetEnumerator();
		IEnumerator<Bit> IEnumerable<Bit>.GetEnumerator() => Bits.Cast<Bit>().GetEnumerator();
	}
}
