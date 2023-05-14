using Arbor;
using UnityEngine;

namespace SBEPIS.Utils.State
{
	[AddBehaviourMenu("Transition/CaseComponentTransition")]
	public class CaseComponentTransition : StateBehaviour
	{
		[SerializeField] private FlexibleComponent value = new();
		
		[SerializeField] private StateLink onExist = new();
		[SerializeField] private StateLink onNull = new();
		
		public override void OnStateBegin() => Transition(value.value ? onExist : onNull);
	}
}
