using NUnit.Framework;
using SBEPIS.Bits;
using System;
using UnityEngine;

namespace SBEPIS.Tests.EditMode
{
	public class BitTests
	{
		private static readonly Bit Handled = new("Handled");
		private static readonly Bit Pound = new("Pound");
		private static readonly Bit Aerated = new("Aerated");
		private static readonly Bit Musical = new("Musical");
		private static readonly char[] HashCharacters = "AB".ToCharArray();
		private static readonly BitList BitsList = new(new[]{ Handled, Pound, Aerated, Musical }, HashCharacters);
		private static readonly BitSet Bits = new(new[]{ Handled, Aerated });
		private static readonly BitSet OtherBits = new(new[]{ Aerated, Musical });

		[Test]
		public void BitsEqualInOrder()
		{
			Assert.AreEqual(new BitSet(new[]{ Handled, Aerated }), Bits);
		}
		
		[Test]
		public void BitsEqualOutOfOrder()
		{
			Assert.AreEqual(new BitSet(new[]{ Aerated, Handled }), Bits);
		}
		
		[Test]
		public void BitsAnd()
		{
			Assert.AreEqual(new BitSet(new[]{ Aerated }), Bits & OtherBits);
		}
		
		[Test]
		public void BitsOr()
		{
			Assert.AreEqual(new BitSet(new[]{ Handled, Aerated, Musical }), Bits | OtherBits);
		}
		
		[Test]
		public void BitsXor()
		{
			Assert.AreEqual(new BitSet(new[]{ Handled, Musical }), Bits ^ OtherBits);
		}
		
		[Test]
		public void BitsMinus()
		{
			Assert.AreEqual(new BitSet(new[]{ Handled }), Bits - OtherBits);
		}
		
		[Test]
		public void BitsNot()
		{
			Assert.AreEqual(new BitSet(new[]{ Pound, Musical }), BitsList - Bits);
		}

		[Test]
		public void BitsNumBitsInCharacter()
		{
			Assert.AreEqual(1, BitsList.numBitsInCharacterGeneral);
		}
		
		[Test]
		public void BitsNumCharactersInCode()
		{
			Assert.AreEqual(4, BitsList.numCharactersInCode);
		}
		
		[Test]
		public void BitsIsBitSetBitAt()
		{
			Assert.AreEqual(true, BitsList.IsBitSetBitAt(Bits, 0));
			Assert.AreEqual(false, BitsList.IsBitSetBitAt(Bits, 1));
			Assert.AreEqual(true, BitsList.IsBitSetBitAt(Bits, 2));
			Assert.AreEqual(false, BitsList.IsBitSetBitAt(Bits, 3));
		}

		[Test]
		public void BitsToCode()
		{
			Assert.AreEqual("BABA", BitsList.BitSetToCode(Bits));
		}
		
		[Test]
		public void BitsFromCode()
		{
			Assert.AreEqual(Bits, BitsList.BitSetFromCode("BABA"));
		}
	}
}
