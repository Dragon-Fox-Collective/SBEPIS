//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// PointがRectIntの内側にあるかどうか。
	/// </summary>
#else
	/// <summary>
	/// Whether the Point is inside the RectInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.Contains")]
	[BehaviourTitle("RectInt.Contains")]
	[BuiltInBehaviour]
	public sealed class RectIntContainsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定する点。
		/// </summary>
#else
		/// <summary>
		/// Point to test.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Point = new FlexibleVector2Int();

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
			_Result.SetValue(_RectInt.value.Contains(_Point.value));
		}
	}
}