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
		
		public Rigidbody staticRigidbody;
		public MonoBehaviour coroutineOwner;
		public StrengthSettings cardStrength;
		
		public IEnumerable<Func<LerpTarget>> targetProviders => new Func<LerpTarget>[]
		{
			() => owner.dequeBox.lowerTarget,
			() => owner.dequeBox.upperTarget,
			() => upperTarget,
		};
		
		public bool isBound => owner.dequeBox;
		
		public DequeOwner owner { get; set; }

		private DequePage currentPage;
		
		public bool isOpen => currentPage;
		
		public DequeCaptureLayout layout { get; private set; }
		
		private void Awake()
		{
			layout = GetComponentInChildren<DequeCaptureLayout>(includeInactive:true);
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
		
		public bool ShouldCardBeDisplayed(DequeStorable card) => isOpen && currentPage.HasCard(card);
		
		public void UpdateCardTexture() => UpdateCardTexture(owner.dequeBox);
		public void UpdateCardTexture(DequeBox dequeBox)
		{
			if (dequeBox)
				GetComponentsInChildren<CardTarget>().Where(cardTarget => cardTarget.card).Do(cardTarget => dequeBox.definition.UpdateCardTexture(cardTarget.card));
			else
				GetComponentsInChildren<CardTarget>().Where(cardTarget => cardTarget.card).Do(cardTarget => cardTarget.card.split.ResetTexture());
		}

		public LerpTarget GetLerpTarget(DequeStorable card) => currentPage.GetLerpTarget(card);
		public CardTarget GetCardTarget(DequeStorable card) => currentPage.GetCardTarget(card);
	}
}
