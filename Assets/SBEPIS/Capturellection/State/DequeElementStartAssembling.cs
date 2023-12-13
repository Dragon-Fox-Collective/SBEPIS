using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/StartAssembling")]
	[BehaviourTitle("StartAssembling")]
	public class DequeElementStartAssembling : StateBehaviour
	{
		[SerializeField] private FlexibleDequeElement dequeElement;
		
		public override void OnStateBegin() => dequeElement.value.StartAssembling();
	}
}
