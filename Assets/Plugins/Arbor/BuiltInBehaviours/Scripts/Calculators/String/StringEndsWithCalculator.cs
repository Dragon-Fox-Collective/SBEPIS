//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列の末尾が一致するかどうかを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns whether the end of the string matches.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.EndsWith")]
	[BehaviourTitle("String.EndsWith")]
	[BuiltInBehaviour]
	public sealed class StringEndsWithCalculator : Calculator
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
		/// 判定する文字列
		/// </summary>
#else
		/// <summary>
		/// String to judge
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Value = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定する際の規則を指定。
		/// </summary>
#else
		/// <summary>
		/// Specify the rules for judgment.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleStringComparison _StringComparison = new FlexibleStringComparison();

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
		private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_String.value.EndsWith(_Value.value, _StringComparison.value));
		}
	}
}