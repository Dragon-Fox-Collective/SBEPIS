using System.Collections.Generic;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Bits
{
	[CreateAssetMenu(fileName=nameof(BitManager))]
	public class BitManager : ScriptableSingleton<BitManager>
	{
		[SerializeField]
		private string _hashCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
		private char[] hashCharactersArray;
		private char[] hashCharacters => hashCharactersArray ??= _hashCharacters.ToCharArray();
		
		[SerializeField]
		private List<Bit> _bits;

		private BitList bitList;
		public BitList bits => bitList ??= new BitList(_bits, hashCharacters);

		private void OnValidate()
		{
			hashCharactersArray = null;
			bitList = null;
		}
	}
}