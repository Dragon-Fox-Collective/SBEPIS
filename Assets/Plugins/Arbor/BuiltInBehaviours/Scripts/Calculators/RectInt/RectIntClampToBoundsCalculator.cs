//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectIntの位置とサイズを指定された範囲にクランプする。
	/// </summary>
#else
	/// <summary>
	/// Clamps the position and size of the RectInt to the given bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.ClampToBounds")]
	[BehaviourTitle("RectInt.ClampToBounds")]
	[BuiltInBehaviour]
	public sealed class RectIntClampToBoundsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// RectIntをクランプするための境界。
		/// </summary>
#else
		/// <summary>
		/// Bounds to clamp the RectInt.
		/// </summary>
#endif
		[SerializeField] private FlexibleRectInt _Bounds = new FlexibleRectInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotRectInt _Result = new OutputSlotRectInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RectInt rectInt = _RectInt.value;
			rectInt.ClampToBounds(_Bounds.value);
			_Result.SetValue(rectInt);
		}
	}
}