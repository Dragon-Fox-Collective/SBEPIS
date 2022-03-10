using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	public class ClickTracker : MonoBehaviour
	{
		[Tooltip("Must be greater than your tap time or it won't work")]
		public float tapTime = 0.5f;
		public UnityEvent OnClick;

		private bool isTracking;
		private float startTime;

		public void StartTracking()
		{
			if (isTracking)
				throw new InvalidOperationException($"Click tracker {gameObject} is already tracking");
			isTracking = true;
			startTime = Time.unscaledTime;
		}

		public void FinishTracking()
		{
			if (!isTracking)
				throw new InvalidOperationException($"Click tracker {gameObject} is not tracking yet");
			isTracking = false;
			print($"{gameObject} finished clicking, held for {Time.unscaledTime - startTime}, clicked? {Time.unscaledTime - startTime <= tapTime}");
			if (Time.unscaledTime - startTime <= tapTime)
				OnClick.Invoke();
		}
	}
}
