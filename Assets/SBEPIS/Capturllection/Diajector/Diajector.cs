using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class Diajector : MonoBehaviour
	{
		public LerpTarget upperTarget;
		public Card cardPrefab;
		public ElectricArc electricArcPrefab;
		public DiajectorPage mainPage;
		public float cardDelay = 0.5f;
		
		public Rigidbody staticRigidbody;
		public MonoBehaviour coroutineOwner;
		public StrengthSettings cardStrength;
		
		public bool IsBound => DequeOwner.Deque;
		
		public DequeOwner DequeOwner { get; set; }
		
		private DiajectorPage currentPage;
		
		public bool IsOpen => currentPage;
		public bool IsLayoutActive => layout && layout.isActiveAndEnabled;
		
		public DiajectorCaptureLayout layout { get; private set; }
		
		private void Awake()
		{
			layout = GetComponentInChildren<DiajectorCaptureLayout>(includeInactive:true);
		}
		
		public void StartAssembly() => StartAssembly(transform.position, transform.rotation);
		public void StartAssembly(Vector3 position, Quaternion rotation) => StartAssembly(position, rotation, mainPage);
		public void StartAssembly(Vector3 position, Quaternion rotation, DiajectorPage page)
		{
			if (IsOpen)
			{
				Debug.LogError("Tried to start assembly when already assembled");
				return;
			}
			
			gameObject.SetActive(true);
			transform.SetPositionAndRotation(position, rotation);
			AssembleNewPage(page);
		}
		
		public void ChangePage(DiajectorPage page)
		{
			DisassembleCurrentPage();
			AssembleNewPage(page);
		}
		
		public UnityAction ChangePageMethod(DiajectorPage page) => () => ChangePage(page);
		
		private void AssembleNewPage(DiajectorPage page)
		{
			currentPage = page;
			currentPage.StartAssembly();
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
		
		public bool ShouldCardBeDisplayed(Card card) => IsOpen && currentPage.HasCard(card);
		
		public LerpTarget GetLerpTarget(Card card) => currentPage ? currentPage.GetLerpTarget(card) : null;
		public CardTarget GetCardTarget(Card card) => currentPage ? currentPage.GetCardTarget(card) : null;
	}
}
