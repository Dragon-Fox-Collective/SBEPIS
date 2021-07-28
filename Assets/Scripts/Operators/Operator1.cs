using UnityEngine;

namespace WrightWay.SBEPIS.Operators
{
	/// <summary>
	/// When a captcha card collides with this, does an operation with it
	/// </summary>
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
					newCard.transform.Translate(Vector3.up);
			}
		}

		protected abstract CaptchalogueCard Operate(CaptchalogueCard card);
	}
}