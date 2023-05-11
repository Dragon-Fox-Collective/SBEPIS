//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// PointがRectの内側にあるかどうか。
	/// </summary>
#else
	/// <summary>
	/// Whether the Point is inside the Rect.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.Contains")]
	[BehaviourTitle("Rect.Contains")]
	[BuiltInBehaviour]
	public sealed class RectContainsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定する点。
		/// </summary>
#else
		/// <summary>
		/// Point to test.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Point = new FlexibleVector2();

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
			_Result.SetValue(_Rect.value.Contains(_Point.value));
		}
	}
}
