using UnityEngine;

namespace WrightWay.SBEPIS.Operators
{
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
					Instantiate(itemType.prefab, Vector3.up * 2, Quaternion.identity);
				return null;
			}
		}
	}
}