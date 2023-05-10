﻿using System.Collections.Generic;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Bits
{
	[CreateAssetMenu]
	public class BitManager : ScriptableSingleton<BitManager>
	{
		[SerializeField] private string _hashCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
		private char[] hashCharactersArray;
		private char[] HashCharacters => hashCharactersArray ??= _hashCharacters.ToCharArray();
		
		[FormerlySerializedAs("_bits")]
		[SerializeField] private Bit[] bits;
		
		private BitList bitList;
		public BitList Bits => bitList ??= new BitList(bits, HashCharacters);
		
		private void OnValidate()
		{
			hashCharactersArray = null;
			bitList = null;
		}
	}
}