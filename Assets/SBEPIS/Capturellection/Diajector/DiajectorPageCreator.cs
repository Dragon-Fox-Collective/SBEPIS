using System.Collections.Generic;
using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DiajectorPageCreator : MonoBehaviour
	{
		[SerializeField, Anywhere] private DequeElement menuCardPrefab;
		[SerializeField, Anywhere] private Deque deque;
		[SerializeField, Anywhere] private LerpTarget startTarget;
		public LerpTarget StartTarget => startTarget;
		
		private void OnValidate() => this.ValidateRefs();
		
		public IEnumerable<(DequeElement, CardTarget)> CreateCards(IEnumerable<CardTarget> targets)
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
	}
}
