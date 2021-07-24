using UnityEngine;

public class Punch : Operator1
{
	protected override CaptchalogueCard Operate(CaptchalogueCard card)
	{
		if (card.heldItem)
		{
			CaptchalogueCard newCard = Instantiate(cardPrefab);
			newCard.Punch(card.heldItem ? card.heldItem.itemType.captchaHash : 0);
			return newCard;
		}
		else
		{
			ItemType.itemTypes.TryGetValue(card.punchedHash, out ItemType itemType);
			if (itemType)
				Instantiate(itemType.prefab, Vector3.up, itemType.prefab.GetComponent<CaptchalogueCard>() ? Quaternion.Euler(90, 0, 0) : Quaternion.identity);
			return null;
		}
	}
}
