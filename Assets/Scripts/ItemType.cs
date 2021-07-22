using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemType : ScriptableObject
{
	public static readonly Dictionary<long, ItemType> itemTypes = new Dictionary<long, ItemType>();

	public string captchaCode;
	public long captchaHash { get; private set; }

	private void OnEnable()
	{
		captchaHash = hashCaptcha(captchaCode);
		itemTypes.Add(captchaHash, this);
	}

	private static readonly char[] hashCharacters =
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
		'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
		'?', '!'
	};

	public static long hashCaptcha(string captchaCode)
	{
		if (captchaCode.Length != 8)
			throw new ArgumentException("Captcha code must have 8 characters", nameof(captchaCode));

		long hash = 0;
		for (int i = 0; i < 8; i++)
		{
			if (Array.IndexOf(hashCharacters, captchaCode[i]) == -1)
				throw new ArgumentException("Captcha code contains illegal characters", nameof(captchaCode));

			hash |= (1L << i * 6) * Array.IndexOf(hashCharacters, captchaCode[i]);
		}
		return hash;
	}

	public static string unhashCaptcha(long captchaHash)
	{
		if ((captchaHash & ~((1L << 48) - 1L)) != 0)
			throw new ArgumentException("Captcha hash is too big of a value", nameof(captchaHash));

		string code = "";
		for (int i = 0; i < 8; i++)
			code += hashCharacters[(captchaHash >> 6 * i) & ((1L << 6) - 1)];
		return code;
	}
}
