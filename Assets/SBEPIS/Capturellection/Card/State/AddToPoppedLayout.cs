using System;
using Arbor;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("DequeElement/AddToPoppedLayout")]
	public class AddToPoppedLayout : StateBehaviour
	{
		[SerializeField] private FlexibleInventoryStorable card;
		[SerializeField] private FlexibleLayoutAdder layoutAdder;
		
		public override void OnStateBegin()
		{
			if (!layoutAdder.value)
				throw new NullReferenceException($"{card.value} doesn't have a LayoutAdder but reached its state");
			
			DiajectorCaptureLayout layout = layoutAdder.value.PopAllLayouts();
			layout.inventory.FlushCard(card.value).Forget();
			CardTarget target = layout.AddPermanentTargetAndCard(card.value);
			card.value.DequeElement.Animator.TeleportTo(target.LerpTarget);
		}
	}
}
