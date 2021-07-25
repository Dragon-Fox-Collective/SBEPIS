using UnityEngine;

public abstract class Operator2 : MonoBehaviour
{
	public CaptchalogueCard cardPrefab;

	private CaptchalogueCard card1;

	private void OnCollisionEnter(Collision collision)
	{
		CaptchalogueCard collisionCard = collision.gameObject.GetComponent<CaptchalogueCard>();
		if (collisionCard)
		{
			if (card1)
			{
				CaptchalogueCard newCard = Operate(card1, collisionCard);
				if (newCard)
					newCard.transform.Translate(Vector3.up);

				card1 = null;
			}

			else
				card1 = collisionCard;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.GetComponent<CaptchalogueCard>() == card1)
			card1 = null;
	}

	protected abstract CaptchalogueCard Operate(CaptchalogueCard card1, CaptchalogueCard card2);
}
