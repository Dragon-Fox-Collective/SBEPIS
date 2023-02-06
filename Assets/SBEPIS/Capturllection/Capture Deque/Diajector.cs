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
		public bool isOpen => currentPage;

		public void StartAssembly(Vector3 position, Quaternion rotation) => StartAssembly(position, rotation, mainPage);

		public void StartAssembly(Vector3 position, Quaternion rotation, DequePage page)
		{
			if (isOpen)
			{
				Debug.LogError("Tried to start assembly when already assembled");
				return;
			}

			gameObject.SetActive(true);
			transform.SetPositionAndRotation(position, rotation);
			AssembleNewPage(page);
		}

		public void RefreshPage()
		{
			ChangePage(currentPage);
		}

		public void ChangePage(DequePage page)
		{
			DisassembleCurrentPage();
			AssembleNewPage(page);
		}

		private void AssembleNewPage(DequePage page)
		{
			currentPage = page;
			currentPage.StartAssembly();
		}
		
		private void DisassembleCurrentPage()
		{
			if (!isOpen)
			{
				Debug.LogError("Tried to start disassembly when not assembled");
				return;
			}

			currentPage.StartDisassembly();
			currentPage = null;
		}
		
		private void ForceCloseCurrentPage()
		{
			if (!isOpen)
			{
				Debug.LogError("Tried to start disassembly when not assembled");
				return;
			}

			currentPage.ForceClose();
			currentPage = null;
		}

		public void StartDisassembly()
		{
			DisassembleCurrentPage();
			gameObject.SetActive(false);
		}

		public void ForceClose()
		{
			coroutineOwner.StopAllCoroutines();
			ForceCloseCurrentPage();
			gameObject.SetActive(false);
		}
	}
}
