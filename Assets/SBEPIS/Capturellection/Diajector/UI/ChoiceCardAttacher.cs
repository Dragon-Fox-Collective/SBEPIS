using System;
using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.UI
{
	[RequireComponent(typeof(CardTarget))]
	public class ChoiceCardAttacher : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private CardTarget cardTarget;
		
		[SerializeField, Anywhere] private Transform point;
		[SerializeField, Anywhere] private ChoiceCardSlot slot;
		
		public UnityEvent onChosen = new();
		
		public ChoiceCard ChoiceCard { get; private set; }
		
		public void Attach(DequeElement card)
		{
			if (!card.TryGetComponent(out Grabbable grabbable))
				throw new NullReferenceException($"Card {card} has no grabbable");
			
			ChoiceCard = card.gameObject.AddComponent<ChoiceCard>();
			ChoiceCard.target = cardTarget;
			ChoiceCard.point = point;
			ChoiceCard.slot = slot;
			ChoiceCard.onChosen = onChosen;
			grabbable.onDrop.AddListener((_, _) => ChoiceCard.ClampNewPosition());
		}
	}
}