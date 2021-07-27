using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemLathe : MonoBehaviour // TODO: Screens
{
	public PlacementHelper card1Placement, card2Placement, dowelPlacement;
	public SkinnedMeshRenderer chisel;
	public Animator animator;
	public float latheProgress;

	private bool isLathing;
	private long captchaHash;

	private void Update()
	{
		if (isLathing)
		{
			string captchaCode = ItemType.unhashCaptcha(captchaHash);
			for (int i = 0; i < 8; i++)
			{
				int charIndex = Array.IndexOf(ItemType.hashCharacters, captchaCode[i]);
				float sliceChiselPercent = charIndex / 63f;
				float startLatheProgress = 1f - sliceChiselPercent;
				if (latheProgress >= startLatheProgress)
					dowelPlacement.item.GetComponent<Dowel>().renderer.SetBlendShapeWeight(i, (latheProgress - startLatheProgress) * 100);
			}
			if (latheProgress == 1)
				isLathing = false;
		}
	}

	public void SetCaptchaHash()
	{
		if (!card1Placement.item)
			captchaHash = 0;
		else if (!card2Placement.item)
			captchaHash = card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash;
		else
			captchaHash = card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash & card2Placement.item.GetComponent<CaptchalogueCard>().punchedHash;
	}

	public void StartLathing()
	{
		animator.SetBool("Adopted", true);
	}

	public void StopLathing()
	{
		animator.SetBool("Adopted", false);
	}

	public void Lathe()
	{
		SetCaptchaHash();
		string captchaCode = ItemType.unhashCaptcha(captchaHash);
		for (int i = 0; i < 8; i++)
		{
			int charIndex = Array.IndexOf(ItemType.hashCharacters, captchaCode[i]);
			chisel.SetBlendShapeWeight(i, (1f - charIndex / 63f) * 100);
		}
		isLathing = true;
		dowelPlacement.item.GetComponent<Dowel>().captchaHash = captchaHash;
	}

	public void CardInFirst()
	{
		card1Placement.AllowOrphan();
		card2Placement.collider.enabled = true;
		animator.SetInteger("Cards", 1);
	}
	public void CardOutFirst()
	{
		card2Placement.collider.enabled = false;
		animator.SetInteger("Cards", 0);
	}

	public void CardInSecond()
	{
		card1Placement.DisallowOrphan();
		card2Placement.AllowOrphan();
		animator.SetInteger("Cards", 2);
	}

	public void CardOutSecond()
	{
		card1Placement.AllowOrphan();
		animator.SetInteger("Cards", 1);
	}

	public void AllowOrphan()
	{
		dowelPlacement.AllowOrphan();
	}
}
