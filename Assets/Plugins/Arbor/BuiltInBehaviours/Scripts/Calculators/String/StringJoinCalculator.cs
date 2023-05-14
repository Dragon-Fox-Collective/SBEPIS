//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列を連結する。各要素の間には区切り記号を挿入する。
	/// </summary>
#else
	/// <summary>
	/// Concatenate strings. using the specified separator between each element.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.Join")]
	[BehaviourTitle("String.Join")]
	[BuiltInBehaviour]
	public sealed class StringJoinCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 区切り記号
		/// </summary>
#else
		/// <summary>
		/// Separator
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Separator = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 連結する文字列リスト
		/// </summary>
#else
		/// <summary>
		/// Concatenated string list
		/// </summary>
#endif
		[SerializeField]
		private List<FlexibleString> _Values = new List<FlexibleString>();

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
			string[] values = new string[_Values.Count];

			for (int i = 0; i < _Values.Count; i++)
			{
				values[i] = _Values[i].value;
			}

			_Result.SetValue(string.Join(_Separator.value, values));
		}
	}
}