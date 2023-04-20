using KBCore.Refs;
using SBEPIS.Capturellection.DequeState;
using SBEPIS.Controller;
using SBEPIS.Physics;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Deque), typeof(DequeBoxStateMachine), typeof(GravitySum))]
	[RequireComponent(typeof(Rigidbody))]
	public class DequeBox : MonoBehaviour
	{
		[SerializeField, Self] private Deque deque;
		public Deque Deque => deque;
		
		[SerializeField, Self] private DequeBoxStateMachine state;
		
		[SerializeField, Self] private GravitySum gravitySum;
		public GravitySum GravitySum => gravitySum;
		
		[SerializeField, Self] private new Rigidbody rigidbody;
		public Rigidbody Rigidbody => rigidbody;
		
		private void OnValidate() => this.ValidateRefs();
		
		public UnityEvent onToss = new();
		public UnityEvent onRetrieve = new();
		public UnityEvent<PlayerReference> onBindToPlayer = new();
		
		public bool IsDeployed => state.IsDeployed;
		
		public void Retrieve(DequeBoxOwner dequeBoxOwner)
		{
			state.OwnerLerpTarget = dequeBoxOwner.LerpTarget;
			state.OwnerSocket = dequeBoxOwner.Socket;
			state.IsDeployed = false;
			onRetrieve.Invoke();
		}
		
		public void Unretrieve(DequeBoxOwner dequeBoxOwner)
		{
			if (state.Plug.CoupledSocket == dequeBoxOwner.Socket)
				dequeBoxOwner.Socket.Decouple(state.Plug);
		}
		
		public void Toss(DequeBoxOwner dequeBoxOwner)
		{
			state.OwnerSocket.Decouple(state.Plug);
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
		
		public void BindToPlayer(Grabber grabber, Grabbable _)
		{
			if (!grabber.TryGetComponent(out PlayerReference playerReference))
				return;
			
			BindToPlayer(playerReference);
		}
		
		public void BindToPlayer(PlayerReference playerReference)
		{
			onBindToPlayer.Invoke(playerReference);
		}
	}
}
