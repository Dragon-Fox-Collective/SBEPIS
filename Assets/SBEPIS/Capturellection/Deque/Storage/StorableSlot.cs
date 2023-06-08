using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public class StorableSlot : MonoBehaviour, Storable
	{
		private DequeElement dequeElement;
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
		
		public int NumEmptySlots => card ? 0 : 1;
		
		public bool HasNoCards => !HasAllCards;
		public bool HasAllCards => card;
		
		public bool HasAllCardsEmpty => container && container.IsEmpty;
		public bool HasAllCardsFull => !HasAllCardsEmpty;
		
		public void InitPage(DiajectorPage page) { }
		
		public void Tick(float deltaTime) { }
		public void Layout(Vector3 direction) { }
		public void LayoutTarget(InventoryStorable card, CardTarget target)
		{
			if (Contains(card))
				target.transform.SetPositionAndRotation(transform.position, transform.rotation);
		}
		
		public bool CanFetch(InventoryStorable card) => Contains(card);
		public bool Contains(InventoryStorable card) => this.card == card;
		
		public UniTask<StoreResult> StoreItem(Capturellectable item)
		{
			if (!container) return UniTask.FromResult(new StoreResult());
			
			Capturellectable ejectedItem = container.Fetch();
			container.Capture(item);
			StoreResult res = new(card, container, ejectedItem);
			return UniTask.FromResult(res);
		}
		
		public UniTask<FetchResult> FetchItem(InventoryStorable card)
		{
			if (!container) return UniTask.FromResult(new FetchResult());
			
			FetchResult res = new(Contains(card) ? container.Fetch() : null);
			return UniTask.FromResult(res);
		}
		
		public UniTask Interact<TState>(InventoryStorable card, DequeRuleset targetDeque, DequeInteraction<TState> action) =>
			throw new ArgumentException($"Interacting with {card} never hit {targetDeque}");
		
		public void LoadInit(List<InventoryStorable> cards) => Load(cards);
		public void Load(List<InventoryStorable> cards)
		{
			if (HasAllCards || cards.Count == 0) return;
			Load(cards.Pop());
		}
		public void Load(InventoryStorable card)
		{
			if (HasAllCards) throw new InvalidOperationException($"Slot {this} is already full but tried to load {card}");
			this.card = card;
			container = card.GetComponent<CaptureContainer>();
			MaxPossibleSize = card.DequeElement.Size;
		}
		public IEnumerable<InventoryStorable> Save()
		{
			if (HasNoCards) yield break;
			yield return card;
			Destroy(gameObject);
		}
		
		public Storable GetNewStorableLikeThis() => StorableGroupDefinition.GetNewStorable(null);
		
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card, IEnumerable<IEnumerable<Texture2D>> textures, int indexOfThisInParent) => textures.ElementAtOrLast(indexOfThisInParent);
		
		public IEnumerator<InventoryStorable> GetEnumerator()
		{
			yield return card;
		}
		
		private void OnDrawGizmosSelected() => Storable.DrawSize(MaxPossibleSize, transform, Color.cyan);
	}
}
