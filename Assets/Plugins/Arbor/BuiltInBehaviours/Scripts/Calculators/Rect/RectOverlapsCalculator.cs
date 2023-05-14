//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
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
	[AddBehaviourMenu("Rect/Rect.Overlaps")]
	[BehaviourTitle("Rect.Overlaps")]
	[BuiltInBehaviour]
	public sealed class RectOverlapsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 重なっているか確認するRect
		/// </summary>
#else
		/// <summary>
		/// Check whether overlap Rect
		/// </summary>
#endif
		[SerializeField] private FlexibleRect _Other = new FlexibleRect();

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
			_Result.SetValue(_Rect.value.Overlaps(_Other.value));
		}
	}
}
