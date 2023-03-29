using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class StorableSlot : Storable
	{
		private Card _card;
		public Card card
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
		public override Vector3 maxPossibleSize => _maxPossibleSize;
		
		public override int inventoryCount => card ? 1 : 0;
		
		public override bool hasNoCards => !hasAllCards;
		public override bool hasAllCards => card;
		
		public override bool hasAllCardsEmpty => card && card.CanStoreInto;
		public override bool hasAllCardsFull => !hasAllCardsEmpty;

		public override void Tick(float deltaTime) { }
		public override void LayoutTarget(Card card, CardTarget target)
		{
			if (Contains(card))
				target.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
		
		public override bool CanFetch(Card card) => Contains(card);
		public override bool Contains(Card card) => this.card == card;
		
		public override UniTask<(Card, Capturellectainer, Capturellectable)> Store(Capturellectable item)
		{
			Capturellectable ejectedItem = card.Container.Fetch();
			card.Container.Capture(item);
			return UniTask.FromResult((card, container: card.Container, ejectedItem));
		}
		public override UniTask<Capturellectable> Fetch(Card card)
		{
			return UniTask.FromResult(Contains(card) ? card.Container.Fetch() : null);
		}
		public override UniTask Flush(List<Card> cards)
		{
			Load(cards);
			return UniTask.FromResult(0);
		}
		
		public override void Load(List<Card> cards)
		{
			if (hasAllCards || cards.Count == 0)
				return;
			card = cards.Pop();
		}
		
		public override IEnumerable<Texture2D> GetCardTextures(Card card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent)
		{
			return (textures.Skip(indexOfThisInParent).FirstOrDefault() ?? textures.Last())?.ToList();
		}
		
		public override IEnumerator<Card> GetEnumerator()
		{
			yield return card;
		}
	}
}
