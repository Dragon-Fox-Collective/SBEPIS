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
		public long captchaHash { get; private set; }

		public static readonly Dictionary<long, Itemkind> itemkinds = new Dictionary<long, Itemkind>();

		private void OnEnable()
		{
			if (captchaCode == null || captchaCode.Length == 0)
				for (int i = 0; i < 8; i++)
					captchaCode += CaptureCodeUtils.hashCharacters[Random.Range(0, CaptureCodeUtils.hashCharacters.Length)];
			captchaHash = CaptureCodeUtils.HashCaptureCode(captchaCode);
			itemkinds.Add(captchaHash, this);
		}
	}
}