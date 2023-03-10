using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class StorableSlot : Storable
	{
		private DequeStorable _card;
		public DequeStorable card
		{
			get => _card;
			set
			{
				_card = value;

				if (card)
				{
					card.state.hasBeenAssembled = false;
					
					_maxPossibleSize = ExtensionMethods.Multiply(card.bounds.localBounds.size, card.bounds.transform.localScale);
				}
				else
				{
					_maxPossibleSize = Vector3.zero;
				}
			}
		}

		private Vector3 _maxPossibleSize;
		public override Vector3 maxPossibleSize => _maxPossibleSize;
		
		public override int inventoryCount => card ? 1 : 0;
		
		public override bool hasNoCards => !hasAllCards;
		public override bool hasAllCards => card;
		
		public override bool hasAllCardsEmpty => card && card.canStoreInto;
		public override bool hasAllCardsFull => !hasAllCardsEmpty;

		public override void Tick(float deltaTime) { }
		public override void LayoutTarget(DequeStorable card, CardTarget target)
		{
			if (Contains(card))
				target.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
		
		public override bool CanFetch(DequeStorable card) => Contains(card);
		public override bool Contains(DequeStorable card) => this.card == card;
		
		public override (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem)
		{
			ejectedItem = card.container.Fetch();
			card.container.Capture(item);
			return (card, card.container);
		}
		public override Capturllectable Fetch(DequeStorable card)
		{
			return Contains(card) ? card.container.Fetch() : null;
		}
		public override void Flush(List<DequeStorable> cards)
		{
			if (hasAllCards || cards.Count == 0)
				return;
			card = cards.Pop();
		}
		
		public override IEnumerable<Texture2D> GetCardTextures(DequeStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent)
		{
			return (textures.Skip(indexOfThisInParent).FirstOrDefault() ?? textures.Last())?.ToList();
		}
		
		public override IEnumerator<DequeStorable> GetEnumerator()
		{
			yield return card;
		}
	}
}
