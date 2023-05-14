using Arbor;
using SBEPIS.Controller.State;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("CouplingPlug/CouplingPlug.IsCoupled")]
	[BehaviourTitle("CouplingPlug.IsCoupled")]
	public class CouplingPlugIsCoupledCalculator : Calculator
	{
		[SerializeField] private FlexibleCouplingPlug couplingPlug = new();
		[SerializeField] private OutputSlotBool isAssembling = new();
		
		public override void OnCalculate() => isAssembling.SetValue(couplingPlug.value.IsCoupled);
	}
}
