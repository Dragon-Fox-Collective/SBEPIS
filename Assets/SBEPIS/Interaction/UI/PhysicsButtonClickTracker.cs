using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(PhysicsButton))]
	public class PhysicsButtonClickTracker : ClickTracker
	{
		private PhysicsButton button;

		private void Awake()
		{
			button = GetComponent<PhysicsButton>();
		}

		private void OnEnable()
		{
			button.onPressed.AddListener(StartTracking);
			button.onUnpressed.AddListener(FinishTracking);
		}

		private void OnDisable()
		{
			button.onPressed.RemoveListener(StartTracking);
			button.onUnpressed.RemoveListener(FinishTracking);
		}
	}
}
