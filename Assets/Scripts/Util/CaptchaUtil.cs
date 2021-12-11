using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrightWay.SBEPIS.Util
{
	public class CaptchaUtil : MonoBehaviour
	{
		private static readonly Dictionary<long, Texture2D> captchaTextures = new Dictionary<long, Texture2D>();

		/// <summary>
		/// The canonical order that the characters go in, with 0 is no punches and ! is all 6 punches
		/// </summary>
		public static readonly char[] hashCharacters =
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
			'?', '!'
		};

		/// <summary>
		/// Turn a captcha code into its hashed number version
		/// </summary>
		public static long HashCaptcha(string captchaCode)
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

		/// <summary>
		/// Turn a hashed captcha code into its unhashed string version
		/// </summary>
		public static string UnhashCaptcha(long captchaHash)
		{
			if (captchaHash == -1)
				return null;

			if ((captchaHash & ~((1L << 48) - 1L)) != 0)
				throw new ArgumentException("Captcha hash is too big of a value");

			string code = "";
			for (int i = 0; i < 8; i++)
				code += hashCharacters[(captchaHash >> 6 * i) & ((1L << 6) - 1)];
			return code;
		}

		/// <summary>
		/// Return a single digit of a captcha code
		/// </summary>
		public static int GetCaptchaDigit(long captchaHash, int i)
		{
			return (int) ((captchaHash >> 6 * i) & ((1L << 6) - 1));
		}

		/// <summary>
		/// Return a single digit of a captcha code as a fraction between 0 ("0") and 1 ("!")
		/// </summary>
		public static float GetCaptchaPercent(long captchaHash, int i)
		{
			return GetCaptchaDigit(captchaHash, i) / 63f;
		}

		/// <summary>
		/// Return a single digit of a captcha code as a character
		/// </summary>
		public static char GetCaptchaChar(long captchaHash, int i)
		{
			return hashCharacters[GetCaptchaDigit(captchaHash, i)];
		}

		/// <summary>
		/// Look up or generate a Texture2D of a captcha code string
		/// </summary>
		public static Texture2D GetCaptchaTexture(long captchaHash)
		{
			if (!captchaTextures.ContainsKey(captchaHash))
				captchaTextures.Add(captchaHash, GameManager.instance.captcharoid.Captcha(captchaHash));

			captchaTextures.TryGetValue(captchaHash, out Texture2D texture);
			return texture;
		}
	}
}