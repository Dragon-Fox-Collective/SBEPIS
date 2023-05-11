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
	[AddBehaviourMenu("RectInt/RectInt.GetWidth")]
	[BehaviourTitle("RectInt.GetWidth")]
	[BuiltInBehaviour]
	public sealed class RectIntGetWidthCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 横幅の出力
		/// </summary>
#else
		/// <summary>
		/// Output width
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Width = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Width.SetValue(_RectInt.value.width);
		}
	}
}