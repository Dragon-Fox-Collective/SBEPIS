using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorPage : MonoBehaviour
	{
		[SerializeField, Parent(Flag.IncludeInactive)] private Diajector diajector;
		[SerializeField, Parent(Flag.IncludeInactive | Flag.Optional)] private DiajectorPageCreator pageCreator;
		
		[FormerlySerializedAs("onPreparePagePre")]
		public UnityEvent onPrepareCardCreation = new();
		[FormerlySerializedAs("onPreparePagePost")]
		public UnityEvent onPreparePage = new();
		
		private bool hasCreatedCards = false;
		
		private readonly Dictionary<DequeElement, CardTarget> cardTargets = new();
		
		private void OnValidate() => this.ValidateRefs();

		public void AddCard(DequeElement card, CardTarget target)
		{
			cardTargets.Add(card, target);
			target.onCardBound.Invoke(card);
			card.Diajector = diajector;
		}
		
		public void RemoveCard(DequeElement card)
		{
			cardTargets.Remove(card);
			card.Diajector = null;
		}
		
		public bool HasCard(DequeElement card) => cardTargets.ContainsKey(card);
		public CardTarget GetCardTarget(DequeElement card) => cardTargets.ContainsKey(card) ? cardTargets[card] : null;
		public LerpTarget GetLerpTarget(DequeElement card) => cardTargets.ContainsKey(card) ? GetCardTarget(card).LerpTarget : null;
		
		public void StartAssembly()
		{
			PrepareOpeningPage();
			diajector.CoroutineOwner.StartCoroutine(SpawnCards());
		}

		private void PrepareOpeningPage()
		{
			gameObject.SetActive(true);
			CreateCardsIfNeeded();
			onPreparePage.Invoke();
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = true;
		}
		
		private void PrepareClosingPage()
		{
			gameObject.SetActive(false);
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = false;
		}
		
		public void CreateCardsIfNeeded()
		{
			if (!pageCreator || hasCreatedCards)
				return;
			
			onPrepareCardCreation.Invoke();
			pageCreator.CreateCards(GetComponentsInChildren<CardTarget>()).ForEach(AddCard);
			hasCreatedCards = true;
		}
		
		private IEnumerator SpawnCards()
		{
			// Give cards a moment to get into the In Deque state
			yield return 0;
			yield return 0;
			
			foreach ((DequeElement card, CardTarget target) in cardTargets)
			{
				target.onPrepareCard.Invoke();
				card.State.IsAssembling = true;
				card.State.IsDisassembling = false;
				yield return new WaitForSeconds(diajector.CardDelay);
			}
		}
		
		public void StartDisassembly()
		{
			PrepareClosingPage();
			diajector.CoroutineOwner.StartCoroutine(DespawnCards());
		}
		
		private IEnumerator DespawnCards()
		{
			foreach ((DequeElement card, CardTarget _) in cardTargets)
			{
				card.State.IsAssembling = false;
				card.State.IsDisassembling = true;
				yield return new WaitForSeconds(diajector.CardDelay);
			}
		}
		
		public void ForceOpen()
		{
			PrepareOpeningPage();
			foreach ((DequeElement card, CardTarget _) in cardTargets)
			{
				card.State.IsAssembling = false;
				card.State.IsDisassembling = false;
				card.State.HasBeenAssembled = true;
				card.State.ForceOpen();
			}
		}
		
		public void ForceClose()
		{
			PrepareClosingPage();
			foreach ((DequeElement card, CardTarget _) in cardTargets)
			{
				card.State.IsAssembling = false;
				card.State.IsDisassembling = false;
				card.State.ForceClose();
			}
		}
		
		private void OnDestroy()
		{
			if (isActiveAndEnabled)
				diajector.ForceRestart();
			
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				Destroy(card);
		}
	}
}
