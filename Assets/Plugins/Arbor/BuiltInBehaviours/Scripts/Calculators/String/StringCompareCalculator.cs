//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Stringを比較する。
	/// </summary>
#else
	/// <summary>
	/// Compare String.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/String.Compare")]
	[BehaviourTitle("String.Compare")]
	[BuiltInBehaviour]
	public sealed class StringCompareCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較タイプ。
		/// </summary>
#else
		/// <summary>
		/// Comparison type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(CompareType))]
		private FlexibleCompareType _CompareType = new FlexibleCompareType(CompareType.Equals);

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較する際の規則を指定。
		/// </summary>
#else
		/// <summary>
		/// Specify the rules for comparison.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleStringComparison _StringComparison = new FlexibleStringComparison();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値1
		/// </summary>
#else
		/// <summary>
		/// Value 1
		/// </summary>
#endif
		[SerializeField] private FlexibleString _Value1 = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value 2
		/// </summary>
#endif
		[SerializeField] private FlexibleString _Value2 = new FlexibleString();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			int compare = string.Compare(_Value1.value, _Value2.value, _StringComparison.value);
			bool result = CompareUtility.Compare(_Value1.value, _Value2.value, _CompareType.value, _StringComparison.value);
			_Result.SetValue(result);
		}
	}
}
