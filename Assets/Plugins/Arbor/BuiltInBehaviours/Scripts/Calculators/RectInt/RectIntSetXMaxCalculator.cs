//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のXMax成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the XMax component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.SetXMax")]
	[BehaviourTitle("RectInt.SetXMax")]
	[BuiltInBehaviour]
	public sealed class RectIntSetXMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// XMax成分
		/// </summary>
#else
		/// <summary>
		/// XMax component
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _XMax = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotRectInt _Result = new OutputSlotRectInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RectInt value = _RectInt.value;
			value.xMax = _XMax.value;
			_Result.SetValue(value);
		}
	}
}