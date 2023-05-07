using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	public abstract class CaptureContainer : MonoBehaviour
	{
		public Capturellectable CapturedItem { get; protected set; }
		public bool HasCapturedItem => CapturedItem;
		public bool IsEmpty => !HasCapturedItem;
		
		public abstract void Capture(Capturellectable item);
		public abstract Capturellectable Fetch();
	}
	
	[Serializable]
	public class CaptureEvent : UnityEvent<CaptureContainer, Capturellectable> { }
}
