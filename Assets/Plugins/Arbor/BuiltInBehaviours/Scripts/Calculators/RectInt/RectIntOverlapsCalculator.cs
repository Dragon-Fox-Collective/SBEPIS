//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形同士が重なっているかどうか。
	/// </summary>
#else
	/// <summary>
	/// Whether rectangles overlap each other or not.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.Overlaps")]
	[BehaviourTitle("RectInt.Overlaps")]
	[BuiltInBehaviour]
	public sealed class RectIntOverlapsCalculator : Calculator
	{
#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 重なっているか確認するRectInt
		/// </summary>
#else
		/// <summary>
		/// Check whether overlap RectInt
		/// </summary>
#endif
		[SerializeField] private FlexibleRectInt _Other = new FlexibleRectInt();

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
			_Result.SetValue(_RectInt.value.Overlaps(_Other.value));
		}
	}
}