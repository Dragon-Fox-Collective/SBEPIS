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

		public const byte BYTE_MASK = 0x3F;
		public const ulong INT_MASK = 0x3F3F3F3F;
		public const ulong LONG_MASK = 0x3F3F3F3F3F3F3F3F;

		public static readonly char[] hashCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();

		[SerializeField]
		private Bits1 bits1;
		[SerializeField]
		private Bits2 bits2;

		public BitSet(Bits1 bits1, Bits2 bits2)
		{
			this.bits1 = (Bits1)((ulong)bits1 & INT_MASK);
			this.bits2 = (Bits2)((ulong)bits2 & INT_MASK);
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

		public MemberedBitSet With(Member[] members) => new(this, members);
		public MemberedBitSet With(Member[] a, Member[] b) => With(MemberAppender.Append(a, b));

		public bool Has(BitSet other) => (this & other) == other;

		public byte DigitAt(int i) => (byte)(((ulong)this >> 8 * i) & BYTE_MASK);
		public float PercentAt(int i) => DigitAt(i) / 63f;
		public char CharAt(int i) => hashCharacters[DigitAt(i)];
		public bool BitAt(int i) => (this & FromBit(i)) != NOTHING;

		/// <summary>
		/// Return a Bit with the ith bit on
		/// </summary>
		public static BitSet FromBit(int i) => (BitSet)(1UL << (i / 6 * 8 + i % 6));
		
		public static BitSet FromCode(string code)
		{
			if (code.Length != 8)
				throw new ArgumentException("Captcha code must have 8 characters");

			BitSet bits = BitSet.NOTHING;
			for (int i = 0; i < 8; i++)
			{
				if (Array.IndexOf(hashCharacters, code[i]) == -1)
					throw new ArgumentException("Captcha code contains illegal characters");

				bits |= (BitSet)((ulong)Array.IndexOf(hashCharacters, code[i]) << i * 8);
			}
			return bits;
		}

		public string ToCode()
		{
			string code = "";
			for (int i = 0; i < 8; i++)
				code += CharAt(i);
			return code;
		}

		public float Seed
		{
			get
			{
				float seed = 0;
				if (this != BitSet.NOTHING)
					for (int i = 0; i < 8; i++)
						seed += Mathf.Pow(10f, i - 4) * DigitAt(i);
				return seed;
			}
		}

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
			int score = 0;
			for (int i = 0; i < 64; i++)
				if ((((ulong)uniqueBits >> i) & 1) == 1)
					score++;
				else if ((((ulong)commonBits >> i) & 1) == 1)
					score--;
			return score;
		}
	}
}
