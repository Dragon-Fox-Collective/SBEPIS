﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2をfloatで除算する。
	/// </summary>
#else
	/// <summary>
	/// Divide Vector2 by float.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.Div")]
	[BehaviourTitle("Vector2.Div")]
	[BuiltInBehaviour]
	public sealed class Vector2DivCalculator : Calculator
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
		[SerializeField] private FlexibleVector2 _Value1 = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value 2
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Value2 = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_Value1.value / _Value2.value);
		}
	}
}
