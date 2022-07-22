using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	public class MassiveBody : MonoBehaviour
	{
		public AnimationCurve gravityCurve;
		public bool useX;
		public bool useY;
		public bool useZ;
		public int priority = 0;
		public AnimationCurve priorityLerpX = AnimationCurve.Constant(0, 1, 1);
		public AnimationCurve priorityLerpY = AnimationCurve.Constant(0, 1, 1);
		public AnimationCurve priorityLerpZ = AnimationCurve.Constant(0, 1, 1);

		private AnimationCurve[] priorityLerps;

		private void Awake()
		{
			priorityLerps = new AnimationCurve[]{ priorityLerpX, priorityLerpY, priorityLerpZ };
		}

		public Vector3 GetGravity(Vector3 centerOfMass, Vector3 lowerPriorityGravity)
		{
			Vector3 localDelta = -transform.InverseTransformPoint(centerOfMass);

			Vector3 toggledLocalDelta = new(useX ? localDelta.x : 0, useY ? localDelta.y : 0, useZ ? localDelta.z : 0);
			Vector3 globalDelta = transform.TransformDirection(toggledLocalDelta);
			Vector3 gravity = gravityCurve.Evaluate(globalDelta.magnitude) * globalDelta.normalized;

			Vector3 priorityLerp = ExtensionMethods.OperateVectorIndex(i => priorityLerps[i].Evaluate(-localDelta[i]));
			
			return Vector3.LerpUnclamped(lowerPriorityGravity, gravity, priorityLerp.x * priorityLerp.y * priorityLerp.z);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.attachedRigidbody)
				return;

			GravitySum gravityNormalizer = other.attachedRigidbody.GetComponent<GravitySum>();
			if (gravityNormalizer)
				gravityNormalizer.Accumulate(this);
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.attachedRigidbody)
				return;

			GravitySum gravityNormalizer = other.attachedRigidbody.GetComponent<GravitySum>();
			if (gravityNormalizer)
				gravityNormalizer.Deaccumulate(this);
		}
	}
}
