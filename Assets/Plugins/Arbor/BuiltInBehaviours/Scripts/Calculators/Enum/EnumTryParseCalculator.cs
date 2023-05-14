//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列からEnumへ変換して返す。変換に成功したかどうかも返す。
	/// </summary>
#else
	/// <summary>
	/// Returns an Enum converted from a string. It also returns whether the conversion was successful.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Enum/Enum.TryParse")]
	[BehaviourTitle("Enum.TryParse")]
	[BuiltInBehaviour]
	public class EnumTryParseCalculator : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 変換する文字列
		/// </summary>
#else
		/// <summary>
		/// String to convert
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _String = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 大文字小文字を無視するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to ignore case.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _IgnoreCase = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 変換に成功したかどうかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether the conversion was successful.
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotBool _Success = new OutputSlotBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField]
		[HideSlotFields]
		private OutputSlotTypable _Result = new OutputSlotTypable();

		// Use this for calculate
		public override void OnCalculate()
		{
			try
			{
				var result = System.Enum.Parse(_Result.dataType, _String.value, _IgnoreCase.value);
				_Result.SetValue(result);
				_Success.SetValue(true);
			}
			catch
			{
				_Success.SetValue(false);
			}
		}
	}
}