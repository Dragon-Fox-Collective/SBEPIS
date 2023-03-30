using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
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
					_maxPossibleSize = ExtensionMethods.Multiply(card.bounds.localBounds.size, card.bounds.transform.localScale);
				}
				else
				{
					_maxPossibleSize = Vector3.zero;
				}
			}
		}

		private Vector3 _maxPossibleSize;
		public override Vector3 MaxPossibleSize => _maxPossibleSize;
		
		public override int InventoryCount => card ? 1 : 0;
		
		public override bool HasNoCards => !HasAllCards;
		public override bool HasAllCards => card;
		
		public override bool HasAllCardsEmpty => card && card.CanStoreInto;
		public override bool HasAllCardsFull => !HasAllCardsEmpty;

		public override void Tick(float deltaTime) { }
		public override void LayoutTarget(DequeStorable card, CardTarget target)
		{
			if (Contains(card))
				target.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
		
		public override bool CanFetch(DequeStorable card) => Contains(card);
		public override bool Contains(DequeStorable card) => this.card == card;
		
		public override UniTask<(DequeStorable, Capturellectainer, Capturellectable)> Store(Capturellectable item)
		{
			Capturellectable ejectedItem = card.Container.Fetch();
			card.Container.Capture(item);
			return UniTask.FromResult((card, container: card.Container, ejectedItem));
		}
		public override UniTask<Capturellectable> Fetch(DequeStorable card)
		{
			return UniTask.FromResult(Contains(card) ? card.Container.Fetch() : null);
		}
		public override UniTask Flush(List<DequeStorable> cards)
		{
			Load(cards);
			return UniTask.FromResult(0);
		}
		
		public override void Load(List<DequeStorable> cards)
		{
			if (HasAllCards || cards.Count == 0)
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
