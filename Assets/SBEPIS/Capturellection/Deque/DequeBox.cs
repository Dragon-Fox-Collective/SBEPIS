using SBEPIS.Capturellection.DequeState;
using SBEPIS.Physics;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Deque), typeof(DequeBoxStateMachine), typeof(GravitySum))]
	public class DequeBox : MonoBehaviour
	{
		public bool IsDeployed => state.IsDeployed;
		
		public Deque Deque { get; private set; }
		private DequeBoxStateMachine state;
		public GravitySum GravitySum { get; private set; }
		public Rigidbody Rigidbody => GravitySum.Rigidbody;
		
		private void Awake()
		{
			Deque = GetComponent<Deque>();
			state = GetComponent<DequeBoxStateMachine>();
			GravitySum = GetComponent<GravitySum>();
		}
		
		public void SetCoupledState()
		{
			state.IsCoupled = true;
		}
		
		public void SetDecoupledState()
		{
			state.IsDeployed = true;
			state.IsCoupled = false;
		}
		
		public void RetrieveDeque(DequeBoxOwner dequeBoxOwner)
		{
			state.lerpTarget = dequeBoxOwner.lerpTarget;
			state.IsDeployed = false;
		}
		
		public void TossDeque(DequeBoxOwner dequeBoxOwner)
		{
			SetDecoupledState();
			
			Transform tossTarget = dequeBoxOwner.tossTarget;
			float tossHeight = dequeBoxOwner.tossHeight;
			
			Rigidbody.velocity = CalcTossVelocity(
				transform,
				tossTarget,
				tossHeight,
				tossTarget.up,
				GravitySum.GravityAcceleration);
		}
		
		private static Vector3 CalcTossVelocity(Transform box, Transform tossTarget, float tossHeight, Vector3 upDirection, float gravityAcceleration)
		{
			gravityAcceleration *= -1;
			float startHeight = tossTarget.InverseTransformPoint(box.position).y;
			float upTossSpeed = CalcTossYVelocity(gravityAcceleration, startHeight, startHeight + tossHeight);
			Vector3 upwardVelocity = upDirection * upTossSpeed;
			
			float timeToHit = (-upTossSpeed - Mathf.Sqrt(upTossSpeed * upTossSpeed - 2 * gravityAcceleration * startHeight)) / gravityAcceleration;
			Vector3 groundDelta = Vector3.ProjectOnPlane(tossTarget.position - box.position, upDirection);
			Vector3 groundVelocity = groundDelta / timeToHit;
			
			return upwardVelocity + groundVelocity;
		}
		
		private static float CalcTossYVelocity(float gravity, float startHeight, float peakHeight) => Mathf.Sqrt(2 * gravity * (startHeight - peakHeight));
	}
}
