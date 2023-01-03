using System;
using UnityEngine;

namespace SBEPIS.Bits
{
	[CreateAssetMenu]
	public class Bit : ScriptableObject
	{
		[SerializeField]
		private string _bitName;
		public string bitName => _bitName;

		public override string ToString() => _bitName;

		public static Bit New(string bitName)
		{
			Bit bit = CreateInstance<Bit>();
			bit._bitName = bitName;
			return bit;
		}
	}
}