using SBEPIS.Controller;
using System.Collections;
using System.Collections.Generic;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection
{
	public class DiajectorPage : MonoBehaviour
	{
		public UnityEvent onPreparePagePre = new();
		[FormerlySerializedAs("onPreparePage")]
		public UnityEvent onPreparePagePost = new();
		
		public Diajector Diajector { get; private set; }
		
		private bool hasCreatedCards = false;
		
		private readonly Dictionary<Card, CardTarget> cardTargets = new();
		
		private void Awake()
		{
			Diajector = GetComponentInParent<Diajector>();
		}
		
		private void CreateCards(IEnumerable<CardTarget> targets)
		{
			foreach (CardTarget target in targets)
			{
				Card card = Instantiate(Diajector.menuCardPrefab);
				card.name += $" ({target.transform.parent.name})";
				target.card = card;
				
				AddCard(card, target);
				card.Animator.TeleportTo(Diajector.DequeOwner.Deque.lowerTarget);
				
				Grabbable cardGrabbable = card.Grabbable;
				cardGrabbable.onGrab.AddListener((_, _) => target.onGrab.Invoke());
				cardGrabbable.onDrop.AddListener((_, _) => target.onDrop.Invoke());
			}
			hasCreatedCards = true;
		}
		
		public void AddCard(Card card, CardTarget target)
		{
			cardTargets.Add(card, target);
			card.DequeOwner = Diajector.DequeOwner;
			target.onCardBound.Invoke(card);
		}
		
		public void RemoveCard(Card card)
		{
			cardTargets.Remove(card);
			card.DequeOwner = null;
		}
		
		public bool HasCard(Card card) => cardTargets.ContainsKey(card);
		public CardTarget GetCardTarget(Card card) => cardTargets.ContainsKey(card) ? cardTargets[card] : null;
		public LerpTarget GetLerpTarget(Card card) => cardTargets.ContainsKey(card) ? GetCardTarget(card).lerpTarget : null;
		
		public void StartAssembly()
		{
			gameObject.SetActive(true);
			onPreparePagePre.Invoke();
			if (!hasCreatedCards)
				CreateCards(GetComponentsInChildren<CardTarget>());
			onPreparePagePost.Invoke();
			foreach ((Card card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = true;
			Diajector.coroutineOwner.StartCoroutine(SpawnCards());
		}
		
		private IEnumerator SpawnCards()
		{
			// Give cards a moment to get into the In Deque state
			yield return 0;
			yield return 0;
			
			foreach ((Card card, CardTarget target) in cardTargets)
			{
				target.onPrepareCard.Invoke();
				card.State.IsAssembling = true;
				card.State.IsDisassembling = false;
				yield return new WaitForSeconds(Diajector.cardDelay);
			}
		}
		
		public void StartDisassembly()
		{
			foreach ((Card card, CardTarget _) in cardTargets)
				card.State.IsPageOpen = false;
			Diajector.coroutineOwner.StartCoroutine(DespawnCards());
			gameObject.SetActive(false);
		}
		
		private IEnumerator DespawnCards()
		{
			foreach ((Card card, CardTarget _) in cardTargets)
			{
				card.State.IsAssembling = false;
				card.State.IsDisassembling = true;
				yield return new WaitForSeconds(Diajector.cardDelay);
			}
		}
		
		public void ForceOpen()
		{
			gameObject.SetActive(true);
			
			foreach ((Card card, CardTarget _) in cardTargets)
			{
				card.State.IsAssembling = false;
				card.State.IsDisassembling = false;
				card.State.HasBeenAssembled = true;
				card.State.ForceOpen();
			}
		}
		
		public void ForceClose()
		{
			foreach ((Card card, CardTarget _) in cardTargets)
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
				Diajector.ForceRestart();
			
			foreach ((Card card, CardTarget _) in cardTargets)
			{
				Destroy(card);
			}
		}
	}
}
