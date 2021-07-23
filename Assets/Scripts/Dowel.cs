using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dowel : MonoBehaviour
{
	public Transform slice0;
	public Transform slice1;
	public Transform slice2;
	public Transform slice3;
	public Transform slice4;
	public Transform slice5;
	public Transform slice6;
	public Transform slice7;

	private Transform[] slices = new Transform[8];

	private long _captchaHash;
	public long captchaHash
	{
		get => _captchaHash;
		set
		{
			_captchaHash = value;

			string captchaCode = ItemType.unhashCaptcha(captchaHash);
			for (int i = 0; i < 8; i++)
			{
				int charIndex = Array.IndexOf(ItemType.hashCharacters, captchaCode[i]);
				slices[i].localScale = new Vector3(1f - charIndex / 64f, 0.1f, 1f - charIndex / 64f);
			}
		}
	}

	private void Awake()
	{
		slices[0] = slice0;
		slices[1] = slice1;
		slices[2] = slice2;
		slices[3] = slice3;
		slices[4] = slice4;
		slices[5] = slice5;
		slices[6] = slice6;
		slices[7] = slice7;
	}
}
