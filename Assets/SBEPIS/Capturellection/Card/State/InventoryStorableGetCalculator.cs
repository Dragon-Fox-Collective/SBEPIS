using System;
using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("InventoryStorable/InventoryStorable.Get")]
	[BehaviourTitle("InventoryStorable.Get")]
	public class InventoryStorableGetCalculator : GetCalculator<InventoryStorable, OutputSlotInventoryStorable> { }
	
	[Serializable]
	public class OutputSlotInventoryStorable : OutputSlot<InventoryStorable> { }
	
	[Serializable]
	public class FlexibleInventoryStorable : FlexibleComponent<InventoryStorable> { }
}
