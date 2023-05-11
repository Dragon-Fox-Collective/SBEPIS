using Arbor;
using SBEPIS.Physics;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("JointTargetter/CreateJointTargetter")]
	public class CreateJointTargetter : StateBehaviour
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
