using NUnit.Framework;
using SBEPIS.Bits;
using System;

namespace SBEPIS.Tests.EditMode
{
	public class CaptureCodeTests
	{
		private const string code = "sk9+LB/A";
		private const ulong bitMask = 0b_00000000_00111111_00000001_00001011_00111110_00111101_00100100_00101100;
		private static readonly BitSet bits = (BitSet)bitMask;

		[Test]
		public void CodeBecomesBits()
		{
			Assert.AreEqual(bits, CaptureCodeUtils.FromCode(code));
		}

		[Test]
		public void BitsBecomeCode()
		{
			Assert.AreEqual(code, bits.ToCode());
		}

		[Test]
		public void BitsBecomeADigit()
		{
			Assert.AreEqual(63, CaptureCodeUtils.GetCaptureDigit(bits, 6));
		}

		[Test]
		public void BitsBecomeAPercent()
		{
			Assert.AreEqual(1, CaptureCodeUtils.GetCapturePercent(bits, 6));
		}

		[Test]
		public void BitsBecomeACharacter()
		{
			Assert.AreEqual('/', CaptureCodeUtils.GetCaptureChar(bits, 6));
		}

		[Test]
		public void BitsBecomeABoolean()
		{
			Assert.AreEqual(true, CaptureCodeUtils.GetCaptureBit(bits, 3));
		}

		[Test]
		public void BitMaskCastsToBits()
		{
			Assert.AreEqual(bits, (BitSet)bitMask);
		}

		[Test]
		public void BitsCastToBitMask()
		{
			Assert.AreEqual(bitMask, (ulong)bits);
		}

		[Test]
		public void BitsAnd()
		{
			Assert.AreEqual((BitSet)0b100, bits & (BitSet)0b110);
		}

		[Test]
		public void BitsOr()
		{
			Assert.AreEqual((BitSet)(bitMask | 0b110), bits | (BitSet)0b110);
		}

		[Test]
		public void BitsXor()
		{
			Assert.AreEqual((BitSet)0b010, bits ^ (BitSet)(bitMask | 0b110));
		}

		[Test]
		public void BitsNegate()
		{
			Assert.AreEqual((BitSet)~bitMask, ~bits);
		}

		[Test]
		public void BitsNegateLongWise()
		{
			Assert.AreEqual(Convert.ToString(~(long)bitMask, 2), Convert.ToString((long)(ulong)~bits, 2));
		}

		[Test]
		public void BitsEqualBits()
		{
			Assert.AreEqual(bits, ~~bits);
		}

		[Test]
		public void BitsDoNotEqualBits()
		{
			Assert.AreNotEqual(bits, ~bits);
		}

		[Test]
		public void BitsHaveUniqueness()
		{
			Assert.AreEqual(1, CaptureCodeUtils.GetUniquenessScore(bits, (BitSet)0b111));
		}
	}
}
