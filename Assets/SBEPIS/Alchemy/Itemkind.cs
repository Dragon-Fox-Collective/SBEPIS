using SBEPIS.Bits;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Alchemy
{
	[CreateAssetMenu]
	public class Itemkind : ScriptableObject
	{
		public string itemName;
		public string captchaCode;
		public Item prefab;

		/// <summary>
		/// You might be seeing this everywhere lol
		/// Captchalogue cards have 48 bits. This is enough bits to be stored as a number in a long. So that's how we're storing them. As storage efficient as you can get
		/// </summary>
		public BitSet bits { get; private set; }

		public static readonly Dictionary<BitSet, Itemkind> itemkinds = new Dictionary<BitSet, Itemkind>();

		private void OnEnable()
		{
			if (captchaCode == null || captchaCode.Length == 0)
				for (int i = 0; i < 8; i++)
					captchaCode += CaptureCodeUtils.hashCharacters[Random.Range(0, CaptureCodeUtils.hashCharacters.Length)];
			bits = (BitSet)captchaCode;
			itemkinds.Add(bits, this);
		}
	}
}