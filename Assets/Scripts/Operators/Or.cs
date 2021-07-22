public class Or : Operator
{
	protected override CaptchalogueCard Operate(CaptchalogueCard card1, CaptchalogueCard card2)
	{
		CaptchalogueCard newCard = Instantiate(cardPrefab);
		newCard.itemHash = card1.itemHash | card2.itemHash;
		return newCard;
	}
}
