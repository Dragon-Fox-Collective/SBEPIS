using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Bits
{
	[CreateAssetMenu]
	public class Bit : ScriptableObject
	{
		[FormerlySerializedAs("_bitName")]
		[SerializeField] private string bitName;
		public string BitName => bitName;
		
		public override string ToString() => bitName;
		
		public static Bit New(string bitName)
		{
			Bit bit = CreateInstance<Bit>();
			bit.bitName = bitName;
			return bit;
		}
		
		public static BitSet operator |(Bit a, Bit b) => new(EnumerableOf.Of(a, b));
	}
}