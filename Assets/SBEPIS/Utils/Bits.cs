using System;
using UnityEngine;

namespace SBEPIS.Bits
{
	// Unity only supports 32-bit enums :/
	// Unity also only supports signed ints, so you gotta use the first 30 bits, plus one more to make sure selecting everything doesn't tip over to -1

	[Flags]
	public enum WeaponTypeBits : uint
	{
		None = 0,
		DUMMY = 1 << 30,
		All = 63,

		Handled = 1 << 0,
		Pointed = 1 << 1,
		Edged = 1 << 2,
		Explosive = 1 << 3,
		Launched = 1 << 4,
		Thrown = 1 << 5,
	}

	[Flags]
	public enum WeaponUseBits : uint
	{
		None = 0,
		DUMMY = 1 << 30,
		All = 63,

		Aerated = 1 << 0,
		Pound = 1 << 1,
		Luck = 1 << 2,
		Aquatic = 1 << 3,
		Loaded = 1 << 4,
		UNUSED = 1 << 5,
	}

	[Flags]
	public enum EquipmentBits : uint
	{
		None = 0,
		DUMMY = 1 << 30,
		All = 63,

		Head = 1 << 0,
		Face = 1 << 1,
		Torso = 1 << 2,
		Back = 1 << 3,
		Hands = 1 << 4,
		Feet = 1 << 5,
	}

	[Flags]
	public enum ElementalBits : uint
	{
		None = 0,
		DUMMY = 1 << 30,
		All = 63,

		Wet = 1 << 0,
		Enflamed = 1 << 1,
		Frozen = 1 << 2,
		Electrical = 1 << 3,
		Magic = 1 << 4,
		Luminescent = 1 << 5,
	}

	[Flags]
	public enum MultiplicityBits : uint
	{
		None = 0,
		DUMMY = 1 << 30,
		All = 63,

		Bifurcated = 1 << 0,
		Doubled = 1 << 1,
		Halved = 1 << 2,
		Mirrored = 1 << 3,
		Collection = 1 << 4,
		Secondary = 1 << 5,
	}

	[Flags]
	public enum StructureBits : uint
	{
		None = 0,
		DUMMY = 1 << 30,
		All = 63,

		Corded = 1 << 0,
		Platform = 1 << 1,
		Hollow = 1 << 2,
		Legged = 1 << 3,
		Mechanized = 1 << 4,
		Housing = 1 << 5,
	}

	[Flags]
	public enum InteractionBits : uint
	{
		None = 0,
		DUMMY = 1 << 30,
		All = 63,

		Viewing = 1 << 0,
		Interactable = 1 << 1,
		Informative = 1 << 2,
		Living = 1 << 3,
		Musical = 1 << 4,
		Edible = 1 << 5,
	}

	[Flags]
	public enum MaterialBits : uint
	{
		None = 0,
		DUMMY = 1 << 30,
		All = 63,

		Paper = 1 << 0,
		Attractive = 1 << 1,
		Conductive = 1 << 2,
		UNUSED1 = 1 << 3,
		UNUSED2 = 1 << 4,
		UNUSED3 = 1 << 5,
	}

	[Serializable]
	public struct BitSet
	{
		public static readonly BitSet Nothing = (BitSet)0UL;
		public static readonly BitSet Everything = (BitSet)~0UL;

		[SerializeField]
		private WeaponTypeBits weaponTypeBits;
		[SerializeField]
		private WeaponUseBits weaponUseBits;
		[SerializeField]
		private EquipmentBits equipmentBits;
		[SerializeField]
		private ElementalBits elementalBits;
		[SerializeField]
		private MultiplicityBits multiplicityBits;
		[SerializeField]
		private StructureBits structureBits;
		[SerializeField]
		private InteractionBits interactionBits;
		[SerializeField]
		private MaterialBits materialBits;

		private BitSet(WeaponTypeBits weaponTypeBits,
			WeaponUseBits weaponUseBits,
			EquipmentBits equipmentBits,
			ElementalBits elementalBits,
			MultiplicityBits multiplicityBits,
			StructureBits structureBits,
			InteractionBits interactionBits,
			MaterialBits materialBits)
		{
			this.weaponTypeBits = weaponTypeBits;
			this.weaponUseBits = weaponUseBits;
			this.equipmentBits = equipmentBits;
			this.elementalBits = elementalBits;
			this.multiplicityBits = multiplicityBits;
			this.structureBits = structureBits;
			this.interactionBits = interactionBits;
			this.materialBits = materialBits;
		}

		public override string ToString() => $"BitSet{{ {weaponTypeBits}; {weaponUseBits}; {equipmentBits}; {elementalBits}; {multiplicityBits}; {structureBits}; {interactionBits}; {materialBits} }}";
		public override int GetHashCode() => (weaponTypeBits, weaponUseBits, equipmentBits, elementalBits, multiplicityBits, structureBits, interactionBits, materialBits).GetHashCode();
		public override bool Equals(object obj) => this == (BitSet)obj;

		public static BitSet operator |(BitSet a, BitSet b) => (BitSet)((ulong)a | (ulong)b);
		public static BitSet operator &(BitSet a, BitSet b) => (BitSet)((ulong)a & (ulong)b);
		public static BitSet operator ^(BitSet a, BitSet b) => (BitSet)((ulong)a ^ (ulong)b);
		public static BitSet operator ~(BitSet a) => (BitSet)~(ulong)a;
		public static bool operator ==(BitSet a, BitSet b) => (ulong)a == (ulong)b;
		public static bool operator !=(BitSet a, BitSet b) => (ulong)a != (ulong)b;

