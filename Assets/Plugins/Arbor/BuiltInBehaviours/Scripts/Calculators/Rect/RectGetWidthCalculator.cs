//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形の横幅を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the width of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetWidth")]
	[BehaviourTitle("Rect.GetWidth")]
	[BuiltInBehaviour]
	public sealed class RectGetWidthCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 横幅の出力
		/// </summary>
#else
		/// <summary>
		/// Output width
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Width = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Width.SetValue(_Rect.value.width);
		}
	}
}
