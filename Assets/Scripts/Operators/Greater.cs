using System;

namespace WrightWay.SBEPIS.Operators
{
	/// <summary>
	/// Non-canon. Blank wanted to know what would happen if you used two cards on the same dowel.
	/// This assumes the dowels canonically have 8 segments corresponding to the 8 captcha digits.
	/// </summary>
	public class Greater : Operator2
	{
		protected override CaptchalogueCard Operate(CaptchalogueCard card1, CaptchalogueCard card2)
		{
			CaptchalogueCard newCard = Instantiate(cardPrefab);
			string captcha1 = ItemType.unhashCaptcha(card1.punchedHash);
			string captcha2 = ItemType.unhashCaptcha(card2.punchedHash);
			string captcha3 = "";
			for (int i = 0; i < 8; i++)
				captcha3 += ItemType.hashCharacters[Math.Max(Array.IndexOf(ItemType.hashCharacters, captcha1[i]), Array.IndexOf(ItemType.hashCharacters, captcha2[i]))];
			newCard.Punch(ItemType.hashCaptcha(captcha3));
			return newCard;
		}
	}
}