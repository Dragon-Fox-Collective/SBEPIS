//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2つのRectIntが等しい場合にtrueを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns true if the rectangles are the same.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.Equals")]
	[BehaviourTitle("RectInt.Equals")]
	[BuiltInBehaviour]
	public sealed class RectIntEqualsCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値1
		/// </summary>
#else
		/// <summary>
		/// Value 1
		/// </summary>
#endif
		[SerializeField] private FlexibleRectInt _Value1 = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value 2
		/// </summary>
#endif
		[SerializeField] private FlexibleRectInt _Value2 = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			RectInt value1 = _Value1.value;
			RectInt value2 = _Value2.value;
			_Result.SetValue(value1.Equals(value2));
		}
	}
}