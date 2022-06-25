using SBEPIS.Bits.Bits;
using System;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public struct BitSet
	{
		public static readonly BitSet NOTHING = new(0, 0);
		public static readonly BitSet EVERYTHING = ~NOTHING;

		[SerializeField]
		private Bits1 bits1;
		[SerializeField]
		private Bits2 bits2;

		public BitSet(Bits1 bits1, Bits2 bits2)
		{
			this.bits1 = (Bits1)((ulong)bits1 & CaptureCodeUtils.INT_MASK);
			this.bits2 = (Bits2)((ulong)bits2 & CaptureCodeUtils.INT_MASK);
		}

		public override string ToString() => "BitSet{" + ((bits1 != 0 ? bits1 : "") + (bits1 != 0 && bits2 != 0 ? ", " : "") + (bits2 != 0 ? bits2 : "")) + "}";
		public override int GetHashCode() => (bits1, bits2).GetHashCode();
		public override bool Equals(object obj) => obj is BitSet set && this == set;

		public static BitSet operator |(BitSet a, BitSet b) => new(a.bits1 | b.bits1, a.bits2 | b.bits2);
		public static BitSet operator &(BitSet a, BitSet b) => new(a.bits1 & b.bits1, a.bits2 & b.bits2);
		public static BitSet operator ^(BitSet a, BitSet b) => new(a.bits1 ^ b.bits1, a.bits2 ^ b.bits2);
		public static BitSet operator ~(BitSet a) => new(~a.bits1, ~a.bits2);
		public static bool operator ==(BitSet a, BitSet b) => a.bits1 == b.bits1 && a.bits2 == b.bits2;
		public static bool operator !=(BitSet a, BitSet b) => a.bits1 != b.bits1 || a.bits2 != b.bits2;

		public static explicit operator ulong(BitSet a) => (ulong)a.bits1 | ((ulong)a.bits2 << 32);
		public static explicit operator BitSet(ulong a) => new((Bits1)a, (Bits2)(a >> 32));

		public static implicit operator BitSet(Bits1 a) => new(a, 0);
		public static implicit operator BitSet(Bits2 a) => new(0, a);

		public bool Has(BitSet other) => (this & other) == other;

		public MemberedBitSet With(Member[] members) => new(this, members);
		public MemberedBitSet With(Member[] a, Member[] b) => With(MemberAppender.Append(a, b));
	}
}
