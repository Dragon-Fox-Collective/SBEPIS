using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.UI
{
	public class SwitchCard : MonoBehaviour
	{
		public Transform offPoint;
		public Transform onPoint;
		public CardTarget target;

		public UnityEvent<bool> onSwitchValueChanged = new();

		private bool switchValue;
		public bool SwitchValue
		{
			get => switchValue;
			set
			{
				switchValue = value;
				target.transform.SetPositionAndRotation(
					(switchValue ? onPoint : offPoint).position,
					(switchValue ? onPoint : offPoint).rotation);
				onSwitchValueChanged.Invoke(switchValue);
			}
		}

		public void ClampNewPosition()
		{
			SwitchValue = (transform.position - onPoint.position).sqrMagnitude < (transform.position - offPoint.position).sqrMagnitude;
		}
	}
}
