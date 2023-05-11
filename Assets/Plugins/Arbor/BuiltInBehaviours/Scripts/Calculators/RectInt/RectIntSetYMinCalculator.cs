//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形のYMin成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the YMin component of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.SetYMin")]
	[BehaviourTitle("RectInt.SetYMin")]
	[BuiltInBehaviour]
	public sealed class RectIntSetYMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// YMin成分
		/// </summary>
#else
		/// <summary>
		/// YMin component
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _YMin = new FlexibleInt();

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
			value.yMin = _YMin.value;
			_Result.SetValue(value);
		}
	}
}