﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector4を減算する。
	/// </summary>
#else
	/// <summary>
	/// Subtract Vector4.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.Sub")]
	[BehaviourTitle("Vector4.Sub")]
	[BuiltInBehaviour]
	public sealed class Vector4SubCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値1
		/// </summary>
#else
		/// <summary>
		/// Value 1
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _Value1 = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value 2
		/// </summary>
#endif
		[SerializeField] private FlexibleVector4 _Value2 = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector4 _Result = new OutputSlotVector4();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_Value1.value - _Value2.value);
		}
	}
}
