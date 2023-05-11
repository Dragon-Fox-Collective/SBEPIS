//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 正規化座標を指定して長方形内部の位置を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates a point inside a rectangle, given normalized coordinates.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.NormalizedToPoint")]
	[BehaviourTitle("Rect.NormalizedToPoint")]
	[BuiltInBehaviour]
	public sealed class RectNormalizedToPointCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// ポイントを取得するための正規化座標
		/// </summary>
#else
		/// <summary>
		/// Normalized coordinates to get a point for.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _NormalizedRectCoordinates = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Rect.NormalizedToPoint(_Rect.value, _NormalizedRectCoordinates.value));
		}
	}
}
