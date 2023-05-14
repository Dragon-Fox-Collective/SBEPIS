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
	[AddBehaviourMenu("RectInt/RectInt.GetXMin")]
	[BehaviourTitle("RectInt.GetXMin")]
	[BuiltInBehaviour]
	public sealed class RectIntGetXMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// XMin成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output XMin component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _XMin = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_XMin.SetValue(_RectInt.value.xMin);
		}
	}
}