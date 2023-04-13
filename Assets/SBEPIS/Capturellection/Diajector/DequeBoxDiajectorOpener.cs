using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeBox))]
	public class DequeBoxDiajectorOpener : MonoBehaviour
	{
		public Diajector diajector;
		
		private DequeBox dequeBox;
		
		private DiajectorCloser closer;
		
		private void Awake()
		{
			dequeBox = GetComponent<DequeBox>();
		}

		public void BindToPlayer(Grabber grabber, Grabbable grabbable)
		{
			if (!grabber.TryGetComponent(out PlayerReference playerReference))
				return;
			DiajectorCloser newCloser = playerReference.GetComponent<DiajectorCloser>();
			closer = newCloser;
		}
		
		public void OpenDiajector()
		{
			Vector3 position = dequeBox.transform.position;
			Vector3 upDirection = dequeBox.GravitySum.UpDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(-dequeBox.Rigidbody.velocity, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			diajector.StartAssembly(closer, position, rotation);
		}
		
		public void CloseDiajector() => diajector.StartDisassembly();
	}
}