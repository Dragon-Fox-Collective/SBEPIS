//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列が最後に見つかったインデックスを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the index where the string was last found.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.LastIndexOf")]
	[BehaviourTitle("String.LastIndexOf")]
	[BuiltInBehaviour]
	public sealed class StringLastIndexOfCalculator : Calculator
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
		/// 含まれているか判定する文字列
		/// </summary>
#else
		/// <summary>
		/// String to judge whether it is rare
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Value = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定する開始インデックス。負値を指定した場合は文字列の末尾から判定する。
		/// </summary>
#else
		/// <summary>
		/// Start index to judge. If a negative value is specified, the judgment is made from the end of the character string.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _StartIndex = new FlexibleInt(-1);

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定する文字列の文字数。負値を指定した場合は文字列の先頭までを判定する。
		/// </summary>
#else
		/// <summary>
		/// The number of characters in the string to be judged. If a negative value is specified, the first part of the character string is judged.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _Count = new FlexibleInt(-1);

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
		private OutputSlotInt _Result = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			string str = _String.value;
			int startIndex = _StartIndex.value;
			int count = _Count.value;
			if (startIndex <= -1)
			{
				startIndex = str.Length - 1;
			}
			if (count <= -1)
			{
				count = startIndex + 1;
			}
			_Result.SetValue(_String.value.LastIndexOf(_Value.value, startIndex, count, _StringComparison.value));
		}
	}
}