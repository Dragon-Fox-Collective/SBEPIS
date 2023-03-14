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
		public DiajectorPage mainPage;
		public float cardDelay = 0.5f;
		
		public Rigidbody staticRigidbody;
		public MonoBehaviour coroutineOwner;
		public StrengthSettings cardStrength;
		
		public bool isBound => owner.dequeBox;
		
		public DequeOwner owner { get; set; }

		private DiajectorPage currentPage;
		
		public bool isOpen => currentPage;
		public bool isLayoutActive => layout && layout.isActiveAndEnabled;
		
		public DiajectorCaptureLayout layout { get; private set; }
		
		private void Awake()
		{
			layout = GetComponentInChildren<DiajectorCaptureLayout>(includeInactive:true);
		}
		
		public void StartAssembly() => StartAssembly(transform.position, transform.rotation);
		public void StartAssembly(Vector3 position, Quaternion rotation) => StartAssembly(position, rotation, mainPage);
		public void StartAssembly(Vector3 position, Quaternion rotation, DiajectorPage page)
		{
			if (isOpen)
			{
				Debug.LogError("Tried to start assembly when already assembled");
				return;
			}
			
			gameObject.SetActive(true);
			transform.SetPositionAndRotation(position, rotation);
			owner.inventory.transform.SetParent(layout.transform);
			owner.inventory.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			AssembleNewPage(page);
		}
		
		public void ChangePage(DiajectorPage page)
		{
			DisassembleCurrentPage();
			AssembleNewPage(page);
		}
		
		private void AssembleNewPage(DiajectorPage page)
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

		public void ForceRestart()
		{
			ForceClose();
			StartAssembly();
		}
		
		public bool ShouldCardBeDisplayed(DequeStorable card) => isOpen && currentPage.HasCard(card);

		public void UpdateCardTexture()
		{
			List<Texture2D> defaultTextures = owner.dequeBox ? owner.dequeBox.definition.ruleset.GetCardTextures().ToList() : null;
			foreach (DequeStorable card in GetComponentsInChildren<CardTarget>().Where(cardTarget => cardTarget.card).Select(cardTarget => cardTarget.card))
				card.split.UpdateTexture(defaultTextures);
			foreach (DequeStorable card in owner.inventory)
				card.split.UpdateTexture(owner.inventory.GetCardTextures(card).ToList());
		}

		public LerpTarget GetLerpTarget(DequeStorable card) => currentPage ? currentPage.GetLerpTarget(card) : null;
		public CardTarget GetCardTarget(DequeStorable card) => currentPage ? currentPage.GetCardTarget(card) : null;
	}
}
