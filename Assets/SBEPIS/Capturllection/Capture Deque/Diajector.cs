using SBEPIS.Physics;
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

		public bool isBound => deque;

		public CaptureDeque deque { get; private set; }

		private DequePage currentPage;
		public bool hasPageOpen => currentPage;

		private void CreateCards(DequePage page)
		{
			page.baseTargets.AddRange(deque.cardStart, deque.cardTarget, upperTarget);
			page.curve = curve;
			page.cardDelay = cardDelay;
			page.staticRigidbody = staticRigidbody;
			page.cardStrength = cardStrength;
			page.CreateCards(deque, cardPrefab);
		}

		public void StartAssembly(CaptureDeque deque) => StartAssembly(deque, mainPage);

		public void StartAssembly(CaptureDeque deque, DequePage page)
		{
			if (!isBound)
				this.deque = deque;

			if (hasPageOpen)
				StartDisassembly();
			currentPage = page;

			gameObject.SetActive(true);
			currentPage.gameObject.SetActive(true);
			if (!currentPage.cardsCreated)
				CreateCards(currentPage);
			currentPage.StartAssembly(deque);
		}

		public void StartDisassembly()
		{
			if (hasPageOpen)
			{
				currentPage.StartDisassembly(deque);
				currentPage.gameObject.SetActive(false);
				currentPage = null;
			}
			gameObject.SetActive(false);
		}

		public void ForceClose()
		{
			if (isBound)
				deque.StopAllCoroutines();
			if (hasPageOpen)
			{
				currentPage.ForceClose();
				currentPage.gameObject.SetActive(false);
				currentPage = null;
			}
			gameObject.SetActive(false);
		}
	}
}
