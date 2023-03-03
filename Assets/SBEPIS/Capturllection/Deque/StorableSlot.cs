using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class StorableSlot : Storable
	{
		public DequeStorable card;
		public CardTarget target;
		
		public override Vector3 position
		{
			get => target.transform.localPosition;
			set => target.transform.localPosition = value;
		}
		
		public override Quaternion rotation
		{
			get => target.transform.localRotation;
			set => target.transform.localRotation = value;
		}

		public override bool isEmpty => card;

		public override void Tick(float deltaTime) { }

		public override void Layout()
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
		}

		public override bool CanFetch(DequeStorable card) => this.card == card;
		
		public override (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem)
		{
			Capturellectainer container = card.GetComponent<Capturellectainer>();
			ejectedItem = container.Fetch();
			container.Capture(item);
			return (card, container);
		}

		public override Capturllectable Fetch(DequeStorable card)
		{
			if (!CanFetch(card))
				return null;
			
			return card.GetComponent<Capturellectainer>().Fetch();
		}

		public override void Flush(DequeStorable card)
		{
			throw new InvalidOperationException("Can't flush into a single slot");
		}
	}
}
