using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Grabbable))]
	public class GrabClickTracker : ClickTracker
	{
		private Grabbable grabbable;

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
		}

		private void OnEnable()
		{
			grabbable.onGrab.AddListener(StartTracking);
			grabbable.onDrop.AddListener(FinishTracking);
		}

		private void OnDisable()
		{
			grabbable.onGrab.RemoveListener(StartTracking);
			grabbable.onDrop.RemoveListener(FinishTracking);
		}

		public void StartTracking(Grabber grabber, Grabbable grabbable) => StartTracking();
		public void FinishTracking(Grabber grabber, Grabbable grabbable) => FinishTracking();
	}
}
