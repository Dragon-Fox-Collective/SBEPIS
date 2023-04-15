using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class Inventory : MonoBehaviour, IEnumerable<DequeStorable>
	{
		public Deque deque;
		[FormerlySerializedAs("cardPrefab")]
		public DequeStorable initialCardPrefab;
		public int initialCardCount = 0;
		
		[Tooltip("Purely organizational for the hierarchy")]
		public Transform cardParent;
		
		public UnityEvent<Inventory> onLoadIntoDeque = new();
		public UnityEvent<Inventory, List<DequeStorable>> onSaveFromDeque = new();
		
		private List<DequeStorable> savedInventory = new();
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
				DequeStorable card = Instantiate(initialCardPrefab);
				savedInventory.Add(card);
			}
		}
		
		private void LoadInventoryIntoDeque(StorableGroupDefinition definition, Transform ejectTransform)
		{
			storable = StorableGroupDefinition.GetNewStorable(definition);
			Load(savedInventory);
			foreach (DequeStorable card in savedInventory)
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

		private void SetupCard(DequeStorable card)
		{
			card.Deque = deque;
			card.transform.parent = cardParent;
		}
		private static void TearDownCard(DequeStorable card)
		{
			card.Deque = null;
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
		public void LayoutTarget(DequeStorable card, CardTarget target) => storable.LayoutTarget(card, target);
		public bool CanFetch(DequeStorable card) => storable.CanFetch(card);
		public UniTask<Capturellectable> Fetch(DequeStorable card)
		{
			TearDownCard(card);
			return storable.Fetch(card);
		}
		public async UniTask<(DequeStorable, Capturellectainer, Capturellectable)> Store(Capturellectable item)
		{
			(DequeStorable card, Capturellectainer container, Capturellectable ejectedItem) = await storable.Store(item);
			SetupCard(card);
			return (card, container, ejectedItem);
		}
		private void Load(List<DequeStorable> cards)
		{
			foreach (DequeStorable card in cards)
				SetupCard(card);
			storable.Load(cards);
		}
		private List<DequeStorable> Save()
		{
			List<DequeStorable> cards = new();
			storable.Save(cards);
			foreach (DequeStorable card in cards)
				TearDownCard(card);
			return cards;
		}
		public IEnumerable<Texture2D> GetCardTextures(DequeStorable card) => storable.GetCardTextures(card);
		public IEnumerator<DequeStorable> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
