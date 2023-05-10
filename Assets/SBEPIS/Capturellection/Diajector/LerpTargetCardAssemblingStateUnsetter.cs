using SBEPIS.Capturellection.CardState;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class LerpTargetCardAssemblingStateUnsetter : MonoBehaviour
	{
		public void UnsetIsAssemblingAndIsDisassembling(LerpTargetAnimator animator)
		{
			if (!animator.TryGetComponent(out DequeElementStateMachine state))
				return;
			
			state.IsAssembling = false;
			state.IsDisassembling = false;
		}
	}
}
