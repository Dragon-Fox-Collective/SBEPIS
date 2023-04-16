using System;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabbable : MonoBehaviour
	{
		[SerializeField, Self]
		private new Rigidbody rigidbody;
		public Rigidbody Rigidbody => rigidbody;
		
		private void OnValidate() => this.ValidateRefs();
		
		public List<GrabPoint> grabPoints = new();
		public GrabEvent onTouch = new();
		public GrabEvent onGrab = new();
		public GrabEvent onHoldUpdate = new();
		public GrabEvent onUse = new();
		public GrabEvent onDrop = new();
		public GrabEvent onStopTouch = new();
		
		private List<Grabber> grabbingGrabbers = new();
		
		public bool IsBeingHeld => grabbingGrabbers.Count != 0;
		
		public void OnGrabbed(Grabber grabber)
		{
			grabbingGrabbers.Add(grabber);
			onGrab.Invoke(grabber, this);
		}
		
		public void HoldUpdate(Grabber grabber)
		{
			onHoldUpdate.Invoke(grabber, this);
		}
		
		public void OnDropped(Grabber grabber)
		{
			grabbingGrabbers.Remove(grabber);
			onDrop.Invoke(grabber, this);
		}
		
		public void Drop()
		{
			grabbingGrabbers.ForEach(grabber => grabber.Drop());
		}
	}
}