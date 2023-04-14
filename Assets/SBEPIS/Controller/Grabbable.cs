using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Grabbable : MonoBehaviour
	{
		public List<GrabPoint> grabPoints = new();
		public ItemEvent onTouch = new();
		public ItemEvent onGrab = new();
		public ItemEvent onHoldUpdate = new();
		public ItemEvent onUse = new();
		public ItemEvent onDrop = new();
		public ItemEvent onStopTouch = new();
		
		public readonly List<Grabber> grabbingGrabbers = new();
		public Rigidbody Rigidbody { get; private set; }
		public bool IsBeingHeld => grabbingGrabbers.Count != 0;
		
		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
		}
		
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

	[Serializable]
	public class ItemEvent : UnityEvent<Grabber, Grabbable> { }
}