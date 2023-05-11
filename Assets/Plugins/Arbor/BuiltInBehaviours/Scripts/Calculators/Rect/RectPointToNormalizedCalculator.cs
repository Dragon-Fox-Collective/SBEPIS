//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ポイントに対応する正規化座標を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates the normalized coordinates cooresponding the the point.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.PointToNormalized")]
	[BehaviourTitle("Rect.PointToNormalized")]
	[BuiltInBehaviour]
	public sealed class RectPointToNormalizedCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 正規化座標の内側を取得するための矩形
		/// </summary>
#else
		/// <summary>
		/// Rectangle to get normalized coordinates inside.
		/// </summary>
#endif
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 正規化座標を取得するための矩形内部のポイント
		/// </summary>
#else
		/// <summary>
		/// A point inside the rectangle to get normalized coordinates for.
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
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Rect.PointToNormalized(_Rect.value, _Point.value));
		}
	}
}
