using Arbor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("Transition/InvokeTransition")]
	public class InvokeTransition : StateBehaviour
	{
		[FormerlySerializedAs("onTrigger")]
		[SerializeField] private StateLink onInvoke = new();
		
		public void Invoke() => Transition(onInvoke);
	}
}
