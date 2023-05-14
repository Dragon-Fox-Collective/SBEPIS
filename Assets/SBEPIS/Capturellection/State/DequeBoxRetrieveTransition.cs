using Arbor;
using SBEPIS.Utils;
using SBEPIS.Utils.State;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Transition/DequeBox.Retrieve")]
	[BehaviourTitle("DequeBox.Retrieve")]
	public class DequeBoxRetrieveTransition : StateBehaviour
	{
		[SerializeField] private OutputSlotLerpTarget lerpTarget;
		[SerializeField] private StateLink onRetrieve;
		
		public void Retrieve(LerpTarget lerpTarget)
		{
			this.lerpTarget.SetValue(lerpTarget);
			Transition(onRetrieve);
		}
	}
}
