using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Bits
{
	[Serializable]
	public class BitList : IEnumerable<Bit>
	{
		[SerializeField] private Bit[] bits;
		[SerializeField] private char[] hashCharacters;
		
		public BitList(IEnumerable<Bit> bits, char[] hashCharacters)
		{
			this.bits = bits.ToArray();
			this.hashCharacters = hashCharacters;
			//Debug.Log($"New BitList {numBitsInCharacterGeneral} {NumBitsInCharacterAt(numCharactersInCode - 1)} {numCharactersInCode} {numHashCharacters}");
		}
		
		public int NumBits => bits.Length;
		public int NumBitsInCharacterGeneral => (int)Mathf.Log(hashCharacters.Length, 2);
		public int NumBitsInCharacterAt(int codeIndex) =>
			codeIndex < 0 ? 0 :
			codeIndex < NumCharactersInCode - 1 ? NumBitsInCharacterGeneral :
			codeIndex < NumCharactersInCode ? NumBits - NumBitsInCharacterGeneral * codeIndex :
			0;
		public int NumCharactersInCode => Mathf.CeilToInt((float)NumBits / NumBitsInCharacterGeneral);
		public int NumHashCharacters => hashCharacters.Length;
		
		public int BitIndex(int codeIndex, int i) => codeIndex * NumBitsInCharacterGeneral + i;
		
		public Bit this[int index] => bits[index];
		public int IndexOf(Bit bit) => Array.IndexOf(bits, bit);
		
		public BitSet Not(BitSet bits) => new(this.Except(bits));
		
		public BitSet BitSetFromCharacter(char character, int codeIndex)
		{
			int index = Array.IndexOf(hashCharacters, character);
			if (index == -1)
				throw new ArgumentException($"Captcha code contains an illegal character {character}");
			
			return Enumerable.Range(0, NumBitsInCharacterAt(codeIndex)).Where(bitIndex => (index & (1 << bitIndex)) != 0).Aggregate(new BitSet(), (codeBits, bitIndex) => codeBits | BitSetFromBitAt(codeIndex, bitIndex));
		}
		public char BitSetCharacterAt(BitSet bits, int codeIndex) => GetHashCharacter(BitSetDigitAt(bits, codeIndex));
		
		public char GetHashCharacter(int i) => hashCharacters[i];
		
		public int BitSetDigitAt(BitSet bits, int codeIndex) => Enumerable.Range(0, NumBitsInCharacterAt(codeIndex)).Where(bitIndex => IsBitSetBitAt(bits, codeIndex, bitIndex)).Aggregate(0, (result, bitIndex) => result | (1 << bitIndex));
		public float BitSetFractionAt(BitSet bits, int codeIndex) => BitSetDigitAt(bits, codeIndex) / (NumHashCharacters - 1f);
		
		public BitSet BitSetFromBitAt(int i) => bits[i];
		public BitSet BitSetFromBitAt(int codeIndex, int i) => BitSetFromBitAt(codeIndex * NumBitsInCharacterGeneral + i);
		public bool BitSetHasBitAt(BitSet bits, int i) => bits.Has(this[i]);
		public bool IsBitSetBitAt(BitSet bits, int codeIndex, int i) => BitSetHasBitAt(bits, codeIndex * NumBitsInCharacterGeneral + i);
		
		public BitSet BitSetFromCode(string code)
		{
			if (code.Length != NumCharactersInCode)
				throw new ArgumentException($"Captcha code must have {NumCharactersInCode} characters, has {code.Length}");
			
			return Enumerable.Range(0, NumCharactersInCode).Aggregate(new BitSet(), (codeBits, i) => codeBits | BitSetFromCharacter(code[i], i));
		}
		public string BitSetToCode(BitSet bits) => Enumerable.Range(0, NumCharactersInCode).Aggregate("", (code, i) => code + BitSetCharacterAt(bits, i));
		
		public float BitSetToSeed(BitSet bits) => Enumerable.Range(0, NumCharactersInCode).Aggregate(0f, (seed, i) => seed + Mathf.Pow(10f, i - 4) * BitSetDigitAt(bits, i));
		
		public override string ToString() => $"BitList{bits.ToDelimString()}";
		
		public IEnumerator GetEnumerator() => bits.GetEnumerator();
		IEnumerator<Bit> IEnumerable<Bit>.GetEnumerator() => bits.Cast<Bit>().GetEnumerator();
	}
}