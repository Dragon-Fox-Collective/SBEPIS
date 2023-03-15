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
		
		public Diajector diajector { get; private set; }

		private bool hasCreatedCards = false;
		
		private readonly Dictionary<DequeStorable, CardTarget> cardTargets = new();
		
		private void Awake()
		{
			diajector = GetComponentInParent<Diajector>();
		}
		
		private void CreateCards(IEnumerable<CardTarget> targets)
		{
			foreach (CardTarget target in targets)
			{
				DequeStorable card = Instantiate(diajector.cardPrefab);
				card.name += $" ({target.transform.parent.name})";
				card.owner = diajector.owner.dequeBox.owner;
				target.card = card;
				
				AddCard(card, target);
				card.animator.TeleportTo(diajector.owner.dequeBox.lowerTarget);
				
				Capturellectainer container = card.GetComponent<Capturellectainer>();
				container.isFetchingAllowed = false;
				
				Capturllectable capturllectable = card.GetComponent<Capturllectable>();
				capturllectable.canCapturllect = false;
				
				Grabbable cardGrabbable = card.GetComponent<Grabbable>();
				cardGrabbable.onGrab.AddListener((_, _) => target.onGrab.Invoke());
				cardGrabbable.onDrop.AddListener((_, _) => target.onDrop.Invoke());
			}
			hasCreatedCards = true;
		}
		
		public void AddCard(DequeStorable card, CardTarget target)
		{
			cardTargets.Add(card, target);
			target.onCardBound.Invoke(card);
		}
		
		public void RemoveCard(DequeStorable card)
		{
			cardTargets.Remove(card);
		}
		
		public bool HasCard(DequeStorable card) => cardTargets.ContainsKey(card);
		public CardTarget GetCardTarget(DequeStorable card) => cardTargets.ContainsKey(card) ? cardTargets[card] : null;
		public LerpTarget GetLerpTarget(DequeStorable card) => cardTargets.ContainsKey(card) ? GetCardTarget(card).lerpTarget : null;
		
		public void StartAssembly()
		{
			gameObject.SetActive(true);
			onPreparePagePre.Invoke();
			if (!hasCreatedCards)
				CreateCards(GetComponentsInChildren<CardTarget>());
			onPreparePagePost.Invoke();
			foreach ((DequeStorable card, CardTarget _) in cardTargets)
				card.state.isPageOpen = true;
			diajector.coroutineOwner.StartCoroutine(SpawnCards());
		}
		
		private IEnumerator SpawnCards()
		{
			// Give cards a moment to get into the In Deque state
			yield return 0;
			yield return 0;
			
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
			{
				target.onPrepareCard.Invoke();
				card.state.isAssembling = true;
				card.state.isDisassembling = false;
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}
		
		public void StartDisassembly()
		{
			foreach ((DequeStorable card, CardTarget _) in cardTargets)
				card.state.isPageOpen = false;
			diajector.coroutineOwner.StartCoroutine(DespawnCards());
			gameObject.SetActive(false);
		}
		
		private IEnumerator DespawnCards()
		{
			foreach ((DequeStorable card, CardTarget _) in cardTargets)
			{
				card.state.isAssembling = false;
				card.state.isDisassembling = true;
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}
		
		public void ForceClose()
		{
			foreach ((DequeStorable card, CardTarget _) in cardTargets)
			{
				card.state.isAssembling = false;
				card.state.isDisassembling = false;
				card.state.ForceClose();
			}
			
			gameObject.SetActive(false);
		}
		
		private void OnDestroy()
		{
			if (isActiveAndEnabled)
				diajector.ForceRestart();
			
			foreach ((DequeStorable card, CardTarget _) in cardTargets)
			{
				Destroy(card);
			}
		}
	}
}
