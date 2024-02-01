using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorPage : ValidatedMonoBehaviour
	{
		[SerializeField, Parent(Flag.IncludeInactive)] private Diajector diajector;
		public Diajector Diajector => diajector;
		[SerializeField, Parent(Flag.IncludeInactive | Flag.Optional)] private DiajectorPageCreator pageCreator;
		
		[FormerlySerializedAs("onPrepareCardCreation")]
		[FormerlySerializedAs("onPreparePagePre")]
		public UnityEvent<DiajectorPage> OnPrepareCardCreation = new();
		[FormerlySerializedAs("onOpen")]
		[FormerlySerializedAs("onPreparePage")]
		[FormerlySerializedAs("onPreparePagePost")]
		public UnityEvent OnOpen = new();
		[FormerlySerializedAs("onClose")]
		public UnityEvent OnClose = new();
		
		private bool hasCreatedCards = false;
		
		public bool IsOpen => Diajector.CurrentPage == this;
		
		private readonly Dictionary<DequeElement, CardTarget> cardTargets = new();
		public IEnumerable<CardTarget> CardTargets => cardTargets.Values;
		
		public void AddCard(DequeElement card, CardTarget target)
		{
			cardTargets.Add(card, target);
			target.OnCardBound.Invoke(card);
			card.Page = this;
		}
		
		public void RemoveCard(DequeElement card)
		{
			cardTargets.Remove(card);
			card.Page = null;
		}
		
		public bool HasCard(DequeElement card) => cardTargets.ContainsKey(card);
		public CardTarget GetCardTarget(DequeElement card) => cardTargets.ContainsKey(card) ? cardTargets[card] : null;
		public LerpTarget GetLerpTarget(DequeElement card) => cardTargets.ContainsKey(card) ? GetCardTarget(card).LerpTarget : null;
		
		public void StartAssembly()
		{
			PrepareOpeningPage();
			diajector.CoroutineOwner.StartCoroutine(SpawnCards(cardTargets.Select(pair => (pair.Key, pair.Value))));
		}
		
		private void PrepareOpeningPage()
		{
			gameObject.SetActive(true);
			CreateCardsIfNeeded();
			OnOpen.Invoke();
		}
		
		private void PrepareClosingPage()
		{
			gameObject.SetActive(false);
			OnClose.Invoke();
		}
		
		public void CreateCardsIfNeeded()
		{
			if (!pageCreator || hasCreatedCards)
				return;
			
			OnPrepareCardCreation.Invoke(this);
			pageCreator.CreateCards(GetComponentsInChildren<CardTarget>().Where(target => !cardTargets.ContainsValue(target))).ForEach(AddCard);
			hasCreatedCards = true;
		}
		
		public void StartAssemblyForCards(IEnumerable<DequeElement> cards)
		{
			diajector.CoroutineOwner.StartCoroutine(SpawnCards(cards.Select(card => (card, cardTargets[card]))));
		}
		
		private IEnumerator SpawnCards(IEnumerable<(DequeElement, CardTarget)> cards)
		{
			// Wait for cards to Start
			yield return 0;
			
			foreach ((DequeElement card, CardTarget target) in cards)
			{
				target.OnPrepareCard.Invoke();
				card.StartAssembling();
				yield return new WaitForSeconds(target.DealDelay);
			}
		}
		
		public void StartDisassembly()
		{
			PrepareClosingPage();
			diajector.CoroutineOwner.StartCoroutine(DespawnCards());
		}
		
		private IEnumerator DespawnCards()
		{
			foreach ((DequeElement card, CardTarget target) in cardTargets)
			{
				card.StartDisassembling();
				yield return new WaitForSeconds(target.DealDelay);
			}
		}
		
		public void ForceOpen()
		{
			PrepareOpeningPage();
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.ForceOpen();
		}
		
		public void ForceClose()
		{
			PrepareClosingPage();
			foreach ((DequeElement card, CardTarget _) in cardTargets)
				card.ForceClose();
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
