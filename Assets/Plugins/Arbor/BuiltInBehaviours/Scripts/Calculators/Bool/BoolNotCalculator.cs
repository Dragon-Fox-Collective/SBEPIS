//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Bool値をNot演算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the Bool value by Not.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bool/Bool.Not")]
	[BehaviourTitle("Bool.Not")]
	[BuiltInBehaviour]
	public sealed class BoolNotCalculator : Calculator
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
		[SerializeField] private FlexibleBool _Value = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(!_Value.value);
		}
	}
}
