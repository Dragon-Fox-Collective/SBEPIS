﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列からFloatへ変換して返す。変換に成功したかどうかも返す。
	/// </summary>
#else
	/// <summary>
	/// Returns an Float converted from a string. It also returns whether the conversion was successful.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Float/Float.TryParse")]
	[BehaviourTitle("Float.TryParse")]
	[BuiltInBehaviour]
	public sealed class FloatTryParseCalculator : Calculator
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
		/// 変換に成功したかどうかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether the conversion was successful.
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Success = new OutputSlotBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Result = new OutputSlotFloat();

#endregion // Serialize fields

		public override void OnCalculate()
		{
			float value;
			if(float.TryParse(_String.value, out value) )
			{
				_Success.SetValue(true);
				_Result.SetValue(value);
			}
			else
			{
				_Success.SetValue(false);
			}
		}
	}
}
