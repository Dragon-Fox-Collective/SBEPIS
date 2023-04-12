using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeBox))]
	public class DequeBoxDiajectorOpener : MonoBehaviour
	{
		public Diajector diajector;
		
		private DequeBox dequeBox;
		
		private void Awake()
		{
			dequeBox = GetComponent<DequeBox>();
		}
		
		public void OpenDiajector()
		{
			Vector3 position = dequeBox.transform.position;
			Vector3 upDirection = dequeBox.GravitySum.UpDirection;
			Vector3 groundDelta = Vector3.ProjectOnPlane(-dequeBox.Rigidbody.velocity, upDirection);
			Quaternion rotation = Quaternion.LookRotation(groundDelta, upDirection);
			diajector.StartAssembly(position, rotation);
		}
		public void CloseDiajector() => diajector.StartDisassembly();
	}
}