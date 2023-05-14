using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("EnablerDisabler/DisableWhileInState")]
	[BehaviourTitle("DisableWhileInState")]
	public class EnablerDisablerDisableWhileInState : StateBehaviour
	{
		[SerializeField] private FlexibleEnablerDisabler enablerDisabler = new();
		
		public override void OnStateBegin()
		{
			enablerDisabler.value.Disable();
		}
		
		public override void OnStateEnd()
		{
			enablerDisabler.value.Enable();
		}
	}
}
