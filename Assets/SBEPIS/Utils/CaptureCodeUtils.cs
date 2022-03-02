using System;
using UnityEngine;

public static class CaptureCodeUtils
{
	/// <summary>
	/// The canonical order that the characters go in, with A as no punches and / as all 6 punches
	/// </summary>
	public static readonly char[] hashCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();

	/// <summary>
	/// Turn a captcha code into its hashed number version
	/// </summary>
	public static long HashCaptureCode(string captureCode)
	{
		if (captureCode.Length != 8)
			throw new ArgumentException("Captcha code must have 8 characters");

		long hash = 0;
		for (int i = 0; i < 8; i++)
		{
			if (Array.IndexOf(hashCharacters, captureCode[i]) == -1)
				throw new ArgumentException("Captcha code contains illegal characters");

			hash |= (1L << i * 6) * Array.IndexOf(hashCharacters, captureCode[i]);
		}
		return hash;
	}

	/// <summary>
	/// Turn a hashed captcha code into its unhashed string version
	/// </summary>
	public static string UnhashCaptureHash(long captureHash)
	{
		if (captureHash == -1)
			return null;

		if ((captureHash & ~((1L << 48) - 1L)) != 0)
			throw new ArgumentException("Captcha hash is too big of a value");

		string code = "";
		for (int i = 0; i < 8; i++)
			code += GetCaptureChar(captureHash, i);
		return code;
	}

	/// <summary>
	/// Return a single digit of a captcha code
	/// </summary>
	public static int GetCaptureDigit(long captureHash, int i)
	{
		return (int) ((captureHash >> 6 * i) & ((1L << 6) - 1));
	}

	/// <summary>
	/// Return a single digit of a captcha code as a fraction between 0 ("0") and 1 ("!")
	/// </summary>
	public static float GetCapturePercent(long captureHash, int i)
	{
		return GetCaptureDigit(captureHash, i) / 63f;
	}

	/// <summary>
	/// Return a single digit of a captcha code as a character
	/// </summary>
	public static char GetCaptureChar(long captureHash, int i)
	{
		return hashCharacters[GetCaptureDigit(captureHash, i)];
	}

	/// <summary>
	/// Return a single bit of a captcha code
	/// </summary>
	public static bool GetCaptureBit(long captureHash, int i)
	{
		return (captureHash & (1L << i)) != 0;
	}

	/// <summary>
	/// Return a seed that can be used in randomness based on a capture code
	/// </summary>
	public static float GetCaptureSeed(long captureHash)
	{
		float seed = 0;
		if (captureHash != 0)
			for (int i = 0; i < 8; i++)
				seed += Mathf.Pow(10f, i - 4) * GetCaptureDigit(captureHash, i);
		return seed;
	}
}