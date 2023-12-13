using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.UI
{
	[RequireComponent(typeof(PhysicsSlider))]
	public class PhysicsSliderProgressProxy : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private PhysicsSlider slider;
		
		public UnityEvent<float> onInvokeProgress = new();
		
		public void Invoke() => onInvokeProgress.Invoke(slider.Progress);
	}
}
