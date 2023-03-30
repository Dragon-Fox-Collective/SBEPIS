using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	public class SwitchCard : MonoBehaviour
	{
		public Transform offPoint;
		public Transform onPoint;
		public CardTarget target;

		public UnityEvent<bool> onSwitchValueChanged = new();

		private bool _switchValue;
		public bool SwitchValue
		{
			get => _switchValue;
			set
			{
				_switchValue = value;
				target.transform.SetPositionAndRotation(
					(_switchValue ? onPoint : offPoint).position,
					(_switchValue ? onPoint : offPoint).rotation);
				onSwitchValueChanged.Invoke(_switchValue);
			}
		}

		public void ClampNewPosition()
		{
			SwitchValue = (transform.position - onPoint.position).sqrMagnitude < (transform.position - offPoint.position).sqrMagnitude;
		}
	}
}
