using System.Collections.Generic;
using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DiajectorPageCreator : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private GameObject menuCardPrefab;
		[SerializeField, Anywhere] private Deque deque;
		[SerializeField, Anywhere] private LerpTarget startTarget;
		[SerializeField, Anywhere] private Transform cardParent;
		public LerpTarget StartTarget => startTarget;
		
		public IEnumerable<(DequeElement, CardTarget)> CreateCards(IEnumerable<CardTarget> targets)
		{
			List<(DequeElement, CardTarget)> cards = new();
			foreach (CardTarget target in targets)
			{
				DequeElement card = Instantiate(menuCardPrefab).GetComponentInChildren<DequeElement>();
				card.SetParent(cardParent);
				cards.Add((card, target));
				card.name += $" ({target.transform.parent.name})";
				target.Card = card;
				
				card.Deque = deque;
				card.Animator.TeleportTo(startTarget);
				
				if (card.TryGetComponent(out Grabbable cardGrabbable))
				{
					cardGrabbable.onGrab.AddListener((_, _) => target.onGrab.Invoke());
					cardGrabbable.onDrop.AddListener((_, _) => target.onDrop.Invoke());
				}
			}
			return cards;
		}
	}
}
