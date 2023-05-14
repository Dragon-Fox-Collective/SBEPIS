using KBCore.Refs;
using SBEPIS.Capturellection.State;
using SBEPIS.Controller;
using SBEPIS.Physics;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Deque), typeof(Rigidbody), typeof(GravitySum))]
	public class DequeBox : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private Deque deque;
		public Deque Deque => deque;
		
		[SerializeField, Self] private GravitySum gravitySum;
		public GravitySum GravitySum => gravitySum;
		
		[SerializeField, Self] private new Rigidbody rigidbody;
		public Rigidbody Rigidbody => rigidbody;
		
		[SerializeField, Self] private CouplingPlug plug;
		
		[SerializeField, Anywhere] private DequeBoxRetrieveTransition retrieve;
		
		[SerializeField, Anywhere] private PlayerBinder[] startBinders;
		
		public UnityEvent onToss = new();
		public UnityEvent onRetrieve = new();
		
		public bool IsDeployed => !plug.IsCoupled;
		
		public void Retrieve(DequeBoxOwner dequeBoxOwner)
		{
			onRetrieve.Invoke();
			retrieve.Retrieve(dequeBoxOwner.LerpTarget);
		}
		
		public void Detatch()
		{
			if (plug.CoupledSocket)
				plug.CoupledSocket.Decouple(plug);
		}
		
		public void Toss(DequeBoxOwner dequeBoxOwner)
		{
			Detatch();
			rigidbody.velocity += CalcTossVelocity(
				transform,
				dequeBoxOwner.TossTarget,
				dequeBoxOwner.TossHeight,
				dequeBoxOwner.TossTarget.up,
				gravitySum.GravityAcceleration);
			onToss.Invoke();
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
		
		public void StartBindToPlayer(PlayerReference playerReference)
		{
			foreach (PlayerBinder binder in startBinders)
				binder.BindToPlayer(playerReference);
		}
	}
}
