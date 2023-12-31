using Arbor;
using UnityEngine;

namespace SBEPIS.Utils.State
{
	[AddBehaviourMenu("Component/Component.GetTransform")]
	[BehaviourTitle("Component.GetTransform")]
	public class ComponentGetTransformCalculator : Calculator
	{
		[SerializeField] private FlexibleComponent component;
		[SerializeField] private new OutputSlotTransform transform;
		
		public override void OnCalculate() => transform.SetValue(component.value.transform);
	}
}
