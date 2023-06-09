using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	// Don't try to turn this into an interface, it's used as a component in other places
	public abstract class CaptureContainer : MonoBehaviour
	{
		[SerializeField] private bool canBeFetchedByCapturellectors = true;
		public bool CanBeFetchedByCapturellectors => canBeFetchedByCapturellectors;
		
		public abstract Capturellectable CapturedItem { get; }
		public bool HasCapturedItem => CapturedItem;
		public bool IsEmpty => !HasCapturedItem;
		
		public abstract void Capture(Capturellectable item);
		public abstract Capturellectable Fetch();
	}
	
	[Serializable]
	public class CaptureEvent : UnityEvent<CaptureContainer, Capturellectable> { }
}
