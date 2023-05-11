//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 文字列を挿入する。
	/// </summary>
#else
	/// <summary>
	/// Insert a string.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.Insert")]
	[BehaviourTitle("String.Insert")]
	[BuiltInBehaviour]
	public sealed class StringInsertCalculator : Calculator
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
		/// 挿入するインデックス
		/// </summary>
#else
		/// <summary>
		/// Index to insert
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _StartIndex = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 挿入する文字列
		/// </summary>
#else
		/// <summary>
		/// String to insert
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Value = new FlexibleString();

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
			_Result.SetValue(_String.value.Insert(_StartIndex.value, _Value.value));
		}
	}
}