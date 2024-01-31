using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.UI
{
	[RequireComponent(typeof(Grabbable))]
	public class GrabClickTracker : ClickTracker
	{
		[SerializeField, Self] private Grabbable grabbable;
		
		private void OnEnable()
		{
			grabbable.OnGrab.AddListener(StartTracking);
			grabbable.OnDrop.AddListener(FinishTracking);
		}

		private void OnDisable()
		{
			grabbable.OnGrab.RemoveListener(StartTracking);
			grabbable.OnDrop.RemoveListener(FinishTracking);
		}

		public void StartTracking(Grabber grabber, Grabbable grabbable) => StartTracking();
		public void FinishTracking(Grabber grabber, Grabbable grabbable) => FinishTracking();
	}
}
