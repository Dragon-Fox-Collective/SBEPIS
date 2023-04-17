using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementTargetIndexSetter : StateMachineBehaviour<DequeElementStateMachine>
	{
		[Tooltip("Negative values count from the end of the list")]
		[SerializeField] private int index;
		
		protected override void OnEnter()
		{
			State.TargetIndex = index >= 0 ? index : State.Card.Diajector.LerpTargetCount + index - 1;
		}
	}
}
