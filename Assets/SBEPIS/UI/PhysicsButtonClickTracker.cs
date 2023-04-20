using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.UI
{
	[RequireComponent(typeof(PhysicsButton))]
	public class PhysicsButtonClickTracker : ClickTracker
	{
		[SerializeField, Self]
		private PhysicsButton button;
		
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
