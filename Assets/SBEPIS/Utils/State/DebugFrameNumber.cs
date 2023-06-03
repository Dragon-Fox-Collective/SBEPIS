using Arbor;
using UnityEngine;

namespace SBEPIS.Utils.State
{
	[AddBehaviourMenu("Transition/DebugFrameNumber")]
	public class DebugFrameNumber : StateBehaviour
	{
		public override void OnStateBegin() => Debug.Log($"{node}: {Time.frameCount}", this);
	}
}
