using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(PhysicsSlider))]
	public class PhysicsSliderProgressProxy : MonoBehaviour
	{
		public UnityEvent<float> onInvokeProgress = new();

		private PhysicsSlider slider;

		private void Awake()
		{
			slider = GetComponent<PhysicsSlider>();
		}

		public void Invoke()
		{
			onInvokeProgress.Invoke(slider.progress);
		}
	}
}
