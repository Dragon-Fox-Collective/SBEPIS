﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MinとMaxの範囲に値を制限する。
	/// </summary>
#else
	/// <summary>
	/// Clamps a value between a Min and Max value.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.ClampFloat")]
	[BehaviourTitle("Mathf.ClampFloat")]
	[BuiltInBehaviour]
	public sealed class MathfClampFloatCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値
		/// </summary>
#else
		/// <summary>
		/// Value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Value = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最小値
		/// </summary>
#else
		/// <summary>
		/// Minimum value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Min = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大値
		/// </summary>
#else
		/// <summary>
		/// Maximum value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Max = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Result = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Mathf.Clamp(_Value.value, _Min.value, _Max.value));
		}
	}
}
