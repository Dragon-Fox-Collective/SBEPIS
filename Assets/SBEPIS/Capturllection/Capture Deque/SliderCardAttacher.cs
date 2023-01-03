using SBEPIS.UI;
using System.Collections;
using System.Collections.Generic;
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

		private void Awake()
		{
			cardTarget = GetComponent<CardTarget>();
		}

		public void Attach(DequeStorable card)
		{
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
