using System;
using System.Collections.Generic;
using UnityEngine;

namespace WrightWay.SBEPIS
{
	[CreateAssetMenu]
	public class ItemType : ScriptableObject
	{
		public static readonly Dictionary<long, ItemType> itemTypes = new Dictionary<long, ItemType>();
		private static readonly Dictionary<long, Texture2D> captchaTextures = new Dictionary<long, Texture2D>();

		public string itemName;
		public string captchaCode;
		public Item prefab;

		public long captchaHash { get; private set; }
		/*private Texture2D _icon;
		public Texture2D icon {
			get
			{
				if (!_icon)
					_icon = FindObjectOfType<Captcharoid>().Captcha(this);
				return _icon;
			}
		}*/

		private void OnEnable()
		{
			if (captchaCode != null)
			{
				captchaHash = hashCaptcha(captchaCode);
				itemTypes.Add(captchaHash, this);
			}
		}

		public static readonly char[] hashCharacters =
		{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
		'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
		'?', '!'
	};

		public static long hashCaptcha(string captchaCode)
		{
			if (captchaCode.Length != 8)
				throw new ArgumentException("Captcha code must have 8 characters");

			long hash = 0;
			for (int i = 0; i < 8; i++)
			{
				if (Array.IndexOf(hashCharacters, captchaCode[i]) == -1)
					throw new ArgumentException("Captcha code contains illegal characters");

				hash |= (1L << i * 6) * Array.IndexOf(hashCharacters, captchaCode[i]);
			}
			return hash;
		}

		public static string unhashCaptcha(long captchaHash)
		{
			if ((captchaHash & ~((1L << 48) - 1L)) != 0)
				throw new ArgumentException("Captcha hash is too big of a value");

			string code = "";
			for (int i = 0; i < 8; i++)
				code += hashCharacters[(captchaHash >> 6 * i) & ((1L << 6) - 1)];
			return code;
		}

		public static Texture2D GetCaptchaTexture(long captchaHash)
		{
			if (!captchaTextures.ContainsKey(captchaHash))
				captchaTextures.Add(captchaHash, GameManager.instance.captcharoid.Captcha(captchaHash));

			captchaTextures.TryGetValue(captchaHash, out Texture2D texture);
			return texture;
		}
	}
}