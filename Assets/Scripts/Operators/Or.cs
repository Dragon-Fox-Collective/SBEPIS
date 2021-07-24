public class Or : Operator2
{
	protected override CaptchalogueCard Operate(CaptchalogueCard card1, CaptchalogueCard card2)
	{
		CaptchalogueCard newCard = Instantiate(cardPrefab);
		newCard.Punch(card1.punchedHash | card2.punchedHash);
		return newCard;
	}
}
