using Arbor;
using UnityEngine;

namespace SBEPIS.Physics.State
{
	[AddBehaviourMenu("JointTargetter/JointTargetter.Create")]
	[BehaviourTitle("JointTargetter.Create")]
	public class JointTargetterCreate : StateBehaviour
	{
		[SerializeField] private FlexibleRigidbody staticRigidbody;
		[SerializeField] private FlexibleTransform target;
		[SerializeField] private new FlexibleRigidbody rigidbody;
		[SerializeField] private FlexibleStrengthSettings strengthSettings;
		
		private JointTargetter targetter;
		
		public override void OnStateBegin()
		{
			if (!rigidbody.value)
				return;
			
			targetter = staticRigidbody.value.gameObject.AddComponent<JointTargetter>();
			targetter.Init(rigidbody.value, target.value, strengthSettings.value);
		}
		
		public override void OnStateEnd()
		{
			if (targetter)
				Destroy(targetter);
		}
	}
}
