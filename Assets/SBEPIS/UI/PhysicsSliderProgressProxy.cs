using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.UI
{
	[RequireComponent(typeof(PhysicsSlider))]
	public class PhysicsSliderProgressProxy : MonoBehaviour
	{
		[SerializeField, Self]
		private PhysicsSlider slider;
		
		private void OnValidate() => this.ValidateRefs();
		
		public UnityEvent<float> onInvokeProgress = new();
		
		public void Invoke() => onInvokeProgress.Invoke(slider.progress);
	}
}
