using SBEPIS.Controller;
using System.Collections;
using System.Collections.Generic;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorPage : MonoBehaviour
	{
		public UnityEvent onPreparePagePre = new();
		[FormerlySerializedAs("onPreparePage")]
		public UnityEvent onPreparePagePost = new();
		
		private Diajector diajector;
		
		private bool hasCreatedCards = false;
		
		private readonly Dictionary<DequeElement, CardTarget> cardTargets = new();
		
		private void CreateCards(IEnumerable<CardTarget> targets, Deque deque, DequeElement menuCardPrefab, LerpTarget startTarget)
		{
			foreach (CardTarget target in targets)
			{
				DequeElement card = Instantiate(menuCardPrefab);
				card.name += $" ({target.transform.parent.name})";
				target.Card = card;
				
				AddCard(card, target);
				card.Deque = deque;
				card.Animator.TeleportTo(startTarget);
				
				Grabbable cardGrabbable = card.Grabbable;
				cardGrabbable.onGrab.AddListener((_, _) => target.onGrab.Invoke());
				cardGrabbable.onDrop.AddListener((_, _) => target.onDrop.Invoke());
			}
			hasCreatedCards = true;
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
		
		public void StartAssembly(Diajector diajector)
		{
			this.diajector = diajector;
			gameObject.SetActive(true);
			onPreparePagePre.Invoke();
			if (!hasCreatedCards)
				CreateCards(GetComponentsInChildren<CardTarget>(), diajector.deque, diajector.menuCardPrefab, diajector.startTarget);
			onPreparePagePost.Invoke();
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = true;
			diajector.coroutineOwner.StartCoroutine(SpawnCards());
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
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}
		
		public void StartDisassembly()
		{
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = false;
			diajector.coroutineOwner.StartCoroutine(DespawnCards());
			gameObject.SetActive(false);
		}
		
		private IEnumerator DespawnCards()
		{
			foreach ((DequeElement card, CardTarget _) in cardTargets)
			{
				card.State.IsAssembling = false;
				card.State.IsDisassembling = true;
				yield return new WaitForSeconds(diajector.cardDelay);
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
