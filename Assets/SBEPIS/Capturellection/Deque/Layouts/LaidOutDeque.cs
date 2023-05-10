using SBEPIS.Capturellection.Deques;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class LaidOutDeque<TLayout, TState> : SingleDeque<TLayout, TState> where TLayout : DequeLayout where TState : InventoryState, DirectionState, new()
	{
		[SerializeField] private TLayout layout;
		
		public override void Tick(TState state, float deltaTime) => layout.Tick(state.Inventory, state, deltaTime);
		public override Vector3 GetMaxPossibleSizeOf(TState state) => layout.GetMaxPossibleSizeOf(state.Inventory, state);
		
		protected override TLayout SettingsPageLayoutData => layout;
	}
}
