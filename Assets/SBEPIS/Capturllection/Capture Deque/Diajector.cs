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

		public CaptureDeque deque { get; private set; }

		private void CreateCards(CaptureDeque deque)
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

		public void StartAssembly(CaptureDeque deque)
		{
			if (!this.deque)
				CreateCards(deque);

			gameObject.SetActive(true);
			mainPage.StartAssembly(deque);
		}

		public void StartDisassembly()
		{
			if (deque)
				mainPage.StartDisassembly(deque);
			gameObject.SetActive(false);
		}

		public void ForceClose()
		{
			if (deque)
				deque.StopAllCoroutines();
			mainPage.ForceClose();
			gameObject.SetActive(false);
		}
	}
}
