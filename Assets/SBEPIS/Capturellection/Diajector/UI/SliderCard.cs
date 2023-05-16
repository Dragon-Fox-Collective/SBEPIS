using SBEPIS.UI;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.UI
{
	public class SliderCard : MonoBehaviour, Slider
	{
		public Transform startPoint;
		public Transform endPoint;
		public CardTarget target;

		public UnityEvent<float> onSliderValueChanged = new();

		private float sliderValue;
		public float SliderValue
		{
			get => sliderValue;
			set
			{
				sliderValue = value;
				target.transform.SetPositionAndRotation(
					Vector3.LerpUnclamped(startPoint.position, endPoint.position, sliderValue),
					Quaternion.LerpUnclamped(startPoint.rotation, endPoint.rotation, sliderValue));
				onSliderValueChanged.Invoke(sliderValue);
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
