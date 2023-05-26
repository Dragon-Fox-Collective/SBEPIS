using System;
using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/AddToPoppedLayout")]
	[BehaviourTitle("AddToPoppedLayout")]
	public class DequeElementAddToPoppedLayout : StateBehaviour
	{
		[SerializeField] private FlexibleInventoryStorable card;
		[SerializeField] private FlexibleLayoutAdder layoutAdder;
		
		public override void OnStateBegin()
		{
			if (!layoutAdder.value)
				throw new NullReferenceException($"{card.value} doesn't have a LayoutAdder but reached its state");
			
			DiajectorCaptureLayout layout = layoutAdder.value.PopAllLayouts();
			layout.Inventory.Load(card.value);
			CardTarget target = layout.AddPermanentTargetAndCard(card.value);
			card.value.DequeElement.Animator.TeleportTo(target.LerpTarget);
		}
	}
}
