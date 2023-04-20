using System;
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
		
		public bool IsCoupled => plug;
		
		private CouplingPlug plug;
		private FixedJoint joint;
		
		public void OnTriggerEnter(Collider other)
		{
			CouplingPlug newPlug = other.GetAttachedComponent<CouplingPlug>();
			if (!newPlug)
				return;
			
			newPlug.Grabbable.onDrop.AddListener(Couple);
		}
		
		private void OnTriggerExit(Collider other)
		{
			CouplingPlug newPlug = other.GetAttachedComponent<CouplingPlug>();
			if (!newPlug)
				return;
			
			newPlug.Grabbable.onDrop.RemoveListener(Couple);
		}
		
		public void Couple(Grabber grabber, Grabbable grabbable) => Couple(grabbable.GetComponent<CouplingPlug>());
		public void Couple(CouplingPlug plug)
		{
			if (IsCoupled)
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
			joint.connectedBody = plug.Grabbable.Rigidbody;
			joint.anchor = joint.connectedAnchor = Vector3.zero;
			
			this.plug = plug;
			plug.OnCoupled(this);
			onCouple.Invoke(plug, this);
		}
		
		public void Decouple(Grabber grabber, Grabbable grabbable) => Decouple(grabbable.GetComponent<CouplingPlug>());
		public void Decouple(CouplingPlug plug)
		{
			if (!IsCoupled)
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
			plug.OnDecoupled();
			onDecouple.Invoke(plug, this);
		}
	}
	
	[Serializable]
	public class CoupleEvent : UnityEvent<CouplingPlug, CouplingSocket> { }
}
