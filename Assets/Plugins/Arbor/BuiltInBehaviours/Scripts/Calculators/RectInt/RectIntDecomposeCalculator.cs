//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectIntを位置とサイズに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose RectInt into position and size.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.Decompose")]
	[BehaviourTitle("RectInt.Decompose")]
	[BuiltInBehaviour]
	public sealed class RectIntDecomposeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のX座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output the X coordinate of the rectangle.
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _X = new OutputSlotInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のY座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output the Y coordinate of the rectangle.
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Y = new OutputSlotInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の幅を出力
		/// </summary>
#else
		/// <summary>
		/// Output the width of the rectangle
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Width = new OutputSlotInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の高さを出力
		/// </summary>
#else
		/// <summary>
		/// Output the height of the rectangle
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Height = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RectInt rectInt = _RectInt.value;
			_X.SetValue(rectInt.x);
			_Y.SetValue(rectInt.y);
			_Width.SetValue(rectInt.width);
			_Height.SetValue(rectInt.height);
		}
	}
}