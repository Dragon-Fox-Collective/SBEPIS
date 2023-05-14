//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsの最小コーナーの位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the minimum corner of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.GetMin")]
	[BehaviourTitle("Bounds.GetMin")]
	[BuiltInBehaviour]
	public sealed class BoundsGetMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// Boundsの最小コーナーの位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the minimum corner of the Bounds.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Min = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Min.SetValue(_Bounds.value.min);
		}
	}
}
