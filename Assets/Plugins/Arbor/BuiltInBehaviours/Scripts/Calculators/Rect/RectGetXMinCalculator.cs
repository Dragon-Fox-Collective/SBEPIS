//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のXMin成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the XMin component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetXMin")]
	[BehaviourTitle("Rect.GetXMin")]
	[BuiltInBehaviour]
	public sealed class RectGetXMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// XMin成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output XMin component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _XMin = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_XMin.SetValue(_Rect.value.xMin);
		}
	}
}
