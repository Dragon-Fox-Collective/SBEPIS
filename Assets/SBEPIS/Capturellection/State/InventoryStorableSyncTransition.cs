using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Transition/InventoryStorableSyncTransition")]
	public class InventoryStorableSyncTransition : StateBehaviour
	{
		[SerializeField] private FlexibleInventoryStorable inventoryStorable;
		
		[SerializeField] private StateLink onSyncAdded;
		[SerializeField] private StateLink onSyncRemoved;
		
		private bool addedListeners = false;
		
		public override void OnStateBegin()
		{
			if (inventoryStorable.value)
				AddListeners();
		}
		
		public override void OnStateEnd()
		{
			if (addedListeners)
				RemoveListeners();
		}
		
		private void AddListeners()
		{
			addedListeners = true;
			inventoryStorable.value.OnSyncAdded.AddListener(OnSyncAdded);
			inventoryStorable.value.OnSyncRemoved.AddListener(OnSyncRemoved);
		}
		
		private void RemoveListeners()
		{
			addedListeners = false;
			inventoryStorable.value.OnSyncAdded.RemoveListener(OnSyncAdded);
			inventoryStorable.value.OnSyncRemoved.RemoveListener(OnSyncRemoved);
		}
		
		private void OnSyncAdded() => Transition(onSyncAdded);
		private void OnSyncRemoved() => Transition(onSyncRemoved);
	}
}
