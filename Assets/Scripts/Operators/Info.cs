using System;
using TMPro;
using UnityEngine;

public class Info : Operator1
{
	public Dowel dowel;
	public TextMeshProUGUI itemName;
	public TextMeshProUGUI captchaCode;

	protected override CaptchalogueCard Operate(CaptchalogueCard card)
	{
		dowel.captchaHash = card.punchedHash != 0 ? card.punchedHash : card.heldItem ? card.heldItem.itemType.captchaHash : 0;
		ItemType.itemTypes.TryGetValue(dowel.captchaHash, out ItemType itemType);
		for (int i = 0; i < 8; i++)
		{
			int charIndex = Array.IndexOf(ItemType.hashCharacters, itemType.captchaCode[i]);
			dowel.renderer.SetBlendShapeWeight(i, (1f - charIndex / 63f) * 100);
		}

		if (itemType)
		{
			itemName.text = itemType.itemName;
			captchaCode.text = itemType.captchaCode;
		}
		else
		{
			itemName.text = "Unexpected item in bagging area";
			captchaCode.text = "JDbSuprm";
		}

		return null;
	}
}
