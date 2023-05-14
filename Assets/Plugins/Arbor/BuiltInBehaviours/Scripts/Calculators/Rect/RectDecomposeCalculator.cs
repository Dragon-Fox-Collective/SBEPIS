//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rectを位置とサイズに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Rect into position and size.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.Decompose")]
	[BehaviourTitle("Rect.Decompose")]
	[BuiltInBehaviour]
	public sealed class RectDecomposeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のX座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output the X coordinate of the rectangle.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _X = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形のY座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output the Y coordinate of the rectangle.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Y = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の幅を出力
		/// </summary>
#else
		/// <summary>
		/// Output the width of the rectangle
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Width = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の高さを出力
		/// </summary>
#else
		/// <summary>
		/// Output the height of the rectangle
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Height = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Rect rect = _Rect.value;
			_X.SetValue(rect.x);
			_Y.SetValue(rect.y);
			_Width.SetValue(rect.width);
			_Height.SetValue(rect.height);
		}
	}
}
