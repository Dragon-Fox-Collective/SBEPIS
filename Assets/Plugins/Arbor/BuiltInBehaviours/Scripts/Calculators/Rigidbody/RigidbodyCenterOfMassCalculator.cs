//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Transform の原点に対する質量の中心
	/// </summary>
#else
	/// <summary>
	/// The center of mass relative to the transform's origin.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.CenterOfMass")]
	[BehaviourTitle("Rigidbody.CenterOfMass")]
	[BuiltInBehaviour]
	public sealed class RigidbodyCenterOfMassCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 質量の中心
		/// </summary>
#else
		/// <summary>
		/// The center of mass
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _CenterOfMass = new OutputSlotVector3();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody rigidbody = _Rigidbody.value;
			if (rigidbody != null)
			{
				_CenterOfMass.SetValue(rigidbody.centerOfMass);
			}
		}
	}
}
