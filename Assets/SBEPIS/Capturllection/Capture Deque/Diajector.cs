using System;
using System.Collections.Generic;
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
		public MonoBehaviour coroutineOwner;
		public StrengthSettings cardStrength;

		public IEnumerable<Func<Transform>> targetProviders => new Func<Transform>[]
		{
			() => deque.cardStart,
			() => deque.cardTarget,
			() => upperTarget,
		};

		public bool isBound => deque;
		[NonSerialized]
		public CaptureDeque deque;

		private DequePage currentPage;
		public bool hasPageOpen => currentPage;

		public void StartAssembly() => StartAssembly(mainPage);

		public void StartAssembly(DequePage page)
		{
			if (hasPageOpen)
				StartDisassembly();
			currentPage = page;

			gameObject.SetActive(true);
			currentPage.gameObject.SetActive(true);
			currentPage.StartAssembly();
		}

		public void StartDisassembly()
		{
			if (hasPageOpen)
			{
				currentPage.StartDisassembly();
				currentPage.gameObject.SetActive(false);
				currentPage = null;
			}
			gameObject.SetActive(false);
		}

		public void ForceClose()
		{
			coroutineOwner.StopAllCoroutines();
			
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
