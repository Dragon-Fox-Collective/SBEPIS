using System.Collections.Generic;
using SBEPIS.Predicates;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class CouplingSocket : MonoBehaviour
	{
		public List<GameObjectPredicate> predicates = new();
		
		public CoupleEvent onCouple = new();
		public CoupleEvent onDecouple = new();
		
		public bool isCoupled => plug;
		
		public CouplingPlug plug { get; private set; }
		public FixedJoint joint { get; private set; }
		
		private List<CouplingPlug> collidingPlugs = new();
		private new Rigidbody rigidbody;
		
		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}
		
		public void OnTriggerEnter(Collider other)
		{
			CouplingPlug newPlug = other.GetAttachedComponent<CouplingPlug>();
			if (!newPlug)
				return;
			
			collidingPlugs.Add(newPlug);
			newPlug.Grabbable.onDrop.AddListener(Couple);
		}
		
		private void OnTriggerExit(Collider other)
		{
			CouplingPlug newPlug = other.GetAttachedComponent<CouplingPlug>();
			if (!newPlug)
				return;
			
			collidingPlugs.Remove(newPlug);
			newPlug.Grabbable.onDrop.RemoveListener(Couple);
		}
		
		public void Couple(Grabber grabber, Grabbable grabbable) => Couple(grabbable.GetComponent<CouplingPlug>());
		public void Couple(CouplingPlug plug)
		{
			if (isCoupled)
			{
				Debug.LogError($"Tried to couple {plug} to {this} when socket already coupled to {this.plug}");
				return;
			}
			
			if (!predicates.AreTrue(plug.gameObject))
				return;
			
			plug.transform.position = transform.position;
			plug.transform.rotation = transform.rotation;
			joint = gameObject.AddComponent<FixedJoint>();
			joint.autoConfigureConnectedAnchor = false;
			joint.connectedBody = plug.Grabbable.rigidbody;
			joint.anchor = joint.connectedAnchor = Vector3.zero;
			
			this.plug = plug;
			plug.GetCoupled(this);
			onCouple.Invoke(plug, this);
		}
		
		public void Decouple(Grabber grabber, Grabbable grabbable) => Decouple(grabbable.GetComponent<CouplingPlug>());
		public void Decouple(CouplingPlug plug)
		{
			if (!isCoupled)
			{
				Debug.LogError($"Tried to decouple plug from {this} when socket had no coupling");
				return;
			}
			if (this.plug != plug)
			{
				Debug.LogError($"Tried to decouple {plug} from {this} when socket had {this.plug} instead");
			}
			
			Destroy(joint);
			
			this.plug = null;
			plug.GetDecoupled();
			onDecouple.Invoke(plug, this);
		}
	}
	
	public class CoupleEvent : UnityEvent<CouplingPlug, CouplingSocket> { }
}
