//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列からLongへ変換して返す。
	/// </summary>
#else
	/// <summary>
	/// Returns an Long converted from a string.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Long/Long.Parse")]
	[BehaviourTitle("Long.Parse")]
	[BuiltInBehaviour]
	public sealed class LongParseCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 変換する文字列
		/// </summary>
#else
		/// <summary>
		/// String to convert
		/// </summary>
#endif
		[SerializeField] private FlexibleString _String = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotLong _Result = new OutputSlotLong();

#endregion // Serialize fields

		public override void OnCalculate()
		{
			long value = long.Parse(_String.value);
			_Result.SetValue(value);
		}
	}
}
