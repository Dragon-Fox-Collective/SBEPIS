using SBEPIS.Capturellection.Deques;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class LaidOutDeque<TSettings, TLayout, TState> : SingleDeque<TState> where TSettings : LayoutSettings<TLayout>, new() where TLayout : DequeLayout where TState : InventoryState, DirectionState, new()
	{
		[SerializeField] private TSettings settings = new();
		protected TSettings Settings => settings;
		
		public override void Tick(TState state, float deltaTime) => settings.Layout.Tick(state.Inventory, state, deltaTime);
		public override void Layout(TState state) => settings.Layout.Layout(state.Inventory, state);
		public override Vector3 GetMaxPossibleSizeOf(TState state) => settings.Layout.GetMaxPossibleSizeOf(state.Inventory, state);
		
		protected override object DequeSettings => settings;
	}
}
