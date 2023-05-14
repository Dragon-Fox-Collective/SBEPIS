//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形の最小コーナーの位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the minimum corner of the rect.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetMin")]
	[BehaviourTitle("Rect.GetMin")]
	[BuiltInBehaviour]
	public sealed class RectGetMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 矩形の最小コーナーの位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the minimum corner of the rect.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Min = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Min.SetValue(_Rect.value.min);
		}
	}
}
