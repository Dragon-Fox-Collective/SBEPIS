using System;
using System.Collections.Generic;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	public class Diajector : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private DiajectorPage mainPage;
		
		[SerializeField, Anywhere] private MonoBehaviour coroutineOwner;
		public MonoBehaviour CoroutineOwner => coroutineOwner;
		
		[SerializeField, Anywhere] private Rigidbody staticRigidbody;
		public Rigidbody StaticRigidbody => staticRigidbody;
		
		[Tooltip("Ordered from deque to diajector")]
		[SerializeField] private List<LerpTarget> lerpTargets = new();
		
		public UnityEvent<Diajector> onOpen = new();
		public UnityEvent<Diajector> onClose = new();
		
		public DiajectorPage CurrentPage { get; private set; }
		public bool IsOpen => CurrentPage;
		
		private void StartAssembly() => StartAssembly(transform.position, transform.rotation);
		public void StartAssembly(Vector3 position, Quaternion rotation) => StartAssembly(position, rotation, mainPage);
		
		private void StartAssembly(Vector3 position, Quaternion rotation, DiajectorPage page)
		{
			if (IsOpen)
			{
				Debug.LogError("Tried to start assembly when already assembled");
				return;
			}
			
			gameObject.SetActive(true);
			transform.SetPositionAndRotation(position, rotation);
			AssembleNewPage(page);
			onOpen.Invoke(this);
		}
		
		public void ChangePage(DiajectorPage page)
		{
			DisassembleCurrentPage();
			AssembleNewPage(page);
		}
		
		private void AssembleNewPage(DiajectorPage page)
		{
			CurrentPage = page;
			CurrentPage.StartAssembly();
		}
		
		private void ForceOpenCurrentPage()
		{
			if (!CurrentPage)
				CurrentPage = mainPage;
			CurrentPage.ForceOpen();
		}
		
		private void DisassembleCurrentPage()
		{
			if (!IsOpen)
			{
				Debug.LogError("Tried to start disassembly when not assembled");
				return;
			}
			
			CurrentPage.StartDisassembly();
			CurrentPage = null;
		}
		
		private void ForceCloseCurrentPage()
		{
			if (!IsOpen)
			{
				Debug.LogError("Tried to start disassembly when not assembled");
				return;
			}
			
			CurrentPage.ForceClose();
			CurrentPage = null;
		}
		
		public void StartDisassembly()
		{
			DisassembleCurrentPage();
			gameObject.SetActive(false);
			onClose.Invoke(this);
		}
		
		public void ForceOpen()
		{
			gameObject.SetActive(true);
			coroutineOwner.StopAllCoroutines();
			ForceOpenCurrentPage();
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
		
		public bool ShouldCardBeDisplayed(DequeElement card) => IsOpen && CurrentPage.HasCard(card);
		
		public LerpTarget GetLerpTargetAtPathIndex(DequeElement card, int index) => index >= 0 && index < lerpTargets.Count ? lerpTargets[index] : index == lerpTargets.Count ? card.LerpTarget : throw new IndexOutOfRangeException();
		public int LerpTargetPathCount => lerpTargets.Count + 1;
	}
}
