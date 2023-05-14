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
	[AddBehaviourMenu("RectInt/RectInt.GetYMax")]
	[BehaviourTitle("RectInt.GetYMax")]
	[BuiltInBehaviour]
	public sealed class RectIntGetYMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// YMax成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output YMax component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _YMax = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_YMax.SetValue(_RectInt.value.yMax);
		}
	}
}