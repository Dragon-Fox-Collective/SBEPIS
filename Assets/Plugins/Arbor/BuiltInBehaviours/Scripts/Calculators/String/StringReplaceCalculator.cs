//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 指定した文字列を別の文字列へ置換する。
	/// </summary>
#else
	/// <summary>
	/// Replaces the specified string with another string.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.Replace")]
	[BehaviourTitle("String.Replace")]
	[BuiltInBehaviour]
	public sealed class StringReplaceCalculator : Calculator
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
		/// 元の文字列
		/// </summary>
#else
		/// <summary>
		/// Old string
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _OldValue = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 新しいの文字列
		/// </summary>
#else
		/// <summary>
		/// New string
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _NewValue = new FlexibleString();

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
			_Result.SetValue(_String.value.Replace(_OldValue.value, _NewValue.value));
		}
	}
}