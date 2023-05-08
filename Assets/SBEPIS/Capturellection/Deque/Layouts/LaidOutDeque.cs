using System.Collections.Generic;
using SBEPIS.Capturellection.Deques;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public abstract class LaidOutDeque<TLayout, TState> : SingleDeque<TLayout, TState> where TLayout : DequeLayout where TState : DirectionState, new()
	{
		[SerializeField] private TLayout layout;
		
		public override void Tick(List<Storable> inventory, TState state, float deltaTime) => layout.Tick(inventory, state, deltaTime);
		public override Vector3 GetMaxPossibleSizeOf(List<Storable> inventory, TState state) => layout.GetMaxPossibleSizeOf(inventory, state);
		
		protected override TLayout SettingsPageLayoutData => layout;
	}
}
