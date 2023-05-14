//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Bool値をAnd演算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the Bool value by And.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bool/Bool.And")]
	[BehaviourTitle("Bool.And")]
	[BuiltInBehaviour]
	public sealed class BoolAndCalculator : Calculator
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
		[SerializeField] private FlexibleBool _Value1 = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value 2
		/// </summary>
#endif
		[SerializeField] private FlexibleBool _Value2 = new FlexibleBool();

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

			_Result.SetValue(_Value1.value && _Value2.value);
		}
	}
}
