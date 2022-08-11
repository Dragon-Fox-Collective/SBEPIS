using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class Diajector : MonoBehaviour
	{
		public Transform upperTarget;
		public GameObject cardPrefab;
		public DequePage mainPage;
		public float cardDelay = 0.5f;
		public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1.5f, 3);

		public Rigidbody staticRigidbody;
		public StrengthSettings cardStrength;

		private CaptureDeque deque;

		public void StartAssembly(CaptureDeque deque)
		{
			if (!this.deque)
			{
				this.deque = deque;
				mainPage.CreateCards(cardPrefab, new Transform[] { deque.cardStart, deque.cardTarget, upperTarget }, curve, cardStrength, staticRigidbody);
			}

			gameObject.SetActive(true);
			mainPage.StartAssembly(deque, cardDelay);
		}

		public void StartDisassembly(CaptureDeque deque)
		{
			if (deque)
				mainPage.StartDisassembly(deque, cardDelay);
			gameObject.SetActive(false);
		}
	}
}
