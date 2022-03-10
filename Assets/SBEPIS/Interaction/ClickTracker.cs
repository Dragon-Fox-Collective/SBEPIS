using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Grabbable))]
	public class ClickTracker : MonoBehaviour
	{
		public UnityEvent onClick;

		/// <summary>
		/// Must be greater than your mouse tap time or it won't work
		/// </summary>
		private const float TAP_TIME = 0.3f;

		private Grabbable grabbable;
		private bool isTracking;
		private float startTime;

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

		private void StartTracking(Grabber grabber)
		{
			if (isTracking)
				throw new InvalidOperationException($"Click tracker {gameObject} is already tracking");
			isTracking = true;
			startTime = Time.unscaledTime;
		}

		private void FinishTracking(Grabber grabber)
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
