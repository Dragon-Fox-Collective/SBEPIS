using SBEPIS.Controller;
using SBEPIS.Physics;
using System.Collections;
using System.Collections.Generic;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class DequePage : MonoBehaviour
	{
		public UnityEvent onPreparePage = new();
		
		public Diajector diajector { get; private set; }

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
				card.name += $" ({target.label})";
				card.owner = diajector.owner.dequeBox.owner;
				card.state.SetBool(DequeStorable.IsBound, true);
				diajector.owner.dequeBox.definition.UpdateCardTexture(card);
				target.card = card;
				
				AddCard(card, target);
				card.animator.TeleportTo(diajector.owner.dequeBox.lowerTarget);

				Capturellectainer container = card.GetComponent<Capturellectainer>();
				container.isRetrievingAllowed = false

				Capturllectable capturllectable = card.GetComponent<Capturllectable>();
				capturllectable.canCapturllect = false;
			}
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
		public CardTarget GetCardTarget(DequeStorable card) => cardTargets[card];
		public LerpTarget GetLerpTarget(DequeStorable card) => GetCardTarget(card).lerpTarget;

		public void Refresh()
		{
			onPreparePage.Invoke();
		}
		
		public void StartAssembly()
		{
			gameObject.SetActive(true);
			if (cardTargets.Count == 0)
				CreateCards(GetComponentsInChildren<CardTarget>());
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
				card.state.SetBool(DequeStorable.IsPageOpen, true);
			onPreparePage.Invoke();
			diajector.coroutineOwner.StartCoroutine(SpawnCards());
		}

		private IEnumerator SpawnCards()
		{
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
			{
				target.onPrepareCard.Invoke();
				card.state.SetBool(DequeStorable.IsAssembling, true);
				card.state.SetBool(DequeStorable.IsDisassembling, false);
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}

		public void StartDisassembly()
		{
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
				card.state.SetBool(DequeStorable.IsPageOpen, false);
			diajector.coroutineOwner.StartCoroutine(DespawnCards());
			gameObject.SetActive(false);
		}

		private IEnumerator DespawnCards()
		{
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
			{
				card.state.SetBool(DequeStorable.IsAssembling, false);
				card.state.SetBool(DequeStorable.IsDisassembling, true);
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}

		public void ForceClose(LerpTarget bottomTarget)
		{
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
			{
				card.state.SetBool(DequeStorable.IsAssembling, false);
				card.state.SetBool(DequeStorable.IsDisassembling, false);
				card.state.SetTrigger(DequeStorable.ForceClose);
			}
			
			gameObject.SetActive(false);
		}
	}
}
