using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableSlot : Storable
	{
		private InventoryStorable card;
		private CaptureContainer container;
		
		private Vector3 maxPossibleSize;
		public override Vector3 MaxPossibleSize => maxPossibleSize;
		
		public override int InventoryCount => card ? 1 : 0;
		
		public override bool HasNoCards => !HasAllCards;
		public override bool HasAllCards => card;
		
		public override bool HasAllCardsEmpty => container && container.IsEmpty;
		public override bool HasAllCardsFull => !HasAllCardsEmpty;

		public override void Tick(float deltaTime) { }
		public override void LayoutTarget(InventoryStorable card, CardTarget target)
		{
			if (Contains(card))
				target.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
		
		public override bool CanFetch(InventoryStorable card) => Contains(card);
		public override bool Contains(InventoryStorable card) => this.card == card;
		
		public override UniTask<(InventoryStorable, CaptureContainer, Capturellectable)> Store(Capturellectable item)
		{
			if (!container)
				throw new NullReferenceException($"Tried to store in a card {card} that has no container");
			
			Capturellectable ejectedItem = container.Fetch();
			container.Capture(item);
			return UniTask.FromResult((card, container, ejectedItem));
		}
		public override UniTask<Capturellectable> Fetch(InventoryStorable card)
		{
			if (!container)
				throw new NullReferenceException($"Tried to fetch from a card {this.card} that has no container");
			
			return UniTask.FromResult(Contains(card) ? container.Fetch() : null);
		}
		public override UniTask Flush(List<InventoryStorable> cards)
		{
			Load(cards);
			return UniTask.FromResult(0);
		}
		
		public override void Load(List<InventoryStorable> cards)
		{
			if (HasAllCards || cards.Count == 0)
				return;
			card = cards.Pop();
			container = card.GetComponent<CaptureContainer>();
			maxPossibleSize = card.DequeElement.bounds ? ExtensionMethods.Multiply(card.DequeElement.bounds.localBounds.size, card.DequeElement.bounds.transform.localScale) : Vector3.zero;
		}
		public override void Save(List<InventoryStorable> cards)
		{
			if (HasNoCards)
				return;
			cards.Add(card);
			card = null;
			container = null;
			maxPossibleSize = Vector3.zero;
		}
		
		public override IEnumerable<Texture2D> GetCardTextures(InventoryStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent) => textures.ElementAtOrLast(indexOfThisInParent);
		
		public override IEnumerator<InventoryStorable> GetEnumerator()
		{
			yield return card;
		}
	}
}
