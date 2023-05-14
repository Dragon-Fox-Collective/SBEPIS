//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のXMax成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the XMax component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetXMax")]
	[BehaviourTitle("Rect.GetXMax")]
	[BuiltInBehaviour]
	public sealed class RectGetXMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// XMax成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output XMax component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _XMax = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_XMax.SetValue(_Rect.value.xMax);
		}
	}
}
