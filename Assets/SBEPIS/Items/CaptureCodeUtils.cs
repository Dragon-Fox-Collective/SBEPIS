using System;
using UnityEngine;

namespace SBEPIS.Bits
{
	public static class CaptureCodeUtils
	{
		public const byte BYTE_MASK = 0x3F;
		public const ulong INT_MASK = 0x3F3F3F3F;
		public const ulong LONG_MASK = 0x3F3F3F3F3F3F3F3F;

		/// <summary>
		/// The canonical order that the characters go in, with A as no punches and / as all 6 punches
		/// </summary>
		public static readonly char[] hashCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();

		/// <summary>
		/// Return a single digit of a captcha code
		/// </summary>
		public static byte GetCaptureDigit(BitSet bits, int i) => (byte)(((ulong)bits >> 8 * i) & BYTE_MASK);

		/// <summary>
		/// Return a single digit of a captcha code as a fraction between 0 ("0") and 1 ("!")
		/// </summary>
		public static float GetCapturePercent(BitSet bits, int i) => GetCaptureDigit(bits, i) / 63f;

		/// <summary>
		/// Return a single digit of a captcha code as a character
		/// </summary>
		public static char GetCaptureChar(BitSet bits, int i) => hashCharacters[GetCaptureDigit(bits, i)];

		/// <summary>
		/// Return a Bit with the ith bit on
		/// </summary>
		public static BitSet GetCapturePlacement(int i) =>  (BitSet)(1UL << (i / 6 * 8 + i % 6));

		/// <summary>
		/// Return a single bit of a captcha code
		/// </summary>
		public static bool GetCaptureBit(BitSet bits, int i) => (bits & GetCapturePlacement(i)) != BitSet.NOTHING;

		/// <summary>
		/// Return a seed that can be used in randomness based on a capture code
		/// </summary>
		public static float GetCaptureSeed(BitSet bits)
		{
			float seed = 0;
			if (bits != BitSet.NOTHING)
				for (int i = 0; i < 8; i++)
					seed += Mathf.Pow(10f, i - 4) * GetCaptureDigit(bits, i);
			return seed;
		}

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

		public static string ToCode(this BitSet bits)
		{
			string code = "";
			for (int i = 0; i < 8; i++)
				code += GetCaptureChar(bits, i);
			return code;
		}

		/// <summary>
		/// Return a score based on the uniqueness of the applied bits on the base bits
		/// </summary>
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
