using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.UI
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

		public void StartTracking(Grabber grabber) => StartTracking();
		public void FinishTracking(Grabber grabber) => FinishTracking();
	}
}
