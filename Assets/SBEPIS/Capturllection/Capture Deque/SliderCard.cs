using SBEPIS.Controller;
using SBEPIS.UI;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class SliderCard : MonoBehaviour, Slider
	{
		public Transform startPoint;
		public Transform endPoint;
		public CardTarget target;

		public UnityEvent<float> onSliderValueChanged = new();

		private float _sliderValue;
		public float SliderValue
		{
			get => _sliderValue;
			set
			{
				_sliderValue = value;
				target.transform.SetPositionAndRotation(
					Vector3.LerpUnclamped(startPoint.position, endPoint.position, _sliderValue),
					Quaternion.LerpUnclamped(startPoint.rotation, endPoint.rotation, _sliderValue));
				onSliderValueChanged.Invoke(_sliderValue);
			}
		}

		public void ClampNewPosition()
		{
			Vector3 relativePosition = transform.position - startPoint.position;
			Vector3 relativeEnd = endPoint.position - startPoint.position;
			Vector3 relativeNewPosition = Vector3.Project(relativePosition, relativeEnd);
			float value = relativeNewPosition.magnitude / relativeEnd.magnitude;
			float signedValue = value * Mathf.Sign(Vector3.Dot(relativeNewPosition, relativeEnd));
			float clampedValue = Mathf.Clamp01(signedValue);
			SliderValue = clampedValue;
		}
	}
}
