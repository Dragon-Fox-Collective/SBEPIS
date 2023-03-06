using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
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
				}
			}
		}
		
		public override bool hasNoCards => !hasAllCards;
		public override bool hasAllCards => card;
		
		public override bool hasAllCardsEmpty => card && card.canStoreInto;
		public override bool hasAllCardsFull => !hasAllCardsEmpty;

		public override void Tick(float deltaTime) { }
		public override void Layout()
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
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

		public override DequeStorable Flush(DequeStorable card)
		{
			if (hasAllCards)
				return card;
			this.card = card;
			return null;
		}
		
		public override IEnumerable<DequeStorable> Save() => Enumerable.Repeat(card, 1);
		public override IEnumerable<DequeStorable> Load(IEnumerable<DequeStorable> newInventory)
		{
			(DequeStorable newCard, IEnumerable<DequeStorable> rtn) = newInventory.Pop();
			card = newCard;
			return rtn;
		}
		public override void Clear() => card = null;
	}
}
