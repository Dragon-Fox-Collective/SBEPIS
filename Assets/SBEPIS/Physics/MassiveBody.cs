using UnityEngine;

namespace SBEPIS.Physics
{
	public abstract class MassiveBody : MonoBehaviour
	{
		public int priority;
		
		public abstract Vector3 GetPriority(Vector3 centerOfMass);
		public abstract Vector3 GetGravity(Vector3 centerOfMass);
		
		private void OnTriggerEnter(Collider other)
		{
			if (!other.attachedRigidbody)
				return;
			
			GravitySum gravitySum = other.attachedRigidbody.GetComponent<GravitySum>();
			if (gravitySum)
				gravitySum.Accumulate(this);
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (!other.attachedRigidbody)
				return;
			
			GravitySum gravitySum = other.attachedRigidbody.GetComponent<GravitySum>();
			if (gravitySum)
				gravitySum.Deaccumulate(this);
		}
	}
}
