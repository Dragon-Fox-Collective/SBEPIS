using Arbor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("Transition/InvokeTransition")]
	public class InvokeTransition : StateBehaviour
	{
		[SerializeField] private StateLink onInvoke = new(){ transitionTiming = TransitionTiming.Immediate };
		
		public void Invoke() => Transition(onInvoke);
	}
}
