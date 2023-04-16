using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.UI
{
	[RequireComponent(typeof(Grabbable))]
	public class GrabClickTracker : ClickTracker
	{
		[SerializeField, Self]
		private Grabbable grabbable;

		private void OnValidate() => this.ValidateRefs();

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
