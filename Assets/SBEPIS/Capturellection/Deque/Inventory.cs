using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class Inventory : MonoBehaviour, IEnumerable<InventoryStorable>
	{
		[SerializeField, Anywhere] public Deque deque;
		[FormerlySerializedAs("cardPrefab")]
		[SerializeField, Anywhere] private InventoryStorable initialCardPrefab;
		[SerializeField] public int initialCardCount = 0;
		
		[Tooltip("Purely organizational for the hierarchy")]
		[SerializeField, Anywhere] private Transform cardParent;
		public Transform CardParent => cardParent;
		
		public UnityEvent<Inventory> onLoadIntoDeque = new();
		public UnityEvent<Inventory, List<InventoryStorable>> onSaveFromDeque = new();
		
		private List<InventoryStorable> savedInventory = new();
		private Storable storable;
		
		private void Awake()
		{
			SaveInitialInventory();
			LoadInventoryIntoDeque(deque.definition, deque.transform);
		}
		
		private void SaveInitialInventory()
		{
			for (int _ = 0; _ < initialCardCount; _++)
			{
				InventoryStorable card = Instantiate(initialCardPrefab);
				savedInventory.Add(card);
			}
		}
		
		private void LoadInventoryIntoDeque(StorableGroupDefinition definition, Transform ejectTransform)
		{
			storable = StorableGroupDefinition.GetNewStorable(definition);
			Load(savedInventory);
			foreach (InventoryStorable card in savedInventory)
			{
				print($"Ejecting leftover card {card}");
				card.gameObject.SetActive(true);
				card.transform.SetPositionAndRotation(ejectTransform.position, ejectTransform.rotation);
			}
			savedInventory.Clear();
			onLoadIntoDeque.Invoke(this);
		}
		
		private void SaveInventoryFromDeque()
		{
			savedInventory = Save();
			Destroy(storable.gameObject);
			onSaveFromDeque.Invoke(this, savedInventory);
		}

		private void SetupCard(InventoryStorable card)
		{
			card.Inventory = this;
			card.transform.parent = cardParent;
		}
		private static void TearDownCard(InventoryStorable card)
		{
			card.Inventory = null;
			card.transform.parent = null;
		}
		
		public void SetStorableParent(Transform transform) => storable.transform.SetParent(transform);
		
		public Vector3 Direction
		{
			get => storable.state.direction;
			set => storable.state.direction = value;
		}
		public Vector3 Position
		{
			get => storable.Position;
			set => storable.Position = value;
		}
		public Quaternion Rotation
		{
			get => storable.Rotation;
			set => storable.Rotation = value;
		}
		public Vector3 MaxPossibleSize => storable.MaxPossibleSize;
		public void Tick(float deltaTime) => storable.Tick(deltaTime);
		public void LayoutTarget(InventoryStorable card, CardTarget target) => storable.LayoutTarget(card, target);
		public bool CanFetch(InventoryStorable card) => storable.CanFetch(card);
		public UniTask<Capturellectable> Fetch(InventoryStorable card)
		{
			TearDownCard(card);
			return storable.Fetch(card);
		}
		public async UniTask<(InventoryStorable, CaptureContainer, Capturellectable)> Store(Capturellectable item)
		{
			(InventoryStorable card, CaptureContainer container, Capturellectable ejectedItem) = await storable.Store(item);
			SetupCard(card);
			return (card, container, ejectedItem);
		}
		public UniTask Flush(List<InventoryStorable> cards)
		{
			foreach (InventoryStorable card in cards)
				SetupCard(card);
			return storable.Flush(cards);
		}
		public UniTask Flush(InventoryStorable card) => Flush(new List<InventoryStorable>{ card });
		private void Load(List<InventoryStorable> cards)
		{
			foreach (InventoryStorable card in cards)
				SetupCard(card);
			storable.Load(cards);
		}
		private List<InventoryStorable> Save()
		{
			List<InventoryStorable> cards = new();
			storable.Save(cards);
			foreach (InventoryStorable card in cards)
				TearDownCard(card);
			return cards;
		}
		public IEnumerable<Texture2D> GetCardTextures(InventoryStorable card) => storable.GetCardTextures(card);
		public IEnumerator<InventoryStorable> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
