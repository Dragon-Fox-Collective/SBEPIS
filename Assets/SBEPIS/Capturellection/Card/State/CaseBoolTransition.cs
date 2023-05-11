using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("Transition/CaseBoolTransition")]
	public class CaseBoolTransition : StateBehaviour
	{
		[SerializeField] private FlexibleBool value = new();
		
		[SerializeField] private StateLink onTrue = new();
		[SerializeField] private StateLink onFalse = new();
		
		public override void OnStateBegin() => Transition(value.value ? onTrue : onFalse);
	}
}
