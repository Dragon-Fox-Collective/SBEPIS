//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列から一部分を取り除く。
	/// </summary>
#else
	/// <summary>
	/// Remove a part from the string.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.Remove")]
	[BehaviourTitle("String.Remove")]
	[BuiltInBehaviour]
	public sealed class StringRemoveCalculator : Calculator
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
		/// 取り除く開始インデックス
		/// </summary>
#else
		/// <summary>
		/// Start index to remove
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _StartIndex = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 取り除く文字列の文字数。負値を指定した場合は文字列の末尾までを取り除く。
		/// </summary>
#else
		/// <summary>
		/// The number of characters in the string to remove. If a negative value is specified, the end of the character string is removed.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _Count = new FlexibleInt();

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
			string str = _String.value;
			int startIndex = _StartIndex.value;
			int count = _Count.value;
			if( count <= -1)
			{
				count = str.Length - startIndex;
			}
			_Result.SetValue(str.Remove(startIndex, count));
		}
	}
}