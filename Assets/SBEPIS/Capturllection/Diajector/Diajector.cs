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
		
		public bool isBound => owner.dequeBox;
		
		public DequeOwner owner { get; set; }

		private DequePage currentPage;
		
		public bool isOpen => currentPage;
		public bool isLayoutActive => layout && layout.isActiveAndEnabled;
		
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
			owner.inventory.transform.SetParent(layout.transform);
			owner.inventory.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			AssembleNewPage(page);
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

		public void UpdateCardTexture()
		{
			List<Texture2D> cardTextures = owner.dequeBox.definition.ruleset.GetCardTextures().ToList();
			GetComponentsInChildren<CardTarget>().Where(cardTarget => cardTarget.card).Select(cardTarget => cardTarget.card).Concat(owner.inventory).Do(card => card.split.UpdateTexture(cardTextures));
		}

		public LerpTarget GetLerpTarget(DequeStorable card) => currentPage ? currentPage.GetLerpTarget(card) : null;
		public CardTarget GetCardTarget(DequeStorable card) => currentPage ? currentPage.GetCardTarget(card) : null;
	}
}
