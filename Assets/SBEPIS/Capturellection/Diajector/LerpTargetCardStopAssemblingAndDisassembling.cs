using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class LerpTargetCardStopAssemblingAndDisassembling : MonoBehaviour
	{
		public void UnsetIsAssemblingAndIsDisassembling(LerpTargetAnimator animator)
		{
			if (!animator.TryGetComponent(out DequeElement card))
				return;
			
			card.OnStopAssemblingAndDisassembling();
		}
	}
}
