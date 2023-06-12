using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public abstract class LaidOutRuleset<TSettings, TLayout, TState> : SingleRuleset<TState> where TSettings : LayoutSettings<TLayout>, new() where TLayout : DequeLayout where TState : InventoryState, DirectionState, new()
	{
		[SerializeField] private TSettings settings = new();
		protected TSettings Settings => settings;
		
		protected override void Tick(TState state, float deltaTime) => settings.Layout.Tick(state.Inventory, state, deltaTime);
		protected override void Layout(TState state) => settings.Layout.Layout(state.Inventory, state);
		protected override Vector3 GetMaxPossibleSizeOf(TState state) => settings.Layout.GetMaxPossibleSizeOf(state.Inventory, state);
		
		protected override object DequeSettings => settings;
	}
	
	public interface LayoutSettings<out TLayout>
	{
		public TLayout Layout { get; }
	}
}
