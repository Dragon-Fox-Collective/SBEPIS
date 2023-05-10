using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeBox))]
	public class DequeBoxDiajectorOpener : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private DequeBox dequeBox;
		
		public Diajector diajector;
		
		private Transform playerTransform;
		
		public void BindToPlayer(PlayerReference playerReference)
		{
			playerTransform = playerReference.GetReferencedComponent<Transform>();
		}
		
		public void OpenDiajector()
		{
			if (diajector.IsOpen)
				return;
			
			Vector3 position = dequeBox.transform.position;
			Vector3 upDirection = dequeBox.GravitySum.UpDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(playerTransform ? playerTransform.position - position : -dequeBox.Rigidbody.velocity, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			diajector.StartAssembly(position, rotation);
		}
		
		public void CloseDiajector()
		{
			if (!diajector.IsOpen)
				return;
			
			diajector.StartDisassembly();
		}
	}
}