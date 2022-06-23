using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	public class ClickTracker : MonoBehaviour
	{
		public UnityEvent onClick = new();

		/// <summary>
		/// Must be greater than your mouse tap time or it won't work
		/// </summary>
		private const float TAP_TIME = 0.3f;

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
			print($"{gameObject} finished clicking, held for {Time.unscaledTime - startTime}, clicked? {Time.unscaledTime - startTime <= TAP_TIME}");
			if (Time.unscaledTime - startTime <= TAP_TIME)
				onClick.Invoke();
		}
	}
}
