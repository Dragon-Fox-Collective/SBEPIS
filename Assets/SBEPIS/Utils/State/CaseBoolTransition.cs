using Arbor;
using UnityEngine;

namespace SBEPIS.Utils.State
{
	[AddBehaviourMenu("Transition/CaseBoolTransition")]
	public class CaseBoolTransition : StateBehaviour
	{
		[SerializeField] private FlexibleBool value;
		
		[SerializeField] private StateLink onTrue = new(){ transitionTiming = TransitionTiming.Immediate };
		[SerializeField] private StateLink onFalse = new(){ transitionTiming = TransitionTiming.Immediate };
		
		public override void OnStateBegin() => Transition(value.value ? onTrue : onFalse);
	}
}
