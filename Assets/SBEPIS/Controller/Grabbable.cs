using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabbable : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private new Rigidbody rigidbody;
		public Rigidbody Rigidbody => rigidbody;
		
		[FormerlySerializedAs("grabPoints")]
		public List<GrabPoint> GrabPoints = new();
		[FormerlySerializedAs("onTouch")]
		public GrabEvent OnTouch = new();
		[FormerlySerializedAs("onGrab")]
		public GrabEvent OnGrab = new();
		[FormerlySerializedAs("onHoldUpdate")]
		public GrabEvent OnHoldUpdate = new();
		[FormerlySerializedAs("onUse")]
		public GrabEvent OnUse = new();
		[FormerlySerializedAs("onDrop")]
		public GrabEvent OnDrop = new();
		[FormerlySerializedAs("onStopTouch")]
		public GrabEvent OnStopTouch = new();
		
		private List<Grabber> grabbingGrabbers = new();
		
		public bool IsBeingHeld => grabbingGrabbers.Count != 0;
		
		public void OnGrabbed(Grabber grabber)
		{
			grabbingGrabbers.Add(grabber);
			OnGrab.Invoke(grabber, this);
		}
		
		public void HoldUpdate(Grabber grabber)
		{
			OnHoldUpdate.Invoke(grabber, this);
		}
		
		public void OnDropped(Grabber grabber)
		{
			grabbingGrabbers.Remove(grabber);
			OnDrop.Invoke(grabber, this);
		}
		
		public void Drop()
		{
			grabbingGrabbers.ToList().ForEach(grabber => grabber.DropManually());
		}
	}
}