using System;
using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DequeElement/StartDisassembling")]
	[BehaviourTitle("StartDisassembling")]
	public class DequeElementStartDisassembling : StateBehaviour
	{
		[SerializeField] private FlexibleDequeElement dequeElement;
		
		public override void OnStateBegin() => dequeElement.value.StartDisassembling();
	}
}
