using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class Diajector : MonoBehaviour
	{
		public LerpTarget upperTarget;
		public DequeStorable cardPrefab;
		public ElectricArc electricArcPrefab;
		public DequePage mainPage;
		public float cardDelay = 0.5f;
		public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1.5f, 3);

		public Rigidbody staticRigidbody;
		public MonoBehaviour coroutineOwner;
		public StrengthSettings cardStrength;

		public IEnumerable<Func<LerpTarget>> targetProviders => new Func<LerpTarget>[]
		{
			() => dequeBox.lowerTarget,
			() => dequeBox.upperTarget,
			() => upperTarget,
		};

		public bool isBound => dequeBox;

		private DequeBox _dequeBox;
		public DequeBox dequeBox
		{
			get => _dequeBox;
			set
			{
				_dequeBox = value;

				if (_dequeBox)
					GetComponentsInChildren<CardTarget>().Where(cardTarget => cardTarget.card).Do(cardTarget => _dequeBox.definition.UpdateCardTexture(cardTarget.card));
				else
					GetComponentsInChildren<CardTarget>().Where(cardTarget => cardTarget.card).Do(cardTarget => cardTarget.card.split.ResetTexture());
			}
		}

		public DequePage currentPage { get; private set; }
		public bool isOpen => currentPage;
		
		public DequeCaptureLayout captureLayout { get; private set; }

		private void Awake()
		{
			captureLayout = GetComponentInChildren<DequeCaptureLayout>(includeInactive:true);
		}

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
			currentPage.Refresh();
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
		
		private void ForceCloseCurrentPage(LerpTarget bottomTarget)
		{
			if (!isOpen)
			{
				Debug.LogError("Tried to start disassembly when not assembled");
				return;
			}

			currentPage.ForceClose(bottomTarget);
			currentPage = null;
		}

		public void StartDisassembly()
		{
			DisassembleCurrentPage();
			gameObject.SetActive(false);
		}

		public void ForceClose(LerpTarget bottomTarget)
		{
			coroutineOwner.StopAllCoroutines();
			ForceCloseCurrentPage(bottomTarget);
			gameObject.SetActive(false);
		}
	}
}
