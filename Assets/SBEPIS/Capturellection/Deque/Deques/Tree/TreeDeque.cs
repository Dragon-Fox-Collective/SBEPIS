using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class TreeDeque : LaidOutDeque<TreeSettings, TreeLayout, TreeState>
	{
		private const string Key = "bepis";
		
		public override bool CanFetch(TreeState state, InventoryStorable card) => state.Tree.ContainsValue(StorableWithCard(state, card));
		
		public override UniTask<StoreResult> StoreItem(TreeState state, Capturellectable item)
		{
			if (TryStoreInExistingTreeSlotIfKeyExists(out UniTask<StoreResult> res)) return res;
			else if (TryStoreInEmptyCard(out res)) return res;
			else return EjectWholeTreeAndStoreInEmptyCard();
			
			bool TryStoreInExistingTreeSlotIfKeyExists(out UniTask<StoreResult> result)
			{
				if (state.Tree.TryGetValue(Key, out Storable storable))
				{
					result = storable.StoreItem(item);
					return true;
				}
				else
				{
					result = default;
					return false;
				}
			}
			
			bool TryStoreInEmptyCard(out UniTask<StoreResult> result)
			{
				Storable storable = state.Inventory.Except(state.Tree).FirstOrDefault(storable => !storable.HasAllCardsFull);
				if (storable != null)
				{
					result = storable.StoreItem(item);
					return true;
				}
				else
				{
					result = default;
					return false;
				}
			}
			
			UniTask<StoreResult> EjectWholeTreeAndStoreInEmptyCard()
			{
				foreach (Storable storable in state.Tree.DropRoot(Settings.Balance))
					storable.Eject();
				
				return state.Inventory.First(storable => !storable.HasAllCardsFull).StoreItem(item);
			}
		}
		
		public override async UniTask<FetchResult> FetchItem(TreeState state, InventoryStorable card)
		{
			Storable storable = StorableWithCard(state, card);
			FetchResult result = await storable.FetchItem(card);
			if (storable.HasAllCardsEmpty)
				foreach (Storable droppedStorable in state.Tree.Drop(storable, Settings.Balance))
					droppedStorable.Eject();
			return result;
		}
		
		public override UniTask<StoreResult> StoreItemHook(TreeState state, Capturellectable item, StoreResult oldResult)
		{
			Storable storable = StorableWithCard(state, oldResult.card);
			if (!state.Tree.ContainsValue(storable))
				state.Tree.Add(Key, storable, Settings.Balance);
			return UniTask.FromResult(oldResult);
		}
	}
	
	public class TreeState : InventoryState, DirectionState, TreeDictionaryState
	{
		public CallbackList<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public TreeDictionary<string, Storable> Tree { get; } = new();
	}
	
	[Serializable]
	public class TreeSettings : LayoutSettings<TreeLayout>, TreeBalanceSettings
	{
		[SerializeField] private TreeLayout layout;
		public TreeLayout Layout => layout;
		[SerializeField] private bool balance = true;
		public bool Balance
		{
			get => balance;
			set => balance = value;
		}
	}
	
	public interface TreeBalanceSettings
	{
		public bool Balance { get; set; }
	}
}
