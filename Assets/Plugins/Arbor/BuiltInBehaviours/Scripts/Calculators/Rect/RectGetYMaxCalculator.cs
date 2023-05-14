//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のYMax成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the YMax component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetYMax")]
	[BehaviourTitle("Rect.GetYMax")]
	[BuiltInBehaviour]
	public sealed class RectGetYMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// YMax成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output YMax component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _YMax = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_YMax.SetValue(_Rect.value.yMax);
		}
	}
}
