//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のYMin成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the YMin component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetYMin")]
	[BehaviourTitle("Rect.GetYMin")]
	[BuiltInBehaviour]
	public sealed class RectGetYMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// YMin成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output YMin component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _YMin = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_YMin.SetValue(_Rect.value.yMin);
		}
	}
}