		public static explicit operator ulong(BitSet a)
		{
			return ((ulong)a.weaponTypeBits)	  |
				((ulong)a.weaponUseBits)	<< 6  |
				((ulong)a.equipmentBits)	<< 12 |
				((ulong)a.elementalBits)	<< 18 |
				((ulong)a.multiplicityBits)	<< 24 |
				((ulong)a.structureBits)	<< 30 |
				((ulong)a.interactionBits)	<< 36 |
				((ulong)a.materialBits)		<< 42 ;
		}
		public static explicit operator BitSet(ulong a)
		{
			return new BitSet(
				(WeaponTypeBits)	 a,
				(WeaponUseBits)		(a >> 6),
				(EquipmentBits)		(a >> 12),
				(ElementalBits)		(a >> 18),
				(MultiplicityBits)	(a >> 24),
				(StructureBits)		(a >> 30),
				(InteractionBits)	(a >> 36),
				(MaterialBits)		(a >> 42)
			);
		}

		public static explicit operator string(BitSet a)
		{
			string code = "";
			for (int i = 0; i < 8; i++)
				code += CaptureCodeUtils.GetCaptureChar(a, i);
			return code;
		}
		public static explicit operator BitSet(string a)
		{
			if (a.Length != 8)
				throw new ArgumentException("Captcha code must have 8 characters");

			ulong bits = 0;
			for (int i = 0; i < 8; i++)
			{
				if (Array.IndexOf(CaptureCodeUtils.hashCharacters, a[i]) == -1)
					throw new ArgumentException("Captcha code contains illegal characters");

				bits |= (1UL << i * 6) * (ulong)Array.IndexOf(CaptureCodeUtils.hashCharacters, a[i]);
			}
			return (BitSet)bits;
		}

		public static implicit operator BitSet(WeaponTypeBits a)	=> new BitSet(a, 0, 0, 0, 0, 0, 0, 0);
		public static implicit operator BitSet(WeaponUseBits a)		=> new BitSet(0, a, 0, 0, 0, 0, 0, 0);
		public static implicit operator BitSet(EquipmentBits a)		=> new BitSet(0, 0, a, 0, 0, 0, 0, 0);
		public static implicit operator BitSet(ElementalBits a)		=> new BitSet(0, 0, 0, a, 0, 0, 0, 0);
		public static implicit operator BitSet(MultiplicityBits a)	=> new BitSet(0, 0, 0, 0, a, 0, 0, 0);
		public static implicit operator BitSet(StructureBits a)		=> new BitSet(0, 0, 0, 0, 0, a, 0, 0);
		public static implicit operator BitSet(InteractionBits a)	=> new BitSet(0, 0, 0, 0, 0, 0, a, 0);
		public static implicit operator BitSet(MaterialBits a)		=> new BitSet(0, 0, 0, 0, 0, 0, 0, a);

		public bool Has(BitSet other) => (this & other) == other;
	}

	public static class CaptureCodeUtils
	{
		public const ulong oneCharacterMask = (1L << 6) - 1;
		public const ulong allBitsMask = (1L << 48) - 1;

		/// <summary>
		/// The canonical order that the characters go in, with A as no punches and / as all 6 punches
		/// </summary>
		public static readonly char[] hashCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();

		/// <summary>
		/// Return a single digit of a captcha code
		/// </summary>
		public static int GetCaptureDigit(BitSet bits, int i)
		{
			return (int)(((ulong)bits >> 6 * i) & oneCharacterMask);
		}

		/// <summary>
		/// Return a single digit of a captcha code as a fraction between 0 ("0") and 1 ("!")
		/// </summary>
		public static float GetCapturePercent(BitSet bits, int i)
		{
			return GetCaptureDigit(bits, i) / 63f;
		}

		/// <summary>
		/// Return a single digit of a captcha code as a character
		/// </summary>
		public static char GetCaptureChar(BitSet bits, int i)
		{
			return hashCharacters[GetCaptureDigit(bits, i)];
		}

		/// <summary>
		/// Return a single bit of a captcha code
		/// </summary>
		public static bool GetCaptureBit(BitSet bits, int i)
		{
			return ((ulong)bits & (1UL << i)) != 0;
		}

		/// <summary>
		/// Return a seed that can be used in randomness based on a capture code
		/// </summary>
		public static float GetCaptureSeed(BitSet bits)
		{
			float seed = 0;
			if ((ulong)bits != 0)
				for (int i = 0; i < 8; i++)
					seed += Mathf.Pow(10f, i - 4) * GetCaptureDigit(bits, i);
			return seed;
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

			ulong baseMask = (ulong)baseBits;
			ulong appliedMask = (ulong)appliedBits;

			ulong uniqueMask = (appliedMask ^ baseMask) & appliedMask;
			ulong commonMask = appliedMask & baseMask;
			int score = 0;
			for (int i = 0; i < 64; i++)
				if (((uniqueMask >> i) & 1) == 1)
					score++;
				else if (((commonMask >> i) & 1) == 1)
					score--;
			return score;
		}
	}
}