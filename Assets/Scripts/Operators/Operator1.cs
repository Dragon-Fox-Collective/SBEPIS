using UnityEngine;

public abstract class Operator1 : MonoBehaviour
{
	public CaptchalogueCard cardPrefab;

	private void OnCollisionEnter(Collision collision)
	{
		CaptchalogueCard collisionCard = collision.gameObject.GetComponent<CaptchalogueCard>();
		if (collisionCard)
		{
			CaptchalogueCard newCard = Operate(collisionCard);
			if (newCard)
				newCard.transform.Rotate(90, 0, 0);
		}
	}

	protected abstract CaptchalogueCard Operate(CaptchalogueCard card);
}
