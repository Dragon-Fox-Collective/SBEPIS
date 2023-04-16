using SBEPIS.Controller;
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
		
		public UnityEvent onPreparePagePre = new();
		[FormerlySerializedAs("onPreparePage")]
		public UnityEvent onPreparePagePost = new();
		
		private bool hasCreatedCards = false;
		
		private readonly Dictionary<DequeElement, CardTarget> cardTargets = new();
		
		private void OnValidate() => this.ValidateRefs();
		
		private static IEnumerable<(DequeElement, CardTarget)> CreateCards(IEnumerable<CardTarget> targets, Deque deque, DequeElement menuCardPrefab, LerpTarget startTarget)
		{
			List<(DequeElement, CardTarget)> cards = new();
			foreach (CardTarget target in targets)
			{
				DequeElement card = Instantiate(menuCardPrefab);
				cards.Add((card, target));
				card.name += $" ({target.transform.parent.name})";
				target.Card = card;
				
				card.Deque = deque;
				card.Animator.TeleportTo(startTarget);
				
				Grabbable cardGrabbable = card.Grabbable;
				cardGrabbable.onGrab.AddListener((_, _) => target.onGrab.Invoke());
				cardGrabbable.onDrop.AddListener((_, _) => target.onDrop.Invoke());
			}
			return cards;
		}
		
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
			
			if (!hasCreatedCards)
			{
				CreateCards(GetComponentsInChildren<CardTarget>(), diajector.Deque, diajector.MenuCardPrefab, diajector.StartTarget).ForEach(AddCard);
				hasCreatedCards = true;
			}
			
			onPreparePagePost.Invoke();
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = true;
			diajector.CoroutineOwner.StartCoroutine(SpawnCards());
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
