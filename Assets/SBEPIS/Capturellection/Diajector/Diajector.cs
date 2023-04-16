using System.Diagnostics.CodeAnalysis;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class Diajector : MonoBehaviour
	{
		public Deque deque;
		public LerpTarget startTarget;
		public LerpTarget upperTarget;
		[FormerlySerializedAs("cardPrefab")]
		public DequeElement menuCardPrefab;
		public ElectricArc electricArcPrefab;
		public DiajectorPage mainPage;
		public float cardDelay = 0.5f;
		
		public Rigidbody staticRigidbody;
		public MonoBehaviour coroutineOwner;
		public StrengthSettings cardStrength;

		public UnityEvent<Diajector> onOpen = new();
		public UnityEvent<Diajector> onClose = new();

		private DiajectorPage currentPage;
		
		public bool IsOpen => currentPage;
		
		private void StartAssembly() => StartAssembly(null, transform.position, transform.rotation);
		public void StartAssembly([MaybeNull] DiajectorCloser closer, Vector3 position, Quaternion rotation) => StartAssembly(closer, position, rotation, mainPage);
		
		private void StartAssembly([MaybeNull] DiajectorCloser closer, Vector3 position, Quaternion rotation, DiajectorPage page)
		{
			if (IsOpen)
			{
				Debug.LogError("Tried to start assembly when already assembled");
				return;
			}
			
			if (closer)
				closer.CloseOldDiajector(this);
			
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
		
		public UnityAction ChangePageAction(DiajectorPage page) => () => ChangePage(page);
		
		private void AssembleNewPage(DiajectorPage page)
		{
			currentPage = page;
			currentPage.StartAssembly(this);
		}

		private void ForceOpenCurrentPage()
		{
			if (!currentPage)
				currentPage = mainPage;
			currentPage.ForceOpen();
		}
		
		private void DisassembleCurrentPage()
		{
			if (!IsOpen)
			{
				Debug.LogError("Tried to start disassembly when not assembled");
				return;
			}
			
			currentPage.StartDisassembly();
			currentPage = null;
		}
		
		private void ForceCloseCurrentPage()
		{
			if (!IsOpen)
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
		
		public bool ShouldCardBeDisplayed(DequeElement card) => IsOpen && currentPage.HasCard(card);
		
		public LerpTarget GetLerpTarget(DequeElement card) => currentPage ? currentPage.GetLerpTarget(card) : null;
		public CardTarget GetCardTarget(DequeElement card) => currentPage ? currentPage.GetCardTarget(card) : null;
	}
}
