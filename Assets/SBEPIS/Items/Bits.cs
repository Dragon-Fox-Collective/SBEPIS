using System;
using UnityEngine;

namespace SBEPIS.Bits.Bits
{
	[Flags]
	public enum Bits1 : uint
	{
		Handled = 1 << 0,
		Pointed = 1 << 1,
		Edged = 1 << 2,
		Explosive = 1 << 3,
		Launched = 1 << 4,
		Thrown = 1 << 5,

		Aerated = 1 << 8,
		Pound = 1 << 9,
		Luck = 1 << 10,
		Aquatic = 1 << 11,
		Loaded = 1 << 12,
		UNUSED = 1 << 13,

		Head = 1 << 16,
		Face = 1 << 17,
		Torso = 1 << 18,
		Back = 1 << 19,
		Hands = 1 << 20,
		Feet = 1 << 21,

		Wet = 1 << 24,
		Enflamed = 1 << 25,
		Frozen = 1 << 26,
		Electrical = 1 << 27,
		Magic = 1 << 28,
		Luminescent = 1 << 29,
	}

	[Flags]
	public enum Bits2 : uint
	{
		Bifurcated = 1 << 0,
		Doubled = 1 << 1,
		Halved = 1 << 2,
		Mirrored = 1 << 3,
		Collection = 1 << 4,
		Secondary = 1 << 5,

		Corded = 1 << 8,
		Platform = 1 << 9,
		Hollow = 1 << 10,
		Legged = 1 << 11,
		Mechanized = 1 << 12,
		Housing = 1 << 13,

		Viewing = 1 << 16,
		Interactable = 1 << 17,
		Informative = 1 << 18,
		Living = 1 << 19,
		Musical = 1 << 20,
		Edible = 1 << 21,

		Paper = 1 << 24,
		Attractive = 1 << 25,
		Conductive = 1 << 26,
		UNUSED1 = 1 << 27,
		UNUSED2 = 1 << 28,
		UNUSED3 = 1 << 29,
	}
}