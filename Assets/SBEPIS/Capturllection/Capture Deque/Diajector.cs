using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class Diajector : MonoBehaviour
	{
		public Transform upperTarget;
		public DequeStorable cardPrefab;
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

				DequePage page = mainPage;
				page.baseTargets.AddRange(deque.cardStart, deque.cardTarget, upperTarget);
				page.curve = curve;
				page.cardDelay = cardDelay;
				page.staticRigidbody = staticRigidbody;
				page.cardStrength = cardStrength;
				page.CreateCards(cardPrefab);
			}

			gameObject.SetActive(true);
			mainPage.StartAssembly(deque);
		}

		public void StartDisassembly(CaptureDeque deque)
		{
			if (deque)
				mainPage.StartDisassembly(deque);
			gameObject.SetActive(false);
		}

		public void ForceClose()
		{
			deque.StopAllCoroutines();
			mainPage.ForceClose();
			gameObject.SetActive(false);
		}
	}
}
