using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("EnablerDisabler/Disable")]
	public class Disable : StateBehaviour
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
