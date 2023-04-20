using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.UI
{
	public class ClickTracker : ValidatedMonoBehaviour
	{
		public UnityEvent onClick = new();

		/// <summary>
		/// Must be greater than your mouse tap time or it won't work
		/// </summary>
		private const float TAP_TIME = 0.3f;

		private int trackingCount;
		private float startTime;

		public void StartTracking()
		{
			trackingCount++;
			startTime = Time.unscaledTime;
		}

		public void FinishTracking()
		{
			if (trackingCount == 0)
				throw new InvalidOperationException($"Click tracker {gameObject} is not tracking");
			trackingCount--;
			if (trackingCount == 0)
			{
				print($"{gameObject} finished clicking, held for {Time.unscaledTime - startTime}, clicked? {Time.unscaledTime - startTime <= TAP_TIME}");
				if (Time.unscaledTime - startTime <= TAP_TIME)
					onClick.Invoke();
			}
		}
	}
}
