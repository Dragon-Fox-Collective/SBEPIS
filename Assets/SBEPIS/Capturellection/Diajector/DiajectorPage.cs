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
		
		public UnityEvent onPreparePagePre = new();
		[FormerlySerializedAs("onPreparePage")]
		public UnityEvent onPreparePagePost = new();
		
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
			gameObject.SetActive(true);
			onPreparePagePre.Invoke();
			CreateCardsIfNeeded();
			onPreparePagePost.Invoke();
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = true;
			diajector.CoroutineOwner.StartCoroutine(SpawnCards());
		}
		
		private void CreateCardsIfNeeded()
		{
			if (!pageCreator || hasCreatedCards)
				return;
			
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
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = false;
			diajector.CoroutineOwner.StartCoroutine(DespawnCards());
			gameObject.SetActive(false);
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
			gameObject.SetActive(true);
			
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
			foreach ((DequeElement card, CardTarget _) in cardTargets)
			{
				card.State.IsAssembling = false;
				card.State.IsDisassembling = false;
				card.State.ForceClose();
			}
			
			gameObject.SetActive(false);
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
