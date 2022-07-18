using SBEPIS.Interaction.Controller;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class RailJoint : MonoBehaviour
	{
		public new Rigidbody rigidbody { get; private set; }
		private List<Grabbable> grabbables = new List<Grabbable>();

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody)
			{
				Grabbable grabbable = other.attachedRigidbody.GetComponent<Grabbable>();
				if (grabbable && grabbable.grabbingGrabber && !grabbables.Contains(grabbable))
				{
					print("Attaching " + other.attachedRigidbody.gameObject);

					grabbables.Add(grabbable);

					FixedJoint joint = other.attachedRigidbody.gameObject.AddComponent<FixedJoint>();
					joint.autoConfigureConnectedAnchor = false;
					joint.connectedBody = rigidbody;
					joint.connectedAnchor = Vector3.zero;
				}
			}
		}
	}
}
