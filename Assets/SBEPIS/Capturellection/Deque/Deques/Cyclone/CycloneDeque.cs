using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class CycloneDeque : SingleDeque<CycloneLayout, CycloneState>
	{
		[SerializeField] private CycloneLayout layout;

		public override void Tick(List<Storable> inventory, CycloneState state, float deltaTime) => layout.Tick(inventory, state, deltaTime);
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, CycloneState state)
		{
			List<Vector3> sizes = inventory.Select(storable => storable.MaxPossibleSize).ToList();
			float longestEdge = sizes.Select(size => size.x).Aggregate(Mathf.Max);
			float innerRadius = CycloneLayout.DistanceToRegularPolygonEdge(inventory.Count, longestEdge);
			Vector3 maxSize = sizes.Aggregate(ExtensionMethods.Max);
			Vector3 sumSize = new(
				(innerRadius + maxSize.y) * 2,
				(innerRadius + maxSize.y) * 2,
				maxSize.z);
			return sumSize;
		}

		public override bool CanFetchFrom(List<Storable> inventory, CycloneState state, InventoryStorable card) => state.TopStorable.CanFetch(card);
		
		public override async UniTask<DequeStoreResult> StoreItem(List<Storable> inventory, CycloneState state, Capturellectable item)
		{
			int index = inventory.IndexOf(state.TopStorable);
			if (index < 0) index = Mathf.Max(inventory.FindIndex(storable => !storable.HasAllCardsFull), 0);
			Storable storable = inventory[index];
			StorableStoreResult res = await storable.StoreItem(item);
			return res.ToDequeResult(index, storable);
		}
		
		public override UniTask FlushCard(List<Storable> inventory, CycloneState state, Storable storable)
		{
			int index = inventory.IndexOf(state.TopStorable);
			if (index < 0) index = inventory.Count;
			inventory.Insert(index, storable);
			return UniTask.CompletedTask;
		}
		
		protected override CycloneLayout SettingsPageLayoutData => layout;
	}
}
