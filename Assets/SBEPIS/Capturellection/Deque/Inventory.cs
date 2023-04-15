using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	public class Inventory : MonoBehaviour, IEnumerable<DequeStorable>
	{
		[SerializeField]
		private Deque deque;
		[SerializeField]
		private DequeStorable cardPrefab;
		[SerializeField]
		private int initialCardCount = 0;
		
		[Tooltip("Purely organizational for the hierarchy")]
		[SerializeField]
		private Transform cardParent;
		
		public UnityEvent<Inventory> onLoadIntoDeque = new();
		public UnityEvent<Inventory, List<DequeStorable>> onSaveFromDeque = new();
		
		private List<DequeStorable> savedInventory = new();
		private Storable storable;
		
		private void Awake()
		{
			SaveInitialInventory();
		}
		
		private void SaveInitialInventory()
		{
			for (int _ = 0; _ < initialCardCount; _++)
			{
				DequeStorable card = Instantiate(cardPrefab);
				savedInventory.Add(card);
			}
		}
		
		private void LoadInventoryIntoDeque(Deque deque)
		{
			storable = StorableGroupDefinition.GetNewStorable(deque.definition);
			foreach (DequeStorable card in savedInventory)
				card.Deque = deque;
			storable.Load(savedInventory);
			foreach (DequeStorable card in savedInventory)
			{
				print($"Ejecting leftover card {card}");
				card.gameObject.SetActive(true);
				card.transform.SetPositionAndRotation(deque.transform.position, deque.transform.rotation);
			}
			savedInventory.Clear();
			onLoadIntoDeque.Invoke(this);
		}
		
		private void SaveInventoryFromDeque()
		{
			savedInventory = storable.ToList();
			Destroy(storable.gameObject);
			foreach (DequeStorable card in savedInventory)
				card.Deque = null;
			onSaveFromDeque.Invoke(this, savedInventory);
		}
		
		private void SetCardParent(DequeStorable card) => card.transform.SetParent(cardParent);
		private static void UnsetCardParent(DequeStorable card) => card.transform.SetParent(null);
		
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
		public UniTask<Capturellectable> Fetch(DequeStorable card) => storable.Fetch(card);
		public async UniTask<(DequeStorable, Capturellectainer, Capturellectable)> Store(Capturellectable item)
		{
			(DequeStorable card, Capturellectainer container, Capturellectable ejectedItem) = await storable.Store(item);
			card.Deque = deque;
			return (card, container, ejectedItem);
		}
		public IEnumerable<Texture2D> GetCardTextures(DequeStorable card) => storable.GetCardTextures(card);
		public IEnumerator<DequeStorable> GetEnumerator() => storable.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
