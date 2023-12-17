using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Controller
{
	public class HoldInputMapSwitchHelper : MonoBehaviour
	{
		public UnityEvent OnStartHolding = new();
		public UnityEvent OnStopHolding = new();
		
		private bool held;
		public void StartHoldingOnTrue(bool holding)
		{
			if (!holding || held)
				return;
			held = true;
			OnStartHolding.Invoke();
		}
		public void StopHoldingOnFalse(bool holding)
		{
			if (holding || !held)
				return;
			held = false;
			OnStopHolding.Invoke();
		}
	}
}