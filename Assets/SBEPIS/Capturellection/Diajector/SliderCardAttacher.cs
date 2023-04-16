using KBCore.Refs;
using SBEPIS.UI;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(CardTarget))]
	public class SliderCardAttacher : MonoBehaviour, Slider
	{
		[SerializeField, Self]
		private CardTarget cardTarget;
		
		private void OnValidate() => this.ValidateRefs();
		
		public Transform startPoint;
		public Transform endPoint;
		public UnityEvent<float> onSliderValueChanged = new();
		
		private SliderCard slider;
		
		private float sliderValue;
		public float SliderValue
		{
			get => slider ? slider.SliderValue : sliderValue;
			set
			{
				if (slider)
					slider.SliderValue = value;
				else
					sliderValue = value;
			}
		}
		
		public void Attach(DequeElement card)
		{
			slider = card.gameObject.AddComponent<SliderCard>();
			slider.startPoint = startPoint;
			slider.endPoint = endPoint;
			slider.target = cardTarget;
			slider.SliderValue = sliderValue;
			slider.onSliderValueChanged = onSliderValueChanged;
			card.Grabbable.onDrop.AddListener((_, _) => slider.ClampNewPosition());
		}
	}
}