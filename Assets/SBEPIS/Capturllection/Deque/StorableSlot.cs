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

		public override Vector3 position { get; set; }
		public override Quaternion rotation { get; set; }
		
		public override bool isEmpty => card;

		public StorableSlot(DequeStorable card)
		{
			this.card = card;
		}
		
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

		public override IEnumerable<DequeStorable> Save() => Enumerable.Repeat(card, 1);
		public override void Load(IEnumerable<DequeStorable> inventory) => card = inventory.First();
		public override void Clear() => card = null;
	}
}
