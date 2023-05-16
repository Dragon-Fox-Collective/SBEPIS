using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableSlot : MonoBehaviour, Storable
	{
		private InventoryStorable card;
		private CaptureContainer container;
		
		public Vector3 Position
		{
			get => transform.localPosition;
			set => transform.localPosition = value;
		}
		public Quaternion Rotation
		{
			get => transform.localRotation;
			set => transform.localRotation = value;
		}
		public Transform Parent
		{
			set => transform.SetParent(value);
			get => transform.parent;
		}
		
		public Vector3 MaxPossibleSize { get; private set; }
		
		public int InventoryCount => card ? 1 : 0;
		
		public bool HasNoCards => !HasAllCards;
		public bool HasAllCards => card;
		
		public bool HasAllCardsEmpty => container && container.IsEmpty;
		public bool HasAllCardsFull => !HasAllCardsEmpty;
		
		public void SetupPage(DiajectorPage page) { }
		
		public void Tick(float deltaTime) { }
		public void Layout(Vector3 direction) { }
		public void LayoutTarget(InventoryStorable card, CardTarget target)
		{
			if (Contains(card))
				target.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
		
		public bool CanFetch(InventoryStorable card) => Contains(card);
		public bool Contains(InventoryStorable card) => this.card == card;
		
		public UniTask<StorableStoreResult> StoreItem(Capturellectable item)
		{
			if (!container) throw new NullReferenceException($"Tried to store in a card {card} that has no container");
			
			Capturellectable ejectedItem = container.Fetch();
			container.Capture(item);
			return UniTask.FromResult(new StorableStoreResult(card, container, ejectedItem));
		}
		
		public UniTask<Capturellectable> FetchItem(InventoryStorable card)
		{
			if (!container) throw new NullReferenceException($"Tried to fetch from a card {this.card} that has no container");
			
			return UniTask.FromResult(Contains(card) ? container.Fetch() : null);
		}
		
		public UniTask FlushCards(List<InventoryStorable> cards)
		{
			Load(cards);
			return UniTask.CompletedTask;
		}
		
		public UniTask<InventoryStorable> FetchCard(InventoryStorable card)
		{
			if (!Contains(card)) return UniTask.FromResult(card);
			
			this.card = null;
			container = null;
			MaxPossibleSize = Vector3.zero;
			return UniTask.FromResult(card);
		}
		
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) =>
			throw new ArgumentException($"Interacting with {card} never hit {targetDeque}");
		
		public void Load(List<InventoryStorable> cards)
		{
			if (HasAllCards || cards.Count == 0) return;
			card = cards.Pop();
			container = card.GetComponent<CaptureContainer>();
			MaxPossibleSize = card.DequeElement.Size;
		}
		public void Save(List<InventoryStorable> cards)
		{
			if (HasNoCards) return;
			cards.Add(card);
			Destroy(gameObject);
		}
		
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent) => textures.ElementAtOrLast(indexOfThisInParent);
		
		public IEnumerator<InventoryStorable> GetEnumerator()
		{
			yield return card;
		}
		
		private void OnDrawGizmosSelected() => Storable.DrawSize(MaxPossibleSize, transform, Color.cyan);
	}
}
