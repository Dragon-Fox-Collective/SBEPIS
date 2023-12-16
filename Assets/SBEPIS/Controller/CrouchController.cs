using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Controller
{
	public class CrouchController : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private ConfigurableJoint joint;
		[SerializeField, Anywhere] private Transform headTracker;
		[SerializeField] private float crouchDistance = 0.5f;
		
		private Vector3 offset => Crouching ? Vector3.down * crouchDistance : Vector3.zero;

		private bool crouching;
		public bool Crouching
		{
			get => crouching;
			set
			{
				if (value == crouching) return;
				crouching = value;
				joint.connectedAnchor = startingAnchor + offset;
				headTracker.localPosition = headTrackerStartingPosition + offset;
			}
		}
		
		private Vector3 startingAnchor;
		private Vector3 headTrackerStartingPosition;
		
		private void Start()
		{
			startingAnchor = joint.connectedAnchor;
			joint.autoConfigureConnectedAnchor = false;
			headTrackerStartingPosition = headTracker.localPosition;
		}
	}
}