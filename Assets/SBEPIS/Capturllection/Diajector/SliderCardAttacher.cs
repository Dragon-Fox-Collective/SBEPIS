using SBEPIS.UI;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(CardTarget))]
	public class SliderCardAttacher : MonoBehaviour, Slider
	{
		public Transform startPoint, endPoint;
		public UnityEvent<float> onSliderValueChanged = new();

		private SliderCard slider;
		private CardTarget cardTarget;

		private float _sliderValue;
		public float SliderValue
		{
			get
			{
				if (slider)
					return slider.SliderValue;
				else
					return _sliderValue;
			}
			set
			{
				if (slider)
					slider.SliderValue = value;
				else
					_sliderValue = value;
			}
		}

		public void Attach(DequeStorable card)
		{
			cardTarget = GetComponent<CardTarget>(); // lol. lmao. awake isn't called before this fires
			slider = card.gameObject.AddComponent<SliderCard>();
			slider.startPoint = startPoint;
			slider.endPoint = endPoint;
			slider.target = cardTarget;
			slider.SliderValue = _sliderValue;
			slider.onSliderValueChanged = onSliderValueChanged;
			card.grabbable.onDrop.AddListener((grabber, grabbable) => slider.ClampNewPosition());
		}
	}
}
