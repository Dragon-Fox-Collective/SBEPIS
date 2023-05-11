//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列がnullまたは空の文字列かどうかを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns whether the string is null or empty.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.IsNullOrEmpty")]
	[BehaviourTitle("String.IsNullOrEmpty")]
	[BuiltInBehaviour]
	public sealed class StringIsNullOrEmptyCalculator : Calculator
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
		private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(string.IsNullOrEmpty(_String.value));
		}
	}
}