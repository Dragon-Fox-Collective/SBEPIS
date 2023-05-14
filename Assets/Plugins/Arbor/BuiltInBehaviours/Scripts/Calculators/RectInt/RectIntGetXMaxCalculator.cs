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
	[AddBehaviourMenu("RectInt/RectInt.GetXMax")]
	[BehaviourTitle("RectInt.GetXMax")]
	[BuiltInBehaviour]
	public sealed class RectIntGetXMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// XMax成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output XMax component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _XMax = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_XMax.SetValue(_RectInt.value.xMax);
		}
	}
}