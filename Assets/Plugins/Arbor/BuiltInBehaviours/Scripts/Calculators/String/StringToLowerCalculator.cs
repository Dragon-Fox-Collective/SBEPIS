﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列を小文字に変換して返す。
	/// </summary>
#else
	/// <summary>
	/// Converts the string to lowercase and returns it.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.ToLower")]
	[BehaviourTitle("String.ToLower")]
	[BuiltInBehaviour]
	public sealed class StringToLowerCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 文字列
		/// </summary>
#else
		/// <summary>
		/// String
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _String = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotString _Result = new OutputSlotString();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_String.value.ToLower());
		}
	}
}