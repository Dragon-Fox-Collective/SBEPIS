using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("Diajector/Diajector.GetStaticRigidbody")]
	[BehaviourTitle("Diajector.GetStaticRigidbody")]
	public class DiajectorGetStaticRigidbodyCalculator : Calculator
	{
		[SerializeField] private FlexibleDiajector diajector;
		[SerializeField] private OutputSlotRigidbody staticRigidbody;
		
		public override void OnCalculate() => staticRigidbody.SetValue(diajector.value.StaticRigidbody);
	}
}
