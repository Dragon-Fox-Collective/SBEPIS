//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsの最小コーナーの位置を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the position of the minimum corner of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.SetMin")]
	[BehaviourTitle("Bounds.SetMin")]
	[BuiltInBehaviour]
	public sealed class BoundsSetMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最小コーナーの位置
		/// </summary>
#else
		/// <summary>
		/// the position of the minimum corner of the Bounds.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Min = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotBounds _Result = new OutputSlotBounds();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds value = _Bounds.value;
			value.min = _Min.value;
			_Result.SetValue(value);
		}
	}
}
