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
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.CenterOfMass")]
	[BehaviourTitle("Rigidbody2D.CenterOfMass")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DCenterOfMassCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 質量の中心
		/// </summary>
#else
		/// <summary>
		/// The center of mass
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _CenterOfMass = new OutputSlotVector2();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody2D rigidbody2D = _Rigidbody2D.value;
			if (rigidbody2D != null)
			{
				_CenterOfMass.SetValue(rigidbody2D.centerOfMass);
			}
		}
	}
}
