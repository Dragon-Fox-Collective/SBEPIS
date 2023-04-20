using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeBox))]
	public class DequeBoxDiajectorOpener : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private DequeBox dequeBox;
		
		public Diajector diajector;
		
		private DiajectorCloser closer;
		private Transform playerTransform;
		
		public void BindToPlayer(PlayerReference playerReference)
		{
			closer = playerReference.GetReferencedComponent<DiajectorCloser>();
			playerTransform = closer.transform;
		}
		
		public void OpenDiajector()
		{
			if (diajector.IsOpen)
				return;
			
			Vector3 position = dequeBox.transform.position;
			Vector3 upDirection = dequeBox.GravitySum.UpDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(closer ? playerTransform.position - position : -dequeBox.Rigidbody.velocity, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			diajector.StartAssembly(closer, position, rotation);
		}

		public void CloseDiajector()
		{
			if (!diajector.IsOpen)
				return;
			
			diajector.StartDisassembly();
		}
	}
}